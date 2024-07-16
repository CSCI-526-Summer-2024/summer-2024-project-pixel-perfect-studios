using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {

    Vector3 positionOfCurrentPortal;
    public GameObject character;
    Vector3 nearestPortalPosition;

    private SendToGoogle googleForm;
    // Start is called before the first frame update
    void Start()
    {
        character = GameObject.Find("Character");
        googleForm = GameObject.Find("GoogleFormManager").GetComponent<SendToGoogle>();
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
            Debug.Log("This is the position of the portal: " + transform.position);
            Debug.Log("This is the position of nearest portal: " + nearestPortal.transform.position);

            nearestPortalPosition = nearestPortal.transform.position;
            googleForm.TrackPortalUse(transform.position, nearestPortalPosition);
            Destroy(nearestPortal);
            // googleForm.TrackPortalUse(nearestPortalPosition);
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
            Debug.Log("This is the position of the portal: " + transform.position);
            Debug.Log("This is the position of nearest portal: " + nearestPortal.transform.position);

            nearestPortalPosition = nearestPortal.transform.position;
            googleForm.TrackPortalUse(transform.position, nearestPortalPosition);
            Destroy(nearestPortal);
            // googleForm.TrackPortalUse(nearestPortalPosition);
            character.transform.position = nearestPortalPosition;
            Destroy(gameObject);
        }
    }

    public void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.name == "Character") {
            Debug.Log("Hit a portal");
            Destroy(gameObject);
            PortalJump();
        }
    }
}
