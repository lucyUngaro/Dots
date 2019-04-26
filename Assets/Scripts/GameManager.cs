using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    LevelSettings levelSettings;
        
    private void Awake()
    {
        GameObject ls = GameObject.FindGameObjectWithTag("LevelSettings");
        levelSettings = ls.GetComponent<LevelSettings>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
