using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class ScaleOnPointerHover : MonoBehaviour, 
    IPointerEnterHandler, IPointerExitHandler
{

    public bool enabled = true;
    
    public Vector3 scaleTo;

    private RectTransform _rectTransform;


    // Start is called before the first frame update
    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!enabled) return;
        LeanTween.scale(_rectTransform, scaleTo, 0.12f).setEase(LeanTweenType.linear);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(!enabled) return;
        LeanTween.scale(_rectTransform, Vector3.one, 0.12f).setEase(LeanTweenType.linear);
    }
    
    
    
}
