using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmRotate : MonoBehaviour
{
    public Transform shootPoint; 

    // Update is called once per frame
    void Update()
    {
        //Vector2 dir = MousePos - (Vector2)transform.position;
        Vector2 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - shootPoint.position;
        float angle = (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
        transform.eulerAngles = new Vector3(0f, 0f, angle);
    }
}
