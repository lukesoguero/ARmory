using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject enemy;
    public float spawnRadius = 5.0f;
    public float spawnTimer = 10f;

    private int enemyCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(SpawnEnemy());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     IEnumerator SpawnEnemy()
     {
         while(enemyCount < 100)
         {
            Vector2 spawnPos = Player.Instance.transform.position; 
            spawnPos += Random.insideUnitCircle.normalized * spawnRadius;

            Instantiate(enemy, spawnPos, Quaternion.identity);
            yield return new WaitForSeconds(spawnTimer);
            enemyCount++; 
         }
        yield return null;
     }
}
