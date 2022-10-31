using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class LevelBuilder : MonoBehaviour
{
    public int currentLevel;
    public Camera mainCamera;
    public TextMeshProUGUI playerTextElement;
    public TextMeshProUGUI boxTextElement;

    private Level level;
   
    
    public void NextLevel()
    {
        currentLevel++;
        if (currentLevel >= GetComponent<Levels>().levels.Count)
        {
            currentLevel = 0;
        }
    }

    public void Build()
    {
        level = GetComponent<Levels>().levels[currentLevel];
        int numberOfPlayers = 0;
        var width = level.Width;
        var height = level.height;
        int x = 0;
        int y = 0;
        int totalCount = 0;
        level.allCells = new Cell[width, height];
        level.allBoxes = new Dictionary<Vector2, GameObject>();
        level.allTargets = new List<Vector2>();
        for (int rowNumber = level.rows.Count - 1; rowNumber >= 0; rowNumber--)
        {
            foreach (var c in level.rows[rowNumber])
            {
                var prefab = MatchCharacterToPrefab(c);
                var prefabObj = Instantiate(prefab, new Vector2(x, y), Quaternion.identity);
                Cell newCell = new Cell();

                if (prefab.name.StartsWith("target"))
                {
                    newCell.type = typeOfStructure.Target;
                    level.allTargets.Add(new Vector2(x, y));
                }
                else if (prefab.name.StartsWith("TrgBox"))
                {
                    newCell.type = typeOfStructure.Target;

                    var prefab2 = Resources.Load<GameObject>("Prefabs/target");
                    Instantiate(prefab2, new Vector2(x, y), Quaternion.identity);
                    level.allBoxes.Add(new Vector2(x, y), prefabObj);
                    level.allTargets.Add(new Vector2(x, y));
                    prefabObj.GetComponent<Box>().SetBoxOnTargetWhenLaunching();
                }
                else if (prefab.name.StartsWith("TrgPlayer"))
                {
                    newCell.type = typeOfStructure.Space;
                    level.startingPositionOfPlayer = new Vector2(x, y);
                    var prefab2 = Resources.Load<GameObject>("Prefabs/target");
                    Instantiate(prefab2, new Vector2(x, y), Quaternion.identity);
                    level.allTargets.Add(new Vector2(x, y));
                    numberOfPlayers++;

                }
                else if (prefab.name.StartsWith("wall"))
                {
                    newCell.type = typeOfStructure.Wall;
                    var prefab2 = Resources.Load<GameObject>("Prefabs/ground");
                    Instantiate(prefab2, new Vector2(x, y), Quaternion.identity);
                }
                else if (prefab.name.StartsWith("player"))
                {
                    newCell.type = typeOfStructure.Space;
                    level.startingPositionOfPlayer = new Vector2(x, y);
                    var prefab2 = Resources.Load<GameObject>("Prefabs/ground");
                    Instantiate(prefab2, new Vector2(x, y), Quaternion.identity);
                    numberOfPlayers++;
                }
                else if (prefab.name.StartsWith("box"))
                {
                    newCell.type = typeOfStructure.Space;

                    var prefab2 = Resources.Load<GameObject>("Prefabs/ground");
                    Instantiate(prefab2, new Vector2(x, y), Quaternion.identity);
                    level.allBoxes.Add(new Vector2(x, y), prefabObj);
                }
                else
                {
                    newCell.type = typeOfStructure.Space;
                    var prefab2 = Resources.Load<GameObject>("Prefabs/ground");
                    Instantiate(prefab2, new Vector2(x, y), Quaternion.identity);
                }

                newCell.position = new Vector2(x, y);
                level.allCells[x, y] = newCell;
                x++;
                totalCount++;
            }
            y++;
            x = 0;
        }
        if (numberOfPlayers > 1)
        {
            
            playerTextElement.text = new string("");
            playerTextElement.gameObject.SetActive(true);
            playerTextElement.text = new string("Not allowed to have more than 1 player per level, please change your text file!");
        }
        if (level.allBoxes.Count< level.allTargets.Count || level.allBoxes.Count < 1 || level.allTargets.Count< 1)
        {
            boxTextElement.text = new string("");
            boxTextElement.gameObject.SetActive(true);
            boxTextElement.text = new string("Should have atleast 1 box, atleast 1 target and boxes>=targets, please change your text file!");

        }
        if (true)
        {

        }
        mainCamera.transform.position = new Vector3(width / 2, height / 2,-10);
       


    }
    //TrgBox is a box initially placed on a target
    //TrgPlayer is the same but or a player
    public GameObject MatchCharacterToPrefab(char c)
    {
        if (c == '#')
        {
            return Resources.Load<GameObject>("Prefabs/wall");
        }
        else if (c == '@')
        {
            var player = Resources.Load<GameObject>("Prefabs/player");
            return player;
        }
        else if (c == '$')
        {
            return Resources.Load<GameObject>("Prefabs/box");
        }
        else if (c == '.')
        {
            return Resources.Load<GameObject>("Prefabs/target");
        }
        else if (c == ' ')
        {
            return Resources.Load<GameObject>("Prefabs/ground");
        }
        else if (c == '^')
        {
            return Resources.Load<GameObject>("Prefabs/TrgBox");

        }
        else if (c == '&')
        {
            return Resources.Load<GameObject>("Prefabs/TrgPlayer");
        }
        else
        {
            return null;
        }
    }


   

}


