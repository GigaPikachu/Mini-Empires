using UnityEngine;
using TMPro;

public class UnitSpawnerBuilding : MonoBehaviour
{
    public GameObject unitPrefab;        // Prefab de la unidad a crear
    [SerializeField] public int unitCost = 5;             // Cuántos recursos cuesta crear una unidad
    public Vector2 spawnOffset = new Vector2(1, 0); // Offset para evitar que aparezca encima del edificio

    private void OnMouseDown()
    {
        // Solo responde al clic izquierdo
        if (Input.GetMouseButtonDown(0))
        {
            Debug.LogWarning("Buscando Error.");
            TrySpawnUnit();
        }
    }

    void TrySpawnUnit()
    {
        if (ResourceTextUI.Instance == null)
        {
            Debug.LogWarning("No se encontró ResourceTextUI en escena.");
            return;
        }

        if (ResourceTextUI.Instance.HasEnoughResources(unitCost))
        {
            Vector3 spawnPosition = transform.position + (Vector3)spawnOffset;
            Instantiate(unitPrefab, spawnPosition, Quaternion.identity);
            ResourceTextUI.Instance.RemoveResource(unitCost);
        }
        else
        {
            Debug.Log("No hay suficientes recursos.");
        }
    }
}