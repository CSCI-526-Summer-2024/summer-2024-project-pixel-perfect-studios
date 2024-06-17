using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {

    Vector3 positionOfCurrentPortal;
    public GameObject character;
    Vector3 nearestPortalPosition;
    // Start is called before the first frame update
    void Start()
    {
        character = GameObject.Find("Character");
    }

    // Update is called once per frame
    void Update()
    {
        positionOfCurrentPortal = transform.position;
    }

    public void PortalJump() {
        if (gameObject.tag == "BluePortal") {
            GameObject[] portals = GameObject.FindGameObjectsWithTag("OrangePortal");
            GameObject nearestPortal = null;
            float minDistance = Mathf.Infinity;

            foreach (GameObject portal in portals) {
                float distance = Vector3.Distance(positionOfCurrentPortal, portal.transform.position);

                if (distance < minDistance) {
                    minDistance = distance;
                    nearestPortal = portal;
                }
            }

            nearestPortalPosition = nearestPortal.transform.position;
            Destroy(nearestPortal);
            character.transform.position = nearestPortalPosition;
            Destroy(gameObject);
        }
        if (gameObject.tag == "OrangePortal") {
            GameObject[] portals = GameObject.FindGameObjectsWithTag("BluePortal");
            GameObject nearestPortal = null;
            float minDistance = Mathf.Infinity;

            foreach (GameObject portal in portals) {
                float distance = Vector3.Distance(positionOfCurrentPortal, portal.transform.position);

                if (distance < minDistance) {
                    minDistance = distance;
                    nearestPortal = portal;
                }
            }
            nearestPortalPosition = nearestPortal.transform.position;
            Destroy(nearestPortal);
            character.transform.position = nearestPortalPosition;
            Destroy(gameObject);
        }
    }
}
