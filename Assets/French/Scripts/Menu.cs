using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.SceneManagement;

public class Menu : MonoBehaviour
{
    int selectorButton = 0;
    int selectorPanel = 0;
    GameObject mainPanel;
    GameObject controlsPanel;
    GameObject creditsPanel;
    GameObject exitPanel;
    GameObject selector;

    Button[] main;
    Button controls;
    Button credits;
    Button[] exit;

    InputController inputController;

    bool moveSelector = false;
    bool selectionPressed = false;

    // Use this for initialization
    void Start ()
    {
        inputController = transform.GetComponent<InputController>();

        mainPanel = transform.GetChild(0).gameObject;
        controlsPanel = transform.GetChild(1).gameObject;
        creditsPanel = transform.GetChild(2).gameObject;
        exitPanel = transform.GetChild(3).gameObject;
        selector = transform.GetChild(4).gameObject;

        main = new Button[mainPanel.transform.childCount - 1];
        exit = new Button[exitPanel.transform.childCount - 1];

        for (int i = 1; i < mainPanel.transform.childCount; i++)
        {
            main[i-1] = mainPanel.transform.GetChild(i).GetComponent<Button>();
        }

        controls = transform.GetChild(1).GetChild(1).GetComponent<Button>();

        credits = transform.GetChild(2).GetChild(1).GetComponent<Button>();

        for (int i = 1; i < exitPanel.transform.childCount; i++)
        {
            exit[i-1] = exitPanel.transform.GetChild(i).GetComponent<Button>();
        }

        controlsPanel.SetActive(false);
        creditsPanel.SetActive(false);
        exitPanel.SetActive(false);

        selector.transform.position = main[0].transform.position;
    }
	
	// Update is called once per frame
	void Update ()
    {
        Debug.Log(inputController.isFiring());

        if (inputController.getDirection().z == 0)
        {
            moveSelector = false;
        }

        if(inputController.getDirection().z != 0)
        {
            int max = 0;
            Button[] array = new Button[0];
            switch (selectorPanel)
            {
                case 0:
                    max = main.Length;
                    array = main;
                    break;
                case 1:
                    max = 0;
                    break;
                case 2:
                    max = 0;
                    break;
                case 3:
                    max = exit.Length;
                    array = exit;
                    break;
            }

            ChangeSelector(max, array);
        }

        switch (selectorPanel)
        {
            case 0:
                mainPanel.SetActive(true);
                controlsPanel.SetActive(false);
                creditsPanel.SetActive(false);
                exitPanel.SetActive(false);
                break;

            case 1:
                mainPanel.SetActive(false);
                controlsPanel.SetActive(true);
                creditsPanel.SetActive(false);
                exitPanel.SetActive(false);
                break;

            case 2:
                mainPanel.SetActive(false);
                controlsPanel.SetActive(false);
                creditsPanel.SetActive(true);
                exitPanel.SetActive(false);
                break;

            case 3:
                mainPanel.SetActive(false);
                controlsPanel.SetActive(false);
                creditsPanel.SetActive(false);
                exitPanel.SetActive(true);
                break;
        }

        if(inputController.isFiring() && !selectionPressed)
        {
            selectionPressed = true;
            Selections();
        }

        if (!inputController.isFiring())
        {
            selectionPressed = false;
        }
    }

    void ChangeSelector(int max, Button[] arrayButton)
    {
        if (inputController.getDirection().z < 0 && selectorButton < max && !moveSelector)
        {
            moveSelector = true;
            selectorButton++;            
        }
        else if (inputController.getDirection().z > 0 && selectorButton > 0 && !moveSelector)
        {
            moveSelector = true;
            selectorButton--;
        }

        MoveSelectorIcon(arrayButton[selectorButton].transform);
    }

    void MoveSelectorIcon(Transform obj)
    {
        selector.transform.position = obj.transform.position;
    }

    //Here there is the button functions
    void Selections()
    {
        if(selectorPanel == 0) //if i am in main menu
        {
            switch (selectorButton)
            {
                case 0:
                    EditorSceneManager.LoadScene(1);
                    break;
                case 1:
                    selectorPanel = 1;
                    MoveSelectorIcon(controls.transform);
                    break;
                case 2:
                    selectorPanel = 2;
                    MoveSelectorIcon(credits.transform);
                    break;
                case 3:
                    selectorPanel = 3;
                    MoveSelectorIcon(exit[0].transform);
                    selectorButton = 0;
                    break;
            }
        }
        else if(selectorPanel == 1) //if i am in controls
        {
            selectorPanel = 0;
            MoveSelectorIcon(main[selectorButton].transform);
        }
        else if (selectorPanel == 2) //if i am in credits
        {
            selectorPanel = 0;
            MoveSelectorIcon(main[selectorButton].transform);
        }
        else if (selectorPanel == 3) //if i am in exit
        {
            switch (selectorButton)
            {
                case 0:
                    Application.Quit();
                    break;
                case 1:
                    selectorPanel = 0;
                    selectorButton = 0;
                    MoveSelectorIcon(main[selectorButton].transform);
                    break;
            }
        }
    }
}
