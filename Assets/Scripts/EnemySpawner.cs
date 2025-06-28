using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject AllySpawner;

    private Health health;
    private int lastHealth;

    void Start()
    {
        health = GetComponent<Health>();

        if (health == null)
        {
            Debug.LogError("La mina requiere un componente Health.");
            enabled = false;
            return;
        }

        lastHealth = health.CurrentHealth;
    }

    void Update()
    {
        health = GetComponent<Health>();
        if (health.CurrentHealth <= 0)
        {
            //crear spawner aliado en la misma pocicion que este objeto
            Instantiate(AllySpawner, transform.position, Quaternion.identity);

        }
    }
}
