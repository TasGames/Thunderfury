using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "New Projectile", menuName = "Projectile")]
public class Projectile : ScriptableObject
{
	public bool isExplosive;
	public float projectileTimer;
	public float damage;
}
