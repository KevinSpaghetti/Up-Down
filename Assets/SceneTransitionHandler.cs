using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionHandler : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(nameof(asyncSceneLoading), sceneName);
    }

    IEnumerator asyncSceneLoading(string name)
    {
        Debug.Log(name);
        yield return SceneManager.LoadSceneAsync(name);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(name));
    }
}
