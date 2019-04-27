using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BoardSettings
{
    public int numberOfRows = 4;
    public int numberOfCols = 4;
    public float dotSpacing = 40;
    public enum Colors { red, blue, yellow };
    public Colors[] initialTiles;

    public BoardSettings()
    {
        initialTiles = new Colors[numberOfRows * numberOfCols];
    }
}
public class LevelSettings : MonoBehaviour
{
    public BoardSettings boardSettings = new BoardSettings();
}