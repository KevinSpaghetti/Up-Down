using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TracksDebugger : MonoBehaviour
{
    public float lineLength; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update(){
        
    }

    void OnDrawGizmos(){
        Gizmos.color = Color.red;
        for (int i = 0; i < transform.childCount; i++){
            Transform t = transform.GetChild(i);
            Gizmos.DrawSphere(t.position, 0.2f);
            Gizmos.DrawLine(t.position, t.position + new Vector3(0f, 0f, -lineLength)); 
        }
    }
}
