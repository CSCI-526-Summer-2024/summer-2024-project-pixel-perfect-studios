using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraVer2 : MonoBehaviour
{
    Camera cam;
    public Transform player;
    public float offsetAmountX;
    public float offsetAmountY;
    public float smoothSpeed;
    private float[] size;
    private float sizeChangeSpeed = 100f;
    public float bulletShootCameraSize;
    public Boolean biggerPlayerView = false;
    private Boolean allowedChange = true;
    private Boolean needFollowPlayer = true;
    private Boolean duringMoving = true;
    public float slowerSmoothSpeed;
    public float thresholdDistance;
    public float cameraSize = 32.7847f;
    Vector2 MousePos
    {
        get
        {
            Vector2 Pos = cam.ScreenToWorldPoint(Input.mousePosition);
            return Pos;
        }
    }

    void Start()
    {
        cam = Camera.main;
        if(cam != null){
            //Debug.Log(cam.orthographicSize);
            size = new float[2];
            //Debug.Log(size);
            //size[0] = cam.orthographicSize;
            size[0] = cameraSize;
            size[1] = bulletShootCameraSize;
            //Debug.Log(size[0]);
        }
    }
    void Awake()
    {
        cam = Camera.main;
    }

    void Update()
    {
        Debug.Log("Allowed_Change:" + allowedChange);
        //Debug.Log(Gun.instance.bullet);
        if(Gun.instance.bullet == null){ // true will be replaced by a boolean
            needFollowPlayer = true;
            //FollowPlayer();
            
        }else{
            needFollowPlayer = false;
            /*
            FollowBullet();
            allowedChange = false;
            StartCoroutine(ChangeOrthographicSize(size[1]));
            */
        }
        //if (Input.GetKeyDown(KeyCode.C) && allowedChange && biggerPlayerView)
        if (Input.GetKeyDown(KeyCode.C) && biggerPlayerView)
        {
            biggerPlayerView = false;
        }
        //else if (Input.GetKeyDown(KeyCode.C) && allowedChange && !biggerPlayerView)
        else if (Input.GetKeyDown(KeyCode.C) && !biggerPlayerView)
        {
            biggerPlayerView = true;
        }
    }

    void FixedUpdate()
    {
        if(needFollowPlayer){
            FollowPlayer();
        }else{
            FollowBullet();
            allowedChange = false;
            //StartCoroutine(ChangeOrthographicSize(size[1]));
            cam.orthographicSize = size[1];
        }

    }

    void FollowPlayer()
    {
        Vector2 playerPos = player.position;
        Vector2 mousePos = MousePos;
        Vector3 desiredPosition;
        //if (biggerPlayerView && allowedChange)
        if (biggerPlayerView && !duringMoving)
        {
            //allowedChange = false;
            //StartCoroutine(ChangeOrthographicSize(size[0] + 50f));
            cam.orthographicSize = size[0] + 50f;
        }
        //else if(!biggerPlayerView && allowedChange)
        else if(!biggerPlayerView && !duringMoving)
        {
            //allowedChange = false;
            //StartCoroutine(ChangeOrthographicSize(size[0]));
            cam.orthographicSize = size[0];
        }
        /*
        // Judge the mosue's relative position
        if (mousePos.x > playerPos.x) // Mosue is at right of the character
        {
            //desiredPosition = new Vector3(playerPos.x + offsetAmountX, transform.position.y, transform.position.z);
            if(mousePos.y > playerPos.y){
                desiredPosition = new Vector3(playerPos.x + offsetAmountX, playerPos.y + offsetAmountY, transform.position.z);
            }else{
                desiredPosition = new Vector3(playerPos.x + offsetAmountX, playerPos.y - offsetAmountY, transform.position.z);
            }
        }
        else // Mosue is at left of the character
        {
            //desiredPosition = new Vector3(playerPos.x - offsetAmountX, transform.position.y, transform.position.z);
            if(mousePos.y > playerPos.y){
                desiredPosition = new Vector3(playerPos.x - offsetAmountX, playerPos.y + offsetAmountY, transform.position.z);
            }else{
                desiredPosition = new Vector3(playerPos.x - offsetAmountX, playerPos.y - offsetAmountY, transform.position.z);
            }
        }
        */
        //Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        desiredPosition = new Vector3(playerPos.x, playerPos.y, transform.position.z);
        Vector3 smoothedPosition;
        Debug.Log("Lower Speed:" + duringMoving);
        if(duringMoving){
            smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, slowerSmoothSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, desiredPosition) < thresholdDistance)
            {
                duringMoving = false;
            }
            //lowerSpeed = false;
        }else{
            smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        }
        
        transform.position = smoothedPosition;
    }

    void FollowBullet(){
        Transform bulletTransform = Gun.instance.bullet.transform;
        Vector2 bulletPos = bulletTransform.position;
        Vector3 desiredPosition;
            
        desiredPosition = new Vector3(bulletPos.x, bulletPos.y, transform.position.z);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }


/*
    IEnumerator ChangeOrthographicSize(float targetSize)
    {
        while (Mathf.Abs(cam.orthographicSize - targetSize) > 0.01f)
        {
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetSize, sizeChangeSpeed * Time.deltaTime);
            yield return null;
        }
        cam.orthographicSize = targetSize;
        allowedChange = true;
    }
*/
}
