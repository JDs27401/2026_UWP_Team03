using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct WavePreviewData 
{
    public string EnemyType;
    public int EnemyCount;
}

public class GameModel 
{
    public int Coins { get; private set; }
    public int BaseHealth { get; private set; }
    public int CurrentWave { get; private set; }

    public event Action<int> OnCoinsChanged;
    public event Action<int> OnBaseHealthChanged;
    public event Action<int> OnWaveChanged;

    public GameModel(int startCoins, int startHealth, int startWave) 
    {
        Coins = startCoins;
        BaseHealth = startHealth;
        CurrentWave = startWave;
    }

    public void SetCoins(int value)
    {
        Coins = Mathf.Max(0, value);
        OnCoinsChanged?.Invoke(Coins);
    }

    public bool TrySpendCoins(int amount)
    {
        if (amount <= 0)
        {
            return true;
        }

        if (Coins < amount)
        {
            return false;
        }

        SetCoins(Coins - amount);
        return true;
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