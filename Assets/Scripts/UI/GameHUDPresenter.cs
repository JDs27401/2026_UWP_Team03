using UnityEngine;
using UnityEngine.InputSystem;
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
    [SerializeField] private string collapsedControlsText = "H - help";
    [TextArea(3, 10)]
    [SerializeField] private string expandedControlsText =
        "Keybinds:\n" +
        "B - Build mode\n" +
        "N - Sell mode\n" +
        "Z - Undo\n" +
        "Y - Redo\n" +
        "LMB - Confirm\n" +
        "Defend the tower! When enemy reaches it, it gets damaged!";

    private GameModel gameModel;
    private int currentLocalWave = 1;
    private bool isHelpExpanded;

    private void Start() 
    {
        gameModel = Managers.GameManager.Instance.GameModel; 
        
        gameModel.OnCoinsChanged += hudView.UpdateCoins;
        gameModel.OnBaseHealthChanged += hudView.UpdateBaseHealth; 

        hudView.UpdateCoins(gameModel.Coins);
        hudView.UpdateBaseHealth(gameModel.BaseHealth); 
        
        isHelpExpanded = false;
        UpdateControlsHelp();
        
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

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.hKey.wasPressedThisFrame)
        {
            isHelpExpanded = !isHelpExpanded;
            UpdateControlsHelp();
        }
    }

    private void UpdateControlsHelp()
    {
        string helpText = isHelpExpanded ? expandedControlsText : collapsedControlsText;
        hudView.UpdateControlsText(helpText);
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