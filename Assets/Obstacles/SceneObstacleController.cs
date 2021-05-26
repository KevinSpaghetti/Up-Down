using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneObstacleController : MonoBehaviour
{
    public GameObject player;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
    
    void OnDrawGizmos(){
        foreach (var child in GetComponentsInChildren<Transform>())
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(child.position, player.transform.position);    
        }
        
    }
}
