using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;


public class ScaleOnPointerHover : MonoBehaviour, 
    IPointerEnterHandler, IPointerExitHandler
{

    public Vector3 scaleTo;

    private RectTransform _rectTransform;


    // Start is called before the first frame update
    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        LeanTween.scale(_rectTransform, scaleTo, 0.12f).setEase(LeanTweenType.linear);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        LeanTween.scale(_rectTransform, Vector3.one, 0.12f).setEase(LeanTweenType.linear);
    }
    
}
