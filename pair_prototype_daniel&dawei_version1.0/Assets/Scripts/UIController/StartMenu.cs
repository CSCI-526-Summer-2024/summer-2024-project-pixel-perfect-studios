using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public void ClickPlayButton ()
    {
        SceneManager.LoadScene("Level Select");
    }

}
