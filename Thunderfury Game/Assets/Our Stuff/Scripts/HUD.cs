using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
	[SerializeField] protected PlayerHealth playerHealth;
	[SerializeField] protected WeaponWheel wW;
	[SerializeField] protected WeaponSwitcher wS;	
	[SerializeField] protected WaveManager wave;
	[SerializeField] protected TextMeshProUGUI tAmmo;
	[SerializeField] protected TextMeshProUGUI tScore;
	[SerializeField] protected TextMeshProUGUI waveCount;
	[SerializeField] protected Image healthBar;
	[SerializeField] protected Image shieldBar;
	[SerializeField] protected Image reloadingImage;
	[SerializeField] protected Image sadHeart;
	[SerializeField] protected Image happyHeart;
	protected bool isSad = false;
	protected UseGun uG;

	public static int totalScore = 0;

	void Update() 
	{
		/*if (gunAmmo != null && tAmmo != null)
			tAmmo.text = gunAmmo.currentMag + " / " + gunAmmo.ammoPool;*/

		Ammo();

		if (tScore != null)
			tScore.text = "¥" + totalScore;

		if (healthBar != null)
			healthBar.fillAmount = playerHealth.pHealth / playerHealth.maxHealth;

		if (shieldBar != null)
			shieldBar.fillAmount = playerHealth.pShield / playerHealth.maxShield;

		/*if (reloadingImage != null)
		{
			if (gunAmmo && gunAmmo.isReloading == true)
				reloadingImage.gameObject.SetActive(true);
			else
				reloadingImage.gameObject.SetActive(false);
		}*/

		if (sadHeart != null)
		{
			if (playerHealth.pHealth < 150 && isSad == false)
			{
				sadHeart.gameObject.SetActive(true);
				happyHeart.gameObject.SetActive(false);
				isSad = true;
			}
			else if (playerHealth.pHealth >= 150 && isSad == true)
			{
				sadHeart.gameObject.SetActive(false);
				happyHeart.gameObject.SetActive(true);
				isSad = false;
			}
		}

		if (waveCount != null)
		{
			waveCount.text = "" + wave.waveCounter;
		}
	}

	void Ammo()
	{
		if (wS != null)
			uG = wS.currentGun.GetComponent<UseGun>();

		if (uG != null && tAmmo != null)
			tAmmo.text = uG.currentMag + " / " + uG.ammoPool;
	}
}
