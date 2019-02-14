using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemiesRemainingUI : MonoBehaviour
{

    public TextMeshProUGUI enemyText;
    WaveManager wManager;

    // Use this for initialization
    void Start()
    {
        enemyText = transform.Find("Enemy#").GetComponent<TextMeshProUGUI>();

        wManager = GameObject.Find("EnemyManager").GetComponent<WaveManager>();
    }

    // Update is called once per frame
    void Update()
    {
        enemyText.text = wManager.enemiesRemaining.ToString();
    }
}
