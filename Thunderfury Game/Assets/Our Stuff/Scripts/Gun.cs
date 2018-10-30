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
	public string description;
		
	[Title("Gun Type")]
	[EnumToggleButtons] public typeEnum selectGunType;
	public bool isAutomatic;
	[ShowIf("selectGunType", typeEnum.rayType)] public bool isContinuous;

	[Title("Gun Stats")]
	public ParticleSystem muzzleFlash;
	public AudioClip fireSound;
	public Animator animator;
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
	
	[Title("Ammo Stats")]
	[ShowIf("selectGunType", typeEnum.rayType)] public float damage;
	[ShowIf("selectGunType", typeEnum.rayType)] [Range(0, 20)] public float damageRange;
	[Title("Ammo Stats")] [ShowIf("selectGunType", typeEnum.projectileType)] public GameObject projectilePrefab;
	[ShowIf("selectGunType", typeEnum.projectileType)] public float projectileForce;

}
