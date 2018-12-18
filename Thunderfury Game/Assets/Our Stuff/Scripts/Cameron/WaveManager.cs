using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour {

    [System.Serializable]
    public class Wave
    {
        public string name;
        public Transform enemy;
        public int enemyCount;
        public float spawnRate;
    }

    public Wave[] waves;

    private int nextWave = 0;

    public float timeBetweenWaves = 5.0f;
    public float waveCountdown = 0.0f;

    void Start()
    {
        waveCountdown = timeBetweenWaves;

    }

    void Update()
    {
        if (waveCountdown <= 0)
        {

        }
    }
}
