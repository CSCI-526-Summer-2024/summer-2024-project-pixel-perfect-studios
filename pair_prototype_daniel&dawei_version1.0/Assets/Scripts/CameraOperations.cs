using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOperations : MonoBehaviour
{
    public float[] size;
    // private float[] size = {34f, 26.41f};
    public Vector3 offset;
    public Transform player;
    public float leftRegionX;
    public float rightRegionX;
    public float leftRegionY;
    public float rightRegionY;
    private Camera mainCamera;
    private bool needFollow = false;
    private Vector3 initialPosition; // Camera's init pos
    public float smoothSpeed;
    public Boolean enableSwitch = true;
    public Boolean moreFocus = false;
    public float moreFocusSize = 20;

    void Start()
    {
        mainCamera = GetComponent<Camera>();
        if(mainCamera != null){
            Debug.Log(mainCamera.orthographicSize);
            size = new float[2];
            Debug.Log(size);
            size[0] = mainCamera.orthographicSize;
            if(moreFocus){
                size[1] = size[0] - moreFocusSize;
            }else{
                size[1] = size[0] - 8f;
            }
            
            Debug.Log(size[0]);
        }
        initialPosition = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        if(needFollow){
            followPlayer();
        }
        if (mainCamera != null && enableSwitch)
        {
            if (Input.GetKeyDown(KeyCode.C) && needFollow)
            {
                UseMode1();
            }
            else if (Input.GetKeyDown(KeyCode.C) && !needFollow)
            {
                UseMode2();
            }
        }
    }

    void UseMode1(){
        mainCamera.orthographicSize = size[0];
        needFollow = false;
        backToDefault();
    }
    void UseMode2(){
        mainCamera.orthographicSize = size[1];
        needFollow = true;
    }
    void backToDefault(){
        transform.position = initialPosition;
    }
    void followPlayer(){
        if (player != null)
        {
            float newXPosition = player.position.x + offset.x;
            float newYPosition = player.position.y + offset.y;
            
            // make sure camera is in a fixed amount of range
            newXPosition = Mathf.Clamp(newXPosition, leftRegionX, rightRegionX);
            newYPosition = Mathf.Clamp(newYPosition, leftRegionY, rightRegionY);
            
            // only update x coordinates
            Vector3 newPosition = transform.position;
            newPosition.x = newXPosition;
            newPosition.y = newYPosition;
            //transform.position = newPosition;
            transform.position = Vector3.Lerp(transform.position, newPosition, smoothSpeed * Time.deltaTime);
        }
        else
        {
            Debug.Log("Please designate which player you want to follow!");
        }
    }
}
