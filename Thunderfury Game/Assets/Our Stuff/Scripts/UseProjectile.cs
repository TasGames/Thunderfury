using System.Collections;
using System.Collections.Generic;
using EZCameraShake;
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
			if (projectile.isExplosive == true && hasExploded == false && projectile.explodeOnImpact == false)
			{
				Explode();
				hasExploded = true;
			}
			else
				Destroy(gameObject);
		}
	}

	void Explode()
	{
		if (projectile.explosionEffect != null)
		{
			GameObject explosion = Instantiate(projectile.explosionEffect, transform.position, transform.rotation);
			Destroy(explosion, 2);
		}

		CameraShaker.Instance.ShakeOnce(projectile.magnitude, projectile.roughness, projectile.fadeIn, projectile.fadeOut);

		Collider[] collidersToDamage = Physics.OverlapSphere(transform.position, projectile.blastRadius);
		foreach (Collider nearbyObject in collidersToDamage)
		{
			Target target = nearbyObject.GetComponent<Target>();
			if (target != null)
				target.TakeDamage(projectile.damage);
		}
		Collider[] collidersToForce = Physics.OverlapSphere(transform.position, projectile.blastRadius);
		foreach (Collider nearbyObject in collidersToForce)
		{
			Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
			if (rb != null)
				rb.AddExplosionForce(projectile.explosionForce, transform.position, projectile.blastRadius);
		}

		Destroy(gameObject);
	}

	void OnCollisionEnter(Collision collision)
	{
		if (projectile.isExplosive == false)
		{
			Target target = collision.gameObject.GetComponent<Target>();
			if(target != null)
			{
				target.TakeDamage(projectile.damage);
				Destroy(gameObject);
			}
		}
		else
		{
			if (projectile.explodeOnImpact == true)
				Explode();
		}
	}
}
