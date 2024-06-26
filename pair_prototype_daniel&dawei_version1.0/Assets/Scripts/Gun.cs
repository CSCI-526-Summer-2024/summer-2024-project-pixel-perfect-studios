using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bulletPrefab; // Assign the Bullet prefab in the Inspector
    public Transform shootPoint; // The point from where the bullet will be shot
    public int bulletsLeft = 20;
    public GameObject bullet = null;

    public LineRenderer lineRenderer; // NEW
    public float timeStep = 0.1f; // NEW

    public LayerMask raycastMask;
    public Transform armPosition; // The point of arm
    public static Gun instance;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start() // NEW
    {
        if (lineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }
    }
    
    void Update()
    {
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - armPosition.position; // NEW
        UpdateTrajectory(armPosition.position, direction); // NEW
        // if (Input.GetKey(KeyCode.T)) // Check if the "T" key is pressed
        // {
            
        // }
        
        if (Input.GetMouseButtonDown(0) && (bulletsLeft > 0) && (bullet == null)) // Detect mouse click
        { 
            Shoot(direction);
            bulletsLeft--;
        }

        if (bullet != null) {
            lineRenderer.enabled = false;
        } else {
            lineRenderer.enabled = true;
        }

    }

    public void Shoot(Vector2 direction)
    {
        // Create a bullet instance at the shootPoint's position and rotation
        bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
        
        // Calculate the direction to shoot the bullet
        //Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - shootPoint.position;

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

    public void UpdateTrajectory(Vector2 startPosition, Vector2 direction) // NEW
    {
        Vector2 currentPosition = startPosition;

        float sphereRadius = 0.3f;
        
        RaycastHit2D hit = Physics2D.CircleCast(currentPosition, sphereRadius, direction, Mathf.Infinity, raycastMask);
        

        float distance = 1000f;
        if (hit.collider != null)
        {
            distance = hit.distance;
            //Debug.Log(distance);
        }

        if (distance > 0)
        {
            int maxPoints = Mathf.CeilToInt(distance / 0.2f);
            //Debug.DrawRay(currentPosition, direction.normalized * maxPoints, Color.red, 1f);
            // Debug.Log(maxPoints);
            lineRenderer.enabled = true;
            lineRenderer.positionCount = maxPoints;

            Vector2 smallVelocity = direction.normalized * 0.2f;

            for (int i = 0; i < maxPoints; i++)
            {
                lineRenderer.SetPosition(i, currentPosition);
                currentPosition += smallVelocity;
            }

            // if (hit.collider != null)
            // {
            //     lineRenderer.SetPosition(maxPoints - 1, hit.point);
            // }
            // else
            // {
            //     lineRenderer.SetPosition(maxPoints - 1, currentPosition);
            // }
        }
    }
    public void ClearTrajectory()
    {
        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 0; // Clear the LineRenderer by setting positionCount to 0
        }
    }
}