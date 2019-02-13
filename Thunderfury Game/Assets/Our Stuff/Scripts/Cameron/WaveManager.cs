using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{

    public enum SpawnState
    {
        Spawning,
        Waiting,
        Counting
    };


    [System.Serializable]
    public class Wave
    {
        public string name;            //Name of the wave 

        public int WaveEnemyCount;     //Amount of enemies for the entire wave
        public int activeAtOnce;       //Amount of enemies that can be in the level at one time

        public float spawnRate;        //Rate to spawn enemies
    }

    [HideInInspector]
    public int enemiesRemaining;

    public Wave[] waves;

    protected EnemySpawner eSpawner;

    private int nextWave = 0;

    public float timeBetweenWaves = 5.0f;   //Time to wait before spawning next wave
    private float waveCountdown;

    private float checkCountdown = 1.0f;    //Check if enemies alive every 1 second

    public SpawnState state = SpawnState.Counting;

    void Start()
    {
        waveCountdown = timeBetweenWaves;

        eSpawner = GetComponent<EnemySpawner>();
    }

    void Update()
    {
        if (state == SpawnState.Waiting)    //If game state is waiting to spawn
        {
            if (eSpawner.activeEnemies.Count == 0)
            {
                Debug.Log("Enemies Remaining: " + enemiesRemaining);
                StartCoroutine(SpawnWave(waves[nextWave]));
            }

            if (!EnemyIsAlive())            //If all enemies are dead
            {
                WaveCompleted();            //Wave is complete
            }
            else
            {
                return;                     //Else keep checking
            }
        }

        if (waveCountdown <= 0)
        {
            if (state != SpawnState.Spawning)   //If game is not spawning
            {
                state = SpawnState.Spawning;
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

        if (nextWave + 1 > waves.Length - 1)
        {
            nextWave = 0;
            Debug.Log("All waves completed. Looping.");
        }
        else
        {
            nextWave++;
        }
    }

    bool EnemyIsAlive()     //Check if any enemies are alive
    {
        checkCountdown -= Time.deltaTime;
        if (checkCountdown <= 0.0f)
        {
            checkCountdown = 1.0f;
            if (GameObject.FindGameObjectWithTag("Enemy1") == null)
            {
                if (enemiesRemaining == 0)
                {
                    return false;
                }
            }

        }
        return true;
    }

    IEnumerator SpawnWave(Wave _wave)   //Spawn next wave
    {
        if (state == SpawnState.Spawning)
            enemiesRemaining = _wave.WaveEnemyCount;

        Debug.Log("Spawning wave: " + _wave.name);


        //for(int i = 0; i < _wave.enemyCount; i++)
        while (eSpawner.activeEnemies.Count < _wave.activeAtOnce)
        {
            if (eSpawner.activeEnemies.Count == enemiesRemaining)
            {
                yield break;
            }
            else
            {
                //SpawnEnemy(_wave.enemy);
                eSpawner.PickSpawnLocation();

                yield return new WaitForSeconds(1.0f / _wave.spawnRate);
            }
        }

        state = SpawnState.Waiting;

        yield break;
    }

}
