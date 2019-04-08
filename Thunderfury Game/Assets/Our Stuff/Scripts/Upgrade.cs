using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Upgrade : MonoBehaviour 
{
	[System.Serializable]
	protected struct UpgradeStuff
	{
		public string statName;
		public GameObject levelHolder;
		public TextMeshProUGUI currentStatText;
		[HideInInspector] public TextMeshProUGUI newStatText;
		[HideInInspector] public float newStat;
		[HideInInspector] public float statIncrease;
		[HideInInspector] public Button levelUp;
		[HideInInspector] public Button levelDown;
		[HideInInspector] public int level;
		[HideInInspector] public TextMeshProUGUI levelText;
	}

	[SerializeField] protected UseGun[] gun;
	[SerializeField] protected Button[] toggleButton;
	[SerializeField] protected UpgradeStuff[] UpgradeInfo;
	
	[SerializeField] protected TextMeshProUGUI gunName;	
	[SerializeField] protected TextMeshProUGUI gunAmmo;
	[SerializeField] protected TextMeshProUGUI nGunAmmo;


	[Title("Credits")]
	[SerializeField] protected TextMeshProUGUI currentCredits;
	[SerializeField] protected TextMeshProUGUI requiredCredits;
	protected int totalCost = 0;
	protected int possibleCredits;

	protected UseGun currentGun;
	protected bool bootedUpOnce = false;
	protected bool gotIncrease = false;


	protected int newMagSize;
	protected int newMaxAmmo;

	void OnEnable()
	{
		if (bootedUpOnce == false)
		{
			for (int i = 0; i < gun.Length; i++)
			{
				int x = i;
				toggleButton[i].onClick.AddListener(() => Toggle(x));
			}

			for (int i = 0; i < UpgradeInfo.Length; i++)
			{
				UpgradeInfo[i].newStatText = UpgradeInfo[i].levelHolder.GetComponent<TextMeshProUGUI>();
				UpgradeInfo[i].levelText = UpgradeInfo[i].levelHolder.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();

				UpgradeInfo[i].levelUp = UpgradeInfo[i].levelHolder.transform.GetChild(1).gameObject.GetComponent<Button>();
				UpgradeInfo[i].levelDown = UpgradeInfo[i].levelHolder.transform.GetChild(0).gameObject.GetComponent<Button>();

				int x = i;
				UpgradeInfo[i].levelUp.onClick.AddListener(() => levelUp(x));
				UpgradeInfo[i].levelDown.onClick.AddListener(() => levelDown(x));
			}

			bootedUpOnce = true;
		}

		UpdateCredits();				
	}

	void Toggle(int i)
	{
		if (gun[i] != null)
		{
			currentGun = gun[i];
			totalCost = 0;

			DisplayCurrentStats();
			CreateNewStats();
			DisplayNewStats();
			DisplayLevel();
			UpdateCredits();
		}
	}

	void levelUp(int i)
	{
		if (currentGun.gun.maxLevel >= UpgradeInfo[i].level + 1 && totalCost + currentGun.gun.upgradeCost <= HUD.totalScore)
		{
			UpgradeInfo[i].level += 1;
			UpgradeInfo[i].newStat += UpgradeInfo[i].statIncrease;
			totalCost += currentGun.gun.upgradeCost;
			DisplayNewStats();
			DisplayLevel();
			UpdateCredits();
		}
	}

	void levelDown(int i)
	{
		if (UpgradeInfo[i].level - 1 >= currentGun.level[i])
		{
			UpgradeInfo[i].level -= 1;
			UpgradeInfo[i].newStat -= UpgradeInfo[i].statIncrease;
			totalCost -= currentGun.gun.upgradeCost;
			DisplayNewStats();
			DisplayLevel();
			UpdateCredits();
		}
	}

    public void Confirm()
    {
		for (int i = 0; i < UpgradeInfo.Length; i++)
		{
			currentGun.prefStat[i] = UpgradeInfo[i].newStat;
			currentGun.level[i] = UpgradeInfo[i].level;
		}

		DisplayCurrentStats();

        HUD.totalScore = possibleCredits;
        totalCost = 0;

		UpdateCredits();
    }

	void DisplayCurrentStats()
	{
		if (gunName != null)
			gunName.text = currentGun.gun.name;

		for (int i = 0; i < UpgradeInfo.Length; i++)
		{
			if (UpgradeInfo[i].currentStatText != null)
            	UpgradeInfo[i].currentStatText.text = UpgradeInfo[i].statName + ": " + currentGun.prefStat[i];
		}

		if (gunAmmo != null)
			gunAmmo.text = "Ammo: " + currentGun.prefMag + " / " + currentGun.prefMaxAmmo;
	}

	void DisplayNewStats()
	{
		for (int i = 0; i < UpgradeInfo.Length; i++)
		{
       		if (UpgradeInfo[i].newStatText != null)
            	UpgradeInfo[i].newStatText.text = UpgradeInfo[0].statName + ": " + UpgradeInfo[i].newStat;
		}

		if (nGunAmmo != null)
			nGunAmmo.text = "Ammo: " + newMagSize + " / " + newMaxAmmo;
	}

	void DisplayLevel()
	{
		for (int i = 0; i < UpgradeInfo.Length; i++)
			UpgradeInfo[i].levelText.text = "" + UpgradeInfo[i].level;
	}

	void CreateNewStats()
	{
		for (int i = 0; i < UpgradeInfo.Length; i++)
		{
			UpgradeInfo[i].newStat = currentGun.prefStat[i];
			UpgradeInfo[i].level = currentGun.level[i];
		}

		newMagSize = currentGun.prefMag;
		newMaxAmmo = currentGun.prefMaxAmmo;

		UpgradeInfo[0].statIncrease = currentGun.gun.damageIncrease;
		UpgradeInfo[1].statIncrease = currentGun.gun.impactIncrease;
		UpgradeInfo[2].statIncrease = currentGun.gun.fireRateIncrease;
		UpgradeInfo[3].statIncrease = currentGun.gun.range;
		UpgradeInfo[4].statIncrease = currentGun.gun.recoilIncrease;
		UpgradeInfo[5].statIncrease = currentGun.gun.reloadDecrease;
	}

	void UpdateCredits()
	{
		possibleCredits = HUD.totalScore - totalCost;

		if (requiredCredits != null)
		{
			requiredCredits.text = "Required: ¥" + totalCost;

			if (currentCredits != null)
				currentCredits.text = "Credits: ¥" + HUD.totalScore + " > ¥" + possibleCredits;
		}
		else if (currentCredits != null)
			currentCredits.text = "Credits: ¥" + HUD.totalScore;
	}
}
