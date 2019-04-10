using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch : MonoBehaviour 
{

	[SerializeField] protected Animator animator;
	[SerializeField] protected GameObject hit;
	
	void Update() 
	{
		if (Input.GetButtonDown("Fire1"))
		{
			if (animator != null)
				animator.SetBool("isPunching", false);
		}
	}
}
