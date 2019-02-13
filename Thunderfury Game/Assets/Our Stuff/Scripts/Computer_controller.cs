using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer_controller : MonoBehaviour 
{
protected Animator anim;
protected bool opened;

	void Start()
	{
		anim = GetComponent<Animator>();
	}
	
	void OnTriggerStay(Collider collider)
	{
		if (collider.gameObject.tag == "Player")
		{
			if(Input.GetKeyDown(KeyCode.E) && opened == false)
			{
				opened = true;
				anim.SetBool("Open", true);
				anim.SetBool("Close", false);
			}
			else if(Input.GetKeyDown(KeyCode.E) && opened == true)
			{
				opened = false;
				anim.SetBool("Close", true);
				anim.SetBool("Open", false);
			}
		}
	}
}
