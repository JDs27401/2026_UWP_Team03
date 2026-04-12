using UnityEngine;

public class Castle : MonoBehaviour
{
    private void Start()
    {
        Managers.GameManager.Instance.Model.OnBaseHealthChanged += CheckGameOver;
    }

    public void TakeDamage(int amount)
    {
        Managers.GameManager.Instance.Model.TakeDamage(amount);
    }

    private void CheckGameOver(int currentHealth)
    {
        if (currentHealth <= 0)
        {
            Debug.Log("Porażka! Zamek zniszczony!");
        }
    }

    private void OnDestroy()
    {
        if (Managers.GameManager.Instance != null && Managers.GameManager.Instance.Model != null)
        {
            Managers.GameManager.Instance.Model.OnBaseHealthChanged -= CheckGameOver;
        }
    }
}