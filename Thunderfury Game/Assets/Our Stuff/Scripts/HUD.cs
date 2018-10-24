using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
 {
	[SerializeField] protected Text tAmmo;

	void Update() 
	{
		//tAmmo.text = UseGun.currentMag + " / " + UseGun.ammoPool;
	}
}
