using UnityEngine;

public class EnemyPresenter : MonoBehaviour 
{
    [SerializeField] private EnemyHealthView healthView;
    
    private EnemyModel model;

    private void Awake() 
    {
        model = new EnemyModel(50f); 
        
        model.OnHealthChanged += healthView.SetHealthFill;
        model.OnDied += HandleDeath;
    }

    private void Start() 
    {
        healthView.SetHealthFill(model.GetNormalizedHealth());
    }

    public void TakeDamage(float damage) 
    {
        model.TakeDamage(damage);
    }

    private void HandleDeath() 
    {
        Destroy(gameObject);
    }

    private void OnDestroy() 
    {
        if (model != null) 
        {
            model.OnHealthChanged -= healthView.SetHealthFill;
            model.OnDied -= HandleDeath;
        }
    }
}