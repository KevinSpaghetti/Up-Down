using System;
using System.Collections;
using System.Collections.Generic;
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

    public PlayableDirector introToGameSequence;
    public PlayableDirector goIntoPauseSequence;
    public PlayableDirector resumeGameSequence;
    public PlayableDirector goIntoEndGameMenuSequence;
    public PlayableDirector returnToMainMenuSequence;

    public GameObject inGamePauseMenu;
    public GameObject inGameEndMenu;

    public PlayerInputController player;
    public Transform playerInitialPosition;
    
    public SongsPlaylistAudioController songsPlaylistAudioController;

    public DifficultyManager difficultyManager;

    public PauseGameButton pauseGameButton;
    public ScoreVisualizationController scoreVisualization;
    
    public int scoreIncreaseConstant = 10;
    
    void Start()
    {
        state = GameState.Menu;
        volumeManager.SetGlobalVolume(PlayerPrefs.GetFloat("volume", 0.5f));
        StartCoroutine(nameof(UpdateScore));
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
        introToGameSequence.Play();
        songsPlaylistAudioController.StartPlaying();
        if (pauseGameButton.isMenuOpen)
        {
            pauseGameButton.SetMenuClosed();
        }
        
        gameStartedBeforeAnimationsCompleteEvent.Invoke();
    }
    public void PlayStartSequenceFinished() {
        gameStartedAfterAnimationsCompleteEvent.Invoke();
        playerInputController.EnableInput();

        Time.timeScale = 1.0f;
        difficultyManager.StartGame();
        
        //User has the control of the game and can play
        state = GameState.StartedAndPlaying;
    }
    
        
    public void PauseGame()
    {
        state = GameState.Paused;
     
        Time.timeScale = 0;
        playerInputController.DisableInput();
        gamePausedBeforeAnimationsCompleteEvent.Invoke();
        difficultyManager.PauseGame();
        
        goIntoPauseSequence.Play();
    }
    public void PlayGotoPauseMenuSequenceFinished()
    {
        gamePausedAfterAnimationsCompleteEvent.Invoke();
        songsPlaylistAudioController.Pause();
        
        state = GameState.Paused;
    }

    
    public void ResumeGame()
    {
        state = GameState.Paused;

        gameResumedBeforeAnimationsCompleteEvent.Invoke();
        songsPlaylistAudioController.Resume();
        resumeGameSequence.Play();
        difficultyManager.ResumeGame();
        
        LeanTween.value(this.gameObject, (float value) => { Time.timeScale = value; }, 0, 1, 0.15f).setEase(LeanTweenType.easeOutQuad).setIgnoreTimeScale(true);
    }
    public void PlayResumeGameSequenceFinished()
    {
        gameResumedAfterAnimationsCompleteEvent.Invoke();
        playerInputController.EnableInput();

        state = GameState.StartedAndPlaying;
    }

    
    public void EndGame()
    {
        state = GameState.EndGameMenu;
        
        difficultyManager.StopGame();
        
        gameEndedBeforeAnimationsCompleteEvent.Invoke();
        scoresPersistenceManager.AddScore(score);
        scoresPersistenceManager.SaveScores();
        playerInputController.DisableInput();
        goIntoEndGameMenuSequence.Play();
        LeanTween.value(this.gameObject, (float value) => { Time.timeScale = value; }, 1, 0, 0.1f).setEase(LeanTweenType.easeOutQuad).setIgnoreTimeScale(true);
    }
    public void PlayGotoEndGameMenuFinished()
    {
        gameEndedAfterAnimationsCompleteEvent.Invoke();
        songsPlaylistAudioController.StopPlaying();
        difficultyManager.PauseGame();
        
        state = GameState.EndGameMenu;
    }
    
    public void ReturnToMainMenu()
    {
        songsPlaylistAudioController.StopPlaying();
        difficultyManager.StopGame();
        playerInputController.DisableInput();
        
        Time.timeScale = 1.0f;
        
        CloseAnyOpenMenu();
        
        returnToMainMenuBeforeAnimationsCompleteEvent.Invoke();
        returnToMainMenuSequence.Play();

        ResetGameState();
        
        Transform initialTransform = player.transform;
        LeanTween.value(this.gameObject, (float value) =>
        {
            //Interpolate Player position and rotation
            Vector3 position = Vector3.Lerp(initialTransform.position, playerInitialPosition.position, value);
            Quaternion rotation = Quaternion.Lerp(initialTransform.rotation, initialTransform.rotation, value);
            player.transform.position = position;
            player.transform.rotation = rotation;
        }, 0, 1, 2f);

    }
    public void ReturnToMainMenuSequenceFinished()
    {
        returnToMainMenuAfterAnimationsCompleteEvent.Invoke();
        state = GameState.Menu;
    }

    private void CloseAnyOpenMenu()
    {
        //Close any open menu
        if (inGamePauseMenu.activeInHierarchy) resumeGameSequence.Play();
        if (inGameEndMenu.activeInHierarchy)
        {
            var canvasGroup = inGameEndMenu.GetComponent<CanvasGroup>();
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            inGameEndMenu.SetActive(false);
        }
    }
    public void RestartGame()
    {
        CloseAnyOpenMenu();
        ResetGameState();

        Time.timeScale = 1.0f;
        
        Transform initialTransform = player.transform;
        LeanTween.value(this.gameObject, (float value) =>
        {
            //Interpolate Player position and rotation
            Vector3 position = Vector3.Lerp(initialTransform.position, playerInitialPosition.position, value);
            Quaternion rotation = Quaternion.Lerp(initialTransform.rotation, initialTransform.rotation, value);
            player.transform.position = position;
            player.transform.rotation = rotation;
        }, 0, 1, 2f).setOnComplete(() =>
        {
            StartGame(); 
        });

    }

    private void ResetGameState()
    {

        difficultyManager.Reset();
    }
    
}
