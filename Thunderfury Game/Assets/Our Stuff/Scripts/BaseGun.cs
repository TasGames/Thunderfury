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

	[Title("Gun Type")]
	[SerializeField] [EnumToggleButtons] protected typeEnum selectGunType;
	[SerializeField] protected bool isAutomatic;

	[Title("Gun Stats")]
	[SerializeField] protected float fireRate;
	[SerializeField] protected float recoil;
	[SerializeField] protected float impact;
	[SerializeField] protected int magSize;
	[SerializeField] protected int maxAmmo;
	[SerializeField] protected float range;
	[SerializeField] [ShowIf("selectGunType", typeEnum.rayType)] [Range(0, 180)] protected float spread;

	[Title("Bullet Stats")]
	[SerializeField] protected float damage;
	[SerializeField] [ShowIf("selectGunType", typeEnum.projectileType)] protected float bulletSpeed;
	[SerializeField] [ShowIf("selectGunType", typeEnum.projectileType)] protected float bulletDespawn;
	[SerializeField] [ShowIf("selectGunType", typeEnum.projectileType)] protected bool isExplosive;
	
	void Start() 
	{
		
	}
	
	void FixedUpdate() 
	{
		if (selectGunType == typeEnum.projectileType)
			ProjectileType();
		else if (selectGunType == typeEnum.rayType)
			RayType();
	}

	void ProjectileType()
	{

	}

	void RayType()
	{

	}
}
