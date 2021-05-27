using System.Collections.Generic;
using System.Linq;
using MLAPI;
using MLAPI.Messaging;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;
    public SpawnPoint[] points
    {
        get
        {
            return FindObjectsOfType<SpawnPoint>();
        }
    }
    public List<GameObject> Players
    {
        get
        {
            return NetworkManager.Singleton.ConnectedClients.Values
                .Select(v => v.PlayerObject.gameObject).ToList();
        }
    }
    public GameObject LocalPlayer
    {
        get
        {
            return NetworkManager.Singleton.ConnectedClients
                [NetworkManager.Singleton.LocalClientId].PlayerObject.gameObject;
        }
    }
    private void Start()
    {
        if (SpawnManager.Instance == null)
        {
            SpawnManager.Instance = this;
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
            DontDestroyOnLoad(this);
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
            foreach (var player in Players)
            {
                SetPlayerMaskLayer(player);
            }

            LocalPlayer.GetComponent<PlayerData>().State = PlayerState.InGame;

            if (NetworkManager.Singleton.IsHost)
            {
                SetPlayersLocations();
            }
            //Debug.Log("Players will be given a new position");
        }
    }


    private void SetPlayersLocations()
    {
        //var players = NetworkManager.Singleton.ConnectedClients.Values.Select(v => v.PlayerObject.gameObject);
        //GiveNewLocation(points, LocalPlayer);

        foreach (var player in Players)
        {
            GiveNewLocation(player);
        }
    }


    public void GiveNewLocationAfterDeath(GameObject player)
    {
        //Debug.Log("New location to be given to the player " + player.name);
        var playerTeam = player.GetComponent<PlayerData>().Team;
        var pos = points[Random.Range(0,points.Length-1)].GetNewPosition();
        player.GetComponent<FirstPersonController>().RespawnClientRpc(pos);
    }


    public void GiveNewLocation(GameObject player)
    {
        Debug.Log("New location to be given to the player " + player.name);
        var playerTeam = player.GetComponent<PlayerData>().Team;
        var pos = points.First(p => p.Team == playerTeam).GetNewPosition();
        if (NetworkManager.Singleton.IsHost)
            player.GetComponent<FirstPersonController>().RespawnClientRpc(pos);
        //else
        //  player.GetComponent<FirstPersonController>().RespawnServerRpc(pos);
    }

    private void SetPlayerMaskLayer(GameObject player)
    {
        var playerTeam = player.GetComponent<PlayerData>().Team;
        if (playerTeam == LocalPlayer.GetComponent<PlayerData>().Team)
        {
            Setlayer(player, LayerMask.NameToLayer("Friendly"));
        }
        else
            Setlayer(player, LayerMask.NameToLayer("Enemy"));
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
}
