using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class BulletManager : MonoBehaviour
{
    public int maxBullets;
    private int currentBullets; 
    public TextMeshProUGUI bulletText;
    private Gun gunScript;
    public SendToGoogle googleForm;


    void Start()
    {
        GameObject gun = GameObject.Find("Gun");
        gunScript = gun.GetComponent<Gun>();
        currentBullets = maxBullets; // Initialize bullets
        UpdateBulletText(); // Update UI text on start
    }

    void Update() {
        currentBullets = gunScript.bulletsLeft;
        UpdateBulletText();
        if (currentBullets == 0) {
            // Call the function to restart the game
            // Show Game Over Screen
            //googleForm.DeathBullet();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    void UpdateBulletText()
    {
        bulletText.text = "Bullets: " + currentBullets; // Update the text display
    }
}
