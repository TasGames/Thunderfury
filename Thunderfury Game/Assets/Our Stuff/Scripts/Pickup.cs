using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public enum ammoTypeEnum
{
	pistol, rifle, shotgun, explosive
}

public class Pickup : MonoBehaviour 
{
	[Title("Health")]
	[SerializeField] protected bool givesHealth;
	[SerializeField] [ShowIf("givesHealth", true)] protected float healthGain;

	[Title("Ammo")]
	[SerializeField] protected bool givesAmmo;
	[SerializeField] [ShowIf("givesAmmo", true)] [EnumToggleButtons] protected ammoTypeEnum selectAmmoType;
	[SerializeField] [ShowIf("givesAmmo", true)] protected int ammoGain;

	protected PlayerHealth playerHealth;

	void Start()
	{
		if (playerHealth == null)
			playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
	}

	void OnCollisionEnter(Collision collision)
	{
		if (givesHealth == true)
		{
			playerHealth.pHealth += healthGain;

			if (playerHealth.pHealth > playerHealth.maxHealth)
				playerHealth.pHealth = playerHealth.maxHealth;
		}

		if (givesAmmo == true)
		{
			UseGun useGun = collision.gameObject.GetComponentInChildren<UseGun>();

			if (useGun != null)
			{
				if (useGun.gun.takesAmmoType == selectAmmoType)
				{
					useGun.ammoPool += ammoGain;
		
					if (useGun.ammoPool > useGun.gun.maxAmmo)
						useGun.ammoPool = useGun.gun.maxAmmo;
						
				}
				else
				{
					if (useGun.gun.takesAmmoType == ammoTypeEnum.pistol)
					{
						useGun.ammoStorageP += ammoGain;
					}
					else if (useGun.gun.takesAmmoType == ammoTypeEnum.rifle)
					{
						useGun.ammoStorageR += ammoGain;
					}
					else if (useGun.gun.takesAmmoType == ammoTypeEnum.shotgun)
					{
						useGun.ammoStorageS += ammoGain;						
					}
					else if (useGun.gun.takesAmmoType == ammoTypeEnum.explosive)
					{
						useGun.ammoStorageE += ammoGain;						
					}
				}
			}
		}

		Destroy(gameObject);
	}

}
