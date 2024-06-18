using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WinningEnemy : MonoBehaviour
{
    // Start is called before the first frame update

//     public TextMeshProUGUI winText;
    void Start()
    {
//         winText.text = "";
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D collision) {
        //winText.text = "You win!";
        OverlayController.instance.characterStatus = OverlayController.playerStatus.WIN;
    }

}
