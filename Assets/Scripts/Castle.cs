using UnityEngine;
using System;

public class Castle : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public event Action<int> OnHealthChanged;

    void Start()
    {
        currentHealth = maxHealth;
        
        OnHealthChanged?.Invoke(currentHealth);
        
        Debug.Log($"Zamek gotowy do obrony! HP: {currentHealth}");
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth < 0) currentHealth = 0;

        OnHealthChanged?.Invoke(currentHealth);
        
        Debug.Log($"Zamek obrywa! Aktualne HP: {currentHealth}");
        
        if (currentHealth <= 0)
        {
            Debug.Log("Porażka! Zamek zniszczony!");
            // Logika Game Over
        }
    }
}