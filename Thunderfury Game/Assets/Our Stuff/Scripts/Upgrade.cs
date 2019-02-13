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

	void OnEnable()
	{
		if (pistol == null)
		{
			if (weaponParent.transform.Find("Pistol").gameObject != null)
				pistol = weaponParent.transform.Find("Pistol").gameObject;
		}
		if (shotgun == null)
		{
			if (weaponParent.transform.Find("Shotgun").gameObject != null)
				shotgun = weaponParent.transform.Find("Shotgun").gameObject;
		}
		if (rifle == null)
		{
			if (weaponParent.transform.Find("Rifle").gameObject != null)
				rifle = weaponParent.transform.Find("Rifle").gameObject;
		}
		if (grenadeLauncher == null)
		{
			if (weaponParent.transform.Find("Grenade Launcher").gameObject != null)
				grenadeLauncher = weaponParent.transform.Find("Grenade Launcher").gameObject;
		}
		if (something == null)
		{
			if (weaponParent.transform.Find("something").gameObject != null)
				something = weaponParent.transform.Find("something").gameObject;
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
