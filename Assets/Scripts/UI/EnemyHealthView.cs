using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthView : MonoBehaviour 
{
    [SerializeField] private Slider healthSlider;

    public void SetHealthFill(float normalizedHealth) 
    {
        healthSlider.value = normalizedHealth;
    }
}