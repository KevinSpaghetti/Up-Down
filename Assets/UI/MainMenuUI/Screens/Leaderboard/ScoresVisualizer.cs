using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class ScoresVisualizer : MonoBehaviour
{

    public ScoresPersistenceManager persistenceManager;
    
    public TextMeshProUGUI scoreDisplayPrefab;
    
    private const int maxScoresToShow = 10;
    private List<TextMeshProUGUI> scoreTextItems;

    public void Awake()
    {
        scoreTextItems = new List<TextMeshProUGUI>(); 
        for (int i = 0; i < maxScoresToShow; ++i)
        {
            var scoreGameObject = Instantiate(scoreDisplayPrefab, gameObject.transform) as TextMeshProUGUI;
            scoreGameObject.gameObject.SetActive(false);
            scoreTextItems.Add(scoreGameObject);
        }

    }

    public void LoadScores()
    {
        persistenceManager.LoadScores();
    }
    
    public void DisplayScores()
    {
        persistenceManager.LoadScores();
        List<int> scores = persistenceManager.scores;
        Debug.Assert(scores.Count <= maxScoresToShow, "Cannot display more than the " + maxScoresToShow + " highest scores");

        //Display all scores by activating the gameobjects needed and giving them the necessary text value, deactivate the others
        
        int scoreIndex = 0;
        foreach (var textItem in scoreTextItems)
        {
            if (scoreIndex < scores.Count)
            {
                textItem.gameObject.SetActive(true);
                textItem.text = scores[scoreIndex].ToString();
            }
            else
            {
                textItem.gameObject.SetActive(false);
            }

            scoreIndex++;
        }
    }
    
}
