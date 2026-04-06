using UnityEngine;

public class EnemyPresenter : MonoBehaviour 
{
    [SerializeField] private EnemyHealthView healthView;

    private float maxHealth = 50f;
    private float currentHealth;

    private void Start() 
    {
        currentHealth = maxHealth;
        UpdateView();
    }

    public void TakeDamage(float damage) 
    {
        currentHealth -= damage;
        if (currentHealth <= 0) 
        {
            currentHealth = 0;
            Die();
        }
        UpdateView();
    }

    private void UpdateView() 
    {
        // Obliczamy procent zdrowia dla Slidera
        float normalizedHealth = currentHealth / maxHealth;
        healthView.SetHealthFill(normalizedHealth);
    }

    private void Die() 
    {
        // Logika śmierci przeciwnika, dodanie zasobów do GameModel itp.
        Destroy(gameObject);
    }
}