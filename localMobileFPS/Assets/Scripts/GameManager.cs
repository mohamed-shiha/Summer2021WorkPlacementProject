using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public List<GameObject> Players;
    public GameObject LocalPlayer;
    private void Awake()
    {
        if (GameManager.Instance == null)
        {
            GameManager.Instance = this;
            DontDestroyOnLoad(this);
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
    }

    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 1)
        {
            SetPlayersLocations();
        }
    }

    private void SetPlayersLocations()
    {
        var points = FindObjectsOfType<SpawnPoint>();
        foreach (var player in Players)
        {
            var playerTeam = player.GetComponent<PlayerData>().team;
            player.transform.position = points.First(p => p.Team == playerTeam).GetNewPosition();
            player.GetComponent<FirstPersonController>().PlayMode = true;
        }
    }
}
