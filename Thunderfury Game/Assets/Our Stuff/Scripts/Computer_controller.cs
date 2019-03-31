using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer_controller : MonoBehaviour 
{
[SerializeField] protected GameObject interactText;
protected Animator anim;
protected bool opened;

[SerializeField] protected Shop shop;

	void Start()
	{
		anim = GetComponent<Animator>();
	}
	
	void OnTriggerStay(Collider collider)
	{
		if (collider.gameObject.tag == "Player")
		{
			if(Input.GetKeyDown(KeyCode.E))
				shop.OpenShop();
		}
	}

	void OnTriggerEnter(Collider collider)
	{
		if (collider.gameObject.tag == "Player")
		{
			interactText.SetActive(true);
			anim.SetBool("Open", true);
			anim.SetBool("Close", false);
		}
	}

	void OnTriggerExit(Collider collider)
	{
		if (collider.gameObject.tag == "Player")
		{
			interactText.SetActive(false);
			anim.SetBool("Close", true);
			anim.SetBool("Open", false);
		}
	}
}
