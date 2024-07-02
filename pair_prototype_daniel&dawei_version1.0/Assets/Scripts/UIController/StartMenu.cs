using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public void ClickPlayButton ()
    {
        SceneManager.LoadScene("Tutorial Level 1");
    }
    public void ClickMenuButton ()
    {
        SceneManager.LoadScene("Level Select");
    }
    public void ClickControlButton ()
    {
        //SceneManager.LoadScene("Level Select");
    }

}
