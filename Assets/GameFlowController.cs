using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class GameFlowController : MonoBehaviour
{

    public int score = 0;
    public float startSpeed = 0.0f;
    
    public void PlayerCollidedWithObstacle(GameObject obstacle)
    {
        
    }

    private void OnGameStart()
    {
        
    }
    private void OnGameEnd() {}

    private void updateSpeed() {}
    private void updateScore() {}


    public void StartGame()
    {
        Debug.Log("Game Started");
        OnGameStart();
    }

    public void EndGame()
    {
        OnGameEnd();
    }
    public void PauseGame() { Time.timeScale = 0; }
    public void ResumeGame() { Time.timeScale = 1; }
}
