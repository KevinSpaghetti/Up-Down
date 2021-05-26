using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Rendering.Universal;

public class UIScreenManager : MonoBehaviour
{

    public GameObject mainMenu;
    
    private GameObject currentScreen;
    private GameObject screenContainer;
    void Start()
    {
        screenContainer = transform.Find("ScreenContainer").gameObject;
        Assert.IsNotNull(screenContainer, 
            "UIScreenManager script must contain a child named ScreenContainer");
        currentScreen = mainMenu;
    }

    
    void Update()
    {
        
    }

    public void ShowScreen(GameObject screen)
    {
        currentScreen.gameObject.SetActive(false);
        currentScreen = screen;
        currentScreen.gameObject.SetActive(true);
    }

    public void StartGame(){
        Debug.Log("StartGame");
    }
}
