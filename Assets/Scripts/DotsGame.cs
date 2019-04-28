using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Directions { none, right, left, up, down };

public struct SelectedDot
{
    public Dot dot;
    public Directions direction;

    public SelectedDot(Dot dot, Directions direction)
    {
        this.dot = dot;
        this.direction = direction;
    }
}

public class DotsGame
{
    private SelectedDot currentSelected;
    private GameManager manager;
    private List<SelectedDot> selectedDots = new List<SelectedDot>();
    private DotLines dotLines;

    public DotsGame(GameManager manager)
    {
        this.manager = manager;
    }

    public void OnDotSelected(Dot dot)
    {
        if (currentSelected.dot != null && !CheckForMatch(dot)) return; // There's already a selected dot and the new one doesn't match, so return

        Directions newDirection = GetDirection(dot);
        currentSelected = new SelectedDot(dot, newDirection);
        SelectDot();
    }

    public void OnMouseUp()
    {
        if (selectedDots.Count > 1)
        {
            // Destroy the dots in the match
        }

        selectedDots.Clear();
        currentSelected.dot = null;
        dotLines.Remove();
    }

    public void CheckForUnselected(Dot dot)
    {
        if (dot == currentSelected.dot)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float yDiff = mousePosition.y - dot.transform.position.y;
            float xDiff = mousePosition.x - dot.transform.position.x;

            Directions mouseDirection = Mathf.Abs(yDiff) > Mathf.Abs(xDiff) ? (yDiff < 0 ? Directions.down : Directions.up) : (xDiff < 0 ? Directions.left : Directions.right);

            if (CheckForOppositeDirections(currentSelected.direction, mouseDirection))
            {
                UnselectCurrentDot();
            }
        }

    }

    private bool CheckForOppositeDirections(Directions d1, Directions d2)
    {
        switch (d1)
        {
            case Directions.up:
                return d2 == Directions.down;
            case Directions.down:
                return d2 == Directions.up;
            case Directions.left:
                return d2 == Directions.right;
            case Directions.right:
                return d2 == Directions.left;
        }

        return false;
    }

    private void SelectDot()
    {
        selectedDots.Add(currentSelected);

        if (dotLines == null)
        {
            dotLines = new GameObject("Line Holder").AddComponent<DotLines>();
        }

        dotLines.ConnectDot(currentSelected.dot);
    }

    private Directions GetDirection(Dot dot)
    {
        if (selectedDots.Count > 0)
        {
            var rowDistance = currentSelected.dot.row - dot.row;

            if (Mathf.Abs(rowDistance) == 1)
            {
                return rowDistance < 0 ? Directions.down : Directions.up;
            }

            var colDistance = currentSelected.dot.col - dot.col;

            if (Mathf.Abs(colDistance) == 1)
            {
                return colDistance < 0 ? Directions.right : Directions.left;
            }

        }

        return Directions.none;
    }

    private void UnselectCurrentDot()
    {
        selectedDots.Remove(currentSelected);
        currentSelected = selectedDots[selectedDots.Count - 1];
        dotLines.DisconnectDot();
    }

    private bool CheckForMatch(Dot dot)
    {
        return dot.GetColor() == currentSelected.dot.GetColor() && IsOneUnitAway(dot, currentSelected.dot) && !IsAlreadySelected(dot);
    }

    private bool IsAlreadySelected(Dot dot)
    {
        for (var i = 0; i < selectedDots.Count; i++)
        {
            if (selectedDots[i].dot == dot) return true;
        }

        return false;
    }

    private bool IsOneUnitAway(Dot d1, Dot d2)
    {
        return (Mathf.Abs(d1.row - d2.row) == 1 && d1.col == d2.col) || (Mathf.Abs(d1.col - d2.col) == 1 && d1.row == d2.row);
    }


}
