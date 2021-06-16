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
        onPointerUp.Invoke();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        onPointerDown.Invoke();
    }
}
