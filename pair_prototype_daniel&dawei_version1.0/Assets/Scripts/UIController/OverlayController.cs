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
    overlayStatus gameStatus = overlayStatus.GAMING;
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
        if(false){   // Todo: a boolean variable determine whether character die or not
            InGameMenuDialog.SetActive(true);
        }
    }
}
