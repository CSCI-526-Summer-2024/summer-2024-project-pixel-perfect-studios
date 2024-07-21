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

    public int ricochetInt = 2;
    public int advancedBulletRichochet = 3;
    public GameObject character;
    public float delayBeforeDespawn = 15f;

    private SendToGoogle googleForm;

    void Start() {
        Destroy(gameObject, delayBeforeDespawn);
        googleForm = GameObject.Find("GoogleFormManager").GetComponent<SendToGoogle>();
    }

    void Awake()
    {
        // Eliminate the effect of gravity on the bullet
        rb.gravityScale = 0;
        character = GameObject.Find("Character");
    }

    void Update() {

        if (Input.GetKey(KeyCode.Alpha1)) {
            Debug.Log("1 key pressed");
            SetRicochetCount(2);
        }
        if (Input.GetKey(KeyCode.Alpha2)) {
            Debug.Log("2 Pressed");
            SetRicochetCount(3);
        }
        if (Input.GetKey(KeyCode.Alpha3)) {
            Debug.Log("3 Pressed");
            SetRicochetCount(4);
        }
    }

    public void Shoot(Vector2 direction, Gun gun)
    {
        this.direction = direction.normalized;
        this.gun = gun;
        rb.velocity = this.direction * speed;
    }

    void SetRicochetCount(int count) {
        ricochetInt = count;
        Debug.Log("Ricochet Count Set To: " + ricochetInt);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.tag == "Enemy") {
            collision.gameObject.GetComponent<Enemy>().KilledEnemy();
            rb.velocity = direction * speed;
        }
        else if (collision.gameObject.tag == "BluePortal" || collision.gameObject.tag == "OrangePortal") {
            Debug.Log("Hit a portal");
            // If collision with Portal tag object, then call the gameObject's portalJump function
            // Destroy the Bullet
            Destroy(gameObject);
            collision.gameObject.GetComponent<Portal>().PortalJump();

        }
        else if (collision.gameObject.tag == "Zombie") {
            // If collision with Zombie tag object, then activate the full trajectory
            // Destroy the Bullet
            googleForm.AddZombiesKilled(collision.gameObject.name);
            Destroy(collision.gameObject);
            gun.advancedBullet += 5;
            StartCoroutine(DestroyBulletAfterDelay(0.02f));
            Debug.Log("Hit a zombie");
        }
        else {
            hitCount++;
            float sphereRadius = 0.3f;

            if(collision.gameObject.tag == "ButtonEffective"){
                collision.gameObject.GetComponent<Button>().hitButton();
            }
            // Reflect the bullet's direction when it hits something
            Vector2 normal = collision.contacts[0].normal;
            // Update the position of the center of bullet
            gameObject.transform.position = collision.contacts[0].point + normal * sphereRadius;
            direction = Vector2.Reflect(direction, normal);
            rb.velocity = direction * speed;
        }

        if (hitCount == ricochetInt && gun.fullTrajectory == false)
        {
            StartCoroutine(DestroyBulletAfterDelay(0.02f));
        }
        else if (hitCount == advancedBulletRichochet && gun.fullTrajectory == true)
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
        gun.bullet = null;
    }
}

