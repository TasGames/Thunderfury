using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{

    public enum SpawnState
    {
        Spawning,
        Waiting,
        Counting,
        DoNothing
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
    [HideInInspector]
    public int waveCounter = 1;

    public Wave[] waves;

    public Transform teleportSpot;
    Transform thePlayer;
    protected EnemySpawner eSpawner;

    private int nextWave = 0;

    public float timeBetweenWaves = 5.0f;   //Time to wait before spawning next wave
    private float waveCountdown;

    private float checkCountdown = 1.0f;    //Check if enemies alive every 1 second

    public SpawnState state = SpawnState.DoNothing;

    public Image fadeImage;
    public float fadeSpeed;

    void Start()
    {
        waveCountdown = timeBetweenWaves;

        eSpawner = GetComponent<EnemySpawner>();

        teleportSpot = GameObject.Find("TeleportSpot").transform;
        thePlayer = GameObject.Find("Player").transform;
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
                ShoppingTime();            //Wave is complete, teleport player to shop
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
        else if (state == SpawnState.Counting)
        {
            waveCountdown -= Time.deltaTime;
        }
    }

    public void ShoppingTime()
    {
        state = SpawnState.DoNothing;
        waveCounter++;
        StartCoroutine(ScreenFadeOut());    //Teleport screen flash

        //Teleport player to shop location
        thePlayer.transform.position = teleportSpot.transform.position;
        thePlayer.transform.rotation = teleportSpot.transform.rotation;

        //Once stepped into trigger to start next wave:
        WaveCompleted();    //Trigger WaveCompleted to trigger countdown for next wave
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
                if (state != SpawnState.Waiting)
                    state = SpawnState.Waiting;

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

    private YieldInstruction fadeInstruction = new YieldInstruction();
    IEnumerator ScreenFadeOut()
    {
        float elapsedTime = 0.0f;
        Color c = fadeImage.color;
        while (elapsedTime < fadeSpeed)
        {
            yield return fadeInstruction;
            elapsedTime += Time.deltaTime;
            c.a = 1.0f - Mathf.Clamp01(elapsedTime / fadeSpeed);
            fadeImage.color = c;
        }
    }

    IEnumerator ScreenFadeIn()
    {
        float elapsedTime = 0.0f;
        Color c = fadeImage.color;
        while (elapsedTime < fadeSpeed)
        {
            yield return fadeInstruction;
            elapsedTime += Time.deltaTime;
            c.a = Mathf.Clamp01(elapsedTime / fadeSpeed);
            fadeImage.color = c;
        }
    }
}
