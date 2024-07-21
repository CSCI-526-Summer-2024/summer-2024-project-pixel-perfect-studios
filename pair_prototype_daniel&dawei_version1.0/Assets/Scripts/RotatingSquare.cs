using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingSquare : MonoBehaviour
{
    public float rotationSpeed = 1000f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }

    public void FindPortal(Vector2 PortalPosition)
    {
        Debug.Log(PortalPosition);
        this.transform.position = PortalPosition;
    }
}
