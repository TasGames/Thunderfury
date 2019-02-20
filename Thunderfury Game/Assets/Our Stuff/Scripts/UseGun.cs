using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseGun : MonoBehaviour
{
	public Gun gun;
	protected Camera cam;
	protected float nextToFire = 0;
	protected float finalDamage;
	protected ParticleSystem muzzleFlash;
	[SerializeField] protected Animator animator;
	[HideInInspector] public bool isReloading = false;
	[HideInInspector] public int ammoPool;
	[HideInInspector] public int currentMag;
	[SerializeField] protected GameObject pos;

	void Start()
	{
		ammoPool = gun.maxAmmo - gun.magSize;
		currentMag = gun.magSize;

		if (cam == null)
			cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

		if (muzzleFlash == null)
			muzzleFlash = gameObject.GetComponentInChildren<ParticleSystem>();
	}

	void OnEnable()
	{
		isReloading = false;

		if (gun.takesAmmoType == ammoTypeEnum.pistol)
		{
			ammoPool += Pickup.ammoStorageP;
			Pickup.ammoStorageP = 0;
		}
		else if (gun.takesAmmoType == ammoTypeEnum.rifle)
		{
			ammoPool += Pickup.ammoStorageR;
			Pickup.ammoStorageR = 0;
		}
		else if (gun.takesAmmoType == ammoTypeEnum.shotgun)
		{
			ammoPool += Pickup.ammoStorageS;
			Pickup.ammoStorageS = 0;
		}
		else if (gun.takesAmmoType == ammoTypeEnum.explosive)
		{
			ammoPool += Pickup.ammoStorageE;
			Pickup.ammoStorageE = 0;
		}

		if (ammoPool > gun.maxAmmo)
			ammoPool = gun.maxAmmo;
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
			if (animator != null)
				animator.SetBool("isFiring", false);

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
				if (animator != null)
					animator.SetBool("isFiring", false);

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
		if (muzzleFlash != null)
			muzzleFlash.Play();

		if (gun.selectGunType == typeEnum.projectileType)
			ProjectileType();
		else if (gun.selectGunType == typeEnum.rayType)
			RayType();

		if (animator != null)
			animator.SetBool("isFiring", true);

		cam.transform.Rotate(new Vector3(-gun.recoil, 0, 0));

		currentMag--;
	}

	IEnumerator ReloadRoutine()
	{
		isReloading = true;
		Debug.Log("Reloading");

		if (animator != null)
		{
			animator.SetBool("isFiring", false);
			animator.SetBool("isReloading", true);
		}

		yield return new WaitForSeconds(gun.reloadTime);

		if (animator != null)
			animator.SetBool("isReloading", false);

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
		GameObject projectile = Instantiate(gun.projectilePrefab, pos.transform.position, pos.transform.rotation);
		Rigidbody projRB = projectile.GetComponent<Rigidbody>();

		if (projRB.useGravity == true)
			projRB.AddForce(-transform.right * gun.projectileForce, ForceMode.VelocityChange);
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

				if (gun.hitEffect != null)
				{
					GameObject hitGO = Instantiate(gun.hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
					Destroy(hitGO, 2);
				}

				if (gun.bulletMark != null)
				{
					GameObject bulGO = Instantiate(gun.bulletMark, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal), hit.transform);
					Destroy(bulGO, 5);
				}

				if (target != null)
					target.TakeDamage(finalDamage);

				if (hit.rigidbody != null)
					hit.rigidbody.AddForce(hit.normal * gun.impact * -1);

				Debug.DrawRay(shootRay.origin, shootRay.direction * 10, Color.red, 10);
				Debug.Log(finalDamage);
			}
		}
	}
}
