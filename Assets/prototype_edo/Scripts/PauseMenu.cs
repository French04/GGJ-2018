using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    InputController inputController;
    GameObject pausePanel;
    GameObject selector;
    Button[] pauseButton;

    bool moveSelector = false;
    [HideInInspector]
    public bool gameInPlay = false;
    [HideInInspector]
    public bool pausePressed = false;
    bool selectionPressed = false;

    int selectorButton = 0;

    PlayerController[] playerController = new PlayerController[4];

    // Use this for initialization
    void Start()
    {
        Time.timeScale = 1;
        gameInPlay = false;
        pausePressed = false;
        inputController = GameObject.FindObjectOfType<InputController>();
        selector = transform.GetChild(0).GetChild(4).gameObject;

        pausePanel = transform.GetChild(0).gameObject;
        pauseButton = new Button[pausePanel.transform.childCount];

        for (int i = 1; i < pausePanel.transform.childCount; i++)
        {
            pauseButton[i - 1] = pausePanel.transform.GetChild(i).GetComponent<Button>();
        }

        pausePanel.SetActive(false);

        playerController = FindObjectsOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update ()
    {
        //reset pause button
        if (!inputController.isPause())
        {
            pausePressed = false;
        }

        //moveselelector reset
        if (inputController.getDirection().z == 0)
        {
            moveSelector = false;
        }

        //switch selector
        if (inputController.getDirection().z < 0 && selectorButton < 2 && !moveSelector && !gameInPlay)
        {
            moveSelector = true;
            selectorButton++;
        }
        else if (inputController.getDirection().z > 0 && selectorButton > 0 && !moveSelector && !gameInPlay)
        {
            moveSelector = true;
            selectorButton--;
        }

        if (inputController.isFiring() && !selectionPressed)
        {
            selectionPressed = true;
            Selections();
        }

        if (!inputController.isFiring())
        {
            selectionPressed = false;
        }

        //move selector icon
        if(!gameInPlay)
            MoveSelectorIcon(pauseButton[selectorButton].transform);
    }

    void MoveSelectorIcon(Transform obj)
    {
        selector.transform.position = obj.transform.position;
    }

    void Resume()
    {
        Time.timeScale = 1;
        gameInPlay = false;
        pausePanel.SetActive(false);
    }

    void Selections()
    {
        switch (selectorButton)
        {
            case 0:
                Resume();
                break;
            case 1:
                Time.timeScale = 1;
                EditorSceneManager.LoadScene(1);
                break;
            case 2:
                Time.timeScale = 1;
                EditorSceneManager.LoadScene(0);
                break;
        }
    }

    bool lastPauseButtonState;
    bool currentPauseButtonState;


    
    public void PauseSwitch()
    {
        if (!gameInPlay)
        {
            Debug.Log("Turn Off Pause menu");
            //Turn Off Pause Menu
            for (int i = 0; i < playerController.Length; i++)
            {
                playerController[i].canMove = true;
            }
            Resume();
        }
            
        else {
            Debug.Log("Turn On Pause menu");
            //Turn On Pause menu
            for (int i = 0; i < playerController.Length; i++)
            {
                playerController[i].canMove = false;
            }
            pausePanel.SetActive(true);
            Time.timeScale = 0;

        }
        gameInPlay = !gameInPlay;
        
    }
}
