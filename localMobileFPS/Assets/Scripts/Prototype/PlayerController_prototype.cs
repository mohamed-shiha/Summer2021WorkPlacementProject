using System;
using UnityEngine;

public enum PlayerState
{
    Lobby,
    InGame,
    Dead,
    PreGame
}
public class PlayerController_prototype : MonoBehaviour
{
    public Action OnPlayerMovement;
    public Action OnPlayerAttacking;
    public Action<PlayerState,PlayerState> OnStateChanged;
    public PlayerState State
    {
        get { return _State; }
        set
        {
            OnStateChanged.Invoke(_State,value);
            _State = value;
        }
    }
    private PlayerState _State;
}
