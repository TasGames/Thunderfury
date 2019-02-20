using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Door_animation_script : MonoBehaviour
 {
	private Animator anim;

	protected int triggerCount = 0;

	[SerializeField] protected WaveManager wave;

	void Start() 
	{
		anim = GetComponent<Animator>();
		

	}
	
	// Update is called once per frame
	void Update()
	{
      
	}

	void OnTriggerEnter(Collider coll)
	{
		if (triggerCount == 0)
		{
			anim.SetTrigger("Open");
			triggerCount = 1;
		}
		else if (triggerCount == 2)
		{
			anim.SetTrigger("Close");
			triggerCount = 3;
			wave.WaveCompleted();
		}
		else if (triggerCount == 1)
		{
			triggerCount = 2;
		}
		else if (triggerCount == 3)
		{
			triggerCount = 0;
		}

	}
	/*void OnTriggerExit(Collider coll)
	{
		anim.SetTrigger("Close");
	}*/
}
