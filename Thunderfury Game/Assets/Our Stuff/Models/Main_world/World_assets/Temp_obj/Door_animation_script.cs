using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Door_animation_script : MonoBehaviour {

	// Use this for initialization
private Animator anim;



	void Start () {
		    anim = GetComponent<Animator>();
		

	

	}
	
	// Update is called once per frame
	void Update () {
	
		
      
	}

	void OnTriggerEnter(Collider coll)
	{
		anim.SetTrigger("Open");

	}
	void OnTriggerExit(Collider coll)
	{
		anim.SetTrigger("Close");
	}
}
