using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public static bool isPaused = false;
    [SerializeField] protected GameObject pauseMenu;

    [SerializeField] protected RigidbodyFirstPersonController rbFPC;

    public void Play()
    {
        HUD.totalScore = 0;
        SceneManager.LoadScene("Axion");
        Time.timeScale = 1f;
        isPaused = false;
        rbFPC.mouseLook.SetCursorLock(true);
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
            rbFPC.mouseLook.SetCursorLock(false);
            isPaused = true;
        }
        else
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
            rbFPC.mouseLook.SetCursorLock(true);
            isPaused = false;
        }
    }

    public void Tutorial()
    {
        SceneManager.LoadScene("AxionTutorial");
        Time.timeScale = 1f;
        isPaused = false;
        rbFPC.mouseLook.SetCursorLock(true);
    }
}
