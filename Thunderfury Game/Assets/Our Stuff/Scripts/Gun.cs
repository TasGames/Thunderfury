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

	[Title("Gun Stats")]
	public Camera cam;
	public ParticleSystem muzzleFlash;
	public AudioClip fireSound;
	public float fireRate;
	public float recoil;
	public float impact;
	public int magSize;
	public int maxAmmo;
	[ShowIf("selectGunType", typeEnum.rayType)] public float range;
	[ShowIf("selectGunType", typeEnum.rayType)] [Range(0, 180)] public float spread;
	[ShowIf("selectGunType", typeEnum.rayType)] public GameObject hitEffect;
	
	[Title("Ammo Stats")]
	public float damage;
	[ShowIf("selectGunType", typeEnum.projectileType)] public GameObject projectilePrefab;
	[ShowIf("selectGunType", typeEnum.projectileType)] public float projectileForce;

}
