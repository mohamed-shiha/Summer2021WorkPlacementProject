using UnityEngine;

public class Health : MonoBehaviour
{
    public float MaxHealth;
    public float CurrentHealth;
    
    public bool TakeDamage(float amount)
    {
        CurrentHealth -= amount;
        return CurrentHealth <= 0;
    }

    public void Die() => Destroy(this.gameObject);

    public void AddHealth(float amount) => CurrentHealth = CurrentHealth + amount >= MaxHealth ? MaxHealth : CurrentHealth + amount;
}
