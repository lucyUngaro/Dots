﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This class is a representation of the game grid. 
/// It calculates the positions of the cells depending on the number of rows/columns and spacing of the cells, which are specified in LevelSettings.
/// The board will be centered on the screen for any number of rows/columns.
/// Dots will reference their corresponding cell to determine positioning. 
/// </summary>

// A class to represent a "cell" on the board
public class Cell
{
    public int row, col;
    public float x, y;
    public Dot dot = null;

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
    private int rows, cols;
    private float cellSpacing;
    private Vector2 startPoint;
    private Cell[] cells;
    private Dot[] dots;
    private BoardSettings boardSettings;

    public void CreateGrid(BoardSettings boardSettings)
    {
        this.boardSettings = boardSettings;
        rows = boardSettings.numberOfRows;
        cols = boardSettings.numberOfCols;

        // The spacing between cells:
        float screenSize = Mathf.Min(Screen.width, Screen.height);
        cellSpacing = screenSize * 0.5f / 4;

        cells = new Cell[rows * cols];

        // Set the initial x/y value of the board:
        SetStartPoint();

        // Create the cells that the dots will use for positioning:
        CreateCells();
    }

    private void SetStartPoint()
    {
        // So that the dots are always in the center of the screen regardless of the number of rows/columns:
        float startX = cellSpacing * (cols - 1) / 2; 
        float startY = cellSpacing * (rows - 1) / 2;

        startPoint = new Vector2(Screen.width / 2 - startX, Screen.height / 2 + startY); // The upper-left corner of the game board
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
    
    public Cell GetCell(int index)
    {
        return cells[index];
    }

    // Overload method to get the cell using row/col instead of index
    public Cell GetCell(int row, int col)
    {
        int index = GetIndexFromRowCol(row, col);

        return index < cells.Length && index >= 0 ? GetCell(index) : null;
    }

    public Cell[] GetCells()
    {
        return cells;
    }

    // The y value of cells in this row
    private float GetCellYFromRow(int row) 
    {
        return startPoint.y - ((row % rows) * cellSpacing);
    }

    // The x value of cells in this column
    private float GetCellXFromCol(int col)
    {
        return startPoint.x + (col % cols) * cellSpacing;
    }

    public int GetIndexFromRowCol(int row, int col)
    {
        return row * cols + col;
    }

}
