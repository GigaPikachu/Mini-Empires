using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private Vector2 mousePos;
    private float speed = 10f;
    private float edgeThickness = 0.1f;
    private float limit;

    public static bool IsCameraLocked = false;

    private void Start()
    {
        limit = Screen.height * edgeThickness;
    }

    void Update()
    {
        if (IsCameraLocked) return; // 🚫 Bloqueo activado

        mousePos = Input.mousePosition;

        float moveX = 0f;
        float moveY = 0f;

        if (mousePos.y >= Screen.height - limit)
            moveY = 1f;
        else if (mousePos.y <= limit)
            moveY = -1f;

        if (mousePos.x >= Screen.width - limit)
            moveX = 1f;
        else if (mousePos.x <= limit)
            moveX = -1f;

        Vector3 moveDir = new Vector3(moveX, moveY, 0f);
        transform.Translate(moveDir * speed * Time.deltaTime);
    }
}