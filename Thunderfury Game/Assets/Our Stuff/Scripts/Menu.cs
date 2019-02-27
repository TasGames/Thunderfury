using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Menu : MonoBehaviour 
{
	 public static bool isPaused = false;
    [SerializeField] protected GameObject pauseMenu;

	public void Play()
	{
        HUD.totalScore = 0;
		SceneManager.LoadScene("Axion");
		Time.timeScale = 1f;
        isPaused = false;
	}

	public void ReturnToMenu()
	{
		SceneManager.LoadScene("Main Menu");
		Time.timeScale = 1f;
        isPaused = false;
	}

	public void Quit()
	{
		Application.Quit();
	}

	public void PauseIt()
    {
        if (isPaused == false)
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
            Cursor.visible = true;
            isPaused = true;
        }
        else
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
            Cursor.visible = false;
            isPaused = false;
        }
    }
}
