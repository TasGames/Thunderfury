using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer_controller : MonoBehaviour {
private Animator anim;
private bool opened;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		
	}
	
	// Update is called once per frame
	void Update () {

	
	}

	void OnTriggerStay(Collider coll)
	{
		Debug.Log("triggered");
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
