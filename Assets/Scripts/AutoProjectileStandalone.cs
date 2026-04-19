using UnityEngine;

public class AutoProjectileStandalone : MonoBehaviour
{
    [SerializeField] private float speed = 35f;
    [SerializeField] private int damage = 10;
    [SerializeField] private float maxLifetime = 4f;

    private Transform target;

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private void Start()
    {
        Destroy(gameObject, maxLifetime);
    }

    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 direction = target.position - transform.position;
        float step = speed * Time.deltaTime;

        if (direction.sqrMagnitude <= step * step)
        {
            HitTarget();
            return;
        }

        transform.position += direction.normalized * step;
        transform.LookAt(target);
    }

    private void HitTarget()
    {
        if (target != null)
        {
            // Nie wymaga konkretnej klasy wroga - wywoła metodę tylko jeśli istnieje.
            target.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
        }

        Destroy(gameObject);
    }
}

