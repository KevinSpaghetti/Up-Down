using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class PauseGameButton : MonoBehaviour,
    IPointerClickHandler
{

    public UnityEvent menuOpen;
    public UnityEvent menuClosed;
    
    //Sprites to show when the menu is closed and open
    public Sprite menuClosedSprite;
    public Sprite menuOpenSprite;

    private Image _imageComponent;

    public bool isMenuOpen = false;
    
    void Start()
    {
        _imageComponent = GetComponent<Image>();
        _imageComponent.sprite = menuClosedSprite;
    }

    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
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
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        if (isMenuOpen)
        {
            _imageComponent.sprite = menuOpenSprite;
        }
        else
        {
            _imageComponent.sprite = menuClosedSprite;
        }
    }
}
