using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[ExecuteInEditMode]
public class TextMaterialAnimationBridge : MonoBehaviour
{
    [SerializeField, Range(-1, 1)] public float dilate;

    private TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        text.materialForRendering.SetFloat("_FaceDilate", dilate);

    }
}
