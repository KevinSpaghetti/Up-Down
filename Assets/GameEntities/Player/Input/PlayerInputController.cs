using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    [SerializeField]
    private bool inputIsEnabled;
    
    public enum PlayerVerticalLocation {
        Top, Bottom
    }
    
    public enum PlayerHorizontalLocation {
        Left, 
        Center, 
        Right
    }
    
    private enum PlayerHorizontalAction{
        GoLeft, 
        GoRight
    }

    private enum PlayerVerticalAction {
        Switch
    }

    public PlayerHorizontalLocation playerHorizontalLocation;
    public PlayerVerticalLocation playerVerticalLocation;

    [Header("Animation Parameters")] 
    public float horizontalAnimationsDuration;
    public float verticalAnimationDuration;
    public Light headlights;
    
    private PlayerInput _inputComponent;
    private Animator _animator;

    [SerializeField]
    private bool inputIsLocked = false;
    
    //TODO: Rework the system to use dotweener to avoid same frame input bugs
    void Awake()
    {
        _inputComponent = GetComponent<PlayerInput>();
        _animator = GetComponent<Animator>();
        
        playerHorizontalLocation = PlayerHorizontalLocation.Center;
        playerVerticalLocation = PlayerVerticalLocation.Bottom;
        
    }

    public void EnableInput()
    {
        inputIsEnabled = true;
        _inputComponent.enabled = true;
    }

    public void DisableInput()
    {
        inputIsEnabled = false;
        _inputComponent.enabled = false;
    }
    
    public void OnMoveLeft(){
        if (!inputIsEnabled) return ;
        if (inputIsLocked) return ;
        inputIsLocked = true;
        var oldLocation = playerHorizontalLocation;
        var newLocation = ComputeNextStateAfterHorizontalAction(PlayerHorizontalAction.GoLeft);
        if (oldLocation == newLocation)
        {
            inputIsLocked = false;
            return;
        }
        
        gotoHorizontalPosition(PlayerHorizontalAction.GoLeft, playerVerticalLocation, oldLocation, newLocation);
        Debug.Log("PlayerHorizontalAction.GoLeft");
    }

    public void OnMoveRight(){
        if (!inputIsEnabled) return ;
        if (inputIsLocked) return ;
        inputIsLocked = true;
        var oldLocation = playerHorizontalLocation;
        var newLocation = ComputeNextStateAfterHorizontalAction(PlayerHorizontalAction.GoRight);
        if (oldLocation == newLocation)
        {
            inputIsLocked = false;
            return;
        }
        
        gotoHorizontalPosition(PlayerHorizontalAction.GoRight, playerVerticalLocation, oldLocation, newLocation);
        Debug.Log("PlayerHorizontalAction.GoRight");
    }

    public void OnSwitchSide(){
        if (!inputIsEnabled) return ;
        if (inputIsLocked) return ;
        inputIsLocked = true;
        var newLocation = playerVerticalLocation == PlayerVerticalLocation.Bottom ? PlayerVerticalLocation.Top : PlayerVerticalLocation.Bottom;
        AnimateSwitch(newLocation);
        Debug.Log("PlayerVerticalAction.Switch");
    }
    
    PlayerHorizontalLocation ComputeNextStateAfterHorizontalAction (PlayerHorizontalAction playerAction) {
        if (playerAction == PlayerHorizontalAction.GoLeft)
        {
            switch (playerHorizontalLocation)
            {
                case PlayerHorizontalLocation.Left: break;
                case PlayerHorizontalLocation.Center:
                    return PlayerHorizontalLocation.Left;
                case PlayerHorizontalLocation.Right: 
                    return PlayerHorizontalLocation.Center;
            }
        }

        if (playerAction == PlayerHorizontalAction.GoRight)
        {
            switch (playerHorizontalLocation)
            {
                case PlayerHorizontalLocation.Left: 
                    return PlayerHorizontalLocation.Center;
                case PlayerHorizontalLocation.Center: 
                    return PlayerHorizontalLocation.Right;
                case PlayerHorizontalLocation.Right: break;
            } 
            
        }
        
        return playerHorizontalLocation;
    }

    void gotoHorizontalPosition(PlayerHorizontalAction action, PlayerVerticalLocation verticalLocation, PlayerHorizontalLocation oldLocation, PlayerHorizontalLocation newLocation)
    {
        switch (action)
        {
            case PlayerHorizontalAction.GoLeft:
                AnimateGoLeft(newLocation);
                break;
            case PlayerHorizontalAction.GoRight:
                AnimateGoRight(newLocation);
                break;
        }

    }

    private void AnimateGoLeft(PlayerHorizontalLocation targetLocation)
    {
        var sequence = HorizontalAnimation(-2.5f, -10);
        sequence.OnStart(() => inputIsLocked = true)
            .OnComplete(() => {
                inputIsLocked = false;
                playerHorizontalLocation = targetLocation;
            });
        sequence.Play();
    }
    
    private void AnimateGoRight(PlayerHorizontalLocation targetLocation)
    {
        var sequence = HorizontalAnimation(2.5f, 10);
        sequence.OnStart(() => inputIsLocked = true)
            .OnComplete(() => {
                inputIsLocked = false;
                playerHorizontalLocation = targetLocation;
            });
        sequence.Play();
    }

    private void AnimateSwitch(PlayerVerticalLocation targetLocation)
    {
        var sequence = DOTween.Sequence();

        var movementAmount = targetLocation == PlayerVerticalLocation.Bottom ? -2.5f : 2.5f;
        
        var move = transform.DOLocalMoveY(movementAmount, verticalAnimationDuration);
        var rotate = transform.DORotate(transform.rotation.eulerAngles + new Vector3(0, 0, 180), verticalAnimationDuration);
        var lightAnimationStage1 = headlights.DOIntensity(0, verticalAnimationDuration / 2.0f);
        var lightAnimationStage2 = headlights.DOIntensity(25, verticalAnimationDuration / 2.0f);
        
        sequence.Insert(0.0f, move);
        sequence.Insert(0.0f, rotate);
        sequence.Insert(0.0f, lightAnimationStage1);
        sequence.Insert(verticalAnimationDuration / 2.0f, lightAnimationStage2);
        
        sequence.OnStart(() => inputIsLocked = true)
            .OnComplete(() => {
                inputIsLocked = false;
                playerVerticalLocation = targetLocation;
            });
        sequence.Play();
    }

    private Sequence HorizontalAnimation(float movementX, float rotation)
    {
        var sequence = DOTween.Sequence();

        var move = transform.DOLocalMoveX(transform.position.x + movementX, horizontalAnimationsDuration);
        var rotateStage1 = transform.DORotate(transform.rotation.eulerAngles + new Vector3(0, rotation, 0), horizontalAnimationsDuration / 2.0f);
        var rotateStage2 = transform.DORotate(transform.rotation.eulerAngles + new Vector3(0, 0, 0), horizontalAnimationsDuration / 2.0f);
        
        sequence.Insert(0.0f, move);
        sequence.Insert(0.0f, rotateStage1);
        sequence.Insert(horizontalAnimationsDuration / 2.0f, rotateStage2);

        return sequence;
    }

    public void GotoInitialPosition()
    {
        playerHorizontalLocation = PlayerHorizontalLocation.Center;
        playerVerticalLocation = PlayerVerticalLocation.Bottom;
    }

}
