﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Dot : MonoBehaviour
{
    public Cell cell;
    public int row, col;
    private Color color;
    private DotManager manager;
    public bool markedForRemoval;

    private void Awake()
    {
        // The initial position should be above the board, until it is set to its cell position
        Vector3 newPosition = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height * 1.5f, 0));
        transform.position = new Vector3(transform.position.x, newPosition.y, transform.position.z);

        // Adjust scale for different screen resolutions
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        float width = GetWidthOfSprite(sprite);
        float screenSize = Screen.width > Screen.height ? Screen.height : Screen.width;
        float scale = screenSize / width * 0.06f;
        transform.localScale *= scale;

    }

    public void SetParameters(DotManager manager, Color color)
    {
        SetManager(manager);
        SetColor(color);
    }

    public void SetCell(Cell cell)
    {
        if (this.cell != null) this.cell.dot = null; 

        this.cell = cell;
        cell.dot = this;
        row = cell.row;
        col = cell.col;

        SetPosition(); // Once the cell is set, move to the cell
    }

    public void SetPosition()
    {
        var worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(cell.x, cell.y, 0));

        // Tween to location
        transform.DOMoveX(worldPosition.x, 0f);
        transform.DOMoveY(worldPosition.y, 0.5f).SetEase(Ease.OutBounce).SetDelay(0.1f);
    }

    private void SetManager(DotManager manager)
    {
        if (this.manager == null) this.manager = manager;
    }

    void OnMouseDown()
    {
        manager.OnDotMouseDown(this);
    }

    private void OnMouseEnter()
    {
        manager.OnDotMouseEnter(this);
    }

    private void OnMouseExit()
    {
        manager.OnDotMouseExit(this);
    }

    private void SetColor(Color color)
    {
        gameObject.GetComponent<SpriteRenderer>().color = color;
        this.color = color;
    }

    public Color GetColor()
    {
        return this.color;
    }

    public void Destroy()
    {
        markedForRemoval = true;
        transform.DOScale(0f, 0.2f).OnComplete(()=>Destroy(gameObject)); // Tween to scale 0 and then destroy
    }

    private float GetWidthOfSprite(SpriteRenderer sprite)
    {
        Bounds bounds = sprite.bounds;
        float rightPoint = bounds.center[0] + bounds.extents[0];
        float leftPoint = bounds.center[0] - bounds.extents[0];
        Vector3 rightEdge = Camera.main.WorldToScreenPoint(new Vector3(rightPoint, 0, 0));
        Vector3 leftEdge = Camera.main.WorldToScreenPoint(new Vector3(leftPoint, 0, 0));

        return rightEdge.x - leftEdge.x;
    }
}