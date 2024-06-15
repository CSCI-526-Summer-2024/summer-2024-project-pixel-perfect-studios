using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bulletPrefab; // Assign the Bullet prefab in the Inspector
    public Transform shootPoint; // The point from where the bullet will be shot
    public int bulletsLeft = 20;
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && (bulletsLeft > 0)) // Detect mouse click
        { 
            Shoot();
            bulletsLeft--;
        }
    }

    public void Shoot()
    {
        // Create a bullet instance at the shootPoint's position and rotation
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
        
        // Calculate the direction to shoot the bullet
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - shootPoint.position;

        // Get the Bullet script component and call Shoot method
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.Shoot(direction, this);
    }

    public void SetNewPosition(Vector2 newPosition)
    {
        // Move the Gun and ShootPoint to the new position
        transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
        shootPoint.position = new Vector3(newPosition.x, newPosition.y, shootPoint.position.z);
    }
}


