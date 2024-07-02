using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WinningEnemy : MonoBehaviour
{
    // Start is called before the first frame update

//     public TextMeshProUGUI winText;
    private SendToGoogle googleForm;
    public int _current_level;

    void Start()
    {
        googleForm = GameObject.Find("GoogleFormManager").GetComponent<SendToGoogle>();
        googleForm.PlayerStartedLevel(_current_level); //unsure where to put this
//         winText.text = "";
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D collision) {
        //winText.text = "You win!";
        OverlayController.instance.characterStatus = OverlayController.playerStatus.WIN;
        googleForm.EnemiesKilled();
        googleForm.PlayerCompletedLevel(_current_level);

    }

}
