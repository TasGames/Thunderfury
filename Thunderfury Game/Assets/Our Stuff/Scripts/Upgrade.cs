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
	[SerializeField] protected GameObject railGun;
	[SerializeField] protected GameObject singularity;

	[Title("Toggle Buttons")]
	[SerializeField] protected GameObject pistolButton;
	[SerializeField] protected GameObject shotgunButton;
	[SerializeField] protected GameObject rifleButton;
	[SerializeField] protected GameObject grenadeButton;
	[SerializeField] protected GameObject railButton;
	[SerializeField] protected GameObject singularityButton;

	[Title("Current Gun Details")]
	[SerializeField] protected TextMeshProUGUI gunName;	
	[SerializeField] protected TextMeshProUGUI gunDamage;
	[SerializeField] protected TextMeshProUGUI gunImpact;
	[SerializeField] protected TextMeshProUGUI gunFireRate;
	[SerializeField] protected TextMeshProUGUI gunRange;
	[SerializeField] protected TextMeshProUGUI gunRecoil;
	[SerializeField] protected TextMeshProUGUI gunReloadTime;
	[SerializeField] protected TextMeshProUGUI gunAmmo;

	[Title("Current Gun Details")]
	[SerializeField] protected TextMeshProUGUI nGunDamage;
	[SerializeField] protected TextMeshProUGUI nGunImpact;
	[SerializeField] protected TextMeshProUGUI nGunFireRate;
	[SerializeField] protected TextMeshProUGUI nGunRange;
	[SerializeField] protected TextMeshProUGUI nGunRecoil;
	[SerializeField] protected TextMeshProUGUI nGunReloadTime;
	[SerializeField] protected TextMeshProUGUI nGunAmmo;	

	[Title("Credits")]
	[SerializeField] protected TextMeshProUGUI currentCredits;
	[SerializeField] protected TextMeshProUGUI requiredCredits;
	protected int totalCost = 0;
	protected int possibleCredits;

	protected UseGun useGun;

	protected float newDamage;
	protected float newImpact;
	protected float newFireRate;
	protected float newRange;
	protected float newRecoil;
	protected int newMagSize;
	protected int newMaxAmmo;
	protected float newReloadTime;

	void OnEnable()
	{
								
	}
	
	void Update() 
	{
		possibleCredits = HUD.totalScore - totalCost;

		if (requiredCredits != null)
		{
			requiredCredits.text = "Required: ¥" + totalCost;

			if (currentCredits != null)
				currentCredits.text = "Credits: ¥" + HUD.totalScore + " > ¥" + possibleCredits;
		}
		else if (currentCredits != null)
			currentCredits.text = "Credits: ¥" + HUD.totalScore;
	}

	public void PistolButton()
	{
		if (pistol != null)
		{
			useGun = pistol.GetComponent<UseGun>();
			DisplayCurrentStats();
			DisplayNewStats();
		}
	}

	public void ShotgunButton()
	{
		if (shotgun != null)
		{
			useGun = shotgun.GetComponent<UseGun>();
			DisplayCurrentStats();
			DisplayNewStats();
		}
	}

	public void RifleButton()
	{
		if (rifle != null)
		{
			useGun = rifle.GetComponent<UseGun>();
			DisplayCurrentStats();
			DisplayNewStats();
		}
	}

	public void GrenadeButton()
	{
		if (grenadeButton != null)
		{
			useGun = grenadeLauncher.GetComponent<UseGun>();
			DisplayCurrentStats();
			DisplayNewStats();
		}
	}

	public void RailButton()
	{
		if (railButton != null)
		{
			useGun = railGun.GetComponent<UseGun>();
			DisplayCurrentStats();
			DisplayNewStats();
		}
	}

	public void SingularityButton()
	{
		if (singularityButton != null)
		{
			useGun = singularity.GetComponent<UseGun>();
			DisplayCurrentStats();
			DisplayNewStats();
		}
	}

    public void Confirm()
    {
        HUD.totalScore = possibleCredits;
        totalCost = 0;
    }

	void DisplayCurrentStats()
	{
		if (gunName != null)
			gunName.text = useGun.gun.name;
        if (gunDamage != null)
            gunDamage.text = "Damage: " + useGun.prefDamage;
		if (gunImpact != null)
			gunImpact.text = "Impact: " + useGun.prefImpact;
		if (gunFireRate != null)
			gunFireRate.text = "Fire Rate: " + useGun.prefFireRate;
		if (gunRange != null)
			gunRange.text = "Range: " + useGun.prefRange;
		if (gunRecoil != null)
			gunRecoil.text = "Recoil: " + useGun.prefRecoil;
		if (gunReloadTime != null)
			gunReloadTime.text = "Reload Time: " + useGun.prefReloadTime;
		if (gunAmmo != null)
			gunAmmo.text = "Ammo: " + useGun.prefMag + " / " + useGun.prefMaxAmmo;
	}

	void DisplayNewStats()
	{
		newDamage = useGun.prefDamage;
		newImpact = useGun.prefImpact;
		newFireRate = useGun.prefFireRate;
		newRange = useGun.prefRange;
		newRecoil = useGun.prefRecoil;
		newMagSize = useGun.prefMag;
		newMaxAmmo = useGun.prefMaxAmmo;
		newReloadTime = useGun.prefReloadTime;	
		
        if (nGunDamage != null)
            nGunDamage.text = "Damage: " + newDamage;
		if (nGunImpact != null)
			nGunImpact.text = "Impact: " + newImpact;
		if (nGunFireRate != null)
			nGunFireRate.text = "Fire Rate: " + newFireRate;
		if (nGunRange != null)
			nGunRange.text = "Range: " + newRange;
		if (nGunRecoil != null)
			nGunRecoil.text = "Recoil: " + newRecoil;
		if (nGunReloadTime != null)
			nGunReloadTime.text = "Reload Time: " + newReloadTime;
		if (nGunAmmo != null)
			nGunAmmo.text = "Ammo: " + newMagSize + " / " + newMaxAmmo;
	}
}
