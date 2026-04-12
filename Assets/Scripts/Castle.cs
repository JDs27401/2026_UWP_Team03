using UnityEngine;

public class Castle : MonoBehaviour
{
    private void Start()
    {
        Managers.GameManager.Instance.GameModel.OnBaseHealthChanged += CheckGameOver;
    }

    public void TakeDamage(int amount)
    {
        Managers.GameManager.Instance.GameModel.TakeDamage(amount);
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
        if (Managers.GameManager.Instance != null && Managers.GameManager.Instance.GameModel != null)
        {
            Managers.GameManager.Instance.GameModel.OnBaseHealthChanged -= CheckGameOver;
        }
    }
}