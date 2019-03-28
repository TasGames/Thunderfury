using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class WaveManager : MonoBehaviour
{

    public enum SpawnState
    {
        Beginning,  //Initial state at beginning of the game
        Spawning,   //Spawning enemies
        Waiting,    //Waiting till next Spawning state (during wave)
        Counting,   //Counting down till next wave
        ShoppingTime   //Do nothing
    };


    [System.Serializable]
    public class Wave
    {
        public int waveEnemyCount;     //Amount of enemies for the entire wave
        public int activeAtOnce;       //Amount of enemies that can be in the level at one time

        public float spawnRate;        //Rate to spawn enemies
    }

    [System.Serializable]
    public class WaveModifiers
    {
        public float enemyNumIncrease;  //# to increase enemy amount by
        public float activeNumIncrease; //# to increase active enemy amount by
        //public float enemyDmgIncrease;  //# to increase damage enemies deal by
        public int maxWaveToIncrease;   //Wave to stop increasing values (set max difficulty)
    }

    // # OF ENEMIES LEFT IN THE WAVE
    //[HideInInspector]
    public int enemiesRemaining;

    // COUNTS CURRENT WAVE
    [HideInInspector]
    public int waveCounter = 0; //# IN UI

    // WAVE
    public Wave wave;

    // MODIFIERS
    public WaveModifiers modifier;

    // TO SPAWN ENEMIES
    protected EnemySpawner eSpawner;
    [HideInInspector]
    public bool canSpawnNextWave = true;

    // TIME TO WAIT BEFORE SPAWNING NEXT WAVE
    public float timeBetweenWaves = 5.0f;
    private float waveCountdown;

    // CHECK EVERY # SECONDS IF ANY ENEMIES ARE ALIVE
    private float checkCountdown = 1.0f;    //Check if enemies alive every 1 second

    // DEFAULT WAVE STATE
    public SpawnState state;

    // TELEPORT PLAYER ON WAVE END
    [Title("Teleport")]
    public Transform teleportSpot;
    Transform thePlayer;
    // TELEPORT SCREEN FLASH
    public Image fadeImage;
    public float fadeSpeed;

    [SerializeField] protected PlayerHealth pH;

    void Start()
    {
        state = SpawnState.Beginning;

        waveCountdown = timeBetweenWaves;

        eSpawner = GetComponent<EnemySpawner>();

        teleportSpot = GameObject.Find("TeleportSpot").transform;
        thePlayer = GameObject.Find("Player").transform;
    }

    void Update()
    {
        if (state == SpawnState.Waiting)    //If game state is waiting to spawn
        {
            if (enemiesRemaining > 0)
            {
                if (eSpawner.activeEnemies.Count == 0)
                {
                    Debug.Log("Enemies Remaining: " + enemiesRemaining);
                    StartCoroutine(SpawnEnemies());
                }
            }


            if (EnemyIsAlive() == false)            //If all enemies are dead
            {
                ShoppingTime();            //Wave is complete, teleport player to shop
            }
            else
            {
                return;                     //Else keep checking
            }
        }

        if (waveCountdown <= 0)             //Once countdown till next wave is completed
        {
            if (state != SpawnState.Spawning || state != SpawnState.ShoppingTime)   //If game is not spawning
            {
                StartCoroutine(SpawnEnemies());
            }
        }
        else if (state == SpawnState.Counting)
        {
            waveCountdown -= Time.deltaTime;
        }
    }

    public void ShoppingTime()
    {
        state = SpawnState.ShoppingTime;
        waveCounter++;                      //Increase wave # in UI

        StartCoroutine(ScreenFadeOut());    //Teleport screen flash

        //Teleport player to shop location
        thePlayer.transform.position = teleportSpot.transform.position;
        thePlayer.transform.rotation = teleportSpot.transform.rotation;

        pH.pShield = pH.maxShield;

        //Change Wave Modifiers
        if (waveCounter <= modifier.maxWaveToIncrease)
        {
            wave.waveEnemyCount = (int)(wave.waveEnemyCount * modifier.enemyNumIncrease);
            wave.activeAtOnce = (int)(wave.activeAtOnce * modifier.activeNumIncrease);

        }

        canSpawnNextWave = true;    //Enables wave trigger
    }

    public void StartWave()
    {
        state = SpawnState.Counting;    //Begin countdown till spawning starts
        waveCountdown = timeBetweenWaves;

        enemiesRemaining = wave.waveEnemyCount;
    }

    bool EnemyIsAlive()     //Check if any enemies are alive
    {
        checkCountdown -= Time.deltaTime;
        if (checkCountdown <= 0.0f)
        {
            checkCountdown = 1.0f;
            if (eSpawner.activeEnemies.Count == 0)
            {
                if (enemiesRemaining == 0)
                {
                    return false;
                }
            }
        }
        return true;
    }

    IEnumerator SpawnEnemies()   //Spawn next wave
    {
        //for(int i = 0; i < _wave.enemyCount; i++)
        while (eSpawner.activeEnemies.Count < wave.activeAtOnce)
        {
            if (eSpawner.activeEnemies.Count >= enemiesRemaining)
            {
                if (state != SpawnState.Waiting && state != SpawnState.ShoppingTime)
                    state = SpawnState.Waiting;

                yield break;
            }
            else if (eSpawner.activeEnemies.Count < enemiesRemaining)
            {
                state = SpawnState.Spawning;
                eSpawner.PickSpawnLocation();

                yield return new WaitForSeconds(1.0f / wave.spawnRate);
            }
        }

        yield break;
    }

    private YieldInstruction fadeInstruction = new YieldInstruction();
    IEnumerator ScreenFadeOut()
    {
        fadeImage.gameObject.SetActive(true); 
        float elapsedTime = 0.0f;
        Color c = fadeImage.color;
        while (elapsedTime < fadeSpeed)
        {
            yield return fadeInstruction;
            elapsedTime += Time.deltaTime;
            c.a = 1.0f - Mathf.Clamp01(elapsedTime / fadeSpeed);
            fadeImage.color = c;
        }
        fadeImage.gameObject.SetActive(false); 
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
