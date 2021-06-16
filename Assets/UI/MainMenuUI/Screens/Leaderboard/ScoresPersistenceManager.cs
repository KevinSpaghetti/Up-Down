using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using UnityEngine;
using UnityEngine.Assertions;

public class ScoresPersistenceManager : MonoBehaviour
{

    public List<int> scores;

    [Serializable]
    private struct SerializerScoresWrapper
    {
        public List<int> scoresToKeep;

        public SerializerScoresWrapper(List<int> scores)
        {
            scoresToKeep = scores;
        }
    }
    
    public string filename;
    public int nOfHighScoresToKeep;

    public void Awake()
    {
        LoadScores();
    }

    public List<int> GetHighestScores(int nOfScoresToKeep)
    {
        Debug.Assert(scores != null);

        int nofScoresToSave = Mathf.Min(scores.Count, nOfScoresToKeep);
        return scores.OrderByDescending(a => a).Take(nOfScoresToKeep).ToList();
    }

    //Save the 10 highest scores
    public void SaveScores()
    {
        Debug.Assert(scores != null);

        string path = Application.persistentDataPath + System.IO.Path.DirectorySeparatorChar + filename + ".json";
        if (!File.Exists(path)) {
            File.Create(path);
        }

        var wrapper = new SerializerScoresWrapper(GetHighestScores(nOfHighScoresToKeep));
        string jsonData = JsonUtility.ToJson(wrapper);
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
            var wrapper = (SerializerScoresWrapper) JsonUtility.FromJson(fileContents, typeof(SerializerScoresWrapper));
            scores = wrapper.scoresToKeep;
        }
        
    }

    public void AddScore(int value)
    {
        Debug.Assert(scores != null);
        
        scores.Add(value);
    }
    
    
    
}
