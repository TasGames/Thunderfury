using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOver : MonoBehaviour 
{
	[SerializeField] protected TMP_InputField inputName;
	[SerializeField] protected TextMeshProUGUI creditsText;
	[SerializeField] protected GameObject leaderboard;
	[SerializeField] protected GameObject gameOver;

	[SerializeField] protected Highscores highscores;

	protected string playerName;
	protected int credits;

	void OnEnable()
	{
		credits = HUD.totalScore;

		creditsText.text = "Credits: ¥" + credits;

	}

	public void Submit()
	{
		LeaderboardRoutine();
	}

	void LeaderboardRoutine()
	{
		Time.timeScale = 1f;

		playerName = inputName.text;
		highscores.AddNewScore(playerName, credits);

		gameOver.SetActive(false);
		leaderboard.SetActive(true);

		//yield return new WaitForSeconds(2f);

		Debug.Log("First");

		Time.timeScale = 0f;

		Debug.Log("Second");

		highscores.DownloadScores();

		Debug.Log("Third");
	}
}
