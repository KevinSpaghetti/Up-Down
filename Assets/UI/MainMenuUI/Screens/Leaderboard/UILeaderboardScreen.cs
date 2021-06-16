using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILeaderboardScreen : UIScreen
{
    public ScoresVisualizer scoreVisualizer;
    
    public override void OnAppear()
    {
        base.OnAppear();
        scoreVisualizer.DisplayScores();
    }

    public override void OnDisappear()
    {
        base.OnDisappear();
    }
    
}
