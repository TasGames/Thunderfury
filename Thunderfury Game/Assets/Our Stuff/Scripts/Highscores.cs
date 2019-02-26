using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highscores : MonoBehaviour 
{
	const string privateCode = "S4xeuB4kBEqQ3kkz5KU6nA9JpafGHHkUyjCy0PLxMzCw";
	const string publicCode = "5c7525f83eba35041ca1d7ce";
	const string webURL = "http://dreamlo.com/lb/";

	public Highscore[] highscoresList;
	protected DisplayLeaderboard leaderboardDisplay;

	void Awake()
	{
		leaderboardDisplay = GetComponent<DisplayLeaderboard>();
	}

	public void AddNewScore(string username, int score)
	{
		StartCoroutine(UploadNewScoreRoutine(username, score));
	}

	public void DownloadScores()
	{
		StartCoroutine(DownloadScoresRoutine());
	}

	IEnumerator UploadNewScoreRoutine(string username, int score)
	{
		WWW www = new WWW(webURL + privateCode + "/add/" + WWW.EscapeURL(username) + "/" + score);
		yield return www;

		if (string.IsNullOrEmpty(www.error))
			print("Upload Worked");
		else
			print("Fail uplaoding " + www.error);
	}

	IEnumerator DownloadScoresRoutine()
	{
		//WWW www = new WWW(webURL + publicCode + "/pipe/");
		WWW www = new WWW(webURL + publicCode + "/pipe/0/10");
		yield return www;

		if (string.IsNullOrEmpty(www.error))
		{
			FormatScores(www.text);
			leaderboardDisplay.OnScoresDownloaded(highscoresList);
		}
		else
			print("Fail downloading " + www.error);
	}

	void FormatScores(string textStream)
	{
		string[] entries = textStream.Split(new char[] {'\n'}, System.StringSplitOptions.RemoveEmptyEntries);
		highscoresList = new Highscore[entries.Length];

		for (int i = 0; i < entries.Length; i++)
		{
			string[] entryInfo = entries[i].Split(new char[] {'|'});
			string username = entryInfo[0];
			int score = int.Parse(entryInfo[1]);
			highscoresList[i] = new Highscore(username, score);

			print(highscoresList[i].username + ": " + highscoresList[i].score);
		}
	}

}

public struct Highscore
{
	public string username;
	public int score;

	public Highscore(string _username, int _score)
	{
		username = _username;
		score = _score;
	}
}
