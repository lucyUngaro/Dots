using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class manages all of the dots and their creation/destruction
/// </summary>
/// 
public class DotManager : MonoBehaviour
{
    public Transform dotPrefab;
    public LevelSettings levelSettings;
    private BoardSettings boardSettings;
    private ColorSettings colorSettings;
    private GameGrid grid;
    private Game dotsGame;
    private string[] colorValues;
    private bool mouseDown = false;
        
    private void Awake()
    {
        // These settings can be edited in the inspector
        boardSettings = levelSettings == null ? new BoardSettings() : levelSettings.boardSettings;
        colorSettings = levelSettings == null ? new ColorSettings() : levelSettings.colorSettings;
        dotsGame = new Game(this);
        grid = new GameGrid(boardSettings);

        CreateInitialTiles();
    }

    private void CreateInitialTiles()
    {
        for (int i = 0; i < boardSettings.numberOfRows * boardSettings.numberOfCols; i++)
        {
            // Convert the color specified for this dot in BoardSettings to a string, and then use that string to find the RGB value defined in ColorSettings 
            Color tileColor = boardSettings.initialTiles.Length > i ? GetDotColorFromString(boardSettings.initialTiles[i].ToString()) : SelectRandomDotColor();
            Dot newDot = CreateDot(tileColor);
            newDot.SetCell(grid.GetCell(i)); // Initial tiles move immediately to their cell without a tween
        }
    }

     public Dot CreateDot()
    {
        return CreateDot(SelectRandomDotColor());
    }

    public Dot CreateDot(Color color)
    {
        Dot newDot = Instantiate(dotPrefab, transform).gameObject.AddComponent<Dot>();
        newDot.SetParameters(this, color);

        return newDot;
    }

    public void AddSelectionTween(Dot dot)
    {
        DotSelectionTween tween = Instantiate(dotPrefab, transform).gameObject.AddComponent<DotSelectionTween>();
        tween.StartTween(dot);
    }

    public void OnDotMouseDown(Dot dot)
    {
        dotsGame.OnDotSelected(dot);
        mouseDown = true;
    }

    public void OnDotMouseEnter(Dot dot)
    {
        if (!mouseDown) return;
        dotsGame.OnDotSelected(dot);
    }

    public void OnDotMouseExit(Dot dot)
    {
        if (!mouseDown) return;
        dotsGame.CheckForDisconnected(dot);
    }

    void Update()
    {
        if (!mouseDown) return;

        if (Input.GetMouseButtonUp(0))
        {
            mouseDown = false;
            dotsGame.OnMouseUp();
        }
    }

    public void ClearMatch(List<ConnectedDot> selectedDots)
    {
        // Create key value pairs where the keys are columns of matched dots and the values are the largest matched row in each column
        // This way, in CompleteClearMatch(), instead of iterating through all the cells, iterate only through the relevant ones in the correct order
        Dictionary<int, int> sortedMatches = new Dictionary<int, int>();

        // Destroy all of the selected dots
        for (var i = 0; i < selectedDots.Count; i++) 
        {
            Dot clearedDot = selectedDots[i].dot;

            int currentMaxRow = sortedMatches.ContainsKey(clearedDot.col) ? sortedMatches[clearedDot.col] : 0;
            sortedMatches[clearedDot.col] = Mathf.Max(currentMaxRow, clearedDot.row);

            clearedDot.Destroy();
        }

        CompleteClearMatch(sortedMatches);
        
    }

    public void CompleteClearMatch(Dictionary<int, int> sortedMatches)
    {
        // sortedMatches contains the positions of matched dots.
        // keys: each column with a match
        // values: the largest rows in each column with a match
        // iterate backwards through the rows and drop a new tile for every empty cell
        foreach (KeyValuePair<int, int> entry in sortedMatches)
        {
            int col = entry.Key; 

            for (int row = entry.Value; row >= 0; row--)
            {
                Cell cell = grid.GetCell(row, col);

                if (cell.dot == null || cell.dot.markedForRemoval) // The dot has been removed, so find the next one in the column or create a new one
                {
                    Dot dot = GetNextDot(GetCellAtRowCol(cell.row - 1, col));

                    if (dot == null) // If no dot was found, create a new one
                    {
                        dot = CreateDot();
                    }

                    dot.SetCell(cell);
                }
            }
        }
         
    }

    public Dot GetNextDot(Cell cell)
    {
        if (cell == null) return null;
 
        if (cell.dot == null || cell.dot.markedForRemoval)
        {
            return GetNextDot(GetCellAtRowCol(cell.row - 1, cell.col));
        }

        return cell.dot;
    }

    public Cell GetCellAtRowCol(int row, int col)
    {
        return grid.GetCell(row, col);
    }

    public List<Dot> GetAllDotsOfColor(Color color)
    {
        List<Dot> dots = new List<Dot>();
        Cell[] cells = grid.GetCells();

        for (int i = 0; i < cells.Length; i++)
        {
            if (cells[i].dot == null) continue;

            if (cells[i].dot.GetColor() == color)
            {
                dots.Add(cells[i].dot);
            }
        }

        return dots;

    }

    public Color SelectRandomDotColor()
    {
        return GetDotColorFromString(LevelSettings.DOT_COLORS[Random.Range(0, LevelSettings.DOT_COLORS.Length)]);
    }

    // Convert a string to type Color
    private Color GetDotColorFromString(string colorName)
    {
        return (Color) colorSettings.GetType().GetField(colorName).GetValue(colorSettings);
    }
}
