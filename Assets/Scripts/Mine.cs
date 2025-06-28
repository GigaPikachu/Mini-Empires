using Unity.VisualScripting;
using UnityEngine;

public class Mine : MonoBehaviour
{

    private Health health;

    void Start()
    {
        health = GetComponent<Health>();
    }

    void Update()
    {
        health = GetComponent<Health>();
        if (health.CurrentHealth <= 0)
        {
            ResourceTextUI.Instance.SumarRecursos(5);
            Destroy(gameObject); // Destruye la mina si ya no tiene salud
        }
    }
}
