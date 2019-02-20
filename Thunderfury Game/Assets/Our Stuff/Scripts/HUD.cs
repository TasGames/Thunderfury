using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
	[SerializeField] protected PlayerHealth playerHealth;
	[SerializeField] protected UseGun gunAmmo;
	[SerializeField] protected WaveManager wave;
	[SerializeField] protected Text tAmmo;
	[SerializeField] protected TextMeshProUGUI tScore;
	[SerializeField] protected TextMeshProUGUI waveCount;
	[SerializeField] protected Image healthBar;
	[SerializeField] protected Image shieldBar;
	[SerializeField] protected Image reloadingImage;
	[SerializeField] protected Image sadHeart;
	[SerializeField] protected Image happyHeart;
	protected bool isSad = false;

	public static int totalScore = 0;

	void Update() 
	{
		if (gunAmmo != null && tAmmo != null)
			tAmmo.text = gunAmmo.currentMag + " / " + gunAmmo.ammoPool;

		if (tScore != null)
			tScore.text = "¥" + totalScore;

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
}
