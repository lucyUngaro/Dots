using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BoardSettings
{
    public int numberOfRows = 4;
    public int numberOfCols = 4;
    public float dotSpacing = 40;
    public enum Colors { red, green, blue, purple, yellow };
    public Colors[] initialTiles;

    public BoardSettings()
    {
        initialTiles = new Colors[numberOfRows * numberOfCols];
    }
}
[System.Serializable]
public struct ColorSettings
{
    public Color red, green, blue, purple, yellow;

}
public class LevelSettings : MonoBehaviour
{
    public static string[] DOT_COLORS = { "red", "green", "blue", "purple", "yellow" };

    public BoardSettings boardSettings = new BoardSettings();
    public ColorSettings colorSettings = new ColorSettings();
}