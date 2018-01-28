using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    InputController inputController;
    public GameObject pausePanel;
    GameObject selector;
    Button[] pauseButton;

    bool moveSelector = false;
    [HideInInspector]
    public bool gameInPlay = false;
    [HideInInspector]
    bool selectionPressed = false;

    int selectorButton = 0;

    PlayerController[] playerController = new PlayerController[4];

    bool canPause = false;

    // Use this for initialization
    void Start()
    {
        Time.timeScale = 1;
        gameInPlay = false;
        inputController = GameObject.FindObjectOfType<InputController>();
        selector = transform.GetChild(0).GetChild(4).gameObject;

        pausePanel = transform.GetChild(0).gameObject;
        pauseButton = new Button[pausePanel.transform.childCount];

        for (int i = 1; i < pausePanel.transform.childCount; i++)
        {
            pauseButton[i - 1] = pausePanel.transform.GetChild(i).GetComponent<Button>();
        }

        //pausePanel.SetActive(false);

        playerController = FindObjectsOfType<PlayerController>();

        StartCoroutine(PauseEnabler());
    }

    // Update is called once per frame
    void Update ()
    {
        if (!gameInPlay)
        {
            //moveselelector reset
            if (inputController.getDirection().z == 0)
            {
                moveSelector = false;
            }

            //switch selector
            if (inputController.getDirection().z < 0 && selectorButton < 2 && !moveSelector)
            {
                moveSelector = true;
                selectorButton++;
            }
            else if (inputController.getDirection().z > 0 && selectorButton > 0 && !moveSelector)
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
            MoveSelectorIcon(pauseButton[selectorButton].transform);
        }
    }

    void MoveSelectorIcon(Transform obj)
    {
        selector.transform.position = obj.transform.position;
    }

    void Selections()
    {
        switch (selectorButton)
        {
            case 0:
                PauseSwitch();
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
        if (canPause) //bug fixing for start
        {
            if (!gameInPlay)
            {
                Debug.Log("Turn Off Pause menu");
                //Turn Off Pause Menu
                for (int i = 0; i < playerController.Length; i++)
                {
                    playerController[i].canMove = true;
                }
                pausePanel.SetActive(false);
                Time.timeScale = 1;
            }
            else
            {
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

    IEnumerator PauseEnabler()
    {
        yield return new WaitForSeconds(3);
        canPause = true;
    }
}
