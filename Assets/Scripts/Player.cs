using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour {
    public GameManager gamemanager;
    private GameObject undoButton;
    private GameObject parentOfLevels;
    private Levels levels;
    private LevelBuilder levelBuilder;
    private Dictionary<Vector2, GameObject> allBoxes;
    private List<Vector2> allTargets;
    private Cell[,] allCells;
    private Vector2 relativeCellPosition;
    private Vector2 lastRelativeCellPosition;
    public PlayerControls playerControls;
    private bool firstTimeToLoad = true;
    private Vector2 boxPositionToErase;
    private Vector2 boxPositionToEnterBack;
    private Vector2 playerPositionToRevertTo;
    private GameObject lastMovedBox;
    private bool lastMovePushedBox;
    private bool firstMove = true;


    public void Awake()
    {

        parentOfLevels = GameObject.Find("Level");
        gamemanager = GameObject.Find("GameManager").GetComponent<GameManager>();
        levels = parentOfLevels.GetComponent<Levels>();
        levelBuilder = parentOfLevels.GetComponent<LevelBuilder>();
        allCells = levels.levels[levelBuilder.currentLevel].allCells;
        allTargets = levels.levels[levelBuilder.currentLevel].allTargets ;
        allBoxes = levels.levels[levelBuilder.currentLevel].allBoxes;
        playerControls = new PlayerControls();
        playerControls.Player.Move.performed += ctx => Move(ctx.ReadValue<Vector2>());
        relativeCellPosition = levels.levels[levelBuilder.currentLevel].startingPositionOfPlayer;
        undoButton = gamemanager.UndoButton;
    }
  
    public bool Move(Vector2 direction) {
        var Direction2 = PreventDiagonal(direction);
        if (Direction2!=Vector2.zero)
        {
            //if (Direction2.x!=0)
            //{
            //    Direction2.y = 0;
            //}
           
            if (CellOccupied(Direction2))
            {
                return false;
            }
            else
            {
                if (firstMove)
                {
                    undoButton.SetActive(true);
                    firstMove = false;
                }
                playerPositionToRevertTo = transform.position;
                transform.Translate(Direction2);
                float x = relativeCellPosition.x;
                float y = relativeCellPosition.y;
                Vector2 newVector = new Vector2(x + Direction2.x, y + Direction2.y);
                lastRelativeCellPosition = relativeCellPosition;
                relativeCellPosition = newVector;
                return true;
            }
        }
        return false;
        
    }

    private Vector2 PreventDiagonal(Vector2 direction)
    {
        if ((direction.x != 0 && direction.x != 1 && direction.x != -1) || (direction.y != 0 && direction.y != 1 && direction.y != -1) || (direction.x == direction.y))
        {
        if (direction.x >0.5 || direction.x < -0.5)
        {
            return new Vector2(Convert.ToInt32(direction.x), 0);
        }
        else
        {
            return new Vector2(0, Convert.ToInt32(direction.y));
        }
        }
        else
        {
            return direction;
        }
    }
    private void OnEnable()
    {
        playerControls.Enable();
    }
    private void OnDisable()
    {
        playerControls.Disable();
    }
    bool CellOccupied(Vector2 direction) {
        while (firstTimeToLoad)
        {
            relativeCellPosition = levels.levels[levelBuilder.currentLevel].startingPositionOfPlayer;
            if (relativeCellPosition != Vector2.zero)
            {
                print(relativeCellPosition);
            firstTimeToLoad = false;
            }
        }

        Vector2 newPos = new Vector2(relativeCellPosition.x + direction.x, relativeCellPosition.y + direction.y);
        var cellToCheck = allCells[(int)newPos.x, (int)newPos.y];
        if (cellToCheck.type== typeOfStructure.Wall)
        {
            return true;
        }
        //else
        //{
        //    print(cellToCheck.type + " " + relativeCellPosition.ToString());
        //}

        if (allBoxes.ContainsKey(newPos))
        {
            var box = allBoxes[newPos];
            if (OccupiedForBox(newPos, direction))
            {
                return true;
            }
            else
            {
                MoveBox(box,direction);
                allBoxes.Remove(newPos);
                var newBoxPosition = new Vector2(newPos.x + direction.x, newPos.y + direction.y);
                allBoxes.Add(newBoxPosition, box);
                box.GetComponent<Box>().CheckIfBoxOnTargetAndChangeColour(allTargets);
                gamemanager.IsLevelComplete();
                return false;
            }
            
        }
        else
        {
            lastMovePushedBox = false;
        }

        return false;
    }

    bool OccupiedForBox(Vector2 position, Vector2 direction)
    {
       
        Vector2 newPos = new Vector2(position.x + direction.x, position.y + direction.y);
        var cellToCheck = allCells[(int)newPos.x, (int)newPos.y];
        if (allBoxes.ContainsKey(newPos) || cellToCheck.type == typeOfStructure.Wall )
        {
            return true;
        }
        return false;
    }
    private void MoveBox(GameObject box,Vector2 direction)
    {
        boxPositionToEnterBack = box.transform.position;
        box.transform.Translate(direction);
        boxPositionToErase = box.transform.position;
        lastMovedBox = box;
        lastMovePushedBox = true;
    }




   public void UndoMove()
    {
        if (lastMovePushedBox)
        {
            var box = lastMovedBox;
            allBoxes.Remove(boxPositionToErase);
            allBoxes.Add(boxPositionToEnterBack,box);
            lastMovedBox.transform.position = boxPositionToEnterBack;
            box.GetComponent<Box>().CheckIfBoxOnTargetAndChangeColour(allTargets);

        }
        transform.position = playerPositionToRevertTo;
        relativeCellPosition = lastRelativeCellPosition;
    }

}

