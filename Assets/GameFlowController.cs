using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlowController : MonoBehaviour
{

    public int score = 0;
    public float startSpeed = 0.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayerCollidedWithObstacle(GameObject obstacle)
    {
        
    }

    public void OnGameStart() {}
    public void OnGameEnd() {}

    private void updateSpeed() {}
    private void updateScore() {}
    
    
    public void PauseGame() { Time.timeScale = 0; }
    public void ResumeGame() { Time.timeScale = 1; }
}
