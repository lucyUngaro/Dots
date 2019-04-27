using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform dotPrefab; 

    BoardSettings boardSettings;
    GameGrid grid;
    Dot[] dots; 
        
    private void Awake()
    {
        LevelSettings levelSettings = GameObject.FindObjectOfType<LevelSettings>();
        boardSettings = levelSettings == null ? new BoardSettings() : levelSettings.boardSettings;
        grid = new GameGrid(boardSettings, dotPrefab);
    }

 
}
