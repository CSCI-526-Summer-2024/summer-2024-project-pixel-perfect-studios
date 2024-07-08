using UnityEngine;
using UnityEngine.UI;

public class LevelIndicatorController : MonoBehaviour
{
    [SerializeField]
    private GameObject levelIndicatorCanvas;
    public float displayDuration = 2f; // Duration to display the level indicator
    private float startTime;
    private float timer;
    void Start()
    {   
        PauseGame();
        
        // Show the level indicator
        levelIndicatorCanvas.SetActive(true);
        // Initialize the timer
        // timer = displayDuration;
        // Record the start time
        startTime = Time.realtimeSinceStartup;
        //PauseGame();
    }


    void Update()
    {
        PauseGame();
        float elapsedTime = Time.realtimeSinceStartup - startTime;
        // Reduce the timer by the time passed since the last frame
        //timer -= 0.0001f;
        //timer -= Time.unscaledDeltaTime;

        // Check if the timer has reached zero
        if (elapsedTime >= displayDuration)
        {
            // Hide the level indicator
            levelIndicatorCanvas.SetActive(false);
            ResumeGame();
            this.enabled = false;
        }
    }

    void PauseGame()
    {
        Gun.instance.allowShooting = false;
        Time.timeScale = 0;
        //isPaused = true;
        // Optionally, disable player controls or other elements here
    }

    void ResumeGame()
    {
        Gun.instance.allowShooting = true;
        Time.timeScale = 1;
        //isPaused = false;
        // Optionally, re-enable disabled controls or elements here
    }

}
