using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;
using System.Linq;

public class SendToGoogle : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private string URL;

    private long _sessionID;
    private int _die_of_enemy_1 = 0;
    private int _die_of_enemy_2 = 0;
    private int _die_of_enemy_3 = 0;
    private int _die_of_no_bullet_1 = 0;
    private int _die_of_no_bullet_2 = 0;
    private int _die_of_no_bullet_3 = 0;
    private List<Vector2> _enemies_killed = new List<Vector2>();

    public int _current_level;

    private void Awake()
    {
        // Assign ID to identify playtest
        _sessionID = DateTime.Now.Ticks;
    }

    public void DeathEnemy()
    {
        if (_current_level == 1)
        {
            _die_of_enemy_1++;
        }
        else if (_current_level == 2)
        {
            _die_of_enemy_2++;
        }
        else if (_current_level == 3)
        {
            _die_of_enemy_3++;
        }
        Debug.Log("Send Enemy Death Data!");
        Send();
    }

    public void DeathBullet()
    {
        if (_current_level == 1)
        {
            _die_of_no_bullet_1++;
        }
        else if (_current_level == 2)
        {
            _die_of_no_bullet_2++;
        }
        else if (_current_level == 3)
        {
            _die_of_no_bullet_3++;
        }
        Debug.Log("Send Bullet Data!");
        Send();
    }

    public void AddEnemiesKilled(Vector2 enemy_position)
    {
        _enemies_killed.Add(enemy_position);
    }

    public void EnemiesKilled()
    {
        Debug.Log("Send Enemies Data!");
        Send();
    }

    string ListToString(List<Vector2> list)
    {
        // Use String.Join and LINQ to convert the list to a string
        return "[" + string.Join(", ", list.Select(v => v.ToString())) + "]";
    }


    public void Send()
    {
        // _die_of_enemy = UnityEngine.Random.Range(0, 200);
        // _die_of_no_bullet = UnityEngine.Random.Range(0, 200);

        string _enemies_killed_str = ListToString(_enemies_killed);

        StartCoroutine(Post(_sessionID.ToString(), _die_of_enemy_1.ToString(),_die_of_enemy_2.ToString(), _die_of_enemy_3.ToString(), _die_of_no_bullet_1.ToString(), _die_of_no_bullet_2.ToString(), _die_of_no_bullet_3.ToString(), _enemies_killed_str));
        _enemies_killed = new List<Vector2>();
    }

    private IEnumerator Post(string sessionID, string _die_of_enemy_1, string _die_of_enemy_2, string _die_of_enemy_3, string _die_of_no_bullet_1, string _die_of_no_bullet_2, string _die_of_no_bullet_3, string _enemies_killed)
    {
        WWWForm form = new WWWForm();
        //https://docs.google.com/forms/u/1/d/e/1FAIpQLSfel1Kq9fm8JetHvPYqWWsYKrl3gxc5ViDl-x2FY894ZfNpKA/formResponse
        form.AddField("entry.1449005772", sessionID);
        form.AddField("entry.1490287160", _current_level);
        if (_current_level == 1)
        {
            form.AddField("entry.1685270148", _die_of_enemy_1);
            form.AddField("entry.1012120333", _die_of_no_bullet_1);
        }
        else if (_current_level == 2)
        {
            form.AddField("entry.1685270148", _die_of_enemy_2);
            form.AddField("entry.1012120333", _die_of_no_bullet_2);
        }
        else if (_current_level == 3)
        {
            form.AddField("entry.1685270148", _die_of_enemy_3);
            form.AddField("entry.1012120333", _die_of_no_bullet_3);
        }
        form.AddField("entry.425979302", _enemies_killed);
        // form.AddField("entry.1685270148", _die_of_enemy_1);
        // form.AddField("entry.840553378", _die_of_enemy_2);
        // form.AddField("entry.1530451059", _die_of_enemy_3);
        // form.AddField("entry.1139271357", _die_of_no_bullet_1);
        // form.AddField("entry.1300743613", _die_of_no_bullet_2);
        // form.AddField("entry.1012120333", _die_of_no_bullet_3);
        // form.AddField("entry.425979302", _enemies_killed);


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
}
