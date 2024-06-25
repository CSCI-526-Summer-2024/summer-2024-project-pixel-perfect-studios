using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlUIController : MonoBehaviour
{
    [SerializeField]
    private GameObject Menu;
    [SerializeField]
    private GameObject Ctrl;
    private Boolean showMenu = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Pause")){
            if(showMenu){
                Menu.SetActive(false);
                Ctrl.SetActive(true);
                showMenu = false;
            }else{
                Menu.SetActive(true);
                Ctrl.SetActive(false);
                showMenu = true;
            }
        }
    }
}
