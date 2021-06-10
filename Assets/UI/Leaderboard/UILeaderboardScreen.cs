using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILeaderboardScreen : UIScreen
{
    public ScoresPersistenceManager scoresPersistenceManager;

    public override void OnAppear()
    {
        base.OnAppear();

    }

    public override void OnDisappear()
    {
        base.OnDisappear();
        scoresPersistenceManager.SaveScores();
    }
    
}
