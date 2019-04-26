using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Board
{
    public float boardWidth = Screen.width * 0.8f;
    public float boardHeight = Screen.height * 0.8f;
    public int numberOfRows = 4;
    public int numberOfCols = 4;
    public enum Colors { red, blue, yellow };
    public Colors[] initialTiles;

    public Board()
    {
        initialTiles = new Colors[numberOfRows * numberOfCols];
    }
}
public class LevelSettings : MonoBehaviour
{
    public Board boardLayout = new Board();
}
