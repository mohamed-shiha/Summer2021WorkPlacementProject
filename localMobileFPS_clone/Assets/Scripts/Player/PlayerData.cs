using System;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using UnityEngine;

public enum PlayerState
{
    Lobby,
    InGame,
    Dead,
    PreGame
}

public class PlayerData : NetworkBehaviour
{
    public Action<PlayerState, PlayerState> OnStateChanged;
    public PlayerState State
    {
        get { return _State; }
        set
        {
            OnStateChanged?.Invoke(_State, value);
            _State = value;
        }
    }
    private PlayerState _State;
    public Teams Team;
    public SkinnedMeshRenderer Renderer;
    public MeshRenderer _Renderer;
    public bool PlayMode = false;
    public float yVelocity = 0f;
    [Range(5f, 25f)]
    public float gravity = 15f;
    //the speed of the player movement
    [Range(5f, 15f)]
    public float movementSpeed = 10f;
    //jump speed
    [Range(5f, 15f)]
    public float jumpSpeed = 10f;

    //now the camera so we can move it up and down
    
    public float pitch = 0f;
    [Range(1f, 90f)]
    public float maxPitch = 85f;
    [Range(-1f, -90f)]
    public float minPitch = -85f;
    [Range(0.5f, 5f)]
    public float mouseSensitivity = 2f;


    // Testing
    //public Canvas _Canvas;

    private NetworkVariable<Teams> _team = new NetworkVariable<Teams>();
    public NetworkVariableBool Ready = new NetworkVariableBool();

    private void Awake()
    {
        _team.OnValueChanged += TeamValueChanged;
    }

    private void Start()
    {

        // _Canvas.enabled = IsLocalPlayer;
    }

    private void OnDisable()
    {
        _team.OnValueChanged -= TeamValueChanged;
    }

    private void TeamValueChanged(Teams previousValue, Teams newValue)
    {
        if (IsClient)
        {
            LocalSetTeam(newValue);
        }
    }

    public void LocalSetTeam(Teams newValue)
    {
        Team = newValue;
        if (Renderer != null)
            Renderer.material = DataManager.Instance.AllMaterials[newValue];
        else
            _Renderer.material = DataManager.Instance.AllMaterials[newValue];
    }

    [ServerRpc]
    public void _SetReadyServerRpc(bool ready)
    {
        this.Ready.Value = ready;
    }

    [ServerRpc]
    public void _SetTeamServerRpc(Teams newTeam)
    {
        _team.Value = newTeam;
        LocalSetTeam(newTeam);
    }



}
