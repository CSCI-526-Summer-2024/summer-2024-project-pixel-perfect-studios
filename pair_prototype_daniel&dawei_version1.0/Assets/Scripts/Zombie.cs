using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject character;
    private SendToGoogle googleForm;
    private Gun gunScript;

    void Start()
    {
        character = GameObject.Find("Character");
        googleForm = GameObject.Find("GoogleFormManager").GetComponent<SendToGoogle>();
        GameObject gun = GameObject.Find("Character/Arm/Gun");
        if (gun != null) 
        {
            gunScript = gun.GetComponent<Gun>();
            if (gunScript == null)
            {
                Debug.LogError("Gun script not found on the Gun object!");
            }
        } 
        else {
            Debug.LogError("Gun object not found in the scene!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Character") 
        {
            googleForm.AddZombiesKilled(gameObject.name);
            //Debug.Log(gameObject.name);
            Destroy(gameObject);
            gunScript.advancedBullet += 5;
            Debug.Log("Hit a zombie");
        }
    }
}
