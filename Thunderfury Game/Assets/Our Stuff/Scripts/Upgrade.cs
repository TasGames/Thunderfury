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
	[SerializeField] protected GameObject pistol;
	[SerializeField] protected GameObject shotgun;
	[SerializeField] protected GameObject rifle;
	[SerializeField] protected GameObject grenadeLauncher;
	[SerializeField] protected GameObject something;
	
	[SerializeField] protected GunDisplay gunDisplay;

	[SerializeField] protected GameObject pistolButton;
	[SerializeField] protected GameObject shotgunButton;
	[SerializeField] protected GameObject rifleButton;
	[SerializeField] protected GameObject grenadeButton;
	[SerializeField] protected GameObject somethingButton;


	void OnEnable()
	{
		/*pistol = GameObject.Find("Pistol");

		if (pistol != null)
			pistolButton.SetActive(true);

		shotgun = GameObject.Find("Shotgun");
		
		if (shotgun != null)
			shotgunButton.SetActive(true);

		rifle = GameObject.Find("Rifle");
		
		if (rifle != null)
			rifleButton.SetActive(true);

		grenadeLauncher = GameObject.Find("Grenade Launcher");

		if (grenadeLauncher != null)
			grenadeButton.SetActive(true);

		something = GameObject.Find("something");

		if (something != null)
			somethingButton.SetActive(true);*/									
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
			gunDisplay.Display();
		}
	}

	public void ShotgunButton()
	{
		if (shotgun != null)
		{
			gunDisplay.SetGunPrefab(shotgun);
			gunDisplay.Display();
		}
	}

	public void RifleButton()
	{
		if (rifle != null)
		{
			gunDisplay.SetGunPrefab(rifle);
			gunDisplay.Display();
		}
	}

	public void GrenadeButton()
	{
		if (grenadeButton != null)
		{
			gunDisplay.SetGunPrefab(grenadeButton);
			gunDisplay.Display();
		}
	}

	public void SomethingButton()
	{
		if (something != null)
		{
			gunDisplay.SetGunPrefab(something);
			gunDisplay.Display();
		}
	}
}
