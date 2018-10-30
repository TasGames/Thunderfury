using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseGun : MonoBehaviour
{
	[SerializeField] protected Gun gun;
	protected Camera cam;
	protected float nextToFire = 0;
	protected float finalDamage;
	[HideInInspector] public bool isReloading = false;
	[HideInInspector] public int ammoPool;
	[HideInInspector] public int currentMag;

	void Start()
	{
		ammoPool = gun.maxAmmo - gun.magSize;
		currentMag = gun.magSize;

		if (cam == null)
			cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
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
			StartCoroutine(ReloadRoutine());
			return;
		}

		if (Time.time >= nextToFire)
		{
			if (gun.animator != null)
				gun.animator.SetBool("isFiring", false);

			if (gun.isAutomatic == false)
			{
				if (Input.GetButtonDown("Fire1"))
				{
					nextToFire = Time.time + 1 /gun.fireRate;
					Shoot();
				}
			}
			else if (gun.isAutomatic == true)
			{
				if (gun.animator != null)
					gun.animator.SetBool("isFiring", false);

				if (Input.GetButton("Fire1"))
				{
					nextToFire = Time.time + 1 /gun.fireRate;
					Shoot();
				}
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

		if (gun.animator != null)
			gun.animator.SetBool("isFiring", true);

		cam.transform.Rotate(new Vector3(-gun.recoil, 0, 0));

		currentMag--;
	}

	IEnumerator ReloadRoutine()
	{
		isReloading = true;
		Debug.Log("Reloading");

		gun.animator.SetBool("isFiring", false);
		gun.animator.SetBool("isReloading", true);

		yield return new WaitForSeconds(gun.reloadTime);

		gun.animator.SetBool("isReloading", false);

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

		if (projRB.useGravity == true)
			projRB.AddForce(transform.forward * gun.projectileForce, ForceMode.VelocityChange);
		else
			projRB.velocity = transform.forward * gun.projectileForce;
	}

	void RayType()
	{
		for (int i = 0; i < gun.numRays; i++)
		{
			RaycastHit hit;
			Ray shootRay = new Ray(cam.transform.position, cam.transform.forward + cam.transform.up * Random.Range(-gun.spread, gun.spread) + cam.transform.right * Random.Range(-gun.spread, gun.spread));

			if (Physics.Raycast(shootRay.origin, shootRay.direction, out hit, gun.range))
			{
				Target target = hit.transform.GetComponent<Target>();

				finalDamage = gun.damage + Mathf.Round(Random.Range(-gun.damageRange, gun.damageRange) * 100.0f) / 100.0f;

				if (target != null)
					target.TakeDamage(finalDamage);

				if (hit.rigidbody != null)
					hit.rigidbody.AddForce(hit.normal * gun.impact * -1);

				if (gun.hitEffect != null)
				{
					GameObject hitGO = Instantiate(gun.hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
					Destroy(hitGO, 2);
				}

				Debug.DrawRay(shootRay.origin, shootRay.direction * 10, Color.red, 10);
				Debug.Log(finalDamage);
			}
		}
	}
}
