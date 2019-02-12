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

		UseGun useGun = gunPrefab.GetComponent<UseGun>();
		gun = useGun.gun;

		gunName.text = gun.name;
		gunIcon.sprite = gun.icon;
		gunDescription.text = gun.description;
		gunDamage.text = "Damage: " + gun.damage;
		gunImpact.text = "Impact: " + gun.impact;
		gunFireRate.text = "Fire Rate: " + gun.fireRate;
		gunRecoil.text = "Recoil: " + gun.recoil;
		gunAmmo.text = "Ammo: " + gun.magSize + " / " + gun.maxAmmo;
		gunCost.text = "$" + gun.cost;

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
			
		}
	}

	public void Upgrade()
	{

	}
}
