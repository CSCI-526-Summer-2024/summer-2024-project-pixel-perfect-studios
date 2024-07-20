using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectorySegment
{
    public Vector2 startPosition;
    public Vector2 direction;
    public int maxPoints;

    public TrajectorySegment(Vector2 startPos, Vector2 dir, int points)
    {
        startPosition = startPos;
        direction = dir;
        maxPoints = points;
    }
}

public class Gun : MonoBehaviour
{
    public GameObject bulletPrefab; // Assign the Bullet prefab in the Inspector
    public Transform shootPoint; // The point from where the bullet will be shot
    public int bulletsLeft = 20;
    public int advancedBullet = 0;
    public GameObject bullet = null;

    public LineRenderer lineRenderer; // NEW
    public float timeStep = 0.1f; // NEW 

    private Color color1 = Color.red;
    private Color color2 = Color.yellow;

    public LayerMask raycastMask;
    public Transform armPosition; // The point of arm
    public static Gun instance;
    public Boolean allowShooting = true;

    public Boolean fullTrajectory = false;

    private List<TrajectorySegment> trajectorySegments = new List<TrajectorySegment>();

    private SendToGoogle googleForm;
    
    private Boolean portalFlag = false;

    private GameObject portal;
    private GameObject nearestPortal;

    //public GameObject powerUpsDisplay;
    
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
        googleForm = GameObject.Find("GoogleFormManager").GetComponent<SendToGoogle>();
    }
    
    void Update()
    {
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - armPosition.position; // NEW
        UpdateTrajectory(armPosition.position, direction); // NEW
        if (Input.GetKey(KeyCode.T) && (advancedBullet > 0)) // Check if the "T" key is pressed. Use to
        {
            fullTrajectory = true;
        }
        
        if (Input.GetKey(KeyCode.P)) // Check if the "T" key is pressed. Use to
        {
            fullTrajectory = false;
        }
        
        if (Input.GetMouseButtonDown(0) && (fullTrajectory == false) && (bulletsLeft > 0) && (bullet == null) && allowShooting) // Detect mouse click
        { 
            Shoot(direction);
            bulletsLeft--;
        }
        else if (Input.GetMouseButtonDown(0) && (fullTrajectory == true) && (advancedBullet > 0) && (bullet == null) && allowShooting) // Detect mouse click
        {
            Shoot(direction);
            advancedBullet--;
        }
        
        if ((advancedBullet == 0) && (bullet == null))
        {
            fullTrajectory = false;
        }

        if (bullet != null) {
            lineRenderer.enabled = false;
        } else {
            lineRenderer.enabled = true;
        }

        if (portalFlag == false) {
            if (portal != null) {
                Portal portalScript = portal.GetComponent<Portal>();
                if (portalScript != null)
                {
                    portalScript.ResetColor(nearestPortal);
                }
            }
        }
    }

    public void Shoot(Vector2 direction)
    {
        // Create a bullet instance at the shootPoint's position and rotation
        bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
        googleForm.TrackShooting(shootPoint.position);
        Debug.Log("Shooting Position:"+ shootPoint.position);
        
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
        trajectorySegments.Clear();
        Vector2 currentPosition = startPosition;
        float sphereRadius = 0.3f;
        
        int maxBounce = 1;
        if (fullTrajectory == true && advancedBullet > 0)
        {
            maxBounce = 5;
            Debug.Log(advancedBullet); 
            Debug.Log("Full Trajectory"); 
            lineRenderer.startColor = color2;
            lineRenderer.endColor = color2;
        }
        else
        {
            lineRenderer.startColor = color1;
            lineRenderer.endColor = color1;
        }

        for (int bounces = 0; bounces < maxBounce; bounces++)
        {
            RaycastHit2D hit = Physics2D.CircleCast(currentPosition, sphereRadius, direction, Mathf.Infinity, raycastMask);
            float distance = 1000f;

            //Debug.DrawRay(hit.point + hit.normal * sphereRadius, Vector2.Reflect(direction, hit.normal) * 2f, Color.red, 2f);
            //Debug.DrawRay(currentPosition + direction.normalized * hit.distance, Vector2.Reflect(direction, hit.normal) * 2f, Color.red, 2f);

            if (hit.collider != null)
            {
                Debug.Log(hit.collider.tag);
                distance = hit.distance;
                int maxPoints = Mathf.CeilToInt(distance / 0.2f);
                trajectorySegments.Add(new TrajectorySegment(currentPosition, direction, maxPoints));

                currentPosition = hit.point + hit.normal * sphereRadius;
                direction = Vector2.Reflect(direction, hit.normal);
                if (hit.collider.tag == "GoldEnemy")
                {
                    break;
                }
                if (hit.collider.CompareTag("BluePortal") || hit.collider.CompareTag("OrangePortal"))
                {
                    portalFlag = true;
                    portal = hit.collider.gameObject;
                    Portal portalScript = portal.GetComponent<Portal>();
                    if (portalScript != null)
                    {
                        nearestPortal = portalScript.LocatePortal();
                    }
                    break;
                } else {
                    portalFlag = false;
                }
            }
            else
            {
                int maxPoints = Mathf.CeilToInt(distance / 0.2f);
                trajectorySegments.Add(new TrajectorySegment(currentPosition, direction, maxPoints));
                break;
            }
        }
        lineRenderer.positionCount = 0;
        foreach (var segment in trajectorySegments)
        {
            lineRenderer.positionCount += segment.maxPoints;
            currentPosition = segment.startPosition;
            Vector2 smallVelocity = segment.direction.normalized * 0.2f;
            for (int i = 0; i < segment.maxPoints; i++)
            {
                lineRenderer.SetPosition(lineRenderer.positionCount - segment.maxPoints + i, currentPosition);
                currentPosition += smallVelocity;
            }
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