using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Menu : MonoBehaviour 
{
	public void Play()
	{
		SceneManager.LoadScene("Tom");
	}

	public void ReturnToMenu()
	{
		SceneManager.LoadScene("Main Menu");
	}

	public void Quit()
	{
		Application.Quit();
	}

}
