using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class PauseGameButton : MonoBehaviour
{

    public UnityEvent menuOpen;
    public UnityEvent menuClosed;
    
    //Sprites to show when the menu is closed and open
    public Sprite menuClosedSprite;
    public Sprite menuOpenSprite;

    private Image _imageComponent;
    private Button _buttonComponent;
    
    public bool isMenuOpen = false;
    
    void Awake()
    {
        _buttonComponent = GetComponent<Button>();
        _imageComponent = GetComponent<Image>();
        _imageComponent.sprite = menuClosedSprite;

        _buttonComponent.onClick.AddListener(OnClick);
    }
    
    public void OnClick()
    {
        
        isMenuOpen = !isMenuOpen;
        if (isMenuOpen)
        {
            menuOpen.Invoke();
        }
        else
        {
            menuClosed.Invoke();
        }
    }

    public void SetMenuOpen()
    {
        isMenuOpen = true;
        _imageComponent.sprite = menuOpenSprite;
    }
    public void SetMenuClosed()
    {
        isMenuOpen = false;
        _imageComponent.sprite = menuClosedSprite;
    }
    
}
