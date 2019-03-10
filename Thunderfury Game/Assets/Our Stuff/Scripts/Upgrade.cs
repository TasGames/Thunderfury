using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Upgrade : MonoBehaviour 
{
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

	[Title("Current Gun Details")]
	[SerializeField] protected TextMeshProUGUI gunName;	
	[SerializeField] protected TextMeshProUGUI gunDamage;
	[SerializeField] protected TextMeshProUGUI gunImpact;
	[SerializeField] protected TextMeshProUGUI gunFireRate;
	[SerializeField] protected TextMeshProUGUI gunRange;
	[SerializeField] protected TextMeshProUGUI gunRecoil;
	[SerializeField] protected TextMeshProUGUI gunReloadTime;
	[SerializeField] protected TextMeshProUGUI gunAmmo;	

	[Title("Credits")]
	[SerializeField] protected TextMeshProUGUI currentCredits;
	[SerializeField] protected TextMeshProUGUI requiredCredits;
	protected int totalCost = 0;
	protected int possibleCredits;

	protected UseGun useGun;

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
			useGun = pistol.GetComponent<UseGun>();
			DisplayCurrentStats(useGun);
		}
	}

	public void ShotgunButton()
	{
		if (shotgun != null)
		{
			useGun = shotgun.GetComponent<UseGun>();
			DisplayCurrentStats(useGun);
		}
	}

	public void RifleButton()
	{
		if (rifle != null)
		{
			useGun = rifle.GetComponent<UseGun>();
			DisplayCurrentStats(useGun);
		}
	}

	public void GrenadeButton()
	{
		if (grenadeButton != null)
		{
			useGun = grenadeLauncher.GetComponent<UseGun>();
			DisplayCurrentStats(useGun);
		}
	}

	public void SomethingButton()
	{
		if (something != null)
		{
			useGun = something.GetComponent<UseGun>();
			DisplayCurrentStats(useGun);
		}
	}

    public void Confirm()
    {
        HUD.totalScore = possibleCredits;
        totalCost = 0;
    }

	void DisplayCurrentStats(UseGun UG)
	{
		if (gunName != null)
			gunName.text = UG.gun.name;
        if (gunDamage != null)
            gunDamage.text = "Damage: " + UG.prefDamage;
		if (gunImpact != null)
			gunImpact.text = "Impact: " + UG.prefImpact;
		if (gunFireRate != null)
			gunFireRate.text = "Fire Rate: " + UG.prefFireRate;
		if (gunRange != null)
			gunRange.text = "Range: " + UG.prefRange;
		if (gunRecoil != null)
			gunRecoil.text = "Recoil: " + UG.prefRecoil;
		if (gunReloadTime != null)
			gunReloadTime.text = "Reload Time: " + UG.prefReloadTime;
		if (gunAmmo != null)
			gunAmmo.text = "Ammo: " + UG.prefMag + " / " + UG.prefMaxAmmo;
	}
}
