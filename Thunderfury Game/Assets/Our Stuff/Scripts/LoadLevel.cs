using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadLevel : MonoBehaviour 
{
	[SerializeField] protected GameObject loadingScreen;
	[SerializeField] protected RigidbodyFirstPersonController playerMovement;
	[SerializeField] protected GameObject[] guns;
	[SerializeField] protected WeaponSwitcher weapon;
	[SerializeField] protected float waitTime;
	protected UseGun gun;

	public static bool isloaded = false;

	void Start() 
	{
		StartCoroutine(LoadRoutine());
	}

	IEnumerator LoadRoutine()
	{
		yield return new WaitForSeconds(waitTime);

		for (int i = 0; i < guns.Length; i++)
		{
			if (i > 0)
				guns[i].SetActive(false);
		}

		weapon.enabled = true;
		loadingScreen.SetActive(false);
		playerMovement.enabled = true;
		isloaded = true;

	}
	
}
