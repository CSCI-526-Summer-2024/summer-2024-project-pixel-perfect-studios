using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {

    Vector3 positionOfCurrentPortal;
    public GameObject character;
    Vector3 nearestPortalPosition;
    private GameObject nearestPortal = null;
    private Color originalColor;

    public GameObject pointedAtPortal;

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

   void ApplyColorChange(GameObject portal)
    {
        Renderer renderer = portal.GetComponent<Renderer>();
        if (renderer != null)
        {
            originalColor = renderer.material.color;
            renderer.material.color = Color.green;
        }
    }

    public void ResetColor(GameObject portal)
    {
        if (portal != null)
        {
            Renderer renderer = portal.GetComponent<Renderer>();

            if (portal.tag == "OrangePortal") {
                originalColor = new Color(1.0f, 0.3696f, 0.0156f);
            }
            if (portal.tag == "BluePortal") {
                originalColor = HexToColor(0x04a1ff);
            }
            if (renderer != null)
            {
                renderer.material.color = originalColor;
            }
        }
    }

    private Color HexToColor(uint hex)
    {
        float r = ((hex >> 16) & 0xFF) / 255.0f;
        float g = ((hex >> 8) & 0xFF) / 255.0f;
        float b = (hex & 0xFF) / 255.0f;
        return new Color(r, g, b);
    }

    public GameObject LocatePortal()
    {
        if (this.tag == "BluePortal")
        {
            GameObject[] portals = GameObject.FindGameObjectsWithTag("OrangePortal");
            float minDistance = Mathf.Infinity;

            foreach (GameObject portal in portals)
            {
                float distance = Vector3.Distance(positionOfCurrentPortal, portal.transform.position);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestPortal = portal;
                }
            }
            if (nearestPortal != null)
            {
                ApplyColorChange(nearestPortal);
            }
        }
        else if (this.tag == "OrangePortal")
        {
            GameObject[] portals = GameObject.FindGameObjectsWithTag("BluePortal");
            float minDistance = Mathf.Infinity;

            foreach (GameObject portal in portals)
            {
                float distance = Vector3.Distance(positionOfCurrentPortal, portal.transform.position);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestPortal = portal;
                }
            }
            if (nearestPortal != null)
            {
                ApplyColorChange(nearestPortal);
            }
        }
        if (nearestPortal != null)
        {
            return nearestPortal;
        } else {
            return null;
        }
    }

   void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Character")
        {
            Debug.Log("Character hit a portal");
            Destroy(gameObject);
            PortalJump();
        }
    }
}
