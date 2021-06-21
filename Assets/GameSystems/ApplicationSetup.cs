using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ApplicationSetup : MonoBehaviour
{
    void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 90;

        DOTween.defaultAutoPlay = AutoPlay.None;
        DOTween.defaultTimeScaleIndependent = true;
        DOTween.maxSmoothUnscaledTime = Single.Epsilon;
        DOTween.defaultUpdateType = UpdateType.Normal;
    }
    

}
