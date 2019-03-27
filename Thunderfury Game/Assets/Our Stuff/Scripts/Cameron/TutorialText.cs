using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TutorialText : MonoBehaviour
{

    public TMP_Text text;

	public float textDisplayTime;

    // Use this for initialization
    void Start()
    {
        //text.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(ChangeText());
        }

    }

    IEnumerator ChangeText()
    {
		text.gameObject.SetActive(true);	//Display text

        yield return new WaitForSeconds(textDisplayTime);	//Display for # secs
		Debug.Log("Works");

		text.gameObject.SetActive(false);	//Hide text

		this.gameObject.SetActive(false);   //Disable trigger
        yield break;
    }

}
