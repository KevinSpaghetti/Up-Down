using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MenuOpenCloseSequence : AnimationSequence
{
    [Header("Menu")]
    public GameObject menu;
    public CanvasGroup menuCanvasGroup;
    
    [Header("Volume")]
    public AudioSource songsAudioSource;
    
    public override void Play(Action onEndCallback)
    {
        var sequence = DOTween.Sequence();
        
        var volumeGoesDown = songsAudioSource.DOFade(0.0f, 0.15f);
        sequence.Insert(0, volumeGoesDown);

        var menuScaleUp = menu.transform.DOScale(Vector3.one, 0.15f);
        var menuFadeIn = menuCanvasGroup.DOFade(1.0f, 0.15f)
            .OnStart(() => menu.SetActive(true))
            .OnComplete(() => menuCanvasGroup.interactable = true);

        sequence.Insert(0f, menuFadeIn);
        sequence.Insert(0f, menuScaleUp);

        sequence.SetUpdate(true);
        sequence.OnComplete(() => onEndCallback());
        sequence.Play();
    }

    public override void Backwards(Action onEndCallback)
    {
        var sequence = DOTween.Sequence();
        
        var volumeGoesUp = songsAudioSource.DOFade(1.0f, 0.15f);
        sequence.Insert(0, volumeGoesUp);

        var menuScaleDown = menu.transform.DOScale(Vector3.zero, 0.15f);
        var menuFadeOut = menuCanvasGroup.DOFade(0.0f, 0.15f)
            .OnStart(() => menuCanvasGroup.interactable = false)
            .OnComplete(() => menu.SetActive(false));
            
        sequence.Insert(0f, menuFadeOut);
        sequence.Insert(0f, menuScaleDown);

        sequence.SetUpdate(true);
        sequence.OnComplete(() => onEndCallback());
        sequence.Play();
    } 
    
}
