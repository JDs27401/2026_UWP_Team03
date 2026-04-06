using UnityEngine;

public class Castle : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        Debug.Log($"Zamek gotowy do obrony! HP: {currentHealth}");
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log($"Zamek obrywa! Aktualne HP: {currentHealth}");
        
        if (currentHealth <= 0)
        {
            Debug.Log("Porażka! Zamek zniszczony!");
            // Tutaj w przyszłości dodamy logikę Game Over
        }
    }
}

