using UnityEngine;

public class DrawBox : MonoBehaviour
{

    protected GameObject player;

    protected EnemySpawner spawning;

    [SerializeField] int minDistance;
    [SerializeField] int maxDistance;
    private float distanceToPlayer;

    void Start()
    {
        if (spawning == null)
            spawning = GameObject.FindGameObjectWithTag("EnemyManager").GetComponent<EnemySpawner>();

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (this.gameObject.tag == "InactiveSpawn" || this.gameObject.tag == "ActiveSpawn")
            distanceToPlayer = Vector3.Distance(player.transform.position, transform.position); //Check distance between spawn point and player

        if (this.gameObject.tag == "InactiveSpawn") //If not active, check if within range to become active again
        {
            if (distanceToPlayer <= maxDistance || distanceToPlayer >= minDistance)
            {
                if (!spawning.activeSpawns.Contains(this.gameObject))   //If this spawn is not in the active spawn point list
                {
                    this.gameObject.tag = "ActiveSpawn";
                    spawning.activeSpawns.Add(this.gameObject); //Add to active spawn point list
                }
            }
        }

        if (this.gameObject.tag == "ActiveSpawn")   //If active, check if outside active range
        {
            if (distanceToPlayer < minDistance || distanceToPlayer > maxDistance)
            {
                if (spawning.activeSpawns.Contains(this.gameObject))    //If this spawn is in the active spawn point list
                {
                    this.gameObject.tag = "InactiveSpawn";
                    spawning.activeSpawns.Remove(this.gameObject);  //Remove from active spawn point list
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        if (this.gameObject.tag == "ActiveSpawn")
        {
            Gizmos.color = Color.green; //Spawn point green when active

            //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
            Gizmos.DrawWireCube(transform.position, transform.localScale);
        }
        else if (this.gameObject.tag == "InactiveSpawn")
        {
            Gizmos.color = Color.red;   //Spawn point red when inactive

            //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
            Gizmos.DrawWireCube(transform.position, transform.localScale);
        }
        else
        {
            Gizmos.color = Color.magenta;

            Gizmos.DrawWireCube(transform.position, transform.localScale);
        }
    }
}
