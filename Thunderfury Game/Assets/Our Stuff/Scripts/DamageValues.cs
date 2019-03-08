using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageValues : MonoBehaviour 
{
	public TextMesh damageText;
	void Start() 
	{
		damageText = GetComponent<TextMesh>();
	}

	public void DamageValue(float damage)
	{
		damageText.text = damage.ToString();
	}
	
}
