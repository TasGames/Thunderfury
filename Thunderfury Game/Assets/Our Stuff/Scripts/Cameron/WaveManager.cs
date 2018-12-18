using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour {

    public enum SpawnState {
        Spawning,
        Waiting,
        Counting
    };


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
    public float waveCountdown;

    private float checkCountdown = 1.0f;

    public SpawnState state = SpawnState.Counting;

    void Start()
    {
        waveCountdown = timeBetweenWaves;

    }

    void Update()
    {
        if (state == SpawnState.Waiting)
        {
            if (!EnemyIsAlive())
            {
                WaveCompleted();
            }
            else
            {
                return;
            }
        }

        if (waveCountdown <= 0)
        {
            if (state != SpawnState.Spawning)
            {
                StartCoroutine(SpawnWave(waves[nextWave]));
            }
        }
        else
        {
            waveCountdown -= Time.deltaTime;
        }
    }

    void WaveCompleted()
    {
        Debug.Log("Wave Completed");

        state = SpawnState.Counting;
        waveCountdown = timeBetweenWaves;

        if(nextWave + 1 > waves.Length - 1)
        {
            nextWave = 0;
            Debug.Log("All waves completed. Looping.");
        }
        else
        {
            nextWave++;
        }
    }

    bool EnemyIsAlive()
    {
        checkCountdown -= Time.deltaTime;
        if (checkCountdown <= 0.0f)
        {
            checkCountdown = 1.0f;
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                return false;
            }
        }
        return true;
    }

    IEnumerator SpawnWave(Wave _wave)
    {
        Debug.Log("Spawning wave: " + _wave.name);
        state = SpawnState.Spawning;

        for(int i = 0; i < _wave.enemyCount; i++)
        {
            SpawnEnemy(_wave.enemy);
            yield return new WaitForSeconds(1.0f / _wave.spawnRate);
        }

        state = SpawnState.Waiting;

        yield break;
    }

    void SpawnEnemy(Transform _enemy)
    {
        Instantiate(_enemy, transform.position, transform.rotation);
        Debug.Log("Spawning Enemy: " + _enemy.name);
    }
}
