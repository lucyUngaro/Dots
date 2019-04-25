using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level
{

    public char[] Rows;
}

public class DotGrid : MonoBehaviour
{
    public int Rows = 4;
    public int Cols = 4;
    public Transform dotPrefab; 
   
    // Start is called before the first frame update
    void Start()
    {
        Transform dot = Instantiate(dotPrefab);
        var size = dot.GetComponent<Renderer>().bounds.size;
        Debug.Log(dot.localScale + " " + dot.lossyScale);
        dot.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * dot.localScale.x * size.x, Screen.height / 2, 10));
    }

}
