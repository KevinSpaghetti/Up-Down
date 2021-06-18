using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class GameIntroSequence : AnimationSequence
{

    [Header("In Game UI")]
    public GameObject inGameUI;
    public CanvasGroup inGameUICanvasGroup;
    
    [Header("Main Menu UI")]
    public GameObject mainMenuUI;
    public CanvasGroup mainMenuUICanvasGroup;
    public RectTransform mainMenuUIGameTitlePanelTransform;
    public RectTransform mainMenuUIGameButtonsPanelTransform;
    
    [Header("Player Appear")]
    public GameObject playerCharacter;
    public Transform playerStartPosition;
    public Transform playerInitialPosition;

    [Header("Buttons")] 
    public CanvasGroup buttonsCanvasGroup;
    
    [Header("Volume")]
    public AudioSource inGameSongsAudioSource;

    private float gameTitlePanelInitialYPosition;
    private float buttonsPanelInitialYPosition;

    public float gameTitlePanelMoveYAmount = 500;
    public float buttonsPanelMoveYAmount = -600;

    
    private void Start()
    {
        gameTitlePanelInitialYPosition = mainMenuUIGameTitlePanelTransform.anchoredPosition.y;
        buttonsPanelInitialYPosition = mainMenuUIGameButtonsPanelTransform.anchoredPosition.y;
        
    }

    public override void Play(Action onEndCallback)
    {
        var sequence = DOTween.Sequence();
        
        //Remove UI for game start
        var mainMenuUITitleChangePositions = mainMenuUIGameTitlePanelTransform.DOAnchorPosY(gameTitlePanelMoveYAmount, 1.0f);
        var mainMenuUIButtonsChangePositions = mainMenuUIGameButtonsPanelTransform.DOAnchorPosY(buttonsPanelMoveYAmount, 1.0f);
        var mainMenuUIFade = mainMenuUICanvasGroup.DOFade(0, 1.8f)
            .OnComplete(() => mainMenuUI.SetActive(false));
        sequence.Insert(0, mainMenuUITitleChangePositions);
        sequence.Insert(0, mainMenuUIButtonsChangePositions);
        sequence.Insert(0, mainMenuUIFade);

        //Player appears
        var playerAppear = playerCharacter.transform.DOMove(playerStartPosition.position, 5.0f);
        sequence.Insert(0, playerAppear);

        //Volume increases
        var volumeIncrease = inGameSongsAudioSource.DOFade(1.0f, 5.0f);
        sequence.Insert(0, volumeIncrease);

        //In game UI Appears
        var inGameUIAppears = inGameUICanvasGroup.DOFade(1.0f, 1.0f)
            .OnStart(() => mainMenuUICanvasGroup.interactable = false)
            .OnStart(() => inGameUI.SetActive(true));
        sequence.Insert(2.0f, inGameUIAppears);
        
        //Show buttons
        var showControlButtons = buttonsCanvasGroup.DOFade(0.4f, 1.0f);
        var hideControlButtons = buttonsCanvasGroup.DOFade(0.0f, 1.0f);

        sequence.Insert(3.0f, showControlButtons);
        sequence.Insert(4.0f, hideControlButtons);
        
        sequence.SetUpdate(true);
        sequence.OnComplete(() => onEndCallback());
        sequence.Play();
    }

    public override void Backwards(Action onEndCallback)
    {
        var sequence = DOTween.Sequence();

        //Remove UI for game start
        var mainMenuUITitleChangePositions = mainMenuUIGameTitlePanelTransform.DOAnchorPosY(gameTitlePanelInitialYPosition, 1.0f);
        var mainMenuUIButtonsChangePositions = mainMenuUIGameButtonsPanelTransform.DOAnchorPosY(buttonsPanelInitialYPosition, 1.0f);
        var mainMenuUIFade = mainMenuUICanvasGroup.DOFade(1, 1.8f)
            .OnStart(() => mainMenuUI.SetActive(true))
            .OnComplete(() => mainMenuUICanvasGroup.interactable = true);
        sequence.Insert(0, mainMenuUITitleChangePositions);
        sequence.Insert(0, mainMenuUIButtonsChangePositions);
        sequence.Insert(0, mainMenuUIFade);

        //Player appears
        var playerReturnsToInitialPosition = playerCharacter.transform.DOMove(playerInitialPosition.position, 2.0f);
        sequence.Insert(0, playerReturnsToInitialPosition);

        //Volume increases
        var volumeDecrease = inGameSongsAudioSource.DOFade(0.0f, 2.0f);
        sequence.Insert(0, volumeDecrease);

        //In game UI Appears
        var inGameUIDisappears = inGameUICanvasGroup.DOFade(0.0f, 1.0f)
            .OnStart(() => inGameUI.SetActive(false));
        sequence.Insert(0.0f, inGameUIDisappears);

        sequence.Insert(0.0f, playerCharacter.transform.DOMove(playerInitialPosition.position, 1.0f));
        sequence.Insert(0.0f, playerCharacter.transform.DORotate(playerInitialPosition.rotation.eulerAngles, 1.0f));
        
        sequence.SetUpdate(true);
        sequence.OnComplete(() => onEndCallback());
        sequence.Play();

    }

}
