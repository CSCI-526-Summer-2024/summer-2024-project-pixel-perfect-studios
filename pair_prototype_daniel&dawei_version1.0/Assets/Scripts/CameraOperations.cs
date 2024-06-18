using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOperations : MonoBehaviour
{
    private float[] size = {34f, 26.41f};
    public Vector3 offset;
    public Transform player;
    public float leftRegionX;
    public float rightRegionX;
    private Camera mainCamera;
    private bool needFollow = false;
    private Vector3 initialPosition; // 记录相机的初始位置
    public float smoothSpeed; // 相机平滑过渡速度

    void Start()
    {
        mainCamera = GetComponent<Camera>();
        initialPosition = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        if(needFollow){
            followPlayer();
        }
        if (mainCamera != null)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                UseMode1();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
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
            // 更新相机的x轴位置，使其跟随玩家
            float newXPosition = player.position.x + offset.x;
            
            // 确保相机在指定范围内移动
            newXPosition = Mathf.Clamp(newXPosition, leftRegionX, rightRegionX);
            
            // 仅更新x轴位置
            Vector3 newPosition = transform.position;
            newPosition.x = newXPosition;
            //transform.position = newPosition;
            transform.position = Vector3.Lerp(transform.position, newPosition, smoothSpeed * Time.deltaTime);
        }
        else
        {
            Debug.Log("Please designate which player you want to follow!");
        }
    }
}
