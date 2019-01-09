using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pausing : MonoBehaviour 
{
	[SerializeField] protected GameObject pauseMenu;

	void Update()
	{
		if (Input.GetKeyDown("p"))
			pauseIt();
	}

	void pauseIt()
    {
        if (Menu.isPaused == false)
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
            Cursor.visible = true;
            Menu.isPaused = true;
        }
        else if (Menu.isPaused == true)
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
            Cursor.visible = false;
            Menu.isPaused = false;
        }
    }
}
