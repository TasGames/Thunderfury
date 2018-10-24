using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
 {
	[SerializeField] protected Text tAmmo;
	public UseGun gunAmmo;
	void Update() 
	{
		tAmmo.text = gunAmmo.currentMag + " / " + gunAmmo.ammoPool;
	}
}
