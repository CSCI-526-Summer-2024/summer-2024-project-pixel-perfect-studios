using UnityEngine;

public class LevelInitializer : MonoBehaviour
{
    public int levelIndex;

    void Start()
    {
        LevelManager.currentLevelIndex = levelIndex;
    }
}
