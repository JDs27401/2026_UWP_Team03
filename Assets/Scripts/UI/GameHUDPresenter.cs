using UnityEngine;
using System.Collections.Generic;

public class GameHUDPresenter : MonoBehaviour 
{
    [SerializeField] private GameHUDView hudView;
    
    // Lista fal (w pełnej grze zarządzałby tym osobny WaveManager)
    [SerializeField] private List<WavePreviewData> upcomingWaves;

    private GameModel gameModel;

    private void Awake() 
    {
        // Inicjalizacja modelu
        gameModel = new GameModel(startCoins: 150, startHealth: 100, startWave: 1);

        // Subskrypcja zdarzeń modelu
        gameModel.OnCoinsChanged += hudView.UpdateCoins;
        gameModel.OnBaseHealthChanged += hudView.UpdateBaseHealth;
        gameModel.OnWaveChanged += HandleWaveChanged;
    }

    private void Start() 
    {
        // Ręczne wymuszenie pierwszej aktualizacji widoku po starcie
        hudView.UpdateCoins(gameModel.Coins);
        hudView.UpdateBaseHealth(gameModel.BaseHealth);
        HandleWaveChanged(gameModel.CurrentWave);
    }

    private void HandleWaveChanged(int currentWave) 
    {
        hudView.UpdateWaveCount(currentWave);

        // Generowanie tekstu podglądu kolejnej fali (indeksowanie od 0)
        int nextWaveIndex = currentWave - 1; 
        
        if (nextWaveIndex < upcomingWaves.Count) 
        {
            var nextWave = upcomingWaves[nextWaveIndex];
            hudView.UpdateWavePreview($"- {nextWave.EnemyCount}x {nextWave.EnemyType}");
        } 
        else 
        {
            hudView.UpdateWavePreview("Ostatnia fala!");
        }
    }

    private void OnDestroy() 
    {
        // ZAWSZE odsubskrybuj eventy, aby uniknąć błędów (NullReferenceException)
        if (gameModel != null) 
        {
            gameModel.OnCoinsChanged -= hudView.UpdateCoins;
            gameModel.OnBaseHealthChanged -= hudView.UpdateBaseHealth;
            gameModel.OnWaveChanged -= HandleWaveChanged;
        }
    }

    // --- Metody do testowania w Unity (podepnij pod przyciski) ---
    public void Test_AddCoins() => gameModel.AddCoins(50);
    public void Test_TakeDamage() => gameModel.TakeDamage(10);
    public void Test_NextWave() => gameModel.NextWave();
}