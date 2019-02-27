using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour 
{
	[Range(0, 500)] public float maxHealth;
	[Range(0, 500)] public float maxShield;
	[HideInInspector] public float pHealth;
	[HideInInspector] public float pShield;
	[SerializeField] protected float regenWait;
	protected bool takenDamage = false;

	[SerializeField] protected GameObject gameOver;
	
	void Start()
	{
		pHealth = maxHealth;
		pShield = maxShield;
	}
	
	void Update()
	{
		// Just used for testing
		if (Input.GetKeyDown("l"))
		{
			PlayerTakeDamage(50);
		}

		if (pShield < maxShield && takenDamage == false)
		{
			StartCoroutine(RegenShieldRoutine());
		}

		if (Input.GetKeyDown("o"))
		{
			HUD.totalScore += 1000;
		}
	}

	public void PlayerTakeDamage(float amount)
	{
		pShield -= amount;

		if (pShield < 0)
		{
			pHealth += pShield;
			pShield = 0;

			if (pHealth <= 0)
			{
				gameOver.SetActive(true);
				Time.timeScale = 0f;
			}
		}

			takenDamage = true;
			StartCoroutine(DamageWaitRoutine());	
	}

	IEnumerator RegenShieldRoutine()
	{
		pShield += 1;

		yield return new WaitForSeconds(0.2f);
	}

	IEnumerator DamageWaitRoutine()
	{
		yield return new WaitForSeconds(regenWait);

		takenDamage = false;
	}
}
