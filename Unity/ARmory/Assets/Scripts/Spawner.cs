using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject enemy;
    public float spawnRadius = 5.0f;
    public float spawnTimer = 10f;
    public int maxInScene = 3;
    public int enemyCount = 0;
    public Transform[] spawnPoints;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     IEnumerator SpawnEnemy()
     {
         while(enemyCount < maxInScene)
         {
            Instantiate(enemy, spawnPoints[Random.Range(0, spawnPoints.Length)]);
            // Vector2 spawnPos = Player.Instance.transform.position; 
            // spawnPos += Random.insideUnitCircle.normalized * spawnRadius;

            // Instantiate(enemy, spawnPos, Quaternion.identity);
            yield return new WaitForSeconds(spawnTimer);
            enemyCount++; 
         }
        yield return null;
     }
}
