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
            var playerTeam = player.GetComponent<PlayerData>().Team;
            if (playerTeam == LocalPlayer.GetComponent<PlayerData>().Team)
            {
                Setlayer(player, LayerMask.NameToLayer("Friendly"));
            }
            player.transform.position = points.First(p => p.Team == playerTeam).GetNewPosition();
            player.GetComponent<PlayerController_prototype>().State = PlayerState.InGame;
            //Debug.Log($"Player: {player.name} is in pos: {player.transform.position}");
        }
    }

    private void Setlayer(GameObject gObject, LayerMask layer)
    {

        gObject.layer = layer;
        for (int i = 0; i < gObject.transform.childCount; i++)
        {
            if (gObject.transform.childCount > 0)
                Setlayer(gObject.transform.GetChild(i).gameObject, layer);
        }
    }

    public bool IsAllPlayersReady()
    {
        return Players.Where(p => !p.GetComponent<PlayerData>().Ready.Value).Count() == 1;
    }
}
