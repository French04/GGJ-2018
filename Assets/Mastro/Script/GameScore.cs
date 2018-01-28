using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScore : MonoBehaviour {

	public AudioSource audioSource;
	[SerializeField] AudioClip[] audioClips;

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
	[SerializeField] float highightSize;
	int textStage = 0;
    GameObject winTeam1;
    GameObject winTeam2;
    GameObject draw;

    TextMesh scoreText1;
	TextMesh scoreText2;
	TextMesh timerText;
    PlayerController[] playerController = new PlayerController[4];

    [HideInInspector]
    public bool gameOver = false;

	void Start()
	{
        playerController = FindObjectsOfType<PlayerController>();
		//audioSource = GetComponent<AudioSource>();
        winTeam1 = GameObject.Find("GameOver").transform.GetChild(0).gameObject;
        winTeam2 = GameObject.Find("GameOver").transform.GetChild(1).gameObject;
        draw = GameObject.Find("GameOver").transform.GetChild(2).gameObject;
        winTeam1.SetActive(false);
        winTeam2.SetActive(false);
        draw.SetActive(false);

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
					audioSource.PlayOneShot(audioClips[0]);
					break;
				case 1:
					uiText.text = "Set";
					audioSource.PlayOneShot(audioClips[0]);
					break;
				case 2:
					uiText.text = "Go!";
					audioSource.PlayOneShot(audioClips[1]);
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
			scoreText1.characterSize = highightSize;
        }
        else if(team == 2)
        {
            teamTwoScore++;
			scoreText2.characterSize = highightSize;
        }

		scoreText1.text = teamOneScore.ToString();
		scoreText2.text = teamTwoScore.ToString();
		audioSource.PlayOneShot(audioClips[2]);
	}


	void Update()
	{
		if(textStage == 4)
			gameTime -= Time.deltaTime;
		gameTime = Mathf.Max(gameTime, 0);
		timerText.text = gameTime.ToString("f0");

		if (gameTime == 0)
		{
			GameOver();
		}

		if (scoreText1.characterSize > 1)
			scoreText1.characterSize -= (highightSize - 1) / 10;
		if (scoreText2.characterSize > 1)
			scoreText2.characterSize -= (highightSize - 1) / 10;
	}

	void GameOver()
	{
        gameOver = true;

        if (teamOneScore > teamTwoScore)
        {
            winTeam1.SetActive(true);
            for (int i = 0; i < playerController.Length; i++)
            {
                playerController[i].canMove = false;
            }
            
        }
        else if(teamOneScore < teamTwoScore)
        {
            winTeam2.SetActive(true);
            for (int i = 0; i < playerController.Length; i++)
            {
                playerController[i].canMove = false;
            }
        }
        else if(teamOneScore == teamTwoScore)
        {
            draw.SetActive(true);
            for (int i = 0; i < playerController.Length; i++)
            {
                playerController[i].canMove = false;
            }
        }
	}
}
