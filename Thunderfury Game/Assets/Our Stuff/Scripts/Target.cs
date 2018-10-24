using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Target : MonoBehaviour 
{
	[SerializeField] protected float health;
	[SerializeField] protected GameObject brokenVersion;

	public void TakeDamage(float amount)
	{
		health -= amount;

		if (health <= 0)
			Destroy();
	}

	void Destroy()
	{
		if (brokenVersion != null)
			Instantiate(brokenVersion, transform.position, transform.rotation);
			
		Destroy(gameObject);
	}
}
