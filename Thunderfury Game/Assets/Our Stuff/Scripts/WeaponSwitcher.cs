using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
 {
	protected int selectedWeapon = 0;
	public GameObject currentGun;

	void Start() 
	{
		SelectWeapon();
	}
	
	void Update() 
	{
		ChangeWeapon();
	}

	void SelectWeapon()
	{
		int i = 0;
		foreach (Transform weapon in transform)
		{
			if (i == selectedWeapon)
			{
				weapon.gameObject.SetActive(true);
				currentGun = weapon.gameObject;
			}
			else
				weapon.gameObject.SetActive(false);

			i++;
		}
	}

	void ChangeWeapon()
	{
		int previousWeapon = selectedWeapon;

		if (Input.GetAxis("Mouse ScrollWheel") > 0f)
		{
			if (selectedWeapon >= transform.childCount - 1)
				selectedWeapon = 0;
			else
			selectedWeapon++;
		}
		else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
		{
			if (selectedWeapon == 0)
				selectedWeapon = transform.childCount - 1;
			else
			selectedWeapon--;
		}

		if (previousWeapon != selectedWeapon)
			SelectWeapon();
	}
}
