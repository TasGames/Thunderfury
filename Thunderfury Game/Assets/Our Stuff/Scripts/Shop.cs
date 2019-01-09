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

	void Start() 
	{

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

}
