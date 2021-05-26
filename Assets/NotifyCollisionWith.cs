using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class NotifyCollisionWith : MonoBehaviour
{
    public LayerMask collisionMask;
    public UnityEvent<GameObject> collidedWith;

    void Start()
    {

    }

    private void OnCollisionEnter(Collision other)
    {
        if ((collisionMask.value & other.gameObject.layer) == collisionMask.value)
        {
            collidedWith.Invoke(other.gameObject);    
        }
    }
}
