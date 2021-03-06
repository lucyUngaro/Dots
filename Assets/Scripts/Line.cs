﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  A simple line with a start point and end point. Before the end point is chosen, the line will follow the mouse.
/// </summary>
public class Line : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    private LineRenderer lineRenderer;
    public Line previous;

    public void StartLine(Transform start, Color color)
    {
        startPoint = start;
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startColor = lineRenderer.endColor = color;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.alignment = LineAlignment.TransformZ;
        lineRenderer.startWidth = lineRenderer.endWidth = start.localScale.x / 1f;
        lineRenderer.SetPosition(0, startPoint.position);
    }

    public void EndLine(Transform end)
    {
        endPoint = end;
        lineRenderer.SetPosition(1, endPoint.position);
    }

    void Update()
    {
        if (endPoint != null || lineRenderer == null) return;

        lineRenderer.SetPosition(1, Camera.main.ScreenToWorldPoint(Input.mousePosition));

    }
}
