using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
 {
	[SerializeField] protected UseGun gunAmmo;
	[SerializeField] protected Text tAmmo;
	[SerializeField] protected Image healthBar;
	[SerializeField] protected Image shieldBar;

	void Update() 
	{
		tAmmo.text = gunAmmo.currentMag + " / " + gunAmmo.ammoPool;

		healthBar.fillAmount = PlayerHealth.pHealth / PlayerHealth.maxHealth;
		healthBar.fillAmount = PlayerHealth.pShield / PlayerHealth.maxShield;
	}
}
