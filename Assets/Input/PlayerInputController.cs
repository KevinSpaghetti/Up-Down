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

    public PlayerVerticalLocation startingPlayerZone;
    public PlayerHorizontalLocation startingPlayerSide;

    public Transform leftTrackLocation;
    public Transform centerTrackLocation;
    public Transform rightTrackLocation;

    public Transform downLocation;
    public Transform upLocation;

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
    
    void Awake()
    {
        _inputComponent = GetComponent<PlayerInput>();
        
        playerHorizontalLocation = startingPlayerSide;
        playerVerticalLocation = startingPlayerZone;
        
        gotoPosition(playerHorizontalLocation, playerVerticalLocation);
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
    
    void OnMoveLeft(){
        Debug.Log("Left");
        computeNextStateAfterHorizontalAction(PlayerHorizontalAction.GoLeft);
        gotoPosition(playerHorizontalLocation, playerVerticalLocation);
    }

    void OnMoveRight(){
        Debug.Log("Right");
        computeNextStateAfterHorizontalAction(PlayerHorizontalAction.GoRight);
        gotoPosition(playerHorizontalLocation, playerVerticalLocation);
    }

    void OnSwitchSide(){
        Debug.Log("Switch");
        Debug.Log("State: " + playerVerticalLocation);
        computeNextStateAfterVerticalAction(PlayerVerticalAction.Switch);
        gotoPosition(playerHorizontalLocation, playerVerticalLocation);
    }
    
    void computeNextStateAfterHorizontalAction (PlayerHorizontalAction playerAction) {
        if (playerAction == PlayerHorizontalAction.GoLeft)
        {
            switch (playerHorizontalLocation)
            {
                case PlayerHorizontalLocation.Left: break;
                case PlayerHorizontalLocation.Center:
                    playerHorizontalLocation = PlayerHorizontalLocation.Left; 
                    break;
                case PlayerHorizontalLocation.Right: 
                    playerHorizontalLocation = PlayerHorizontalLocation.Center; 
                    break;
            }
        }

        if (playerAction == PlayerHorizontalAction.GoRight)
        {
            switch (playerHorizontalLocation)
            {
                case PlayerHorizontalLocation.Left: 
                    playerHorizontalLocation = PlayerHorizontalLocation.Center; 
                    break;
                case PlayerHorizontalLocation.Center: 
                    playerHorizontalLocation = PlayerHorizontalLocation.Right; 
                    break;
                case PlayerHorizontalLocation.Right: break;
            } 
            
        }
    }
    void computeNextStateAfterVerticalAction (PlayerVerticalAction playerAction) {
        switch (playerVerticalLocation)
        {
            case PlayerVerticalLocation.Top:
                playerVerticalLocation = PlayerVerticalLocation.Bottom;
                break;
            case PlayerVerticalLocation.Bottom:
                playerVerticalLocation = PlayerVerticalLocation.Top;
                break;
        }
    }

    void gotoPosition(PlayerHorizontalLocation horizontalLocation, PlayerVerticalLocation verticalLocation) {
        switch (playerHorizontalLocation) {
            case PlayerHorizontalLocation.Left:
                transform.position = new Vector3(leftTrackLocation.position.x, transform.position.x, transform.position.z);
                break;
            case PlayerHorizontalLocation.Center:
                transform.position = new Vector3(centerTrackLocation.position.x, transform.position.x, transform.position.z);
                break;
            case PlayerHorizontalLocation.Right:
                transform.position = new Vector3(rightTrackLocation.position.x, transform.position.x, transform.position.z);
                break;
        }

        switch (playerVerticalLocation) {
            case PlayerVerticalLocation.Bottom:
                transform.position = new Vector3(transform.position.x, downLocation.position.y, transform.position.z);
                break;
            case PlayerVerticalLocation.Top:
                transform.position = new Vector3(transform.position.x, upLocation.position.y, transform.position.z);
                break;
        }
    }


}
