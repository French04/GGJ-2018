using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    int selectorButton = 0;
    int selectorPanel = 0;
    GameObject mainPanel;
    //GameObject controlsPanel;
    //GameObject creditsPanel;
    GameObject exitPanel;

    Button[] main;
    Button controls;
    Button credits;
    Button[] exit;

	// Use this for initialization
	void Start ()
    {
        mainPanel = transform.GetChild(0).gameObject;
        for (int i = 0; i < mainPanel.transform.childCount; i++)
        {
            if (i == 0)
                continue;

            main[i] = mainPanel.transform.GetChild(i).GetComponent<Button>();
        }

        controls = transform.GetChild(1).GetChild(1).GetComponent<Button>();

        credits = transform.GetChild(2).GetChild(1).GetComponent<Button>();

        exitPanel = transform.GetChild(3).gameObject;
        for (int i = 0; i < exitPanel.transform.childCount; i++)
        {
            if (i == 0)
                continue;

            exit[i] = exitPanel.transform.GetChild(i).GetComponent<Button>();
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
