using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    private SendToGoogle googleForm;
    private bool _collision = false;

    void Start()
    {
        googleForm = GameObject.Find("GoogleFormManager").GetComponent<SendToGoogle>();
    }

    public void KilledEnemy()
    {
        //Debug.Log("Killed one enemy");
        Debug.Log(transform.position); //Collect coordinate of collision
        Vector2 position2D = new Vector2(transform.position.x, transform.position.y);
        googleForm.AddEnemiesKilled(position2D);
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!_collision && collision.gameObject.name == "Character" && gameObject.tag != "GoldEnemy")
        {
            //Debug.Log(transform.position); //Collect coordinate of collision
            _collision = true;
            googleForm.DeathEnemy();
            Debug.Log("Hit Enemy and Die!!");
            OverlayController.instance.characterStatus = OverlayController.playerStatus.LOSE;
        }
    }
}