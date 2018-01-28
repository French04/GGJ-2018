using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class GameScore : MonoBehaviour {

	[SerializeField] float gameTime;
	[SerializeField] float timeLimit;
	[SerializeField] int teamOneScore = 0;
	[SerializeField] int teamTwoScore = 0;
	[SerializeField] float scoreLimit;
	[SerializeField] GameObject scoreboard1;
	[SerializeField] GameObject scoreboard2;
	[SerializeField] GameObject timer;
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
