using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponWheel : MonoBehaviour
 {
	[SerializeField] protected GameObject pistol;
	[SerializeField] protected GameObject shotgun;
	[SerializeField] protected GameObject rifle;
	[SerializeField] protected GameObject grenadeLauncher;
	[SerializeField] protected GameObject railgun;
	[SerializeField] protected GameObject singularity;
	[HideInInspector] public GameObject currentGun;

	public void ButtonPistol()
	{
		SwitchGun(pistol);
	}

	public void ButtonShotgun()
	{
		SwitchGun(shotgun);
	}

	public void ButtonRifle()
	{
		SwitchGun(rifle);
	}

	public void ButtonGrenade()
	{
		SwitchGun(grenadeLauncher);
	}

	public void ButtonSingularity()
	{
		SwitchGun(singularity);
	}

	public void ButtonRailgun()
	{
		SwitchGun(railgun);
	}	

	void SwitchGun(GameObject gun)
	{
		if (currentGun == null)
			currentGun = pistol;

		currentGun.SetActive(false);
		gun.SetActive(true);
		currentGun = gun;
	}

}
