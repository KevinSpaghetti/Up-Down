using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
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

    private PlayerInput _inputComponent;
    private Animator _animator;
    
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
        var oldLocation = playerHorizontalLocation;
        var newLocation = computeNextStateAfterHorizontalAction(PlayerHorizontalAction.GoLeft);
        if(oldLocation == newLocation) return;
        
        playerHorizontalLocation = newLocation;
        gotoHorizontalPosition(PlayerHorizontalAction.GoLeft);
        Debug.Log("PlayerHorizontalAction.GoLeft");
    }

    public void OnMoveRight(){
        if (!inputIsEnabled) return ;
        var oldLocation = playerHorizontalLocation;
        var newLocation = computeNextStateAfterHorizontalAction(PlayerHorizontalAction.GoRight);
        if(oldLocation == newLocation) return;
        
        playerHorizontalLocation = newLocation;
        gotoHorizontalPosition(PlayerHorizontalAction.GoRight);
        Debug.Log("PlayerHorizontalAction.GoRight");
    }

    public void OnSwitchSide(){
        if (!inputIsEnabled) return ;
        playerVerticalLocation = computeNextStateAfterVerticalAction(PlayerVerticalAction.Switch);
        _animator.SetTrigger("Switch");
        Debug.Log("Switch");
    }
    
    PlayerHorizontalLocation computeNextStateAfterHorizontalAction (PlayerHorizontalAction playerAction) {
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
    PlayerVerticalLocation computeNextStateAfterVerticalAction (PlayerVerticalAction playerAction) {
        switch (playerVerticalLocation)
        {
            case PlayerVerticalLocation.Top:
                return PlayerVerticalLocation.Bottom;
            case PlayerVerticalLocation.Bottom:
                return PlayerVerticalLocation.Top;
        }

        return playerVerticalLocation;
    }

    void gotoHorizontalPosition(PlayerHorizontalAction action)
    {
        string leftTrigger = "Left";
        string rightTrigger = "Right";

        if (playerVerticalLocation == PlayerVerticalLocation.Top)
        {
            leftTrigger = "Right";
            rightTrigger = "Left";
        }

        switch (action) {
            case PlayerHorizontalAction.GoLeft:
                _animator.SetTrigger(leftTrigger);
                break;
            case PlayerHorizontalAction.GoRight:
                _animator.SetTrigger(rightTrigger);
                break;
        }
        
    }

    public void GotoInitialPosition()
    {
        playerHorizontalLocation = PlayerHorizontalLocation.Center;
        playerVerticalLocation = PlayerVerticalLocation.Bottom;
    }

}
