using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveStarter : MonoBehaviour 
{
	[SerializeField] protected WaveManager wave;

	void OnTriggerEnter(Collider coll)
	{
		wave.WaveCompleted();
	}
	
	
}
