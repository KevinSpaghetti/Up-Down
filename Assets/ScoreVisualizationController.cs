using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ScoreVisualizationController : MonoBehaviour
{
    private TextMeshProUGUI _text;
    
    void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    public void SetScore(int score)
    {
        _text.text = score.ToString();
    }
}
