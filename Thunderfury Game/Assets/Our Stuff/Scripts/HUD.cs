using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
	[SerializeField] protected PlayerHealth playerHealth;
	[SerializeField] protected UseGun gunAmmo;
	[SerializeField] protected Text tAmmo;
	[SerializeField] protected Image healthBar;
	[SerializeField] protected Image shieldBar;
	[SerializeField] protected Image reloadingImage;



	void Update() 
	{
		if (gunAmmo != null || tAmmo != null)
			tAmmo.text = gunAmmo.currentMag + " / " + gunAmmo.ammoPool;

		if (healthBar != null)
			healthBar.fillAmount = playerHealth.pHealth / playerHealth.maxHealth;

		if (shieldBar != null)
			shieldBar.fillAmount = playerHealth.pShield / playerHealth.maxShield;

		if (reloadingImage != null)
		{
			if (gunAmmo.isReloading == true)
				reloadingImage.gameObject.SetActive(true);
			else
				reloadingImage.gameObject.SetActive(false);
		}
	}
}
