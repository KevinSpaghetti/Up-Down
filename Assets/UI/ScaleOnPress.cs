using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScaleOnPress : MonoBehaviour,
    IPointerDownHandler, IPointerUpHandler
{
    
    public Vector3 scaleTo;
    
    private RectTransform _rectTransform;

    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        LeanTween.scale(_rectTransform, scaleTo, 0.12f).setEase(LeanTweenType.linear);
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        LeanTween.scale(_rectTransform, Vector3.one, 0.12f).setEase(LeanTweenType.linear);
    }
}
