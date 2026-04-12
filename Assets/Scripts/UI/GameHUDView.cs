using UnityEngine;
using TMPro;

public class GameHUDView : MonoBehaviour 
{
    [Header("Main statistics")]
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI baseHealthText;
    [SerializeField] private TextMeshProUGUI waveCountText;
    
    [Header("Wave preview")]
    [SerializeField] private TextMeshProUGUI wavePreviewText;

    public void UpdateCoins(int coins) 
    {
        coinsText.text = $"Coins: {coins}";
    }

    public void UpdateBaseHealth(int health) 
    {
        baseHealthText.text = $"Health: {health} HP";
    }

    public void UpdateWaveCount(int wave) 
    {
        waveCountText.text = $"Wave: {wave}";
    }

    public void UpdateWavePreview(string previewInfo) 
    {
        wavePreviewText.text = $"Next wave:\n{previewInfo}";
    }
}