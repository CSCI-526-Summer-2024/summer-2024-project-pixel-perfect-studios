using UnityEngine;
using TMPro;

public class BulletManager : MonoBehaviour
{
    public int maxBullets;
    private int currentBullets;
    public TextMeshProUGUI bulletText;
    private Gun gunScript;
    public SendToGoogle googleForm;

    private bool _no_bullet_check=false;

    void Start()
    {
        GameObject gun = GameObject.Find("Character/Arm/Gun");
        if (gun != null) {
            gunScript = gun.GetComponent<Gun>();
            if (gunScript != null) {
                currentBullets = maxBullets;
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
        UpdateBulletText();
        if (!_no_bullet_check && currentBullets == 0 && GameObject.Find("Bullet(Clone)") == null) {
            // Call the function to restart the game
            // Show Game Over Screen
            _no_bullet_check = true;
            googleForm.DeathBullet();
            Debug.Log("No Bullet Left!");
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            OverlayController.instance.characterStatus = OverlayController.playerStatus.LOSE;
        }
    }

    void UpdateBulletText()
    {
        if (bulletText != null) {
            bulletText.text = "Bullets: " + currentBullets;
        } else {
            Debug.LogError("Bullet TextMeshPro component not assigned!");
        }
    }
}
