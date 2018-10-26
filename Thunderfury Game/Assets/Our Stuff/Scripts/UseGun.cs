using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseGun : MonoBehaviour
 {
	[SerializeField] protected Gun gun;

	protected float nextToFire = 0;
	[HideInInspector] public bool isReloading = false;
	[HideInInspector] public int ammoPool;
	[HideInInspector] public int currentMag;

	void Start()
	{
		ammoPool = gun.maxAmmo - gun.magSize;
		currentMag = gun.magSize;
	}

	void OnEnable()
	{
		isReloading = false;
	}

	void FixedUpdate() 
	{
		if (isReloading)
			return;

		if (currentMag <= 0)
		{
			StartCoroutine(Reload());
			return;
		}

		if (gun.isAutomatic == false)
		{
			if (Input.GetButtonDown("Fire1") && Time.time >= nextToFire)
			{
				nextToFire = Time.time + 1 /gun.fireRate;
				Shoot();
			}
		}
		else if (gun.isAutomatic == true)
		{
			if (Input.GetButton("Fire1") && Time.time >= nextToFire)
			{
				nextToFire = Time.time + 1 /gun.fireRate;
				Shoot();
			}
		}
	}
	void Shoot()
	{
		if (gun.muzzleFlash != null)
			gun.muzzleFlash.Play();

		if (gun.selectGunType == typeEnum.projectileType)
			ProjectileType();
		else if (gun.selectGunType == typeEnum.rayType)
			RayType();

		currentMag--;
	}

	IEnumerator Reload()
	{
		isReloading = true;
		Debug.Log("Reloading");

		yield return new WaitForSeconds(gun.reloadTime);

		if (ammoPool >= gun.magSize)
		{
			ammoPool = ammoPool - gun.magSize;
			currentMag = gun.magSize;
		}
		else
		{
			currentMag = ammoPool;
			ammoPool = 0;
		}

		isReloading = false;
		
		Debug.Log("Reloaded");
	}

	void ProjectileType()
	{
		GameObject projectile = Instantiate(gun.projectilePrefab, transform.position, transform.rotation);
		Rigidbody projRB = projectile.GetComponent<Rigidbody>();
		projRB.AddForce(transform.forward * gun.projectileForce, ForceMode.VelocityChange);
	}

	void RayType()
	{
		RaycastHit hit;
		if (Physics.Raycast(gun.cam.transform.position, gun.cam.transform.forward, out hit, gun.range))
		{
			Target target = hit.transform.GetComponent<Target>();
			if (target != null)
				target.TakeDamage(gun.damage);

			if (hit.rigidbody != null)
				hit.rigidbody.AddForce(hit.normal * gun.impact * -1);

			if (gun.hitEffect != null)
			{
				GameObject hitGO = Instantiate(gun.hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
				Destroy(hitGO, 2);
			}
		}
	}
 }
