using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    WaveManager wManager;

    public GameObject tutShopBox;   //Tells player to open shop
    public GameObject shopOpenBox;  //Shows text when shop opens
    public GameObject wepSwap;
    public GameObject wepTest;
    public GameObject endTutBox;
    public GameObject shopScreen;   //The shop screen

    bool tutShopBoxCheck;
    bool endTutBoxCheck;

    // Use this for initialization
    void Start()
    {
        wManager = GetComponent<WaveManager>();
        tutShopBoxCheck = true;
        endTutBoxCheck = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (wManager.waveCounter >= 2 && tutShopBoxCheck)
        {
            tutShopBox.SetActive(true);
            shopOpenBox.SetActive(true);
            wepSwap.SetActive(true);
            wepTest.SetActive(true);
            tutShopBoxCheck = false;
            endTutBoxCheck = true;
        }
        if (wManager.waveCounter >= 3 && endTutBoxCheck){
            endTutBox.SetActive(true);
        }
        // if (shopScreen.activeInHierarchy && shopOpenBoxCheck)
        // {
        //     shopOpenBox.SetActive(true);
        //     shopOpenBoxCheck = false;
        // }
    }
}
