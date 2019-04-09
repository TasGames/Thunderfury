using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_soundcontroller : MonoBehaviour {

[SerializeField] private AudioClip[] enemychatter;

public AudioSource source;
public AudioClip beinghit;
private float timer;
public float timelimit;


	// Use this for initialization
	void Start () {
	

	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if(timer >= timelimit)
		{
			Enemyshout();
            timer = 0;
		}
		
	
	}
	

	void Enemyshout()
	{

int n = Random.Range(1, enemychatter.Length);
source.clip = enemychatter [n];
source.PlayOneShot(source.clip);

	}
}
