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

	[SerializeField] protected GameObject upgradeMenu;
	[SerializeField] protected GameObject subShopMenu;
	[SerializeField] protected GameObject baseShop;
	[SerializeField] protected GameObject hud;

	protected GameObject currentIcon;

	[SerializeField] protected RigidbodyFirstPersonController rbFPC;

	void Start() 
	{
		currentIcon = shopIcon;
	}
	
	void Update()
	{
		/*if (Input.GetKeyDown("i"))
		{
			OpenShop();
		}*/
	}

	public void OpenShop()
	{
		if (shopOpen == false)
		{
			shopMenu.SetActive(true);
			hud.SetActive(false);
			Time.timeScale = 0.0f;
			rbFPC.mouseLook.SetCursorLock(false);
			shopOpen = true;
		}
		else
		{
			shopMenu.SetActive(false);
			hud.SetActive(true);
			Time.timeScale = 1.0f;
			rbFPC.mouseLook.SetCursorLock(true);
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

	public void OpenUpgrade()
	{
		baseShop.SetActive(false);
		upgradeMenu.SetActive(true);
	}

	public void CloseUpgrade()
	{
		upgradeMenu.SetActive(false);
		baseShop.SetActive(true);
	}

	public void OpenSubShop()
	{
		baseShop.SetActive(false);
		subShopMenu.SetActive(true);
	}

	public void CloseSubShop()
	{
		subShopMenu.SetActive(false);
		baseShop.SetActive(true);
	}

	public void CloseShop()
	{
		shopMenu.SetActive(false);
		hud.SetActive(true);
		Time.timeScale = 1.0f;
		Cursor.visible = false;
		shopOpen = false;
	}

}
