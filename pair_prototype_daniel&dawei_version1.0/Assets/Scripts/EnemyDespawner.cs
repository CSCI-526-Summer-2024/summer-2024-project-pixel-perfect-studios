using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDespawner : MonoBehaviour
{
    public float delayBeforeDespawn = 10f;
    void Start()
    {
        Destroy(gameObject, delayBeforeDespawn);
    }
}
