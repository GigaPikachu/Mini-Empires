using UnityEngine;

public class EnemyTargetUpdater : MonoBehaviour
{
    public float detectionRadius = 5f;
    public LayerMask unitLayer;

    private Transform enemyTransform;
    private Transform currentTarget;

    private enum EnemyState
    {
        Idle,
        Chasing
    }

    private EnemyState currentState = EnemyState.Idle;

    void Start()
    {
        enemyTransform = transform.root;
    }

    void Update()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                HandleIdle();
                break;

            case EnemyState.Chasing:
                HandleChasing();
                break;
        }
    }

    void HandleIdle()
    {
        Transform unit = FindClosestUnit();
        if (unit != null)
        {
            currentTarget = unit;
            currentState = EnemyState.Chasing;
        }
    }

    void HandleChasing()
    {
        if (currentTarget == null)
        {
            currentState = EnemyState.Idle;
            return;
        }

        float distance = Vector2.Distance(enemyTransform.position, currentTarget.position);

        if (distance > detectionRadius)
        {
            currentTarget = null;
            currentState = EnemyState.Idle;
            return;
        }

        transform.position = currentTarget.position;
    }

    Transform FindClosestUnit()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(enemyTransform.position, detectionRadius, unitLayer);

        float closestDistance = Mathf.Infinity;
        Transform closest = null;

        foreach (var hit in hits)
        {
            float distance = Vector2.Distance(enemyTransform.position, hit.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = hit.transform;
            }
        }

        return closest;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}