﻿using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopDisplayCredits : MonoBehaviour 
{
	[SerializeField] protected TextMeshProUGUI currentCredits;
	
	void Update() 
	{
		currentCredits.text = "Credits: $" + HUD.totalScore;
	}
}
