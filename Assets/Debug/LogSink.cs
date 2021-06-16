using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogSink : MonoBehaviour
{
    public void Log(string message)
    {
        Debug.Log(message);
    }
}
