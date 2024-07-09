using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PowerUpsDisplay : MonoBehaviour
{
    [SerializeField]
    private GameObject powerUpsDisplay;
    [SerializeField]
    private TextMeshProUGUI powerUpsText;
    // Start is called before the first frame update
    void Start()
    {
        powerUpsText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if(Gun.instance.fullTrajectory){
            powerUpsDisplay.SetActive(true);
            //powerUpsText = powerUpsDisplay.GetComponent<TextMeshProUGUI>();
            powerUpsText.text = "Power-Ups: " + Gun.instance.advancedBullet;
        }else{
            powerUpsDisplay.SetActive(false);
            powerUpsText.text = "";
        }
    }
}
