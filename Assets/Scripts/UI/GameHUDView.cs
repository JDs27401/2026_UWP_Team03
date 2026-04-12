using UnityEngine;
using TMPro;

public class GameHUDView : MonoBehaviour 
{
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI baseHealthText;
    [SerializeField] private TextMeshProUGUI waveCountText;
    [SerializeField] private TextMeshProUGUI wavePreviewText;

    public void UpdateCoins(int coins) 
    {
        coinsText.text = $"Coins: {coins}";
    }

    public void UpdateBaseHealth(int health) 
    {
        baseHealthText.text = $"Base health: {health} HP";
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