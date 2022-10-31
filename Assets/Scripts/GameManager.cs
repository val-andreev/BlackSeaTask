using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public LevelBuilder levelBuilder;
    public Levels levels;
    public GameObject nextButton;
    public GameObject UndoButton;
    public TextMeshProUGUI playerTextElement;
    public TextMeshProUGUI boxTextElement;
    //public InputAction playerControls;
    Dictionary<Vector2, GameObject> allBoxes;
    int targetsToBeReached;
    void Start()
    {
        nextButton.SetActive(false);
        ResetScene();
    }
   
    private void Awake()
    {
        nextButton.SetActive(false);
    }
    void Update()
    {
        

    }

    
    private void FixedUpdate()
    {
      
    }

    public void NextLevel()
    {
        nextButton.SetActive(false);
        levelBuilder.NextLevel();
        StartCoroutine(ResetSceneAsync());
    }

    private void ResetScene()
    {
        StartCoroutine(ResetSceneAsync());
    }

    public void IsLevelComplete()
    {
        if (allBoxes == null)
        {
            allBoxes = levels.levels[levelBuilder.currentLevel].allBoxes;
        }
        if (targetsToBeReached <=0)
        {
            targetsToBeReached = levels.levels[levelBuilder.currentLevel].allTargets.Count;
        }
        //print("targets to be reached" + targetsToBeReached);
        int targetsReached = 0;
       
        foreach (var box in allBoxes)
        {
            if (box.Value.GetComponent<Box>().boxOnTarget)
            {
                targetsReached++;
            }
        }
        if (targetsReached >= targetsToBeReached)
            {
            nextButton.SetActive(true);
        }
        else
        {
            nextButton.SetActive(false);
        }
    }

    IEnumerator ResetSceneAsync()
    {
        allBoxes = null;
        targetsToBeReached = -1;
        UndoButton.SetActive(false);
        boxTextElement.gameObject.SetActive(false);
        playerTextElement.gameObject.SetActive(false);
        if (SceneManager.sceneCount > 1)
        {
            AsyncOperation async = SceneManager.UnloadSceneAsync("LevelScene");
            while (!async.isDone)
            {
                yield return null;
                Debug.Log("Unloading");
            }
            Debug.Log("Unloading done.");
            Resources.UnloadUnusedAssets();
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("LevelScene", LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            yield return null;
            Debug.Log("Loading");
        }
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("LevelScene"));
        levelBuilder.Build();
        Debug.Log("Level loaded.");
    }

    public void Undo()
    {
        var playerObject = GameObject.FindGameObjectWithTag("Player");
        Player playerScript;
        if (playerObject!=null)
        {
            playerScript = playerObject.GetComponent<Player>();
            playerScript.UndoMove();
        }

    }
}
