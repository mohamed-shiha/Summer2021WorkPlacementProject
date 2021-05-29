using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.SceneManagement;
using MLAPI.Transports.UNET;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum ScreenNames
{
    Start,
    PlayMode,
    Settings,
    Lobby,
    PlayerRecords,
}
public class StartMenu : MonoBehaviour
{

    public Transform SpawnLocationsParent;
    public Transform StartScreen;
    public Transform LobbyScreen;
    public Transform SettingsScreen;
    public Transform PlayerRecords;
    public Transform PlayModeScreen;
    public TMP_InputField ServerPasswordInput;
    public TMP_InputField ServerIpAddress;
    public Button ConnectCreateButton;
    public Button ReadyPlayButton;
    public TMP_Dropdown TeamSelectDropDown;
    public TextMeshProUGUI debugText;
    string ipAddress;

    private void Start()
    {
        // start with start screen active
        NetworkManager.Singleton.OnServerStarted += Singleton_OnServerStarted;
        NetworkManager.Singleton.OnClientConnectedCallback += Singleton_OnClientConnectedCallback;
        NetworkManager.Singleton.OnClientDisconnectCallback += Singleton_OnClientDisconnectCallback;

        TeamSelectDropDown.onValueChanged.AddListener((int value) => OnTeamSelected(TeamSelectDropDown.value));
        ConnectCreateButton.onClick.AddListener(() => ConnectOrHost(ConnectCreateButton.GetComponentInChildren<TextMeshProUGUI>().text));
        ShowScreen(ScreenNames.Start);


        // testing
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                debugText.text = ip.ToString();
                ipAddress = ip.ToString();
                break;
            }
        }
    }
    private void OnDestroy()
    {
        if (NetworkManager.Singleton == null) return;
        NetworkManager.Singleton.OnServerStarted -= Singleton_OnServerStarted;
        NetworkManager.Singleton.OnClientConnectedCallback -= Singleton_OnClientConnectedCallback;
        NetworkManager.Singleton.OnClientDisconnectCallback -= Singleton_OnClientDisconnectCallback;
    }

    #region NetworkEvents


    private void Singleton_OnServerStarted()
    {
        if (NetworkManager.Singleton.IsHost)
            Singleton_OnClientConnectedCallback(NetworkManager.Singleton.LocalClientId);
    }

    private void Singleton_OnClientConnectedCallback(ulong obj)
    {
        ShowScreen(ScreenNames.Lobby);
        
        OnTeamSelected(0);
    }

    private void Singleton_OnClientDisconnectCallback(ulong clientId)
    {
        if (clientId == NetworkManager.Singleton.LocalClientId)
            ShowScreen(ScreenNames.Start);
    }
    private void Singleton_ConnectionApprovalCallback(byte[] connectionData, ulong clientId, NetworkManager.ConnectionApprovedDelegate callback)
    {
        // get the password as a string
        string password = Encoding.ASCII.GetString(connectionData);
        // set the approval bool
        bool approveConnection = password.Equals(ServerPasswordInput.text);
        // spawn location in lobby 
        int posIndex = NetworkManager.Singleton.ConnectedClients.Count;
        Transform pos = SpawnLocationsParent.GetChild(posIndex);
        // call the callback
        callback(true, null, approveConnection, pos.position, pos.rotation);
    }
    #endregion

    #region Buttons

    public void PlayReady()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            if (GameManager.Instance.IsAllPlayersReady())
                NetworkSceneManager.SwitchScene("GamePlay");
            else Debug.Log("Ready up Please");
        }
        else
        {
            GameManager.Instance.LocalPlayer.GetComponent<PlayerData>()._SetReadyServerRpc(true);
            ReadyPlayButton.enabled = false;
        }
    }

    public void GoToPlayMode()
    {
        ShowScreen(ScreenNames.PlayMode);
        // Debug.Log("Button Start Game Mode");
    }
    public void LeaveLobby()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            NetworkManager.Singleton.StopHost();
            NetworkManager.Singleton.ConnectionApprovalCallback -= Singleton_ConnectionApprovalCallback;
        }
        else NetworkManager.Singleton.StopClient();

        ShowScreen(ScreenNames.Start);
    }

    public void GoToSettings()
    {
        ShowScreen(ScreenNames.Settings);
    }

    public void SaveAndExit()
    {
        Application.Quit();
        //Debug.Log("Button Save and exit");
    }

    public void ConnectOrHost(string button)
    {
        NetworkManager.Singleton.GetComponent<UNetTransport>().ConnectAddress = ServerIpAddress.text;
        //ipAddress = ServerIpAddress.text;
        string buttonClickedName = button.ToLower();
        if (buttonClickedName.Equals("connect"))
            StartAsClient();
        else StartAsHost();
        //Debug.Log(button);
    }

    public void OnIpClicked()
    {
        ServerIpAddress.text = ipAddress;
    }

    public void ShowPlayerRecords()
    {
        ShowScreen(ScreenNames.PlayerRecords);
        //Debug.Log("Button Show Player Records");
    }

    public void BackToMainMenu()
    {
        ShowScreen(ScreenNames.Start);
        Debug.Log("Button Show BackToMain");
    }

    public void StartAsClient()
    {
        //NetworkManager.Singleton.StopClient();
        ReadyPlayButton.GetComponentInChildren<TextMeshProUGUI>().text = "Ready";
        NetworkManager.Singleton.NetworkConfig.ConnectionData = Encoding.ASCII.GetBytes(ServerPasswordInput.text);
        var task =  NetworkManager.Singleton.StartClient();
        foreach (var item in task.Tasks)
        {
            Debug.Log("Is done ?"+item.IsDone);
            Debug.Log("success ? "+item.Success);
            Debug.Log(item.State +" "+ item.Message);
            Debug.Log(item.TransportCode);
            Debug.Log(item.TransportException);
            //Debug.Log(item.Message);
        }
    }

    public void StartAsHost()
    {
        ReadyPlayButton.GetComponentInChildren<TextMeshProUGUI>().text = "Start Match";
        NetworkManager.Singleton.ConnectionApprovalCallback += Singleton_ConnectionApprovalCallback;
        //Debug.Log(NetworkManager.Singleton.gameObject.GetComponent<UNetTransport>().ConnectAddress);
        NetworkManager.Singleton.StartHost();
    }

    #endregion

    public void OnTeamSelected(int index)
    {
        if (!ReadyPlayButton.enabled)
            ReadyPlayButton.enabled = true;
        if (!NetworkManager.Singleton.IsHost)
            GameManager.Instance.LocalPlayer.GetComponent<PlayerData>()._SetReadyServerRpc(false);
        Teams team = (Teams)index;
        GameManager.Instance.LocalPlayer.GetComponent<PlayerData>()._SetTeamServerRpc(team);
        // if (NetworkManager.Singleton.IsHost)
        //GameManager.Instance.LocalPlayer.GetComponent<PlayerData>().LocalSetTeam(team);
    }

    private void ShowScreen(ScreenNames screen)
    {
        StartScreen.gameObject.SetActive(false);
        SettingsScreen.gameObject.SetActive(false);
        PlayModeScreen.gameObject.SetActive(false);
        PlayerRecords.gameObject.SetActive(false);
        LobbyScreen.gameObject.SetActive(false);


        switch (screen)
        {
            case ScreenNames.Start:
                StartScreen.gameObject.SetActive(true);
                break;
            case ScreenNames.PlayMode:
                PlayModeScreen.gameObject.SetActive(true);
                break;
            case ScreenNames.Settings:
                SettingsScreen.gameObject.SetActive(true);
                break;
            case ScreenNames.Lobby:
                LobbyScreen.gameObject.SetActive(true);
                break;
            case ScreenNames.PlayerRecords:
                PlayerRecords.gameObject.SetActive(true);
                break;
        }
    }
}
