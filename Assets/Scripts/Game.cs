using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Directions { none, right, left, up, down };

public struct ConnectedDot
{
    public Dot dot;
    public Directions direction; // Directions will be used to determine when a dot is unselected

    public ConnectedDot(Dot dot, Directions direction)
    {
        this.dot = dot;
        this.direction = direction;
    }
}

public class Game
{
    private ConnectedDot currentSelected;
    private DotManager manager;
    private List<ConnectedDot> connectedDots = new List<ConnectedDot>();
    private List<ConnectedDot> powerupDots = new List<ConnectedDot>();
    private LineSpawner lineSpawner;
    private bool squareActivated;

    public Game(DotManager manager)
    {
        this.manager = manager;
    }

    public void OnDotSelected(Dot dot)
    {
        if ((currentSelected.dot != null && !CheckForMatch(dot)) || squareActivated) return; // There's already a selected dot and the new one doesn't match, so return

        if (IsPreviousConnection(dot)) // If this is the same dot selected before the current dot, disconnect the current dot
        {
            DisconnectCurrentDot();
            return;
        }

        Directions newDirection = GetDirection(dot);
        currentSelected = new ConnectedDot(dot, newDirection);

        ConnectDot();
    }

    public void OnMouseUp()
    {
        if (connectedDots.Count > 1)
        {
            // Destroy the dots in the match
            List<ConnectedDot> allSelectedDots = new List<ConnectedDot>();
            allSelectedDots.AddRange(connectedDots);
            allSelectedDots.AddRange(powerupDots);
            manager.ClearMatch(allSelectedDots);
        }

        // Clear the array
        connectedDots.Clear();
        currentSelected.dot = null;
        lineSpawner.Remove();

        // Clear the powerup array
        DeactivateSquare();
    }

    public void CheckForDisconnected(Dot dot)
    {
        if (dot == currentSelected.dot && connectedDots.Count > 1)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float yDiff = mousePosition.y - dot.transform.position.y;
            float xDiff = mousePosition.x - dot.transform.position.x;

            Directions mouseDirection = Mathf.Abs(yDiff) > Mathf.Abs(xDiff) ? (yDiff < 0 ? Directions.down : Directions.up) : (xDiff < 0 ? Directions.left : Directions.right);

            // If the user is moving in the opposite direction from the previous direction, the latest dot has been disconnected
            if (CheckForOppositeDirections(currentSelected.direction, mouseDirection))
            {
                Dot previousDot = connectedDots[connectedDots.Count - 2].dot;
                float mouseDistance = Mathf.Max(Mathf.Abs(yDiff), Mathf.Abs(xDiff));
                float dotDistance = Mathf.Abs(Vector3.Distance(dot.transform.position, previousDot.transform.position));

                // If the mouse is less than halfway away from the current dot:
                if (mouseDistance < dotDistance / 4) return;

                DisconnectCurrentDot();
            }
        }

    }

    private void ActivateSquare(Color color)
    {
        squareActivated = true;
        List<Dot> dots = manager.GetAllDotsOfColor(color);

        for (var i = 0; i < dots.Count; i++)
        {
            Dot dot = dots[i];
            powerupDots.Add(new ConnectedDot(dot, Directions.none));
            SelectDot(dot);
        }

    }

    private void DeactivateSquare()
    {
        squareActivated = false;
        powerupDots.Clear();
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

    private void SelectDot(Dot dot)
    {
        // Update visuals
        manager.AddSelectionTween(dot);
    }

    private void ConnectDot()
    {
        // If the dot has already been connected, it's a square!
        if (AlreadyConnected(currentSelected.dot))
        {
            ActivateSquare(currentSelected.dot.GetColor());
        }

        connectedDots.Add(currentSelected);
        SelectDot(currentSelected.dot);

        if (lineSpawner == null)
        {
            lineSpawner = new GameObject("Line Spawner").AddComponent<LineSpawner>();
        }

        lineSpawner.ConnectDot(currentSelected.dot);
    }

    // Get the direction the line is currently traveling in
    private Directions GetDirection(Dot dot)
    {
        if (connectedDots.Count > 0)
        {
            var rowDistance = currentSelected.dot.row - dot.row;
            var colDistance = currentSelected.dot.col - dot.col;

            if (Mathf.Abs(rowDistance) == 1)
            {
                return rowDistance < 0 ? Directions.down : Directions.up;
            }
            else // Otherwise the column distance must be 1 unit
            {
                return colDistance < 0 ? Directions.right : Directions.left;
            }

        }

        // There are no other connected dots, so there isn't a direction yet
        return Directions.none;
    }

    private void DisconnectCurrentDot()
    {
        if (squareActivated)
        {
            DeactivateSquare();
        }

        connectedDots.Remove(currentSelected);
        currentSelected = connectedDots[connectedDots.Count - 1];
        lineSpawner.DisconnectDot();
    }

    private bool CheckForMatch(Dot dot)
    {
        return dot.GetColor() == currentSelected.dot.GetColor() && IsOneUnitAway(dot, currentSelected.dot);
    }

    private bool AlreadyConnected(Dot dot)
    {
        for (var i = 0; i < connectedDots.Count; i++)
        {
            if (connectedDots[i].dot == dot) return true;
        }

        return false;
    }

    private bool IsOneUnitAway(Dot d1, Dot d2)
    {
        return (Mathf.Abs(d1.row - d2.row) == 1 && d1.col == d2.col) || (Mathf.Abs(d1.col - d2.col) == 1 && d1.row == d2.row);
    }

    private bool IsPreviousConnection(Dot dot)
    {
        return connectedDots.Count >= 2 ? dot == connectedDots[connectedDots.Count - 2].dot : false;
    }

}
