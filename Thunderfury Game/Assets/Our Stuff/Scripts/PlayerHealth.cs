﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour 
{
	[Range(0, 500)] public float maxHealth;
	[Range(0, 500)] public float maxShield;
	[HideInInspector] public float pHealth;
	[HideInInspector] public float pShield;

	[SerializeField] protected float regenWait;
	protected float countdown;
	protected bool shieldFull = true;
	protected bool takenDamage = false;
	
	void Start()
	{
		pHealth = maxHealth;
		pShield = maxShield;
		countdown = regenWait;
	}
	
	void Update()
	{
		if (takenDamage == true)
		{
			countdown = regenWait;
			takenDamage = false;
		}

		countdown -= Time.deltaTime;
		if (countdown <= 0 && shieldFull == false)
		{
			
		}	
	}

	void RegenShield()
	{

	}
}
