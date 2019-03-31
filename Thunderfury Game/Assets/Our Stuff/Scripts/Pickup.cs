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

	public static int ammoStorageP;
	public static int ammoStorageR;
	public static int ammoStorageS;
	public static int ammoStorageE;

	protected PlayerHealth playerHealth;

	void Start()
	{
		if (playerHealth == null)
			playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
	}

	void OnTriggerEnter(Collider collision)
	{
		if (givesHealth == true)
		{
			playerHealth.pHealth += healthGain;

			if (playerHealth.pHealth > playerHealth.maxHealth)
			{
				playerHealth.pHealth = playerHealth.maxHealth;
				Destroy(gameObject);
			}
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
						
					Destroy(gameObject);
				}
				else
				{
					if (selectAmmoType == ammoTypeEnum.pistol)
					{
						ammoStorageP += ammoGain;
					}
					else if (selectAmmoType == ammoTypeEnum.rifle)
					{
						ammoStorageR += ammoGain;
					}
					else if (selectAmmoType == ammoTypeEnum.shotgun)
					{
						ammoStorageS += ammoGain;
					}
					else if (selectAmmoType == ammoTypeEnum.explosive)
					{
						ammoStorageE += ammoGain;
					}

					Destroy(gameObject);
				}
			}
		}
	}

}
