﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
	[SerializeField] protected PlayerHealth playerHealth;
	[SerializeField] protected UseGun gunAmmo;
	[SerializeField] protected Text tAmmo;
	[SerializeField] protected TextMeshProUGUI tScore;
	[SerializeField] protected Image healthBar;
	[SerializeField] protected Image shieldBar;
	[SerializeField] protected Image reloadingImage;

	public static int totalScore = 0;

	void Update() 
	{
		if (gunAmmo != null && tAmmo != null)
			tAmmo.text = gunAmmo.currentMag + " / " + gunAmmo.ammoPool;

		if (tScore != null)
			tScore.text = "Score: " + totalScore;

		if (healthBar != null)
			healthBar.fillAmount = playerHealth.pHealth / playerHealth.maxHealth;

		if (shieldBar != null)
			shieldBar.fillAmount = playerHealth.pShield / playerHealth.maxShield;

		if (reloadingImage != null)
		{
			if (gunAmmo && gunAmmo.isReloading == true)
				reloadingImage.gameObject.SetActive(true);
			else
				reloadingImage.gameObject.SetActive(false);
		}
	}
}
