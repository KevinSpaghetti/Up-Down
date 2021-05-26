using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoresVisualizer : MonoBehaviour
{

    public TextMeshProUGUI scoreDisplayPrefab;
    public GameObject viewportGameObject;
    
    public int[] scores = {100000000, 200000000, 300000000, 400000000};
    
    // Start is called before the first frame update
    void Start()
    {
        foreach (var score in scores)
        {
            var scoreGameobject = Instantiate(scoreDisplayPrefab, viewportGameObject.transform) as TextMeshProUGUI;
            scoreGameobject.text = score.ToString("#,##0");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
