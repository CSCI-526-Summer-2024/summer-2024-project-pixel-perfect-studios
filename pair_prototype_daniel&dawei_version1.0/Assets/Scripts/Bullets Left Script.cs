using UnityEngine;
using TMPro;

public class BulletManager : MonoBehaviour
{
    public int maxBullets;
    private int currentBullets;
    private int currentPowerUp;
    public TextMeshProUGUI bulletText;
    private Gun gunScript;
    private SendToGoogle googleForm;

    private bool _no_bullet_check=false;
    private Color redColor = new Color(1f, 0f, 0f, 1f);
    private Color originalColor;

    void Start()
    {
        originalColor = bulletText.color;
        googleForm = GameObject.Find("GoogleFormManager").GetComponent<SendToGoogle>();
        GameObject gun = GameObject.Find("Character/Arm/Gun");
        if (gun != null) {
            gunScript = gun.GetComponent<Gun>();
            if (gunScript != null) {
                currentBullets = maxBullets;
                //bulletText.text = "Bullets: " + currentBullets;
                UpdateBulletText();
            } else {
                Debug.LogError("Gun script not found on the Gun object!");
            }
        } else {
            Debug.LogError("Gun object not found in the scene!");
        }
    }

    void Update() {
        currentBullets = gunScript.bulletsLeft;
        currentPowerUp = gunScript.advancedBullet;
        UpdateBulletText();
        if (!_no_bullet_check && currentBullets == 0 && currentPowerUp == 0 && GameObject.Find("Bullet(Clone)") == null) {
            // Call the function to restart the game
            // Show Game Over Screen
            _no_bullet_check = true;
            googleForm.DeathBullet();
            Debug.Log("No Bullet Left!");
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            OverlayController.instance.characterStatus = OverlayController.playerStatus.LOSE_BY_BULLET;
        }
    }

    void UpdateBulletText()
    {
        if (bulletText != null) {
            bulletText.text = "Bullets: " + currentBullets;
            if(currentBullets <= 2){
                bulletText.color = redColor;
            }else{
                bulletText.color = originalColor;
            }
        } else {
            Debug.LogError("Bullet TextMeshPro component not assigned!");
        }
    }
}
