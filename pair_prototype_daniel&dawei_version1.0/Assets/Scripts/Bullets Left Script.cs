using UnityEngine;
using TMPro;

public class BulletManager : MonoBehaviour
{
    public int maxBullets;
    private int currentBullets;
    public TextMeshProUGUI bulletText;
    private Gun gunScript;

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
        if (gunScript != null) {
            currentBullets = gunScript.bulletsLeft;
            UpdateBulletText();
            if (currentBullets == 0) {
                // Handle game over logic
                // Example: OverlayController.instance.characterStatus = OverlayController.playerStatus.LOSE;
            }
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
