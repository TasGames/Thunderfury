using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoStore : MonoBehaviour 
{
	[System.Serializable]
	protected struct AmmoStuff
	{
		public UseGun gun;
		public int magCost;
		public int fillCost;
	}

	[SerializeField] protected AmmoStuff[] guns;

	[SerializeField] protected GameObject weaponHolder;

	void Start() 
	{
		for (int i = 0; i < weaponHolder.transform.childCount; i++)
		{
			guns[i].gun = weaponHolder.transform.GetChild(i).gameObject.GetComponentInChildren<UseGun>();
			guns[i].magCost = guns[i].gun.gun.ammoCost * guns[i].gun.gun.magSize;
		}
	}
	
}
