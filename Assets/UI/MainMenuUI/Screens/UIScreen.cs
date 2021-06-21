using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIScreen : MonoBehaviour
{
    public UnityEvent onScreenAppear;
    public UnityEvent onScreenDisappear;
    

    public virtual void OnAppear()
    {
        gameObject.SetActive(true);
        onScreenAppear.Invoke();
    }

    public virtual void OnDisappear()
    {
        onScreenDisappear.Invoke();
        gameObject.SetActive(false);
    }
}
