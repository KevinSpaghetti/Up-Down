using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ScoreVisualizationController : MonoBehaviour
{
    public GameFlowController scoreDataSource;
    
    private TextMeshProUGUI _text;
    
    void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        _text.text = scoreDataSource.score.ToString();
    }
}
