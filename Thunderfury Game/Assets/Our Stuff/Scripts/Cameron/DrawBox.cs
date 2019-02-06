using UnityEngine;

public class DrawBox : MonoBehaviour
{

    public GameObject player;
    EnemySpawner spawning;
    void Start()
    {
        spawning = GetComponent<EnemySpawner>();

        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (Vector3.Distance(player.transform.position, transform.position) <= 10 && this.gameObject.tag == "InactiveSpawn")
        {
            if (!spawning.activeSpawns.Contains(this.gameObject))
            {
                this.gameObject.tag = "ActiveSpawn";
                spawning.activeSpawns.Add(this.gameObject);
            }
        }

        if (Vector3.Distance(player.transform.position, transform.position) > 10 && this.gameObject.tag == "ActiveSpawn") ;
        {
            if (spawning.activeSpawns.Contains(this.gameObject))
            {
                this.gameObject.tag = "InactiveSpawn";
                spawning.activeSpawns.Remove(this.gameObject);
            }
        }
    }

    void OnDrawGizmos()
    {
        if (this.gameObject.tag == "ActiveSpawn")
        {
            Gizmos.color = Color.green;

            //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
            Gizmos.DrawWireCube(transform.position, transform.localScale);
        }
        else if (this.gameObject.tag == "InactiveSpawn")
        {
            Gizmos.color = Color.red;

            //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
            Gizmos.DrawWireCube(transform.position, transform.localScale);
        }
    }
}
