using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;

public class SendToGoogle : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private string URL;

    private long _sessionID;
    private int _die_of_enemy = 0;
    private int _die_of_no_bullet = 0;

    private void Awake()
    {
        // Assign ID to identify playtest
        _sessionID = DateTime.Now.Ticks;

    }

    public void DeathEnemy()
    {
        _die_of_enemy++;
        Send();
    }

    public void DeathBullet()
    {
        _die_of_no_bullet++;
        Send();
    }

    public void Send()
    {
        // _die_of_enemy = UnityEngine.Random.Range(0, 200);
        // _die_of_no_bullet = UnityEngine.Random.Range(0, 200);

        StartCoroutine(Post(_sessionID.ToString(), _die_of_enemy.ToString(), _die_of_no_bullet.ToString()));
    }

    private IEnumerator Post(string sessionID, string _die_of_enemy, string _die_of_no_bullet)
    {
        WWWForm form = new WWWForm();
        //https://docs.google.com/forms/u/1/d/e/1FAIpQLSfel1Kq9fm8JetHvPYqWWsYKrl3gxc5ViDl-x2FY894ZfNpKA/formResponse
        form.AddField("entry.1449005772", sessionID);
        form.AddField("entry.1685270148", _die_of_enemy);
        form.AddField("entry.1139271357", _die_of_no_bullet);

        using (UnityWebRequest www = UnityWebRequest.Post(URL, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
            }
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
