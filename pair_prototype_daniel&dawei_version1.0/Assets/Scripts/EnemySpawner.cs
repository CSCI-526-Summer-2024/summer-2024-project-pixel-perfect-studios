using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject fallingObject;
    public float spawnInterval = 2f;
    public float xMin = -8f;
    public float xMax = 8f;
    public float spawnHeight = 6f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies() {
        while (true) {
            float randomX = Random.Range(xMin, xMax);
            Vector3 spawnPosition = new Vector3(randomX, spawnHeight, 0f);

            Instantiate(fallingObject, spawnPosition, Quaternion.identity);

            yield return new WaitForSeconds(spawnInterval);
        }
    }


}
