using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot
{
    public Cell cell;
    public int row, col;
    private Transform prefab;
    private Transform dot; 

    public Dot(Cell cell, Transform prefab)
    {
        this.prefab = prefab; 
        this.cell = cell;
        row = cell.row;
        col = cell.col;

        CreateDot();
        SetPosition();
    }

    public void CreateDot()
    {
        dot = GameObject.Instantiate(prefab);
    }

    public void SetPosition()
    {
        var worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(cell.x, cell.y, 0));
        dot.position = new Vector3(worldPosition.x, worldPosition.y, dot.position.z);
    }
}
