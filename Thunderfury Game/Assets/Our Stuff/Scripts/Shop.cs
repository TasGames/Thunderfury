using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour 
{
	[SerializeField] protected GameObject shopMenu;
	public GameObject parentPrefab;
	protected bool shopOpen = false;

	[SerializeField] protected GameObject shopIcon;
	[SerializeField] protected GameObject upgradeIcon;
	[SerializeField] protected GameObject exitIcon;
	protected GameObject currentIcon;

	void Start() 
	{
		currentIcon = shopIcon;
	}
	
	void Update()
	{
		if (Input.GetKeyDown("i"))
		{
			OpenShop();
		}
	}

	public void OpenShop()
	{
		if (shopOpen == false)
		{
			shopMenu.SetActive(true);
			Time.timeScale = 0.0f;
			Cursor.visible = true;
			shopOpen = true;
		}
		else
		{
			shopMenu.SetActive(false);
			Time.timeScale = 1.0f;
			Cursor.visible = false;
			shopOpen = false;
		}
	}

	public void DisplayShop()
	{
		if (currentIcon != shopIcon)
		{
			currentIcon.SetActive(false);
			shopIcon.SetActive(true);
			currentIcon = shopIcon;
		}
	}

	public void DisplayUpgrade()
	{
		if (currentIcon != upgradeIcon)
		{
			currentIcon.SetActive(false);
			upgradeIcon.SetActive(true);
			upgradeIcon = shopIcon;
		}
	}

	public void DisplayExit()
	{
		if (currentIcon != exitIcon)
		{
			currentIcon.SetActive(false);
			exitIcon.SetActive(true);
			currentIcon = exitIcon;
		}
	}

}
