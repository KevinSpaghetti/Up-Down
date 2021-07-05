using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RoadScrollingSpeedController : MonoBehaviour
{

    public float speed = 0.0f;

    public Renderer upRoadRenderer;
    public Renderer downRoadRenderer;

    private Material _upRoadMaterial;
    private Material _downRoadMaterial;

    private Vector4 offset;
    private int offsetPropertyHash;
    void Awake()
    {
        offset = Vector4.zero;
        offsetPropertyHash = Shader.PropertyToID("_Offset");
        _upRoadMaterial = upRoadRenderer.material;
        _downRoadMaterial = downRoadRenderer.material;
    }

    public void SetScrollingSpeed(float speed)
    {
        this.speed = speed;
    }

    private void Update()
    {
        offset.y += (speed * Time.deltaTime);
        offset.y %= 1.0f;
        _upRoadMaterial.SetVector(offsetPropertyHash, offset);
        _downRoadMaterial.SetVector(offsetPropertyHash, offset);
    }
}
