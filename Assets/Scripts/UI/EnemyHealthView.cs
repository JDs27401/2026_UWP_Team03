using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthView : MonoBehaviour 
{
    [SerializeField] private Slider healthSlider;

    // Przekazujemy znormalizowaną wartość od 0.0 do 1.0
    public void SetHealthFill(float normalizedHealth) 
    {
        healthSlider.value = normalizedHealth;
    }
}