using System;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using UnityEngine;

public class Health : NetworkBehaviour
{

    public NetworkVariableFloat MaxHealth = new NetworkVariableFloat(100);
    public NetworkVariableFloat CurrentHealth = new NetworkVariableFloat(100);
    public Action PlayerOutOfHealth;


    public void TakeDamage(float amount)
    {
        CurrentHealth.Value -= amount;
    }

    private void Update()
    {
        if (CurrentHealth.Value <= 0)
            Die();
    }
    public void Die()
    {
        CurrentHealth.Value = MaxHealth.Value;
        //Debug.Log("Player has no health");
        PlayerOutOfHealth.Invoke();
    }

    public void AddHealth(float amount) => CurrentHealth.Value = CurrentHealth.Value + amount >= MaxHealth.Value ? MaxHealth.Value : CurrentHealth.Value + amount;
}
