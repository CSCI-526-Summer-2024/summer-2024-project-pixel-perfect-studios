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

    Vector2 MousePos
    {
        get
        {
            Vector2 Pos = cam.ScreenToWorldPoint(Input.mousePosition);
            return Pos;
        }
    }

    void Awake()
    {
        cam = Camera.main;
    }

    void Update()
    {
        //Debug.Log(Gun.instance.bullet);
        if(Gun.instance.bullet == null){ // true will be replaced by a boolean
            FollowPlayer();
        }else{
            FollowBullet();
        }
        
        
    }

    void FollowPlayer()
    {
        Vector2 playerPos = player.position;
        Vector2 mousePos = MousePos;
        Vector3 desiredPosition;

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

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
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
    
}
