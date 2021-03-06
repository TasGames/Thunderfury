﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponSwitcher : MonoBehaviour
 {
	protected int selectedWeapon = 0;
	public GameObject currentGun;

	[SerializeField] float countdown = 2;

	[SerializeField] protected GameObject weaponWheel;
	[SerializeField] protected GameObject[] button;
	protected Image image;
	protected bool isOpen = false;

	[SerializeField] protected Color highlightColour;
	[SerializeField] protected Color standardColour;

	[SerializeField] protected UseGun pistol;
	[SerializeField] protected UseGun shotgun;
	[SerializeField] protected UseGun rifle;
	[SerializeField] protected UseGun grenadeLauncher;
	[SerializeField] protected UseGun railgun;
	[SerializeField] protected UseGun singularity;

	[SerializeField] protected TextMeshProUGUI pistolAmmo;
	[SerializeField] protected TextMeshProUGUI rifleAmmo;
	[SerializeField] protected TextMeshProUGUI grenadeAmmo;
	[SerializeField] protected TextMeshProUGUI shotgunAmmo;
	[SerializeField] protected TextMeshProUGUI singularityAmmo;
	[SerializeField] protected TextMeshProUGUI railAmmo;

	void Start() 
	{
		image = button[0].GetComponent<Image>();
		SelectWeapon();
	}
	
	void Update() 
	{
		if (Input.GetAxis("Mouse ScrollWheel") != 0f)
			ChangeWeapon();

		if (isOpen == true)
		{
			countdown -= Time.deltaTime;

			if (countdown <= 0)
			{
				weaponWheel.SetActive(false);
				isOpen = false;
			}
		}
	}

	void SelectWeapon()
	{
		int i = 0;
		foreach (Transform weapon in transform)
		{
			if (i == selectedWeapon)
			{
				weapon.gameObject.SetActive(true);
				currentGun = weapon.gameObject.transform.GetChild(0).gameObject;
				image.color = standardColour;
				image = button[i].GetComponent<Image>();
				image.color = highlightColour;
				

			}
			else
				weapon.gameObject.SetActive(false);

			i++;
		}
	}

	void SelectButton()
	{
		int i = 0;
		foreach (Transform button in weaponWheel.transform)
		{
			if (i == selectedWeapon)
			{
				Image image = button.gameObject.GetComponent<Image>();
				image.color = highlightColour;
			}
			else
			{
				Image image2 = button.gameObject.GetComponent<Image>();
				image2.color = standardColour;
			}

			i++;
		}
	}

	void ChangeWeapon()
	{
		OpenWeaponWheel();

		int previousWeapon = selectedWeapon;

		if (Input.GetAxis("Mouse ScrollWheel") > 0f)
		{
			countdown = 2;

			if (selectedWeapon >= transform.childCount - 1)
				selectedWeapon = 0;
			else
			selectedWeapon++;
		}
		else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
		{
			countdown = 2;

			if (selectedWeapon == 0)
				selectedWeapon = transform.childCount - 1;
			else
			selectedWeapon--;
		}

		if (previousWeapon != selectedWeapon)
		{
			SelectWeapon();
		}
	}

	void DisplayAmmo()
	{
		pistolAmmo.text = pistol.currentMag + " / " + pistol.ammoPool;
		shotgunAmmo.text = shotgun.currentMag + " / " + shotgun.ammoPool;
		grenadeAmmo.text = grenadeLauncher.currentMag + " / " + grenadeLauncher.ammoPool;
		singularityAmmo.text = singularity.currentMag + " / " + singularity.ammoPool;
		railAmmo.text = railgun.currentMag + " / " + railgun.ammoPool;
		rifleAmmo.text = rifle.currentMag + " / " + rifle.ammoPool;
	}

	void OpenWeaponWheel()
    {
        if (isOpen == false)
        {
            weaponWheel.SetActive(true);
			DisplayAmmo();
            isOpen = true;
        }
    }
}
