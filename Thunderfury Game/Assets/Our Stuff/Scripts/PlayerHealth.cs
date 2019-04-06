using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour 
{
	[Range(0, 500)] public float maxHealth;
	[Range(0, 500)] public float maxShield;
	[HideInInspector] public float pHealth;
	[HideInInspector] public float pShield;
	protected bool takenDamage = false;
	[SerializeField] protected float regenAmount;
	[SerializeField] protected float regenTime;
	[SerializeField] protected float maxRegen;
	[SerializeField] protected GameObject gameOver;
	protected bool isGameOver = false;

	protected RigidbodyFirstPersonController rbFPC;
	protected AudioSource damageSound;
	
	void Start()
	{
		pHealth = maxHealth;
		pShield = maxShield;

		rbFPC = GetComponent<RigidbodyFirstPersonController>();
		damageSound = GetComponent<AudioSource>();

		StartCoroutine(RegenHealthRoutine());
	}
	
	void Update()
	{
		// Just used for testing
		/*if (Input.GetKeyDown("l"))
		{
			PlayerTakeDamage(50);
		}*/

		/*if (Input.GetKeyDown("o"))
		{
			HUD.totalScore += 1000;
		}*/
	}

	public void PlayerTakeDamage(float amount)
	{
		pShield -= amount;

		if (isGameOver == false && damageSound.isPlaying == false)
			damageSound.Play();

		if (pShield < 0)
		{
			pHealth += pShield;
			pShield = 0;

			if (pHealth <= 0)
			{
				isGameOver = true;
				gameOver.SetActive(true);
				Time.timeScale = 0f;
				rbFPC.mouseLook.SetCursorLock(false);
			}
		}
			takenDamage = true;
	}

	IEnumerator RegenHealthRoutine()
	{
		while (true)
		{
			if (pHealth < maxRegen)
				pHealth += regenAmount;

			yield return new WaitForSeconds(regenTime);
		}
	}
}
