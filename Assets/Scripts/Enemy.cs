using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    public int damageToCastle = 10;
    public float speed = 5f;
    public int maxHealth = 30;
    [SerializeField] private float castleDamageDistance = 1.25f;
    [SerializeField] private float spawnSnapDistance = 3f;
    [SerializeField] private float destinationSnapDistance = 6f;
    [SerializeField] private float repathInterval = 0.5f;
    [SerializeField] private bool usePathAreaOnly = false;

    private int currentHealth;
    private NavMeshAgent agent;
    private Castle castleTarget;
    private bool destinationAssigned;
    private float repathTimer;
    private int navAreaMask = NavMesh.AllAreas;

    void Start()
    {
        currentHealth = maxHealth;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;

        int pathArea = NavMesh.GetAreaFromName("Path");
        if (usePathAreaOnly && pathArea >= 0)
        {
            navAreaMask = 1 << pathArea;
        }
        else
        {
            navAreaMask = NavMesh.AllAreas;
        }

        agent.areaMask = navAreaMask;

        castleTarget = FindObjectOfType<Castle>();
        if (castleTarget != null)
        {
            if (!TrySnapToNavMesh())
            {
                Debug.LogError("Enemy spawn jest poza NavMesh. Przesuń SpawnPoint na ścieżkę NavMesh.");
                Destroy(gameObject);
                return;
            }

            destinationAssigned = TryAssignDestination();
        }
        else
        {
            Debug.LogError("Nie znaleziono zamku na scenie (Castle)!");
        }
    }

    void Update()
    {
        if (castleTarget == null || agent == null) return;

        if (!agent.isOnNavMesh)
        {
            TrySnapToNavMesh();
            return;
        }

        repathTimer -= Time.deltaTime;
        if (repathTimer <= 0f)
        {
            bool needsRepath = agent.pathStatus == NavMeshPathStatus.PathInvalid || (!agent.pathPending && !agent.hasPath && destinationAssigned);
            if (needsRepath)
            {
                destinationAssigned = TryAssignDestination();
            }

            repathTimer = repathInterval;
        }

        // Damage tylko po fizycznym podejściu do zamku (eliminuje instant damage po spawnie).
        float sqrDistanceToCastle = (castleTarget.transform.position - transform.position).sqrMagnitude;
        bool reachedCastle = sqrDistanceToCastle <= castleDamageDistance * castleDamageDistance;

        if (reachedCastle)
        {
            ReachCastle();
        }
    }

    private bool TrySnapToNavMesh()
    {
        if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, spawnSnapDistance, navAreaMask))
        {
            return agent.Warp(hit.position);
        }

        return false;
    }

    private bool TryAssignDestination()
    {
        Vector3 desiredPosition = castleTarget.transform.position;

        if (NavMesh.SamplePosition(desiredPosition, out NavMeshHit hit, destinationSnapDistance, navAreaMask))
        {
            return agent.SetDestination(hit.position);
        }

        return agent.SetDestination(desiredPosition);
    }

    void ReachCastle()
    {
        if (castleTarget != null)
        {
            castleTarget.TakeDamage(damageToCastle);
        }

        Destroy(gameObject);
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Tu później możesz dodać nagrodę gold/efekt śmierci.
        Destroy(gameObject);
    }
}
