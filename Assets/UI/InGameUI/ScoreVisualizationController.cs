using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ScoreVisualizationController : MonoBehaviour
{

    public int score
    {
        set { _text.text = value.ToString(); }
    }

    private TextMeshProUGUI _text;
    
    void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

}
