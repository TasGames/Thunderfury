using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponWheel : MonoBehaviour
 {
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
	
	protected int selectedButton = 0;
	[SerializeField] protected Color highlightColour;
	[SerializeField] protected Color standardColour;

	[HideInInspector] public GameObject currentGun;

	void Start()
	{
		StartCoroutine(AmmoRoutine());

		//highlightColour = new Color32(30, 30, 30, 190);
		//standardColour = new Color32(0, 0, 0, 190);			
	}
	
	void Update()
	{
		ChangeButton();
	}
	
	/*void SwitchGun(GameObject gun)
	{
		if (currentGun == null)
			currentGun = pistol;

		currentGun.SetActive(false);
		gun.SetActive(true);
		currentGun = gun;
	}*/

	IEnumerator AmmoRoutine()
	{
		while (true)
		{
			pistolAmmo.text = pistol.currentMag + " / " + pistol.ammoPool;
			shotgunAmmo.text = shotgun.currentMag + " / " + shotgun.ammoPool;
			grenadeAmmo.text = grenadeLauncher.currentMag + " / " + grenadeLauncher.ammoPool;
			singularityAmmo.text = singularity.currentMag + " / " + singularity.ammoPool;
			railAmmo.text = railgun.currentMag + " / " + railgun.ammoPool;
			rifleAmmo.text = rifle.currentMag + " / " + rifle.ammoPool;

			yield return new WaitForSeconds(1);
		}
	}

	void ChangeButton()
	{
		int previousButton = selectedButton;

		if (Input.GetAxis("Mouse ScrollWheel") > 0f)
		{
			if (selectedButton >= transform.childCount - 1)
				selectedButton = 0;
			else
			selectedButton++;
		}
		else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
		{
			if (selectedButton == 0)
				selectedButton = transform.childCount - 1;
			else
			selectedButton--;
		}

		if (previousButton != selectedButton)
			SelectButton();
	}

	void SelectButton()
	{
		int i = 0;
		foreach (Transform button in transform)
		{
			if (i == selectedButton)
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

}
