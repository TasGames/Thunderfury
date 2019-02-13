using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Upgrade : MonoBehaviour 
{
	[SerializeField] protected TextMeshProUGUI currentCredits;
	[SerializeField] protected TextMeshProUGUI requiredCredits;
	protected int totalCost = 0;
	protected int possibleCredits;


	void Start()
	{
		
	}
	
	void Update() 
	{
		possibleCredits = HUD.totalScore;

		if (requiredCredits != null)
		{
			requiredCredits.text = "Required: $" + totalCost;

			if (currentCredits != null)
				currentCredits.text = "Credits: $" + HUD.totalScore + " > $" + possibleCredits;
		}
		else if (currentCredits != null)
			currentCredits.text = "Credits: $" + HUD.totalScore;
	}
}
