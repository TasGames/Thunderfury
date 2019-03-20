using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialText : MonoBehaviour
{

    public TMP_Text tutText;


    // Use this for initialization
    void Start()
    {
        Add(tutText, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void Add(TMP_Text tutorialText, float length)
    {
        tutorialText.SetText(tutorialText.text);
    }
}
