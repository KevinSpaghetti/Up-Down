using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Timeline;

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
    
    public string inGameStateName;
    public string outGameStateName;
    
    public PlayerInputController playerInputController;
    
    public UnityEvent gameStartedBeforeAnimationsCompleteEvent;
    public UnityEvent gameStartedAfterAnimationsCompleteEvent;
    public UnityEvent returnToMainMenuBeforeAnimationsCompleteEvent;
    public UnityEvent returnToMainMenuAfterAnimationsCompleteEvent;
    
    public UnityEvent gamePausedEvent;
    public UnityEvent gameResumedEvent;

    public ScoresPersistenceManager scoresPersistenceManager;
    public VolumeManager volumeManager;

    public PlayableDirector introToGameSequence;
    public PlayableDirector goIntoPauseSequence;
    public PlayableDirector resumeGameSequence;
    public PlayableDirector goIntoEndGameMenuSequence;
    public PlayableDirector returnToMainMenuSequence;

    public GameObject inGamePauseMenu;
    public GameObject inGameEndMenu;
    
    void Start()
    {
        state = GameState.Menu;
        volumeManager.SetGlobalVolume(PlayerPrefs.GetFloat("volume", 0.5f));
    }
    
    void Update()
    {
        if (state == GameState.StartedAndPlaying)
        {
            score += 10;
        }
    }
    

    public void PlayStartSequenceFinished() {
        //User has the control of the game and can play
        state = GameState.StartedAndPlaying;
        gameStartedAfterAnimationsCompleteEvent.Invoke();
        
    }
    public void PlayGotoPauseMenuSequenceFinished()
    {
        state = GameState.Paused;
    }
    public void PlayResumeGameSequenceFinished()
    {
        state = GameState.StartedAndPlaying;
    }

    public void PlayGotoEndGameMenuFinished()
    {
        state = GameState.EndGameMenu;
    }

    public void ReturnToMainMenuSequenceFinished()
    {
        state = GameState.Menu;
    }

    public void StartGame()
    {
        float vol = PlayerPrefs.GetFloat("volume", 0.5f);
        volumeManager.SetGlobalVolume(vol);
        playerInputController.DisableInput(); //Begins playing when the intro sequence is finished
        introToGameSequence.Play();
    }
    
    /*
    public void EndGame()
    {
        Debug.Log("Game Ended");
        scoresPersistenceManager.AddScore(score);
        playerInputController.DisableInput();
        goIntoEndGameMenuSequence.Play();
    }
    */
    
    public void PauseGame()
    {
        //playerInputController.DisableInput();
        //Time.timeScale = 0;

        state = GameState.Paused;
        gamePausedEvent.Invoke();
        
        //Make the menu appear
        goIntoPauseSequence.Play();
    }
    public void ResumeGame()
    {
        playerInputController.EnableInput();
        Time.timeScale = 1;

        state = GameState.StartedAndPlaying;
        gameResumedEvent.Invoke();
        
        //Make the menu disappear
        resumeGameSequence.Play();
    }

    public void RestartGame()
    {
        //Reset game state
        //Restart game without intro sequence
    }
    
    public void ReturnToMainMenuFromEndGameMenu()
    {
        if (inGamePauseMenu.activeInHierarchy)
        {
            resumeGameSequence.Play();
        }
        if (inGameEndMenu.activeInHierarchy)
        {
            inGameEndMenu.SetActive(false);
        }

        Debug.Log("Game Ended from end game menu");
        returnToMainMenuSequence.Play();
    }

    public void ReturnToMainMenuFromPauseGameMenu()
    {
        if (inGamePauseMenu.activeInHierarchy)
        {
            resumeGameSequence.Play();
        }
        if (inGameEndMenu.activeInHierarchy)
        {
            inGameEndMenu.SetActive(false);
        }

        Debug.Log("Game Ended from pause");
        returnToMainMenuSequence.Play();
    }
    
}
