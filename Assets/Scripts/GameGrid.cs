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

public class GameGrid : MonoBehaviour
{
    public int rows = 4;
    public int cols = 4;
    public int boardWidth, boardHeight; 
    public Transform dotPrefab;

   
    private float cellSize;
    private Vector2 startPoint;
    private List<Cell> cells = new List<Cell>(); 

    void Awake()
    {
        // Set the dimensions of the board and cels
        SetBoardDimensions();
        CreateCells();
    }

    private void SetBoardDimensions()
    {
        boardWidth = boardWidth <= 0 ? (int)(Screen.width * 0.8) : boardWidth; 
        boardHeight = boardHeight <= 0 ? (int)(Screen.height * 0.6) : boardHeight;
        startPoint = new Vector2(Screen.width / 2 - boardWidth / 2, Screen.height / 2 + boardHeight / 2); // The upper-left corner of the board
        cellSize = Mathf.Min(boardWidth / cols, boardHeight / rows); 
    }

    private void CreateCells()
    {
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                Cell myCell = new Cell(r, c, GetCellXFromCol(c), GetCellYFromRow(r));
                cells.Add(myCell);
            }
        }
    }

    private float GetCellYFromRow(int row)
    {
        return startPoint.y - ((row % rows) * cellSize);
    }

    private float GetCellXFromCol(int col)
    {
        return startPoint.x + (col % cols) * cellSize;
    }

    private float GetWidthOfSprite (SpriteRenderer sprite)
    {
        Bounds bounds = sprite.bounds;
        float rightPoint = bounds.center[0] + bounds.extents[0];
        float leftPoint = bounds.center[0] - bounds.extents[0];
        Vector3 rightEdge = Camera.main.WorldToScreenPoint(new Vector3(rightPoint, 0, 0));
        Vector3 leftEdge = Camera.main.WorldToScreenPoint(new Vector3(leftPoint, 0, 0));
       
        return rightEdge.x - leftEdge.x; 
    }

}
