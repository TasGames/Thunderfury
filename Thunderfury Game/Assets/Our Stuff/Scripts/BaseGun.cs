using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class BaseGun : MonoBehaviour 
{
	protected enum typeEnum
	{
		rayType, projectileType
	}

	protected float nextToFire = 0;

	[Title("Gun Type")]
	[SerializeField] [EnumToggleButtons] protected typeEnum selectGunType;
	[SerializeField] protected bool isAutomatic;

	[Title("Gun Stats")]
	[SerializeField] protected Camera cam;
	[SerializeField] protected ParticleSystem muzzleFlash;
	[SerializeField] protected float fireRate;
	[SerializeField] protected float recoil;
	[SerializeField] protected float impact;
	[SerializeField] protected int magSize;
	[SerializeField] protected int maxAmmo;
	[SerializeField] [ShowIf("selectGunType", typeEnum.rayType)] protected float range;
	[SerializeField] [ShowIf("selectGunType", typeEnum.rayType)] [Range(0, 180)] protected float spread;
	[SerializeField] [ShowIf("selectGunType", typeEnum.rayType)] protected GameObject hitEffect;

	[Title("Ammo Stats")]
	[SerializeField] protected float damage;
	[SerializeField] [ShowIf("selectGunType", typeEnum.projectileType)] protected GameObject projectilePrefab;
	[SerializeField] [ShowIf("selectGunType", typeEnum.projectileType)] protected float projectileForce;
	[SerializeField] [ShowIf("selectGunType", typeEnum.projectileType)] protected float projectileTimer;
	[SerializeField] [ShowIf("selectGunType", typeEnum.projectileType)] protected bool isExplosive;
	
	void FixedUpdate() 
	{
		if (isAutomatic == false)
		{
			if (Input.GetButtonDown("Fire1") && Time.time >= nextToFire)
			{
				nextToFire = Time.time + 1 /fireRate;
				Shoot();
			}
		}
		else if (isAutomatic == true)
		{
			if (Input.GetButton("Fire1") && Time.time >= nextToFire)
			{
				nextToFire = Time.time + 1 /fireRate;
				Shoot();
			}
		}
	}

	void Shoot()
	{
		if (muzzleFlash != null)
			muzzleFlash.Play();

		if (selectGunType == typeEnum.projectileType)
			ProjectileType();
		else if (selectGunType == typeEnum.rayType)
			RayType();
	}

	void ProjectileType()
	{
		GameObject projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
		Rigidbody projRB = projectile.GetComponent<Rigidbody>();
		projRB.AddForce(transform.forward * projectileForce, ForceMode.VelocityChange);
	}

	void RayType()
	{
		RaycastHit hit;
		if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range))
		{
			Debug.Log(hit.transform.name);

			Target target = hit.transform.GetComponent<Target>();
			if (target != null)
				target.TakeDamage(damage);

			if (hit.rigidbody != null)
				hit.rigidbody.AddForce(hit.normal * impact * -1);

			if (hitEffect != null)
			{
				GameObject hitGO = Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
				Destroy(hitGO, 2);
			}
		}
	}
}
