using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Cell
{
    public int row, col;
    public float x, y;

    public Cell(int r, int c, float xPos, float yPos)
    {
        row = r;
        col = c;
        x = xPos;
        y = yPos;
    }
}

public class GameGrid
{ 
    public int rows, cols;
    private float cellSpacing;
    private Vector2 startPoint;
    private Cell[] cells;
    private Dot[] dots;
    public BoardSettings boardSettings;
    public Transform dotPrefab;

    public GameGrid(BoardSettings boardSettings, Transform dotPrefab)
    {
        this.dotPrefab = dotPrefab;
        this.boardSettings = boardSettings;
        rows = boardSettings.numberOfRows;
        cols = boardSettings.numberOfCols;
        cellSpacing = boardSettings.dotSpacing;

        cells = new Cell[rows * cols];
        dots = new Dot[rows * cols];

        // Set the dimensions of the board and cels
        SetBoardDimensions();
        CreateCells();
        CreateTiles();
    }

    private void SetBoardDimensions()
    {
        float boardWidth = cellSpacing * (cols - 1);
        float boardHeight = cellSpacing * (rows - 1);
        startPoint = new Vector2(Screen.width / 2 - boardWidth / 2, Screen.height / 2 + boardHeight / 2); // The upper-left corner of the board
    }

    private void CreateCells()
    {
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                Cell myCell = new Cell(r, c, GetCellXFromCol(c), GetCellYFromRow(r));
                cells[r * cols + c] = myCell;
            }
        }

    }

    public Dot[] CreateTiles()
    {
        int count = 0;
        for (int i = 0; i < cells.Length; i++)
        {
            Cell cell = cells[i];
            Dot dot = new Dot(cell, dotPrefab);
            dots[count] = dot;
            count++;
        }
         
        return dots;
    }

    private float GetCellYFromRow(int row)
    {
        return startPoint.y - ((row % rows) * cellSpacing);
    }

    private float GetCellXFromCol(int col)
    {
        return startPoint.x + (col % cols) * cellSpacing;
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
