using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AmmoStore : MonoBehaviour 
{
	[System.Serializable]
	protected struct AmmoStuff
	{
		public UseGun gun;
		public GameObject ammoHolder;
		public TextMeshProUGUI currentAmmo;
		public Button magButton;
		public Button fillButton;
		public TextMeshProUGUI magInfo;
		public TextMeshProUGUI fillInfo;
		public int magCost;
		public int fillCost;
	}

	[SerializeField] protected AmmoStuff[] ammoInfo;
	[SerializeField] protected PlayerHealth pH;
	[SerializeField] protected int shieldCost;
	protected int shieldFill;
	protected int shield10;
	protected int fillAllCost;
	float neededShield;
	protected bool bootedUpOnce = false;

	[SerializeField] protected TextMeshProUGUI currentShield;
	[SerializeField] protected TextMeshProUGUI shield10Info;
	[SerializeField] protected TextMeshProUGUI shieldFillInfo;

	[SerializeField] protected TextMeshProUGUI FullRefill;

	void OnEnable()
	{
		if (bootedUpOnce == false)
		{
			shield10 = shieldCost * 10;

			for (int i = 0; i < ammoInfo.Length; i++)
			{
				ammoInfo[i].currentAmmo = ammoInfo[i].ammoHolder.GetComponent<TextMeshProUGUI>();
				ammoInfo[i].magInfo = ammoInfo[i].ammoHolder.transform.GetChild(0).gameObject.GetComponentInChildren<TextMeshProUGUI>();
				ammoInfo[i].fillInfo = ammoInfo[i].ammoHolder.transform.GetChild(1).gameObject.GetComponentInChildren<TextMeshProUGUI>();

				ammoInfo[i].magButton = ammoInfo[i].ammoHolder.transform.GetChild(0).gameObject.GetComponent<Button>();
				ammoInfo[i].fillButton = ammoInfo[i].ammoHolder.transform.GetChild(1).gameObject.GetComponent<Button>();

				int x = i;
				ammoInfo[i].magButton.onClick.AddListener(() => FillMag(x));
				ammoInfo[i].fillButton.onClick.AddListener(() => FillAmmo(x));
			}

			bootedUpOnce = true;
		}

		UpdatePrices();
	}

	void UpdatePrices()
	{
		fillAllCost = 0;

		for (int i = 0; i < ammoInfo.Length; i++)
		{
			UseGun thisGun = ammoInfo[i].gun;

			ammoInfo[i].magCost = thisGun.gun.ammoCost * thisGun.prefMag;
			int neededAmmo = thisGun.prefMaxAmmo - thisGun.ammoPool;
			ammoInfo[i].fillCost = thisGun.gun.ammoCost * neededAmmo;

			ammoInfo[i].currentAmmo.text = thisGun.gun.name + ": " + thisGun.ammoPool + " / " + thisGun.prefMaxAmmo;
			ammoInfo[i].magInfo.text = "Mag ¥" + ammoInfo[i].magCost;
			ammoInfo[i].fillInfo.text = "Fill:	¥" + ammoInfo[i].fillCost;

			fillAllCost += ammoInfo[i].fillCost;
		}

		neededShield = pH.maxShield - pH.pShield;
		shieldFill = shieldCost * (int)neededShield;

		currentShield.text = "Shield: " + pH.pShield + " / " + pH.maxShield;
		shield10Info.text = "+10 ¥" + shield10;
		shieldFillInfo.text = "Fill:	¥" + shieldFill;

		fillAllCost += shieldFill;

		FullRefill.text = "Fill All	¥" + fillAllCost;
	}

	void FillAmmo(int i)
	{
		if (HUD.totalScore >= ammoInfo[i].fillCost)
		{
			ammoInfo[i].gun.ammoPool = ammoInfo[i].gun.prefMaxAmmo;
			HUD.totalScore -= ammoInfo[i].fillCost;
			UpdatePrices();
		}
	}

	void FillMag(int i)
	{
		if (HUD.totalScore >= ammoInfo[i].magCost && ammoInfo[i].gun.ammoPool + ammoInfo[i].gun.prefMag <= ammoInfo[i].gun.prefMaxAmmo)
		{
			ammoInfo[i].gun.ammoPool += ammoInfo[i].gun.prefMag;
			HUD.totalScore -= ammoInfo[i].magCost;
			UpdatePrices();
		}
	}

	public void RestorePartShield()
	{
		if (HUD.totalScore >= shield10 && pH.pShield + 10 <= pH.maxShield)
		{
			pH.pShield += 10;
			HUD.totalScore -= shield10;
			UpdatePrices();
		}
	}

	public void RestoreAllShield()
	{
		if (HUD.totalScore >= shieldFill)
		{
			pH.pShield = pH.maxShield;
			HUD.totalScore -= shieldFill;
			UpdatePrices();
		}
	}

	public void FullRestore()
	{
		if (HUD.totalScore >= fillAllCost)
		{
			for (int i = 0; i < ammoInfo.Length; i++)
			{
				ammoInfo[i].gun.ammoPool = ammoInfo[i].gun.prefMaxAmmo;
			}

			pH.pShield = pH.maxShield;
			HUD.totalScore -= fillAllCost;
			UpdatePrices();
		}
	}
	
}
