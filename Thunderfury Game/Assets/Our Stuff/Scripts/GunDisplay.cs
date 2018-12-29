using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class GunDisplay : MonoBehaviour 
{
	[Title("Gun")]
	[SerializeField] protected GameObject gunPrefab;
	protected GameObject parentPrefab;
	protected Gun gun;

	[Title("Gun Details")]
	[SerializeField] protected Text gunName;
	[SerializeField] protected Image gunIcon;
	[SerializeField] protected Text gunDescription;
	[SerializeField] protected Text gunDamage;
	[SerializeField] protected Text gunImpact;
	[SerializeField] protected Text gunFireRate;
	[SerializeField] protected Text gunRecoil;
	[SerializeField] protected Text gunAmmo;

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

	}
	
	public void Buy()
	{
		GameObject gun = Instantiate(gunPrefab, gunPrefab.transform.position + parentPrefab.transform.position, parentPrefab.transform.rotation, parentPrefab.transform);
		gun.SetActive(false);
	}
}
