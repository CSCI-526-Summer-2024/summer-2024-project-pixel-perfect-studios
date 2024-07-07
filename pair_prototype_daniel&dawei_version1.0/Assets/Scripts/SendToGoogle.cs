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

    private string _playerID;
    
    private int _die_of_enemy_1 = 0;
    private int _die_of_enemy_2 = 0;
    private int _die_of_enemy_3 = 0;
    private int _die_of_enemy_4 = 0;
    private int _die_of_enemy_5 = 0;

    private int _die_of_no_bullet_1 = 0;
    private int _die_of_no_bullet_2 = 0;
    private int _die_of_no_bullet_3 = 0;
    private int _die_of_no_bullet_4 = 0;
    private int _die_of_no_bullet_5 = 0;

    // private int _bulletsUsedLevel1 = 0;
    // private int _bulletsUsedLevel2 = 0;
    // private int _bulletsUsedLevel3 = 0;
    // private int _bulletsUsedLevel4 = 0;
    // private int _bulletsUsedLevel5 = 0;

    // private int bullets_used;

    private int[] _playersStarted = new int[5]; //hard-coded for 5 levels
    private int[] _playersCompleted = new int[5];

    private List<Vector2> _enemies_killed = new List<Vector2>();

    private List<Vector2> _portalsUsedLocations = new List<Vector2>();
    private int _portalsUsed = 0;
    private int _totalPortals = 0;
    private float completion_ratio = 0;

    private Vector2 player_death_location_by_enemy;

    public int _current_level;

    private void Awake()
    {
        _playerID = GetPlayerID();

        // Count the number of portals in the scene
        CountPortalsInScene();
    }

    private string GetPlayerID()
    {
        // Check if a player ID already exists
        if (PlayerPrefs.HasKey("PlayerID"))
        {
            return PlayerPrefs.GetString("PlayerID");
        }
        else
        {
            // Generate a new unique player ID
            string newPlayerID = Guid.NewGuid().ToString();
            PlayerPrefs.SetString("PlayerID", newPlayerID);
            PlayerPrefs.Save();
            return newPlayerID;
        }
    }

    private void CountPortalsInScene()
    {
        _totalPortals = GameObject.FindGameObjectsWithTag("OrangePortal").Length + GameObject.FindGameObjectsWithTag("BluePortal").Length;
        Debug.Log("Total Portals in the scene: " + _totalPortals);
    }

    public void TrackPortalUse(Vector2 portalPosition)
    {
        _portalsUsedLocations.Add(portalPosition);
        PortalUsed();
        //
    }

