using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineSpawner : MonoBehaviour
{
    private Line currentLine;

    public void ConnectDot(Dot dot)
    {
        if (currentLine == null || currentLine.endPoint) // If no line has been created yet, or the current line has ended, create a new line
        {
            Line newLine = new GameObject("Line").AddComponent<Line>();
            newLine.transform.SetParent(transform);
            newLine.previous = currentLine; // This will be null if it's the first line spawned
            currentLine = newLine;

            currentLine.StartLine(dot.transform, dot.GetColor());
        }
        else
        {
            currentLine.EndLine(dot.transform); // The line is now connected to two dots and will not follow the mouse
            ConnectDot(dot); // As soon as a line ends, create a new line at the same dot
        }
    }

    public void DisconnectDot()
    {   
        // If we are disconnecting the current line's start point, we should destroy the current line and go back to the previous
        if (currentLine.endPoint == null)
        {
            Line newLine = currentLine.previous;
            Destroy(currentLine.gameObject);
            currentLine = newLine;
        }

        currentLine.endPoint = null; // Follow the mouse point
    }

    public void Remove()
    {
        // On mouse up, destroy all lines:
        Destroy(gameObject);
    }


}
