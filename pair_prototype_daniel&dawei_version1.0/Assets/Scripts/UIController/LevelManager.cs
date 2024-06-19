using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public readonly int levelInTotal = 5;
    public string[] levels;
    public static int currentLevelIndex = 0;
    public GameObject nextLevelButton;
    void Start()
    {
        Debug.Log("You are in" + currentLevelIndex);
        if (currentLevelIndex == levelInTotal && nextLevelButton != null)
        {
            nextLevelButton.gameObject.SetActive(false);
        }
    }
    public void LoadNextLevel()
    {
        /*
        string currentSceneName = SceneManager.GetActiveScene().name;
        
        int currentSceneIndex = System.Array.IndexOf(levels, currentSceneName);

        if (currentSceneIndex >= 0 && currentSceneIndex < levels.Length - 1)
        {
            SceneManager.LoadScene(levels[currentSceneIndex + 1]);
        }
        else
        {
            Debug.LogWarning("No more levels to load, or current level not found in levels array.");
        }*/
        if (currentLevelIndex < levels.Length - 1)
        {
            currentLevelIndex++;
            SceneManager.LoadScene(levels[currentLevelIndex]);
            Debug.Log("You are going to" + currentLevelIndex);
        }
        else
        {
            Debug.LogWarning("Already last");
        }
    }
}
