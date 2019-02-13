using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "New Projectile", menuName = "Projectile")]
public class Projectile : ScriptableObject
{
	[Title("Projectile Stats")]
	public float projectileTimer;
	public float damage;
	[Title("Explosion Stats")]
	public bool isExplosive;
	[ShowIf("isExplosive", true)] public bool explodeOnImpact;
	[ShowIf("isExplosive", true)] public float explosionForce;
	[ShowIf("isExplosive", true)] public float blastRadius;
	[ShowIf("isExplosive", true)] public float magnitude;
	[ShowIf("isExplosive", true)] public float roughness;
	[ShowIf("isExplosive", true)] public float fadeIn;
	[ShowIf("isExplosive", true)] public float fadeOut;
	[ShowIf("isExplosive", true)] public GameObject explosionEffect;
}
