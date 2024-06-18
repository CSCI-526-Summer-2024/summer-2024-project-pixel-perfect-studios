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
    void PauseGame()
    {
        Time.timeScale = 0;
        //isPaused = true;
        // Optionally, disable player controls or other elements here
    }

    void ResumeGame()
    {
        Time.timeScale = 1;
        //isPaused = false;
        // Optionally, re-enable disabled controls or elements here
    }


    public void backToMainMenu(){
        ResumeGame();
        SceneManager.LoadScene("Level Select");
    }
    public void resetLevel(){
        ResumeGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private void Update(){
        if(Input.GetButtonDown("Pause")){
            if(gameStatus == overlayStatus.GAMING){
                gameStatus = overlayStatus.INGAMEMENU;
                InGameMenuDialog.SetActive(true);
                PauseGame();
            }else if(gameStatus == overlayStatus.INGAMEMENU){
                gameStatus = overlayStatus.GAMING;
                InGameMenuDialog.SetActive(false);
                ResumeGame();
            }
        }
        if(characterStatus == playerStatus.LOSE){   // Todo: a boolean variable determine whether character die or not
            GameOverDialog.SetActive(true);
            //PauseGame();
        }
        if(characterStatus == playerStatus.WIN){
            SceneManager.LoadScene("You Win");
        }
    }
}
