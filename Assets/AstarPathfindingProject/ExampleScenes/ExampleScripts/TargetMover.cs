using UnityEngine;
using System.Linq;

namespace Pathfinding
{
    public class TargetMover : MonoBehaviour
    {
        public LayerMask mask;
        public Transform target;
        IAstarAI[] ais;

        // Ya no se usa para doble clic
        public bool onlyOnRightClick = true;
        public bool use2D;

        Camera cam;

        public void Start()
        {
            cam = Camera.main;
            ais = FindObjectsOfType<MonoBehaviour>().OfType<IAstarAI>().ToArray();
            useGUILayout = false;
        }

        void Update()
        {
            // Solo mover el target si se hace clic derecho
            if (onlyOnRightClick && Input.GetMouseButtonDown(1))
            {
            }
            else if (!onlyOnRightClick)
            {
            }
        }
    }
}
