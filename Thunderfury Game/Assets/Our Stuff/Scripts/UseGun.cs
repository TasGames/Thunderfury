using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseGun : MonoBehaviour
{
	public Gun gun;
	protected Camera cam;
	protected AudioSource fireSound;
	protected float nextToFire = 0;
	protected float finalDamage;
	protected bool stillFiring = false;
	[SerializeField] protected ParticleSystem muzzleFlash;
	[SerializeField] protected GameObject fireLocation;
	[SerializeField] protected Animator animator;
	[HideInInspector] public bool isReloading = false;
	[HideInInspector] public int ammoPool;
	[HideInInspector] public int currentMag;

	[HideInInspector] public float prefDamage;
	[HideInInspector] public float prefImpact;
	[HideInInspector] public float prefFireRate;
	[HideInInspector] public float prefRange;
	[HideInInspector] public float prefRecoil;
	[HideInInspector] public float prefReloadTime;
	[HideInInspector] public int prefMag;
	[HideInInspector] public int prefMaxAmmo;

	[HideInInspector] public float[] prefStat;
	[HideInInspector] public int[] level;
	[HideInInspector] public int ammoLevel = 0;

	protected bool itStarted = false;

	void Start()
	{
		level = new int[6];
		prefStat = new float[6];

		prefDamage = gun.damage;
		prefImpact = gun.impact;
		prefFireRate = gun.fireRate;
		prefRange = gun.range;
		prefRecoil = gun.recoil;
		prefReloadTime = gun.reloadTime;
		prefMag = gun.magSize;
		prefMaxAmmo = gun.maxAmmo;

		prefStat[0] = prefDamage;
		prefStat[1] = prefImpact;
		prefStat[2] = prefFireRate;
		prefStat[3] = prefRange;
		prefStat[4] = prefRecoil;
		prefStat[5] = prefReloadTime;

		ammoPool = prefMaxAmmo;
		currentMag = prefMag;

		fireSound = GetComponent<AudioSource>();

		if (cam == null)
			cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

		if (muzzleFlash == null)
			muzzleFlash = gameObject.GetComponentInChildren<ParticleSystem>();

		itStarted = true;
	}

	void OnEnable()
	{
		isReloading = false;

		if (itStarted == true)
		{
			prefDamage = prefStat[0];
			prefImpact = prefStat[1];
			prefFireRate = prefStat[2];
			prefRange = prefStat[3];
			prefRecoil = prefStat[4];
			prefReloadTime = prefStat[5];
		}

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

		if (ammoPool > prefMaxAmmo)
			ammoPool = prefMaxAmmo;
	}

	void Update() 
	{
		if (isReloading)
			return;

		if (currentMag <= 0 && ammoPool > 0)
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
					nextToFire = Time.time + 1 / prefFireRate;
					Shoot();
				}
			}
			else if (gun.isAutomatic == true)
			{
				if (animator != null)
					animator.SetBool("isFiring", false);

				if (Input.GetButton("Fire1"))
				{
					nextToFire = Time.time + 1 / prefFireRate;
					Shoot();
				}
			}
		}
    }

	void Shoot()
	{
		if (muzzleFlash != null)
			muzzleFlash.Play();

		if (fireSound != null)
			fireSound.Play();

		if (gun.selectGunType == typeEnum.projectileType)
			ProjectileType();
		else if (gun.selectGunType == typeEnum.rayType)
		{
			if (gun.isContinuous == false)
				RayType();
			else
				StartCoroutine(ContinuousRoutine());
		}

		if (animator != null)
			animator.SetBool("isFiring", true);

		cam.transform.Rotate(new Vector3(-prefRecoil, 0, 0));

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

		if (ammoPool >= prefMag)
		{
			ammoPool = ammoPool - prefMag;
			currentMag = prefMag;
		}
		else
		{
			currentMag = ammoPool;
			ammoPool = 0;
		}

		isReloading = false;
		
		Debug.Log("Reloaded");
	}

	IEnumerator ContinuousRoutine()
	{
		while(stillFiring == true)
		{
			RayType();
			yield return new WaitForSeconds(0.1f);
		}
	}

	void ProjectileType()
	{
		GameObject projectile = Instantiate(gun.projectilePrefab, transform.position, transform.rotation);
		Rigidbody projRB = projectile.GetComponent<Rigidbody>();

		if (projRB != null)
			projRB.AddForce(transform.forward * gun.projectileForce, ForceMode.VelocityChange);
	}

	void RayType()
	{
		for (int i = 0; i < gun.numRays; i++)
		{
			RaycastHit hit;
			Vector3 rayDirection = cam.transform.forward + cam.transform.up * Random.Range(-gun.spread, gun.spread) + cam.transform.right * Random.Range(-gun.spread, gun.spread);
			Ray shootRay = new Ray(cam.transform.position, rayDirection);
			Vector3 rayEnd = cam.transform.position + rayDirection * gun.range;

			if (gun.isPenetrating == false)
			{
				if (Physics.Raycast(shootRay.origin, shootRay.direction, out hit, gun.range))
				{
					HitEffects(hit);
					DrawLine(hit.point);
				}
				else
					DrawLine(rayEnd);
			}
			else
			{
				RaycastHit[] hits;
				hits = Physics.RaycastAll(shootRay.origin, shootRay.direction, gun.range);

				DrawLine(rayEnd);

				for (int j = 0; j < hits.Length; j++)
       			{	
					hit = hits[j];
					HitEffects(hit);
				}
			}
		}
	}

	void HitEffects(RaycastHit hit)
	{
		Target target = hit.transform.GetComponent<Target>();

		finalDamage = prefDamage + Mathf.Round(Random.Range(-gun.damageRange, gun.damageRange) * 100.0f) / 100.0f;

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
			hit.rigidbody.AddForce(hit.normal * prefImpact * -1);

		//Debug.DrawRay(shootRay.origin, shootRay.direction * 10, Color.red, 10);
		Debug.Log(finalDamage);
	}

 	void DrawLine(Vector3 end)
    {
		GameObject myLine = new GameObject();
		myLine.transform.position = fireLocation.transform.position;
		myLine.AddComponent<LineRenderer>();
		LineRenderer lr = myLine.GetComponent<LineRenderer>();
		lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
		lr.startColor = gun.ammoColour;
		lr.startWidth = gun.trailWidth;
		lr.SetPosition(0, fireLocation.transform.position);
		lr.SetPosition(1, end);
		GameObject.Destroy(myLine, gun.trailTimer);
    }
}
