using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class NotifyCollisionWith : MonoBehaviour
{
    public UnityEvent<GameObject> collidedWith;

    void Start()
    {

    }

    public void OnCollisionEnter(Collision other)
    {
        Debug.Log("Collision");
        collidedWith.Invoke(other.gameObject);
    }
}
