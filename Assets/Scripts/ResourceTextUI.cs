using UnityEngine;
using TMPro;

public class ResourceTextUI : MonoBehaviour
{
    public static ResourceTextUI Instance;

    private int Recursos;
    private TextMeshProUGUI textMesh;
    private void Start()
    {
        Instance = this;
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        textMesh.text = "Recursos: " + Recursos.ToString("0");
    }

    public void SumarRecursos(int RecursosEntrada)
    {
        Recursos += RecursosEntrada;
    }

    public bool HasEnoughResources(int amount)
    {
        return Recursos >= amount;
    }

    public void RemoveResource(int amount)
    {
        Recursos -= amount;
        Recursos = Mathf.Max(Recursos, 0);
    }
}