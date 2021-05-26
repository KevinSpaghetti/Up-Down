using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAnimationManager : MonoBehaviour
{

    public RectTransform menuContainer;
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }


    public void AnimateMenuEnter()
    {
        menuContainer.anchoredPosition = 
            new Vector2(menuContainer.anchoredPosition.x, 0.0f);
    }

    public void AnimateMenuExit()
    {
        
        menuContainer.anchoredPosition = 
            new Vector2(menuContainer.anchoredPosition.x, -1000.0f);
    }
}
