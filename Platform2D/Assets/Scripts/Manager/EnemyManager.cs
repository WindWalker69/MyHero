using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance = null;

    private GameObject[] spawnPoints;

    public GameObject enemy;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if (instance != this)
            Destroy(gameObject);

        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
    }

    public void SpawnEnemies()
    {
        for(int i = 0; i < spawnPoints.Length; i++)
        {
            Instantiate(enemy, spawnPoints[i].transform.position, Quaternion.identity);
        }
    }
}
