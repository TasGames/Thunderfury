using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pausing : MonoBehaviour 
{
	[SerializeField] protected GameObject pauseMenu;

    protected RigidbodyFirstPersonController rbFPC;

    void Start()
    {
        rbFPC = GetComponent<RigidbodyFirstPersonController>();
    }

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
            rbFPC.mouseLook.SetCursorLock(false);
            Menu.isPaused = true;
        }
        else if (Menu.isPaused == true)
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
            rbFPC.mouseLook.SetCursorLock(true);
            Menu.isPaused = false;
        }
    }
}
