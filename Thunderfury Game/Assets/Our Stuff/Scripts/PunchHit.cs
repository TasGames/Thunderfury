using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchHit : MonoBehaviour
{
	void OnCollisionEnter(Collision collisionInfo)
	{
		Target target = collisionInfo.gameObject.GetComponent<Target>();
		target.TakeDamage(5);
	}

}
