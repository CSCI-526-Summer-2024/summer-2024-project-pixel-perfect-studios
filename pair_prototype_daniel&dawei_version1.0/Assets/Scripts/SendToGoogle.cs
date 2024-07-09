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
    
    private int _die_of_enemy = 0;

    private int _die_of_no_bullet = 0;


    private int[] _playersStarted = new int[6]; //hard-coded for 6 levels
    private int[] _playersCompleted = new int[6];

    private List<Vector2> _enemies_killed = new List<Vector2>();

    private List<Vector2> _portalsUsedLocations = new List<Vector2>();

    private List<Vector2> _portalsLocationsAfter = new List<Vector2>();

    private List<Vector2> _playerLocation = new List<Vector2>();

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
        LoadPlayerData();
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

    public void TrackPortalUse(Vector2 portalPosition, Vector2 portalPositionAfter)
    {
        _portalsUsedLocations.Add(portalPosition);
        _portalsLocationsAfter.Add(portalPositionAfter);
        PortalUsed();
    }
    public void TrackShooting(Vector2 playerPosition)
    {
        _playerLocation.Add(playerPosition);
    }

    private void LoadPlayerData()
    {
        for (int i = 1; i <= 6; i++)
        {
            _playersStarted[i - 1] = PlayerPrefs.GetInt($"Level_{i}_Started", 0);
            _playersCompleted[i - 1] = PlayerPrefs.GetInt($"Level_{i}_Completed", 0);
        }
    }

    private void SavePlayerData()
    {
        for (int i = 1; i <= 6; i++)
        {
            PlayerPrefs.SetInt($"Level_{i}_Started", _playersStarted[i - 1]);
            PlayerPrefs.SetInt($"Level_{i}_Completed", _playersCompleted[i - 1]);
        }
        PlayerPrefs.Save();
    }

    public void PlayerStartedLevel(int level)
    {
        if (level >= 1 && level <= 6)
        {
            _playersStarted[level - 1]++;
            SavePlayerData();
        }
        else
        {
            Debug.LogError("Invalid level number.");
        }
    }

    public void PlayerCompletedLevel(int level)
    {
        if (level >= 1 && level <= 6)
        {
            _playersCompleted[level - 1]++;
            SavePlayerData();
        }
        else
        {
            Debug.LogError("Invalid level number.");
        }
    } 

    public void DeathEnemy(Vector2 playerPosition)
    {
        _die_of_enemy++;
        player_death_location_by_enemy = playerPosition;
        Debug.Log("Send Enemy Death Data!");
        Send();
    }

    public void DeathBullet()
    {
        _die_of_no_bullet++;
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
        Debug.Log("Portal used " + _portalsUsed);
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
        string portal_locations_str_after = ListToString(_portalsLocationsAfter);
        string player_shoot_location = ListToString(_playerLocation);

        if (_playersStarted[_current_level - 1] == 0)
        {
            completion_ratio = 0;
        }
        else
        {
            if (_playersStarted[_current_level - 1] > 0)
            {
                completion_ratio = (float)_playersCompleted[_current_level - 1] / _playersStarted[_current_level - 1] * 100;
                Debug.Log("Players Completed " + _playersCompleted[_current_level - 1]);
                Debug.Log("Players Started " + _playersStarted[_current_level - 1]);
            }
        }
        string completion_ratio_str = completion_ratio.ToString("0.00");

        string player_death_location_by_enemy_str = player_death_location_by_enemy.ToString();

        StartCoroutine(Post(_playerID.ToString(), _die_of_enemy.ToString(), _die_of_no_bullet.ToString(), _enemies_killed_str, 
            _portalUsageRate.ToString(), portal_locations_str, completion_ratio_str, player_death_location_by_enemy_str, 
            portal_locations_str_after, player_shoot_location));
        //Reset the values
        // _enemies_killed.Clear();
        // _portalsUsedLocations.Clear();
        _enemies_killed = new List<Vector2>();
        _portalsUsedLocations = new List<Vector2>();
        _portalsLocationsAfter = new List<Vector2>();
        _playerLocation = new List<Vector2>();
    }

    private IEnumerator Post(string playerID, string _die_of_enemy, string _die_of_no_bullet, string _enemies_killed, 
        string _portalUsageRate, string _portal_locations, string _completion_ratio, string _player_death_location_by_enemy, 
        string _portal_locations_after, string _player_shoot_location)
    {
        WWWForm form = new WWWForm();
        //https://docs.google.com/forms/u/1/d/e/1FAIpQLSfel1Kq9fm8JetHvPYqWWsYKrl3gxc5ViDl-x2FY894ZfNpKA/formResponse
        form.AddField("entry.1449005772", playerID);
        form.AddField("entry.1490287160", _current_level);

        form.AddField("entry.1012120333", _die_of_no_bullet);
        form.AddField("entry.1685270148", _die_of_enemy);

        form.AddField("entry.1876543593", _player_death_location_by_enemy);

        form.AddField("entry.425979302", _enemies_killed);

        form.AddField("entry.721250083", _portalUsageRate);
        form.AddField("entry.1802019497", _portal_locations);
        form.AddField("entry.1002300073", _portal_locations_after); 
        form.AddField("entry.1766943039", _completion_ratio);
        form.AddField("entry.1283893895", _player_shoot_location);

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
