using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamage : MonoBehaviour
{

    EnemyBehaviour enemyBehav;

    // Use this for initialization
    void Start()
    {
        enemyBehav = GetComponentInParent<EnemyBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            enemyBehav.DealDamage();
        }

    }
}
