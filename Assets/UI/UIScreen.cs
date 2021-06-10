using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScreen : MonoBehaviour
{

    public void ScreenWillAppear() { }

    public virtual void OnAppear()
    {
        gameObject.SetActive(true);
    }

    public virtual void OnDisappear()
    {
        gameObject.SetActive(false);
    }
}
