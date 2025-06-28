using UnityEngine;
using System.Collections.Generic;

public class DamageDealer : MonoBehaviour
{
    public float detectionRadius = 0.3f;
    public float damageInterval = 1f;
    public int damageAmount = 1;
    public LayerMask damageableLayers;

    private float lastDamageTime;

    void Update()
    {
        if (Time.time - lastDamageTime >= damageInterval)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRadius, damageableLayers);

            HashSet<GameObject> damagedThisTick = new HashSet<GameObject>();

            foreach (var hit in hits)
            {
                if (damagedThisTick.Contains(hit.gameObject)) continue;

                Health health = hit.GetComponent<Health>();
                if (health != null)
                {
                    health.TakeDamage(damageAmount);
                    damagedThisTick.Add(hit.gameObject);
                }
            }

            lastDamageTime = Time.time;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
