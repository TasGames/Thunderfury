using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public enum typeEnum
{
	rayType, projectileType
}

[CreateAssetMenu(fileName = "New Gun", menuName = "Gun")]
public class Gun : ScriptableObject 
{
	[Title("About Gun")]
	public new string name;
	public Sprite icon;
	public string description;
	public int cost;
		
	[Title("Gun Type")]
	[EnumToggleButtons] public typeEnum selectGunType;
	public bool isAutomatic;
	[ShowIf("selectGunType", typeEnum.rayType)] public bool isContinuous;

	[Title("Gun Stats")]
	public AudioClip fireSound;
	public float fireRate;
	public float recoil;
	public int magSize;
	public int maxAmmo;
	public float reloadTime;
	[ShowIf("selectGunType", typeEnum.rayType)] public float impact;
	[ShowIf("selectGunType", typeEnum.rayType)] public float range;
	[ShowIf("selectGunType", typeEnum.rayType)] [Range(1, 50)] public int numRays;
	[ShowIf("selectGunType", typeEnum.rayType)] [Range(0, 1)] public float spread;
	[ShowIf("selectGunType", typeEnum.rayType)] public GameObject hitEffect;
	[ShowIf("selectGunType", typeEnum.rayType)] public GameObject bulletMark;
	
	[Title("Ammo Stats")]
	[EnumToggleButtons] public ammoTypeEnum takesAmmoType;
	[ShowIf("selectGunType", typeEnum.rayType)] public float damage;
	[ShowIf("selectGunType", typeEnum.rayType)] [Range(0, 20)] public float damageRange;
	[ShowIf("selectGunType", typeEnum.projectileType)] public GameObject projectilePrefab;
	[ShowIf("selectGunType", typeEnum.projectileType)] public float projectileForce;

	[Title("Level Increase")]
	public int maxLevel;
	public float damageIncrease;
	public float impactIncrease;
	public float fireRateIncrease;
	public float rangeIncrease;
	public float recoilIncrease;
	public float reloadDecrease;
	public int magIncrease;
	public int ammoIncrease;

}