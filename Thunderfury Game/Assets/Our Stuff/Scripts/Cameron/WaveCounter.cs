using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class WaveCounter : MonoBehaviour
{

    public TextMeshProUGUI waveText;
    WaveManager manager;

    // Use this for initialization
    void Start()
    {
        waveText = transform.Find("Wave#").GetComponent<TextMeshProUGUI>();

        manager = GameObject.Find("EnemyManager").GetComponent<WaveManager>();
    }

    // Update is called once per frame
    void Update()
    {
        waveText.text = manager.waveCounter.ToString();
    }
}
