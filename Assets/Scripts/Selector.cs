using System.Collections.Generic;
using UnityEngine;

public class UnitSelector : MonoBehaviour
{
    private enum SelectorState
    {
        Idle,
        Dragging,
        Ready,
        Commanding
    }

    [SerializeField] private LayerMask UnitLayer;

    private SelectorState currentState = SelectorState.Idle;

    private Vector2 startPos;
    private Vector2 endPos;

    private Texture2D whiteTexture;
    private List<GameObject> selectedUnits = new List<GameObject>();
    private Camera cam;

    void Start()
    {
        cam = Camera.main;

        whiteTexture = new Texture2D(1, 1);
        whiteTexture.SetPixel(0, 0, Color.white);
        whiteTexture.Apply();
    }

    void Update()
    {
        switch (currentState)
        {
            case SelectorState.Idle:
                HandleIdle();
                break;
            case SelectorState.Dragging:
                HandleDragging();
                break;
            case SelectorState.Ready:
                HandleReady();
                break;
            case SelectorState.Commanding:
                HandleCommanding();
                break;
        }
    }

    void HandleIdle()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CameraScript.IsCameraLocked = true;
            ClearSelection();
            startPos = cam.ScreenToWorldPoint(Input.mousePosition);
            currentState = SelectorState.Dragging;
        }
    }

    void HandleDragging()
    {
        if (Input.GetMouseButtonUp(0))
        {
            endPos = cam.ScreenToWorldPoint(Input.mousePosition);
            CameraScript.IsCameraLocked = false;

            SelectUnitsInArea(startPos, endPos);

            currentState = selectedUnits.Count > 0 ? SelectorState.Ready : SelectorState.Idle;
        }
    }

    void HandleReady()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CameraScript.IsCameraLocked = true;
            ClearSelection();
            startPos = cam.ScreenToWorldPoint(Input.mousePosition);
            currentState = SelectorState.Dragging;
        }

        if (Input.GetMouseButtonDown(1))
        {
            currentState = SelectorState.Commanding;
        }

        // (Opcional) Cancela la selección con tecla Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ClearSelection();
            currentState = SelectorState.Idle;
        }
    }

    void HandleCommanding()
    {
        MoveSelectedUnitsToMousePosition();
        currentState = SelectorState.Ready;
    }

    void OnGUI()
    {
        if (currentState != SelectorState.Dragging) return;

        Vector2 screenStart = cam.WorldToScreenPoint(startPos);
        Vector2 screenEnd = Input.mousePosition;

        screenStart.y = Screen.height - screenStart.y;
        screenEnd.y = Screen.height - screenEnd.y;

        Rect rect = GetScreenRect(screenStart, screenEnd);
        GUI.color = new Color(1, 1, 1, 0.25f);
        GUI.DrawTexture(rect, whiteTexture);
        GUI.color = Color.white;
        GUI.DrawTexture(rect, Texture2D.whiteTexture);
    }

    Rect GetScreenRect(Vector2 start, Vector2 end)
    {
        return new Rect(
            Mathf.Min(start.x, end.x),
            Mathf.Min(start.y, end.y),
            Mathf.Abs(start.x - end.x),
            Mathf.Abs(start.y - end.y)
        );
    }

    void SelectUnitsInArea(Vector2 worldStart, Vector2 worldEnd)
    {
        Vector2 min = Vector2.Min(worldStart, worldEnd);
        Vector2 max = Vector2.Max(worldStart, worldEnd);
        Collider2D[] hits = Physics2D.OverlapAreaAll(min, max, UnitLayer);

        foreach (var col in hits)
        {
            GameObject unit = col.gameObject;
            selectedUnits.Add(unit);

            var renderer = unit.GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                renderer.color = new Color(0.5f, 0.3f, 1f); // Azul-violeta
            }
        }
    }

    void ClearSelection()
    {
        foreach (GameObject unit in selectedUnits)
        {
            if (unit == null) continue;

            var renderer = unit.GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                renderer.color = Color.blue;
            }
        }

        selectedUnits.Clear();
    }

    void MoveSelectedUnitsToMousePosition()
    {
        Vector3 rawPosition = cam.ScreenToWorldPoint(Input.mousePosition);
        rawPosition.z = 0;

        float tileSize = 1f;

        Vector3 basePosition = new Vector3(
            Mathf.Floor(rawPosition.x / tileSize) * tileSize + tileSize / 2f,
            Mathf.Floor(rawPosition.y / tileSize) * tileSize + tileSize / 2f,
            0
        );

        selectedUnits.RemoveAll(unit => unit == null);

        int count = selectedUnits.Count;
        int gridSize = Mathf.CeilToInt(Mathf.Sqrt(count));
        int unitIndex = 0;

        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                if (unitIndex >= count) return;

                GameObject unit = selectedUnits[unitIndex];
                if (unit == null) continue;

                var ai = unit.GetComponent<Pathfinding.IAstarAI>();
                if (ai == null) continue;

                var targetHolder = unit.GetComponent<UnitTargetHolder>();
                if (targetHolder == null || targetHolder.target == null) continue;

                Vector3 offset = new Vector3(
                    (x - gridSize / 2f + 0.5f) * tileSize,
                    (y - gridSize / 2f + 0.5f) * tileSize,
                    0
                );

                targetHolder.target.position = basePosition + offset;
                ai.SearchPath();

                unitIndex++;
            }
        }
    }
}