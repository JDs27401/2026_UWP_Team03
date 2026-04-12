using UnityEngine;
using System.Collections.Generic;

public class GameHUDPresenter : MonoBehaviour 
{
    [SerializeField] private GameHUDView hudView;
    [SerializeField] private List<WavePreviewData> upcomingWaves;

    private GameModel gameModel;
    private int currentLocalWave = 1;

    private void Start() 
    {
        gameModel = Managers.GameManager.Instance.GameModel; 
        
        gameModel.OnCoinsChanged += hudView.UpdateCoins;
        gameModel.OnBaseHealthChanged += hudView.UpdateBaseHealth; 

        hudView.UpdateCoins(gameModel.Coins);
        hudView.UpdateBaseHealth(gameModel.BaseHealth); 
        
        if (Managers.WaveManager.Instance != null)
        {
            Managers.WaveManager.Instance.OnWaveCompleted += HandleWaveCompleted;
        }

        UpdateWaveUI();
    }

    private void HandleWaveCompleted()
    {
        hudView.UpdateWavePreview("Wave completed");
        Invoke(nameof(StartNextLocalWave), 3f); 
    }

    private void StartNextLocalWave()
    {
        currentLocalWave++;
        UpdateWaveUI();
    }

    private void UpdateWaveUI()
    {
        hudView.UpdateWaveCount(currentLocalWave);

        int nextWaveIndex = currentLocalWave; 
        
        if (nextWaveIndex < upcomingWaves.Count) 
        {
            var nextData = upcomingWaves[nextWaveIndex];
            hudView.UpdateWavePreview($"{nextData.EnemyCount}x {nextData.EnemyType}");
        } 
        else 
        {
            hudView.UpdateWavePreview("Last wave!");
        }
    }

    private void OnDestroy() 
    {
        if (gameModel != null) 
        {
            gameModel.OnCoinsChanged -= hudView.UpdateCoins;
            gameModel.OnBaseHealthChanged -= hudView.UpdateBaseHealth;
        }
        
        if (Managers.WaveManager.Instance != null)
        {
            Managers.WaveManager.Instance.OnWaveCompleted -= HandleWaveCompleted;
        }
    }
}