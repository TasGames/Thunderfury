using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGun : MonoBehaviour 
{
	[Header("Gun Stats")]
	[SerializeField] protected float fireRate;
	[SerializeField] protected float range;
	[SerializeField] protected float recoil;
	[SerializeField] protected float impact;
	[SerializeField] protected int maxAmmo;

	[Space] [Header("Bullet Stats")]
	[SerializeField] protected float bulletSpeed;
	[SerializeField] protected float bulletDespawn;
	[SerializeField] protected float damage;


	void Start () 
	{
		
	}
	
	void Update () 
	{
		
	}
}
