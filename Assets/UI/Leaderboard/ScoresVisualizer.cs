using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class ScoresVisualizer : MonoBehaviour
{

    public TextMeshProUGUI scoreDisplayPrefab;
    public GameObject viewportGameObject;

    private const int maxScoresToShow = 10;
    private List<TextMeshProUGUI> scoreTextItems;
    
    void Start()
    {
        scoreTextItems = new List<TextMeshProUGUI>(); 
        for (int i = 0; i < maxScoresToShow; ++i)
        {
            var scoreGameObject = Instantiate(scoreDisplayPrefab, viewportGameObject.transform) as TextMeshProUGUI;
            scoreGameObject.gameObject.SetActive(false);
            scoreTextItems.Append(scoreGameObject);
        }
    }

    public void DisplayScores(List<int> scores)
    {
        Assert.IsTrue(scores.Count <= maxScoresToShow, "Cannot display more than the " + maxScoresToShow + " highest scores");

        //Display all scores by activating the gameobjects needed and giving them the necessary text value, deactivate the others
        
        int scoreIndex = 0;
        foreach (var textItem in scoreTextItems)
        {
            if (scoreIndex < scores.Count)
            {
                textItem.gameObject.SetActive(true);
                textItem.text = scores[scoreIndex].ToString("#,##0");
            }
            else
            {
                textItem.gameObject.SetActive(false);
            }

            scoreIndex++;
        }
    }
    
}
