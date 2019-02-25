﻿using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Upgrade : MonoBehaviour 
{
	[Title("Other")]
	[SerializeField] protected GameObject weaponParent;
	[SerializeField] protected GunDisplay gunDisplay;
	[SerializeField] protected GunDisplay gunDisplay2;

	[Title("Gun Prefabs")]
	[SerializeField] protected GameObject pistol;
	[SerializeField] protected GameObject shotgun;
	[SerializeField] protected GameObject rifle;
	[SerializeField] protected GameObject grenadeLauncher;
	[SerializeField] protected GameObject something;

	[Title("Toggle Buttons")]
	[SerializeField] protected GameObject pistolButton;
	[SerializeField] protected GameObject shotgunButton;
	[SerializeField] protected GameObject rifleButton;
	[SerializeField] protected GameObject grenadeButton;
	[SerializeField] protected GameObject somethingButton;

	[Title("Credits")]
	[SerializeField] protected TextMeshProUGUI currentCredits;
	[SerializeField] protected TextMeshProUGUI requiredCredits;
	protected int totalCost = 0;
	protected int possibleCredits;


	void OnEnable()
	{
											
	}
	
	void Update() 
	{
		possibleCredits = HUD.totalScore - totalCost;

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
			gunDisplay2.SetGunPrefab(pistol);
		}
	}

	public void ShotgunButton()
	{
		if (shotgun != null)
		{
			gunDisplay.SetGunPrefab(shotgun);
			gunDisplay2.SetGunPrefab(shotgun);
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
			gunDisplay2.SetGunPrefab(grenadeButton);
			gunDisplay.Display();
			gunDisplay2.Display();
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

    public void Confirm()
    {
        HUD.totalScore = possibleCredits;
        totalCost = 0;
        gunDisplay.Display();
    }
}
