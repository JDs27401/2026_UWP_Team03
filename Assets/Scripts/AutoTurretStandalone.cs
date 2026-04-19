using UnityEngine;

public class AutoTurretStandalone : MonoBehaviour
{
    [Header("Targeting")]
    [SerializeField] private float range = 12f;
    [SerializeField] private LayerMask enemyLayers = ~0;
    [SerializeField] private string enemyTag = "Enemy";
    [SerializeField] private float retargetInterval = 0.2f;

    [Header("Combat")]
    [SerializeField] private float fireRate = 2f;
    [SerializeField] private float castleDamageBuffMultiplier = 1.5f;
    [SerializeField] private float castleDamageBuffDuration = 1f;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;

    [Header("Rotation")]
    [SerializeField] private Transform rotatingPart;
    [SerializeField] private float turnSpeed = 12f;

    private Transform currentTarget;
    private float fireCooldown;
    private float castleDamageBuffTimer;

    private void OnEnable()
    {
        Castle.OnCastleDamaged += HandleCastleDamaged;
    }

    private void OnDisable()
    {
        Castle.OnCastleDamaged -= HandleCastleDamaged;
    }

    private void Start()
    {
        InvokeRepeating(nameof(UpdateTarget), 0f, retargetInterval);
    }

    private void Update()
    {
        if (castleDamageBuffTimer > 0f)
        {
            castleDamageBuffTimer -= Time.deltaTime;
        }

        if (currentTarget == null)
        {
            return;
        }

        RotateTowardsTarget();

        fireCooldown -= Time.deltaTime;
        if (fireCooldown <= 0f)
        {
            Shoot();
            float effectiveFireRate = fireRate;
            if (castleDamageBuffTimer > 0f)
            {
                effectiveFireRate *= castleDamageBuffMultiplier;
            }

            fireCooldown = 1f / Mathf.Max(0.01f, effectiveFireRate);
        }
    }

    private void HandleCastleDamaged()
    {
        // Każde trafienie zamku odświeża czas buffa szybkostrzelności.
        castleDamageBuffTimer = castleDamageBuffDuration;
    }

    private void UpdateTarget()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, range, enemyLayers, QueryTriggerInteraction.Ignore);

        float closestSqr = float.MaxValue;
        Transform bestTarget = null;

        for (int i = 0; i < hits.Length; i++)
        {
            Transform candidate = hits[i].transform;
            if (!candidate.CompareTag(enemyTag))
            {
                continue;
            }

            float sqrDistance = (candidate.position - transform.position).sqrMagnitude;
            if (sqrDistance < closestSqr)
            {
                closestSqr = sqrDistance;
                bestTarget = candidate;
            }
        }

        currentTarget = bestTarget;
    }

    private void RotateTowardsTarget()
    {
        Transform pivot = rotatingPart != null ? rotatingPart : transform;
        Vector3 direction = currentTarget.position - pivot.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.0001f)
        {
            return;
        }

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        pivot.rotation = Quaternion.Lerp(pivot.rotation, targetRotation, Time.deltaTime * turnSpeed);
    }

    private void Shoot()
    {
        if (projectilePrefab == null || firePoint == null)
        {
            return;
        }

        GameObject projectileObject = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        AutoProjectileStandalone projectile = projectileObject.GetComponent<AutoProjectileStandalone>();
        if (projectile != null)
        {
            projectile.SetTarget(currentTarget);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}

