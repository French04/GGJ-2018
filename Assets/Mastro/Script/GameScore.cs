using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScore : MonoBehaviour {

	[SerializeField] float gameTime;
	[SerializeField] float timeLimit;
	[SerializeField] int teamOneScore = 0;
	[SerializeField] int teamTwoScore = 0;
	[SerializeField] float scoreLimit;
	[SerializeField] GameObject scoreboard1;
	[SerializeField] GameObject scoreboard2;
	[SerializeField] GameObject timer;
	[SerializeField] Text uiText;
	[SerializeField] float interval;
	int textStage = 0;
    GameObject winTeam1;
    GameObject winTeam2;

	TextMesh scoreText1;
	TextMesh scoreText2;
	TextMesh timerText;
    PlayerController[] playerController = new PlayerController[4];

	void Start()
	{
        playerController = FindObjectsOfType<PlayerController>();
        winTeam1 = GameObject.Find("GameOver").transform.GetChild(0).gameObject;
        winTeam2 = GameObject.Find("GameOver").transform.GetChild(1).gameObject;
        winTeam1.SetActive(false);
        winTeam2.SetActive(false);

        scoreText1 = scoreboard1.GetComponent<TextMesh>();
		scoreText2 = scoreboard2.GetComponent<TextMesh>();
		timerText = timer.GetComponent<TextMesh>();
		UpdateScore(0);

		StartCoroutine(StartGame());
	}


	IEnumerator StartGame()
	{
		for (int i = 0; i < playerController.Length; i++)
		{
			playerController[i].canMove = false;
		}

		while (textStage < 4)
		{
			yield return new WaitForSeconds(interval);
			switch (textStage)
			{
				case 0:
					uiText.text = "Ready?";
					break;
				case 1:
					uiText.text = "Set";
					break;
				case 2:
					uiText.text = "Go!";
					break;
				case 3:
					uiText.text = "";
					break;
			}
			textStage++;
		}

		for (int i = 0; i < playerController.Length; i++)
		{
			playerController[i].canMove = true;
		}
	}



	public void UpdateScore(int team)
	{
        if(team == 1)
        {
            teamOneScore++;
        }
        else if(team == 2)
        {
            teamTwoScore++;
        }

		scoreText1.text = teamOneScore.ToString();
		scoreText2.text = teamTwoScore.ToString();
	}


	void Update()
	{
		gameTime -= Time.deltaTime;
		gameTime = Mathf.Max(gameTime, 0);
		timerText.text = gameTime.ToString("f0");

		if (gameTime == 0)
		{
			GameOver();
		}
	}

	void GameOver()
	{
		if(teamOneScore > teamTwoScore)
        {
            winTeam1.SetActive(true);
            for (int i = 0; i < playerController.Length; i++)
            {
                playerController[i].canMove = false;
            }
            
        }
        else
        {
            winTeam2.SetActive(true);
            for (int i = 0; i < playerController.Length; i++)
            {
                playerController[i].canMove = false;
            }
        }
	}
}
