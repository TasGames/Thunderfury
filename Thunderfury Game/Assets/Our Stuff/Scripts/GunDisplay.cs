using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GunDisplay : MonoBehaviour 
{
	[Title("Gun")]
	[SerializeField] protected GameObject gunPrefab;
	protected Gun gun;

	[Title("Gun Details")]
	[SerializeField] protected TextMeshProUGUI gunName;
	[SerializeField] protected Image gunIcon;
	[SerializeField] protected TextMeshProUGUI gunDescription;
	[SerializeField] protected TextMeshProUGUI gunDamage;
	[SerializeField] protected TextMeshProUGUI gunImpact;
	[SerializeField] protected TextMeshProUGUI gunFireRate;
	[SerializeField] protected TextMeshProUGUI gunRange;
	[SerializeField] protected TextMeshProUGUI gunRecoil;
	[SerializeField] protected TextMeshProUGUI gunReloadTime;
	[SerializeField] protected TextMeshProUGUI gunAmmo;
	[SerializeField] protected TextMeshProUGUI gunCost;
	[SerializeField] protected GameObject buyButton;
	[SerializeField] protected GameObject purchasedButton;
	[SerializeField] protected GameObject gunButton;
	[SerializeField] protected GameObject weaponSlot;
	[SerializeField] protected GameObject ammoShop;


	void Start() 
	{
		Shop shop = transform.root.GetComponent<Shop>();
		Display();
	}
	
	public void Buy()
	{
		int costCompare = HUD.totalScore;

		if (costCompare >= gun.cost)
		{
			gunPrefab.SetActive(true);

			HUD.totalScore -= gun.cost;
			
			//buyButton.SetActive(false);
			purchasedButton.SetActive(true);
			gunButton.SetActive(true);
			weaponSlot.SetActive(true);
			ammoShop.SetActive(true);
			
		}
	}

	public void SetGunPrefab(GameObject go)
	{
		gunPrefab = go;
	}

	public void Display()
	{
		UseGun useGun = gunPrefab.GetComponent<UseGun>();
		gun = useGun.gun;

		if (gunName != null)
			gunName.text = gun.name;
		if (gunIcon != null)
			gunIcon.sprite = gun.icon;
		if (gunDescription != null)
			gunDescription.text = gun.description;
        if (gunDamage != null)
            gunDamage.text = "Damage: " + gun.damage;
		if (gunImpact != null)
			gunImpact.text = "Impact: " + gun.impact;
		if (gunFireRate != null)
			gunFireRate.text = "Fire Rate: " + gun.fireRate;
		if (gunRange != null)
			gunRange.text = "Range: " + gun.range;
		if (gunRecoil != null)
			gunRecoil.text = "Recoil: " + gun.recoil;
		if (gunReloadTime != null)
			gunReloadTime.text = "Reload Time: " + gun.reloadTime;
		if (gunAmmo != null)
			gunAmmo.text = "Ammo: " + gun.magSize + " / " + gun.maxAmmo;
		if (gunCost != null)
			gunCost.text = "¥" + gun.cost;
	}
}
