using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

public enum GameState
{
    Menu,
    StartedButNotPlaying,
    StartedAndPlaying,
    Paused,
    EndGameMenu
}

public class GameFlowController : MonoBehaviour
{
    public GameState state;
    public int score = 0;
    
    public PlayerInputController playerInputController;
    
    [Header("Game State Events")]
    public UnityEvent gameStartedBeforeAnimationsCompleteEvent;
    public UnityEvent gameStartedAfterAnimationsCompleteEvent;
    public UnityEvent gamePausedBeforeAnimationsCompleteEvent;
    public UnityEvent gamePausedAfterAnimationsCompleteEvent;
    public UnityEvent gameResumedBeforeAnimationsCompleteEvent;
    public UnityEvent gameResumedAfterAnimationsCompleteEvent;
    public UnityEvent gameEndedBeforeAnimationsCompleteEvent;
    public UnityEvent gameEndedAfterAnimationsCompleteEvent;
    public UnityEvent returnToMainMenuBeforeAnimationsCompleteEvent;
    public UnityEvent returnToMainMenuAfterAnimationsCompleteEvent;
    
    public ScoresPersistenceManager scoresPersistenceManager;
    public VolumeManager volumeManager;

    public AnimationSequence introToGameSequence;
    public AnimationSequence goIntoPauseSequence;
    public AnimationSequence goIntoEndGameMenuSequence;

    public GameObject inGamePauseMenu;
    public GameObject inGameEndMenu;

    public PlayerInputController player;
    public Transform playerInitialPosition;
    
    public SongsPlaylistAudioController songsPlaylistAudioController;

    public DifficultyManager difficultyManager;

    public PauseGameButton pauseGameButton;
    public ScoreVisualizationController scoreVisualization;
    
    public int scoreIncreaseConstant = 10;

    public RoadScrollingSpeedController roadSpeedController;
    
    void Start()
    {
        state = GameState.Menu;
        volumeManager.SetGlobalVolume(PlayerPrefs.GetFloat("volume", 0.5f));
        StartCoroutine(nameof(UpdateScore)); //Update score with a coroutine to avoid frame dipendence
    }

    IEnumerator UpdateScore()
    {
        for (;;)
        {
            if (state == GameState.StartedAndPlaying)
            {
                score += scoreIncreaseConstant;
                scoreVisualization.score = score;
            }
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }
    
    public void StartGame()
    {
        state = GameState.StartedButNotPlaying;
        
        Time.timeScale = 1.0f;
        score = 0;
        
        float vol = PlayerPrefs.GetFloat("volume", 0.5f);
        volumeManager.SetGlobalVolume(vol);
        
        playerInputController.DisableInput(); //Begins playing when the intro sequence is finished
        playerInputController.GotoInitialPosition();
        songsPlaylistAudioController.StartPlaying();
        if (pauseGameButton.isMenuOpen) pauseGameButton.SetMenuClosed();

        DOTween.To(() => roadSpeedController.speed, (x) => roadSpeedController.SetScrollingSpeed(x), 3.0f, 5.0f).SetEase(Ease.InCubic).Play();
        
        gameStartedBeforeAnimationsCompleteEvent.Invoke();
        introToGameSequence.Play(() =>
        {
            gameStartedAfterAnimationsCompleteEvent.Invoke();
            playerInputController.EnableInput();

            Time.timeScale = 1.0f;
            difficultyManager.StartGame();
        
            //User has the control of the game and can play
            state = GameState.StartedAndPlaying;
        });

    }

    public void PauseGame()
    {
        state = GameState.Paused;
     
        Time.timeScale = 0;
        playerInputController.DisableInput();
        gamePausedBeforeAnimationsCompleteEvent.Invoke();
        difficultyManager.PauseGame();
        
        goIntoPauseSequence.Play(() =>
        {
            gamePausedAfterAnimationsCompleteEvent.Invoke();
            songsPlaylistAudioController.Pause();

            state = GameState.Paused;  
        });
    }
    
    public void ResumeGame()
    {
        state = GameState.Paused;

        gameResumedBeforeAnimationsCompleteEvent.Invoke();
        songsPlaylistAudioController.Resume();
        difficultyManager.ResumeGame();

        Time.timeScale = 1;
        goIntoPauseSequence.Backwards(() =>
        {
            gameResumedAfterAnimationsCompleteEvent.Invoke();
            playerInputController.EnableInput();

            state = GameState.StartedAndPlaying;
        });
    }

    public void EndGame()
    {
        state = GameState.EndGameMenu;
        
        difficultyManager.StopGame();
        
        gameEndedBeforeAnimationsCompleteEvent.Invoke();
        Debug.Log("Saved score: " + score);
        scoresPersistenceManager.AddScore(score);
        scoresPersistenceManager.SaveScores();
        playerInputController.DisableInput();
        
        Time.timeScale = 0;
        roadSpeedController.SetScrollingSpeed(0);

        goIntoEndGameMenuSequence.Play(() =>
        {
            gameEndedAfterAnimationsCompleteEvent.Invoke();
            songsPlaylistAudioController.StopPlaying();
            difficultyManager.PauseGame();
        
            state = GameState.EndGameMenu;
        });
    }

    public void ReturnToMainMenu()
    {
        songsPlaylistAudioController.StopPlaying();
        difficultyManager.StopGame();
        playerInputController.DisableInput();
        
        roadSpeedController.SetScrollingSpeed(0);
        Time.timeScale = 1.0f;
        
        CloseAnyOpenMenu();
        ResetGameState();
        
        returnToMainMenuBeforeAnimationsCompleteEvent.Invoke();
        introToGameSequence.Backwards(() =>
        {
            returnToMainMenuAfterAnimationsCompleteEvent.Invoke();
            state = GameState.Menu;
        });
    }

    private void CloseAnyOpenMenu()
    {
        //Close any open menu
        if (inGamePauseMenu.activeInHierarchy) goIntoPauseSequence.Backwards(() => { });
        if (inGameEndMenu.activeInHierarchy) goIntoEndGameMenuSequence.Backwards(() => { });
    }
    public void RestartGame()
    {
        CloseAnyOpenMenu();
        ResetGameState();

        Time.timeScale = 1.0f;

        var sequence = DOTween.Sequence();
        sequence.Insert(0.0f, player.transform.DOMove(playerInitialPosition.position, 1.0f));
        sequence.Insert(0.0f, player.transform.DORotate(playerInitialPosition.rotation.eulerAngles, 1.0f));
        sequence.OnComplete(() =>
        {
            StartGame();
        });
        sequence.Play();

    }

    private void ResetGameState()
    {
        score = 0;
        scoreVisualization.score = 0;
        difficultyManager.Reset();
    }
    
}
