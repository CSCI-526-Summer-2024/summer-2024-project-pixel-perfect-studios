using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelction : MonoBehaviour
{
    public void StartLevel(String levelName){
        SceneManager.LoadScene(levelName);
    }
}
