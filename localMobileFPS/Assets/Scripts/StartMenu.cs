using System;
using System.Text;
using MLAPI;
using TMPro;
using UnityEngine;
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
    public Button ConnectCreateButton;

    private void Start()
    {
        // start with start screen 

        NetworkManager.Singleton.OnServerStarted += Singleton_OnServerStarted;
        NetworkManager.Singleton.OnClientConnectedCallback += Singleton_OnClientConnectedCallback;
        NetworkManager.Singleton.OnClientDisconnectCallback += Singleton_OnClientDisconnectCallback;

        ConnectCreateButton.onClick.AddListener(() => ConnectOrHost(ConnectCreateButton.GetComponentInChildren<TextMeshProUGUI>().text));
        ShowScreen(ScreenNames.Start);
    }

    private void Singleton_OnServerStarted()
    {
        if (NetworkManager.Singleton.IsHost)
            Singleton_OnClientConnectedCallback(NetworkManager.Singleton.LocalClientId);
    }

    private void Singleton_OnClientConnectedCallback(ulong obj)
    {
        ShowScreen(ScreenNames.Lobby);
    }

    private void OnDestroy()
    {
        if (NetworkManager.Singleton == null) return;
        NetworkManager.Singleton.OnServerStarted -= Singleton_OnServerStarted;
        NetworkManager.Singleton.OnClientConnectedCallback -= Singleton_OnClientConnectedCallback;
        NetworkManager.Singleton.OnClientDisconnectCallback -= Singleton_OnClientDisconnectCallback;
    }

    private void Singleton_OnClientDisconnectCallback(ulong clientId)
    {
        if (clientId == NetworkManager.Singleton.LocalClientId)
            ShowScreen(ScreenNames.Start);
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
        // start showing animation 
        // Set start menu panel to not active
        // set play mode screen to active 
        ShowScreen(ScreenNames.Settings);
        Debug.Log("Button Start Settings");
    }

    public void GoToPlayMode()
    {
        // start showing animation 
        // Set start menu panel to not active
        // set play mode screen to active 
        ShowScreen(ScreenNames.PlayMode);
        Debug.Log("Button Start Game Mode");
    }

    public void SaveAndExit()
    {
        // start showing animation 
        // Set start menu panel to not active
        // set play mode screen to active 
        Application.Quit();
        Debug.Log("Button Save and exit");
    }


    public void ConnectOrHost(string button)
    {
        string buttonClickedName = button.ToLower();
        if (buttonClickedName.Equals("connect"))
            StartAsClient();
        else StartAsHost();
        //Debug.Log(button);
    }

    public void ShowPlayerRecords()
    {
        // start showing animation 
        // Set start menu panel to not active
        // set play mode screen to active 
        ShowScreen(ScreenNames.PlayerRecords);
        Debug.Log("Button Show Player Records");
    }

    public void BackToMainMenu()
    {
        ShowScreen(ScreenNames.Start);
        Debug.Log("Button Show BackToMain");
    }


    public void StartAsClient()
    {
        NetworkManager.Singleton.NetworkConfig.ConnectionData = Encoding.ASCII.GetBytes(ServerPasswordInput.text);
        NetworkManager.Singleton.StartClient();
    }

    public void StartAsHost()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback += Singleton_ConnectionApprovalCallback;
        NetworkManager.Singleton.StartHost();
    }

    private void Singleton_ConnectionApprovalCallback(byte[] connectionData, ulong clientId, NetworkManager.ConnectionApprovedDelegate callBack)
    {
        string password = Encoding.ASCII.GetString(connectionData);
        bool approveConnection = password.Equals(ServerPasswordInput.text);
        int posIndex = NetworkManager.Singleton.ConnectedClients.Count;
        Transform pos = SpawnLocationsParent.GetChild(posIndex);
        Debug.Log(pos);
        callBack(true, null, approveConnection, pos.position, pos.rotation);
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
