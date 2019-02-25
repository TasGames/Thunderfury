using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Door_animation_script : MonoBehaviour
 {
	protected Animator anim;
	protected bool isOpen = false;
	public bool isLocked = false;

	void Start() 
	{
		anim = GetComponent<Animator>();
	}

	void OnTriggerEnter(Collider coll)
	{
		if (isOpen == false && isLocked == false)
		{
			anim.SetTrigger("Open");
			isOpen = true;
		}
	}
	
	void OnTriggerExit(Collider coll)
	{
		if (isOpen == true)
		{
			anim.SetTrigger("Close");
			isOpen = false;
		}
	}
}