//     public void GetUsedBullets(int bulletsLeft, int maxBullets)
// {
//     int bulletsUsed = maxBullets - bulletsLeft;
//     switch (_current_level)
//     {
//         case 1:
//             _bulletsUsedLevel1 = bulletsUsed;
//             break;
//         case 2:
//             _bulletsUsedLevel2 = bulletsUsed;
//             break;
//         case 3:
//             _bulletsUsedLevel3 = bulletsUsed;
//             break;
//         case 4:
//             _bulletsUsedLevel4 = bulletsUsed;
//             break;
//         case 5:
//             _bulletsUsedLevel5 = bulletsUsed;
//             break;
//         default:
//             Debug.LogError("Invalid level number.");
//             break;
//     }
// }

    public void PlayerStartedLevel(int level)
    {
        if (level >= 1 && level <= 5)
        {
            _playersStarted[level - 1]++;
        }
        else
        {
            Debug.LogError("Invalid level number.");
        }
    }

    public void PlayerCompletedLevel(int level)
    {
        if (level >= 1 && level <= 5)
        {
            _playersCompleted[level - 1]++;
        }
        else
        {
            Debug.LogError("Invalid level number.");
        }
    } 

    public void DeathEnemy(Vector2 playerPosition)
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
        else if (_current_level == 4)
        {
            _die_of_enemy_4++;
        }
        else if (_current_level == 5)
        {
            _die_of_enemy_5++;
        }
        player_death_location_by_enemy = playerPosition;
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
        else if (_current_level == 4)
        {
            _die_of_no_bullet_4++;
        }
        else if (_current_level == 5)
        {
            _die_of_no_bullet_5++;
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

    public void PortalUsed()
    {
        _portalsUsed++;
        Debug.Log("Portal used" + _portalsUsed++);
    }

    public void Send()
    {

        string _enemies_killed_str = ListToString(_enemies_killed);

        float _portalUsageRate;

        if (_totalPortals == 0)
        {
            _portalUsageRate = 0;
        }
        else
        {
            _portalUsageRate = (float)_portalsUsed / _totalPortals;
            _portalUsageRate = (float)Math.Round(_portalUsageRate, 2) * 100;
        }
        string portal_locations_str = ListToString(_portalsUsedLocations);

        if (_playersStarted[_current_level - 1] == 0)
        {
            completion_ratio = 0;
        }
        else
        {
            if (_playersStarted[_current_level - 1] > 0)
            {
                completion_ratio = (float)_playersCompleted[_current_level - 1] / _playersStarted[_current_level - 1] * 100;
            }
        }
        string completion_ratio_str = completion_ratio.ToString("0.00");

        string player_death_location_by_enemy_str = player_death_location_by_enemy.ToString();

        StartCoroutine(Post(_playerID.ToString(), _die_of_enemy_1.ToString(),_die_of_enemy_2.ToString(), _die_of_enemy_3.ToString(), 
            _die_of_enemy_4.ToString(), _die_of_enemy_5.ToString(), _die_of_no_bullet_1.ToString(), _die_of_no_bullet_2.ToString(),
            _die_of_no_bullet_3.ToString(), _die_of_no_bullet_4.ToString(), _die_of_no_bullet_5.ToString(), _enemies_killed_str, 
            _portalUsageRate.ToString(), portal_locations_str, completion_ratio_str, player_death_location_by_enemy_str));
        //Reset the values
        // _enemies_killed.Clear();
        // _portalsUsedLocations.Clear();
        _enemies_killed = new List<Vector2>();
        _portalsUsedLocations = new List<Vector2>();
    }

    private IEnumerator Post(string playerID, string _die_of_enemy_1, string _die_of_enemy_2, string _die_of_enemy_3, 
        string _die_of_enemy_4, string _die_of_enemy_5, string _die_of_no_bullet_1, string _die_of_no_bullet_2, 
        string _die_of_no_bullet_3, string _die_of_no_bullet_4, string _die_of_no_bullet_5, string _enemies_killed, 
        string _portalUsageRate, string _portal_locations, string _completion_ratio, string _player_death_location_by_enemy)
    {
        WWWForm form = new WWWForm();
        //https://docs.google.com/forms/u/1/d/e/1FAIpQLSfel1Kq9fm8JetHvPYqWWsYKrl3gxc5ViDl-x2FY894ZfNpKA/formResponse
        form.AddField("entry.1449005772", playerID);
        form.AddField("entry.1490287160", _current_level);
        if (_current_level == 1)
        {
            form.AddField("entry.1685270148", _die_of_enemy_1);
            form.AddField("entry.1012120333", _die_of_no_bullet_1);
            //form.AddField("entry.1503822787", _bulletsUsedLevel1);
        }
        else if (_current_level == 2)
        {
            form.AddField("entry.1685270148", _die_of_enemy_2);
            form.AddField("entry.1012120333", _die_of_no_bullet_2);
            //form.AddField("entry.1503822787", _bulletsUsedLevel2);
        }
        else if (_current_level == 3)
        {
            form.AddField("entry.1685270148", _die_of_enemy_3);
            form.AddField("entry.1012120333", _die_of_no_bullet_3);
            //form.AddField("entry.1503822787", _bulletsUsedLevel3);
        }
        else if (_current_level == 4)
        {
            form.AddField("entry.1685270148", _die_of_enemy_4);
            form.AddField("entry.1012120333", _die_of_no_bullet_4);
            //form.AddField("entry.1503822787", _bulletsUsedLevel4);
        }
        else if (_current_level == 5)
        {
            form.AddField("entry.1685270148", _die_of_enemy_5);
            form.AddField("entry.1012120333", _die_of_no_bullet_5);
            //form.AddField("entry.1503822787", _bulletsUsedLevel5);
        }
        form.AddField("entry.425979302", _enemies_killed);

        form.AddField("entry.1876543593", _player_death_location_by_enemy);

        form.AddField("entry.721250083", _portalUsageRate);
        form.AddField("entry.1802019497", _portal_locations); 
        form.AddField("entry.1766943039", _completion_ratio);

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
