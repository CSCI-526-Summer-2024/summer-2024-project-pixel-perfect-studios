using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    public SendToGoogle googleForm;
    public void KilledEnemy()
    {
        //Debug.Log("Killed one enemy");

        Destroy(gameObject);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Character")
        {
            Debug.Log("Hit Enemy!!");
            googleForm.DeathEnemy();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}