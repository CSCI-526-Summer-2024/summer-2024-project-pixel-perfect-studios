using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    public SendToGoogle googleForm;
    private bool _collision = false;
    public void KilledEnemy()
    {
        //Debug.Log("Killed one enemy");

        Destroy(gameObject);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!_collision && collision.gameObject.name == "Character")
        {
            _collision = true;
            googleForm.DeathEnemy();
            Debug.Log("Hit Enemy and Die!!");
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            OverlayController.instance.characterStatus = OverlayController.playerStatus.LOSE;
        }
    }
}