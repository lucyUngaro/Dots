using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotLines : MonoBehaviour
{
    private Line currentLine;

    public void ConnectDot(Dot dot)
    {
        if (currentLine == null || currentLine.endPoint)
        {
            Line newLine = new GameObject("Line").AddComponent<Line>();
            newLine.transform.SetParent(transform);
            newLine.previous = currentLine;
            currentLine = newLine;

            currentLine.StartLine(dot.transform, dot.GetColor());
        }
        else
        {
            currentLine.EndLine(dot.transform);
            ConnectDot(dot);
        }
    }

    public void DisconnectDot()
    {
        if (currentLine.endPoint == null)
        {
            Line newLine = currentLine.previous;
            Destroy(currentLine.gameObject);
            currentLine = newLine;
        }

        currentLine.endPoint = null;
    }

    public void Remove()
    {
        Destroy(gameObject);
    }


}
