using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class UseProjectile : MonoBehaviour 
{
	[SerializeField] protected Projectile projectile;
	protected float countdown;
	protected bool hasExploded = false;

	void Start() 
	{
		countdown = projectile.projectileTimer;
	}
	
	void FixedUpdate() 
	{
		countdown -= Time.deltaTime;
		if (countdown <= 0)
		{
			if (projectile.isExplosive == true && hasExploded == false)
			{
				Explode();
				hasExploded = true;
			}
		}
	}

	void Explode()
	{

	}
}
