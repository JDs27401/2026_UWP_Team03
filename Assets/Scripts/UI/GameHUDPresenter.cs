using UnityEngine;
using System.Collections.Generic;

public class GameHUDPresenter : MonoBehaviour 
{
    [SerializeField] private GameHUDView hudView;
    
    [SerializeField] private Castle playerCastle; 
    [SerializeField] private WaveSpawner waveSpawner;

    private void Awake() 
    {
        if (playerCastle != null) 
        {
            playerCastle.OnHealthChanged += hudView.UpdateBaseHealth;
        }

        if (waveSpawner != null) 
        {
            waveSpawner.OnWaveStarted += hudView.UpdateWaveCount;
            
        }
        else 
        {
            Debug.LogWarning("Brak przypisanego WaveSpawner w GameHUDPresenter!");
        }
    }

    private void OnDestroy() 
    {
        if (playerCastle != null) 
        {
            playerCastle.OnHealthChanged -= hudView.UpdateBaseHealth;
        }

        if (waveSpawner != null)
        {
            waveSpawner.OnWaveStarted -= hudView.UpdateWaveCount;
        }
    }
}