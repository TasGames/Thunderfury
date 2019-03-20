﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Pausing : MonoBehaviour 
{
	[SerializeField] protected GameObject pauseMenu;
    [SerializeField] protected GameObject weaponWheel;
    [SerializeField] protected GameObject panel;
    [SerializeField] protected Color panelColour;
    protected Color noAlphaPanel;
    Image image;

    protected RigidbodyFirstPersonController rbFPC;
    protected Animator anim;
    protected bool isOpen = false;

    void Start()
    {
        rbFPC = GetComponent<RigidbodyFirstPersonController>();
        anim = weaponWheel.GetComponent<Animator>();
        image = panel.GetComponent<Image>();
        noAlphaPanel = new Color(0, 0, 0, 0);
    }

	void Update()
	{
		if (Input.GetKeyDown("p"))
			pauseIt();

        if (Input.GetKeyDown("q"))
			StartCoroutine(OpenWeaponWheelRoutine());
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

    IEnumerator OpenWeaponWheelRoutine()
    {
        if (isOpen == false)
        {
            weaponWheel.SetActive(true);
            anim.SetTrigger("Open");
            isOpen = true;
            image.CrossFadeColor(panelColour, 1, true, true);
        }
        else
        {
            anim.SetTrigger("Close");
            isOpen = false;
            image.CrossFadeColor(noAlphaPanel, 1, true, true);
            yield return new WaitForSeconds(0.9f);
            weaponWheel.SetActive(false);
        }
        
    }

}
