using System.Collections.Generic;
using System.Linq;
using MLAPI;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
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


    private void Awake()
    {
        if (GameManager.Instance == null)
        {
            GameManager.Instance = this;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(this.gameObject);
    }

    public bool IsAllPlayersReady()
    {
        return Players.Where(p => !p.GetComponent<PlayerData>().Ready.Value).Count() == 1;
    }
}
