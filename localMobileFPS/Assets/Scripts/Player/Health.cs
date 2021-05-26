using System;
using MLAPI;
using MLAPI.NetworkVariable;
using UnityEngine;

public class Health : NetworkBehaviour
{

    public NetworkVariableFloat MaxHealth = new NetworkVariableFloat(100);
    public NetworkVariableFloat CurrentHealth = new NetworkVariableFloat(100);
    public Action PlayerOutOfHealth;
    public bool TakeDamage(float amount)
    {
        CurrentHealth.Value -= amount;
        return CurrentHealth.Value <= 0;
    }

    public void Die()
    {
        Debug.Log("Player has no health");
        PlayerOutOfHealth?.Invoke();
    }

    public void AddHealth(float amount) => CurrentHealth.Value = CurrentHealth.Value + amount >= MaxHealth.Value ? MaxHealth.Value : CurrentHealth.Value + amount;
}
