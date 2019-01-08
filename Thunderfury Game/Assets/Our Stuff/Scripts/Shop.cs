using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour 
{
	[SerializeField] protected GameObject shopMenu;
	[SerializeField] protected GameObject gunPrefab;
	[SerializeField] protected GameObject gunAmmoPrefab;
	[SerializeField] protected GameObject parentPrefab;
	protected bool shopOpen = false;
	protected bool ownsIt = false;

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

	public void Buy()
	{
		GameObject gun = Instantiate(gunPrefab, gunPrefab.transform.position + parentPrefab.transform.position, parentPrefab.transform.rotation, parentPrefab.transform);
		gun.SetActive(false);
	}
}
