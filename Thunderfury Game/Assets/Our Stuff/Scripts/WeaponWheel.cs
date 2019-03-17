using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponWheel : MonoBehaviour
 {
	[SerializeField] protected GameObject pistol;
	[SerializeField] protected GameObject shotgun;
	[SerializeField] protected GameObject rifle;
	[SerializeField] protected GameObject grenadeLauncher;
	[SerializeField] protected GameObject railgun;
	[SerializeField] protected GameObject singularity;

	[SerializeField] protected TextMeshProUGUI[] ammoText;
	protected UseGun[] uG;
	protected int selectedButton = 0;
	[SerializeField] protected Color highlightColour;
	[SerializeField] protected Color standardColour;

	[HideInInspector] public GameObject currentGun;

	void Start()
	{

	/*	uG[0] = rifle.GetComponent<UseGun>();
		uG[1] = grenadeLauncher.GetComponent<UseGun>();
		uG[3] = railgun.GetComponent<UseGun>();
		uG[2] = shotgun.GetComponent<UseGun>();
		uG[4] = singularity.GetComponent<UseGun>();
		uG[5] = pistol.GetComponent<UseGun>();

		StartCoroutine(AmmoRoutine());*/


		//highlightColour = new Color32(30, 30, 30, 190);
		//standardColour = new Color32(0, 0, 0, 190);			
	}
	
	void Update()
	{
		ChangeButton();
	}

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

	IEnumerator AmmoRoutine()
	{
		while (true)
		{
			for (int i = 0; i < ammoText.Length; i++)
			{
				uG[i] = pistol.GetComponent<UseGun>();
				if (uG[i] != null)
					ammoText[i].text = uG[i].currentMag + " / " + uG[i].ammoPool;
			}
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
