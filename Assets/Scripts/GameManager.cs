using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform dotHolder;
    public Transform dotPrefab;
    public LevelSettings levelSettings;
    private BoardSettings boardSettings;
    private ColorSettings colorSettings;
    private GameGrid grid;
    private DotsGame dotsGame;
    private Dot[] dots;
    private string[] colorValues;
    private bool currentlySelected = false;
        
    private void Awake()
    {
        dotHolder = new GameObject("Dot Holder").transform;

        boardSettings = levelSettings == null ? new BoardSettings() : levelSettings.boardSettings;
        colorSettings = levelSettings == null ? new ColorSettings() : levelSettings.colorSettings;
        dotsGame = new DotsGame(this);
        dots = new Dot[boardSettings.numberOfRows * boardSettings.numberOfCols];
        grid = new GameGrid(boardSettings);

        CreateInitialTiles();
    }

    private void CreateInitialTiles()
    {
        for (int i = 0; i < boardSettings.numberOfRows * boardSettings.numberOfCols; i++)
        {
            // Convert the color specified for this dot in BoardSettings to a string, and then use that string to find the RGB value defined in ColorSettings 
            string colorName = boardSettings.initialTiles.Length > i ? boardSettings.initialTiles[i].ToString() : SelectRandomDotColor() ;
            Color tileColor = (Color) colorSettings.GetType().GetField(colorName).GetValue(colorSettings);

            dots[i] = CreateTile(grid.GetCell(i), tileColor);
   
        }
    }

    public string SelectRandomDotColor()
    {
        return LevelSettings.DOT_COLORS[Random.Range(0, LevelSettings.DOT_COLORS.Length)];
    }

    public Dot CreateTile(Cell cell, Color color)
    {
        Dot newDot = Instantiate(dotPrefab, dotHolder).GetComponent<Dot>();
        newDot.SetParameters(this, cell, color);

        return newDot;
    }

    public void OnDotMouseDown(Dot dot)
    {
        dotsGame.OnDotSelected(dot);
        currentlySelected = true;
    }

    public void OnDotMouseEnter(Dot dot)
    {
        if (!currentlySelected) return;
        dotsGame.OnDotSelected(dot);
    }

    public void OnDotMouseExit(Dot dot)
    {
        if (!currentlySelected) return;
        dotsGame.CheckForUnselected(dot);
    }

    void Update()
    {
        if (!currentlySelected) return;

        if (Input.GetMouseButtonUp(0))
        {
            currentlySelected = false;
            dotsGame.OnMouseUp();
        }
    }


}
