using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Rendering.Universal;

public class UIScreenManager : MonoBehaviour
{

    public UIScreen onLoadScreen;
    
    private Stack<UIScreen> screenStack;
    private GameObject screenContainer;
    void Start()
    {
        screenContainer = transform.Find("ScreenContainer").gameObject;
        Assert.IsNotNull(screenContainer, "UIScreenManager script must contain a child named ScreenContainer");

        screenStack = new Stack<UIScreen>(); 
        screenStack.Push(onLoadScreen);
        screenStack.Peek().OnAppear();
    }

    public void PushScreen(UIScreen screen)
    {
        screenStack.Peek().OnDisappear();
        screenStack.Push(screen);
        screenStack.Peek().OnAppear();
    }

    public void PopScreen()
    {
        screenStack.Peek().OnDisappear();
        screenStack.Pop();
        screenStack.Peek().OnAppear();
        
    }

    public void StartGame(){
        Debug.Log("StartGame");
    }
}
