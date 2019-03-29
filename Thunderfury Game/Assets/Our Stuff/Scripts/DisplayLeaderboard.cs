using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayLeaderboard : MonoBehaviour
 {
	[SerializeField] protected TextMeshProUGUI[] scoresText;
	protected Highscores highscoreManager;

	void Start() 
	{
		for (int i = 0; i < scoresText.Length; i++)
		{
			scoresText[i].text = i + 1 + ". Fetching...";
		}

		highscoreManager = GetComponent<Highscores>();

		//StartCoroutine(RefreshScoresRoutine());
	}

	public void OnScoresDownloaded(Highscore[] highscoreList)
	{
		for (int i = 0; i < scoresText.Length; i++)
		{
			scoresText[i].text = i + 1 + ". ";

			if (highscoreList.Length > i)
				scoresText[i].text += highscoreList[i].username + " - " + highscoreList[i].score;
		}
	}
	
	IEnumerator RefreshScoresRoutine()
	{
		while (true)
		{
			highscoreManager.DownloadScores();
			yield return new WaitForSeconds(10);
		}
	}
}
