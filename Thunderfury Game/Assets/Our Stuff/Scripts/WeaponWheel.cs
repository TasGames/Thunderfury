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

	[HideInInspector] public GameObject currentGun;

	void Start()
	{
		uG[0] = rifle.GetComponent<UseGun>();
		uG[1] = grenadeLauncher.GetComponent<UseGun>();
		uG[3] = railgun.GetComponent<UseGun>();
		uG[2] = shotgun.GetComponent<UseGun>();
		uG[4] = singularity.GetComponent<UseGun>();
		uG[5] = pistol.GetComponent<UseGun>();

		StartCoroutine(AmmoRoutine());
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
				if (uG[i] != null)
					ammoText[i].text = uG[i].currentMag + " / " + uG[i].ammoPool;
			}
			yield return new WaitForSeconds(1);
		}
	}

}
