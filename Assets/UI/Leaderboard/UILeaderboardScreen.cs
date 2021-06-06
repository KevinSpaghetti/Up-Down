using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ScoresVisualizer))]
public class UILeaderboardScreen : UIScreen
{
    public ScoresPersistenceManager scoresPersistenceManager;

    private ScoresVisualizer visualizer;

    private void Start()
    {
        visualizer = GetComponent<ScoresVisualizer>();
    }

    public void OnAppear()
    {
        base.OnAppear();
        scoresPersistenceManager.LoadScores();
        visualizer.DisplayScores(scoresPersistenceManager.scores);
    }

    public void OnDisappear()
    {
        base.OnDisappear();
        scoresPersistenceManager.SaveScores();
    }
    
}
