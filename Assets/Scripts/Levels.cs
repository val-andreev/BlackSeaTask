using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level
{

    public Cell[,] allCells;
    public Dictionary<Vector2,GameObject> allBoxes;
    public List<Vector2> allTargets;
    public Vector2 startingPositionOfPlayer;
    public List<string> rows = new List<string>();
    
    public int height { get { return rows.Count; } }
    public int Width
    {
        get
        {
            int maxLenght = 0;
            foreach (var item in rows)
            {
                if (item.Length > maxLenght)
                {
                    maxLenght = item.Length;
                }
            }
            return maxLenght;
        }
    }
}

    public class Levels : MonoBehaviour {
    public TextAsset textFile;
    public List<Level> levels = new List<Level>();

    private void Awake()
    {
        if (!textFile)
        {
            print("file not found");
        }
        string completeText = textFile.text;
        string[] lines;
        lines = completeText.Split(new string[] { "\n" }, System.StringSplitOptions.None);
        levels.Add(new Level());
        for (int i = 0; i < lines.LongLength; i++)
        {
            string line = lines[i];
            if (line.StartsWith(";"))
            {
                levels.Add(new Level());
                continue;
            }
            levels[levels.Count - 1].rows.Add(line);
        }
    }


}


