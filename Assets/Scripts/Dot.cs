using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : MonoBehaviour
{
    public Cell cell;
    public int row, col;
    private Color color;
    private GameManager manager;

    public void SetParameters(GameManager manager, Cell cell, Color color)
    {
        this.cell = cell;
        row = cell.row;
        col = cell.col;

        SetManager(manager);
        SetColor(color);
        SetPosition();
    }

    private void SetManager(GameManager manager)
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

    public void SetPosition()
    {
        var worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(cell.x, cell.y, 0));
        transform.position = new Vector3(worldPosition.x, worldPosition.y, transform.position.z);
    }
}