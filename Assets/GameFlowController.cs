using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class GameFlowController : MonoBehaviour
{
    
    public int score = 0;
    public float startSpeed = 0.0f;

    public List<Animator> animatorsThatNeedToBeNotified;
    public string triggerName;

    public PlayerInputController inputController;
    public ScoreVisualizationController scoreVisualizer;
    
    void Start()
    {
        inputController.DisableInput();
    }
    
    void Update()
    {
        score += 10;
        scoreVisualizer.SetScore(score);
    }

    
    IEnumerator PlayStartSequence()
    {
        animatorsThatNeedToBeNotified.ForEach(animator => animator.SetTrigger(triggerName));
        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() =>
        {
            Debug.Log(animatorsThatNeedToBeNotified.TrueForAll(animator => animator.GetCurrentAnimatorStateInfo(0).IsName("Target") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1));
            return animatorsThatNeedToBeNotified.TrueForAll(animator => animator.GetCurrentAnimatorStateInfo(0).IsName("Target")  && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1);
        });
        
        //User has the control of the game and can begin playing
        inputController.EnableInput();

        //Enable player inputs;

        //Start spawning obstacles and increase difficulty

    }

    public void PlayerCollidedWithObstacle(GameObject obstacle)
    {
        
    }

    private void OnGameStart()
    {
        StartCoroutine(nameof(PlayStartSequence));
    }
    private void OnGameEnd() {}

    private void updateSpeed() {}
    private void updateScore() {}


    public void StartGame()
    {
        inputController.DisableInput(); //Begins playing when the intro sequence is finished
        OnGameStart();
    }

    public void EndGame()
    {
        OnGameEnd();
    }
    public void PauseGame() { Time.timeScale = 0; }
    public void ResumeGame() { Time.timeScale = 1; }
}
