using System;

public class EnemyModel 
{
    public float MaxHealth { get; private set; }
    public float CurrentHealth { get; private set; }

    public event Action<float> OnHealthChanged; 
    public event Action OnDied;

    public EnemyModel(float maxHealth) 
    {
        MaxHealth = maxHealth;
        CurrentHealth = maxHealth;
    }

    public void TakeDamage(float damage) 
    {
        if (CurrentHealth <= 0) return;

        CurrentHealth -= damage;
        
        if (CurrentHealth <= 0) 
        {
            CurrentHealth = 0;
            OnHealthChanged?.Invoke(GetNormalizedHealth());
            OnDied?.Invoke();
        }
        else
        {
            OnHealthChanged?.Invoke(GetNormalizedHealth());
        }
    }

    public float GetNormalizedHealth() 
    {
        return CurrentHealth / MaxHealth;
    }
}