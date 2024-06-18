using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OverlayController : MonoBehaviour
{
    [SerializeField]
    private GameObject GameOverDialog;

    [SerializeField]
    private GameObject InGameMenuDialog;
    enum overlayStatus
    {
        GAMING,
        GAMEOVER,
        INGAMEMENU
    }
    public enum playerStatus
    {
        PLAY,
        WIN,
        LOSE
    }
    overlayStatus gameStatus = overlayStatus.GAMING;
    public playerStatus characterStatus = playerStatus.PLAY;
    public static OverlayController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void backToMainMenu(){
        SceneManager.LoadScene("Level Select");
    }
    public void resetLevel(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private void Update(){
        if(Input.GetButtonDown("Pause")){
            if(gameStatus == overlayStatus.GAMING){
                gameStatus = overlayStatus.INGAMEMENU;
                InGameMenuDialog.SetActive(true);
            }else if(gameStatus == overlayStatus.INGAMEMENU){
                gameStatus = overlayStatus.GAMING;
                InGameMenuDialog.SetActive(false);
            }
        }
        if(characterStatus == playerStatus.LOSE){   // Todo: a boolean variable determine whether character die or not
            GameOverDialog.SetActive(true);
        }
    }
}
