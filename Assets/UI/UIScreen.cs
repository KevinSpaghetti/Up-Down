using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScreen : MonoBehaviour
{

    public void ScreenWillAppear() { }

    public void OnAppear()
    {
        gameObject.SetActive(true);
    }

    public void OnDisappear()
    {
        gameObject.SetActive(false);
    }
}
