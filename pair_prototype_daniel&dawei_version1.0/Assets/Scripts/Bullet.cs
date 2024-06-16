using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bullet : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float speed = 30f;
    //[SerializeField] float lifetime = 5f; // Bullet lifetime in seconds

    private Vector2 direction;
    private int hitCount = 0;
    private Gun gun;

    public int ricochetInt = 10;
    public GameObject character;

    void Awake()
    {
        // Eliminate the effect of gravity on the bullet
        rb.gravityScale = 0;
        character = GameObject.Find("Character");
    }

    public void Shoot(Vector2 direction, Gun gun)
    {
        this.direction = direction.normalized;
        this.gun = gun;
        rb.velocity = this.direction * speed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Enemy")
        {
            hitCount++;
            if(collision.gameObject.tag == "ButtonEffective"){
                collision.gameObject.GetComponent<Button>().hitButton();
            }
            // Reflect the bullet's direction when it hits something
            Vector2 normal = collision.contacts[0].normal;
            direction = Vector2.Reflect(direction, normal);
            rb.velocity = direction * speed;
        }
        else if (collision.gameObject.tag == "Enemy") {
            collision.gameObject.GetComponent<Enemy>().KilledEnemy();
            rb.velocity = direction * speed;
        }
        else if (collision.gameObject.tag == "BluePortal" || collision.gameObject.tag == "OrangePortal"){ {
            // If collision with Portal tag object, then call the gameObject's portalJump function
            
            // Destroy the Bullet
            Destroy(gameObject);
            collision.gameObject.GetComponent<Portal>().PortalJump();

        }

        }
        if (hitCount == ricochetInt)
        {
            StartCoroutine(DestroyBulletAfterDelay(0.02f));
        }
    }
    
    // This function also teleports the character
    private IEnumerator DestroyBulletAfterDelay(float delay)
    {  
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Log the position right before destruction
        character.transform.position = transform.position;

        // Destroy the bullet
        Destroy(gameObject);
    }
}

