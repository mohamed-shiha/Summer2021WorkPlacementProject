using System.Linq.Expressions;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using UnityEngine;

public class PlayerData : NetworkBehaviour
{

    public Teams Team;
    public MeshRenderer Renderer;
    // Testing
    public Canvas _Canvas;

    private NetworkVariable<Teams> _team = new NetworkVariable<Teams>();
    public NetworkVariableBool Ready = new NetworkVariableBool();

    private void Awake()
    {
        _team.OnValueChanged += TeamValueChanged;
    }

    private void Start()
    {

        _Canvas.enabled = IsLocalPlayer;
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
        Renderer.material = DataManager.Instance.AllMaterials[newValue];
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
