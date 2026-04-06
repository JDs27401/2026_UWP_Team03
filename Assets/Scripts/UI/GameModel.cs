using System;
using System.Collections.Generic;

// Czysta klasa C#, bez dziedziczenia po MonoBehaviour
public class GameModel 
{
    public int Coins { get; private set; }
    public int BaseHealth { get; private set; }
    public int CurrentWave { get; private set; }

    // Eventy informujące o zmianie stanu
    public event Action<int> OnCoinsChanged;
    public event Action<int> OnBaseHealthChanged;
    public event Action<int> OnWaveChanged;

    public GameModel(int startCoins, int startHealth, int startWave) 
    {
        Coins = startCoins;
        BaseHealth = startHealth;
        CurrentWave = startWave;
    }

    public void AddCoins(int amount) 
    {
        Coins += amount;
        OnCoinsChanged?.Invoke(Coins);
    }

    public void TakeDamage(int damage) 
    {
        BaseHealth -= damage;
        if (BaseHealth < 0) BaseHealth = 0;
        OnBaseHealthChanged?.Invoke(BaseHealth);
    }

    public void NextWave() 
    {
        CurrentWave++;
        OnWaveChanged?.Invoke(CurrentWave);
    }
}

// Struktura opisująca nadchodzącą falę
[Serializable]
public struct WavePreviewData 
{
    public string EnemyType;
    public int EnemyCount;
}