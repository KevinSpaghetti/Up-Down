using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.ProBuilder;

public class ScoresPersistenceManager : MonoBehaviour
{
    public List<int> scores;

    public string filename;
    public int nOfHighScoresToKeep;
    
    public List<int> GetHighestScores(int nOfScoresToKeep)
    {
        int nofScoresToSave = Mathf.Min(scores.Count, nOfScoresToKeep);
        return scores.OrderByDescending(a => a).Take(nOfScoresToKeep).ToList();
    }

    //Save the 10 highest scores
    public void SaveScores()
    {
        string path = Application.persistentDataPath + System.IO.Path.DirectorySeparatorChar + filename + ".json";
        if (!File.Exists(path)) {
            File.Create(path);
        }

        string jsonData = JsonUtility.ToJson(GetHighestScores(nOfHighScoresToKeep));
        Debug.Log("saving json:");
        Debug.Log(jsonData);
        
        File.WriteAllText(path, jsonData);
    }

    public void LoadScores()
    {
        string path = Application.persistentDataPath + System.IO.Path.DirectorySeparatorChar + filename + ".json";
        if (!File.Exists(path)) {
            var stream = File.Create(path);
            stream.Close();
            scores = new List<int>();
        }
        else
        {
            string fileContents = File.ReadAllText(path);
            scores = JsonUtility.FromJson(fileContents, typeof(List<int>)) as List<int>;
        }
        
    }

    public void AddScore(int value)
    {
        scores.Append(value);
    }
    
    
    
}
