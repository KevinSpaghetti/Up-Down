using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class EmitEventsOnPointerBehaviour : MonoBehaviour,
    IPointerDownHandler, IPointerUpHandler
{
    public UnityEvent onPointerDown;
    public UnityEvent onPointerUp;
    
    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("Pointer event up invoked");
        onPointerUp.Invoke();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Pointer event down invoked");
        onPointerDown.Invoke();
    }
}
