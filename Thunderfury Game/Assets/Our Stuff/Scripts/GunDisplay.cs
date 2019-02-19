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
	[SerializeField] protected GameObject shotgun;
	protected GameObject parentPrefab;
	protected Gun gun;

	[Title("Gun Details")]
	[SerializeField] protected TextMeshProUGUI gunName;
	[SerializeField] protected Image gunIcon;
	[SerializeField] protected TextMeshProUGUI gunDescription;
	[SerializeField] protected TextMeshProUGUI gunDamage;
	[SerializeField] protected TextMeshProUGUI gunImpact;
	[SerializeField] protected TextMeshProUGUI gunFireRate;
	[SerializeField] protected TextMeshProUGUI gunRecoil;
	[SerializeField] protected TextMeshProUGUI gunAmmo;
	[SerializeField] protected TextMeshProUGUI gunCost;
	[SerializeField] protected GameObject buyButton;
	[SerializeField] protected GameObject purchasedButton;

	void Start() 
	{
		Shop shop = transform.root.GetComponent<Shop>();
		parentPrefab = shop.parentPrefab;

		Display();

	}
	
	public void Buy()
	{
		int costCompare = HUD.totalScore;

		if (costCompare >= gun.cost)
		{
			Quaternion rot = parentPrefab.transform.rotation;

			GameObject gunObject = Instantiate(gunPrefab, gunPrefab.transform.position + parentPrefab.transform.position, rot, parentPrefab.transform);
			gunObject.SetActive(false);

			HUD.totalScore -= gun.cost;
			
			buyButton.SetActive(false);
			purchasedButton.SetActive(true);
			shotgun.SetActive(true);
			
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
		if (gunRecoil != null)
			gunRecoil.text = "Recoil: " + gun.recoil;
		if (gunAmmo != null)
			gunAmmo.text = "Ammo: " + gun.magSize + " / " + gun.maxAmmo;
		if (gunCost != null)
			gunCost.text = "$" + gun.cost;
	}
}
