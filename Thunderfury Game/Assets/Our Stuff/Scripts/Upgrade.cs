using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Upgrade : MonoBehaviour 
{
	[SerializeField] protected TextMeshProUGUI currentCredits;
	[SerializeField] protected TextMeshProUGUI requiredCredits;
	protected int totalCost = 0;
	protected int possibleCredits;

	[SerializeField] protected GameObject weaponParent;
	protected GameObject pistol;
	protected GameObject shotgun;
	protected GameObject rifle;
	protected GameObject grenadeLauncher;
	protected GameObject something;
	
	[SerializeField] protected GunDisplay gunDisplay;

	[SerializeField] protected GameObject pistolButton;
	[SerializeField] protected GameObject shotgunButton;
	[SerializeField] protected GameObject rifleButton;
	[SerializeField] protected GameObject grenadeButton;
	[SerializeField] protected GameObject somethingButton;


	void OnEnable()
	{
		if (pistol == null)
		{
			if (weaponParent.transform.Find("Pistol").gameObject)
			{
				pistol = weaponParent.transform.Find("Pistol").gameObject;
				pistolButton.SetActive(true);
			}
		}

		if (shotgun == null)
		{
			if (weaponParent.transform.Find("Shotgun").gameObject)
			{
				shotgun = weaponParent.transform.Find("Shotgun").gameObject;
				shotgunButton.SetActive(true);
			}
		}

		if (rifle == null)
		{
			if (weaponParent.transform.Find("Rifle").gameObject)
			{
				rifle = weaponParent.transform.Find("Rifle").gameObject;
				rifleButton.SetActive(true);
			}
		}

		if (grenadeLauncher == null)
		{
			if (weaponParent.transform.Find("Grenade Launcher").gameObject)
			{
				grenadeLauncher = weaponParent.transform.Find("Grenade Launcher").gameObject;
				grenadeButton.SetActive(true);
			}
		}
			
		if (something == null)
		{
			if (weaponParent.transform.Find("something").gameObject)
			{
				something = weaponParent.transform.Find("something").gameObject;
				somethingButton.SetActive(true);	
			}
		}								
	}
	
	void Update() 
	{
		possibleCredits = HUD.totalScore;

		if (requiredCredits != null)
		{
			requiredCredits.text = "Required: $" + totalCost;

			if (currentCredits != null)
				currentCredits.text = "Credits: $" + HUD.totalScore + " > $" + possibleCredits;
		}
		else if (currentCredits != null)
			currentCredits.text = "Credits: $" + HUD.totalScore;
	}

	public void PistolButton()
	{
		if (pistol != null)
		{
			gunDisplay.SetGunPrefab(pistol);
		}
	}
}
