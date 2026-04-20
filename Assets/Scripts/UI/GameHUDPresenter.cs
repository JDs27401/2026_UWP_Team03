using UnityEngine;
using Build;
using System.Collections.Generic;

public class GameHUDPresenter : MonoBehaviour 
{
    [SerializeField] private GameHUDView hudView;
    [SerializeField] private List<WavePreviewData> upcomingWaves;
    
    [Header("Builder Reference")]
    [SerializeField] private TurretBuilder turretBuilder;
    
    [Header("Controls Info")]
    [TextArea(3, 10)]
    [SerializeField] private string controlsText = "B - Build Mode\nN - Sell Mode\nZ - Undo\nY - Redo\nLMB - Confirm";

    private GameModel gameModel;
    private int currentLocalWave = 1;

    private void Start() 
    {
        gameModel = Managers.GameManager.Instance.GameModel; 
        
        gameModel.OnCoinsChanged += hudView.UpdateCoins;
        gameModel.OnBaseHealthChanged += hudView.UpdateBaseHealth; 

        hudView.UpdateCoins(gameModel.Coins);
        hudView.UpdateBaseHealth(gameModel.BaseHealth); 
        
        hudView.UpdateControlsText(controlsText);
        
        if (Managers.WaveManager.Instance != null)
        {
            Managers.WaveManager.Instance.OnWaveCompleted += HandleWaveCompleted;
        }

        if (turretBuilder != null)
        {
            turretBuilder.OnModeChanged += HandleModeChanged;
            HandleModeChanged(turretBuilder.CurrentMode);
        }

        UpdateWaveUI();
    }

    private void HandleModeChanged(TurretBuilder.Mode mode)
    {
        hudView.UpdateCurrentMode(mode.ToString());
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