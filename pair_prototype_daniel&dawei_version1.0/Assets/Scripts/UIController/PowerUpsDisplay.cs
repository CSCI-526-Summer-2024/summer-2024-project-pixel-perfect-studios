using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PowerUpsDisplay : MonoBehaviour
{
    private int currentBullets;
    [SerializeField]
    private GameObject powerUpsDisplay;
    [SerializeField]
    private TextMeshProUGUI powerUpsText;
    private Color originalColor;
    public Color newColor;
    [SerializeField]
    private GameObject bulletsDisplay;
    [SerializeField]
    private TextMeshProUGUI bulletsText;
    // Start is called before the first frame update
    void Start()
    {
        powerUpsText.text = "";
        originalColor = powerUpsText.color;
    }

    // Update is called once per frame
    void Update()
    {
        currentBullets = Gun.instance.bulletsLeft;
        //if(Gun.instance.fullTrajectory){
        if(Gun.instance.advancedBullet >= 1){
            powerUpsDisplay.SetActive(true);
            //powerUpsText = powerUpsDisplay.GetComponent<TextMeshProUGUI>();
            powerUpsText.text = "Power-Ups: " + Gun.instance.advancedBullet;
            if(Gun.instance.fullTrajectory){
                bulletsDisplay.SetActive(false);
                powerUpsText.color = newColor;
                bulletsText.text = "";
            }else{
                bulletsDisplay.SetActive(true);
                powerUpsText.color = originalColor;
                bulletsText.text = "Bullets: " + currentBullets;
            }
            
        }else{
            powerUpsDisplay.SetActive(false);
            powerUpsText.text = "";
        }
        //powerUpsDisplay.SetActive(true);
        //powerUpsText.text = "Power-Ups: " + Gun.instance.advancedBullet;
    }
}
