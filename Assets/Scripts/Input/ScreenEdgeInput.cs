using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class ScreenEdgeInput : MonoBehaviour
{
    public float screenEdgeThreshold = 10f;
    public UnityEvent<Vector2> screenEdgeMove = new();

    [SerializeField] private Vector2 moveDirection = Vector2.zero;

    private void Update()
    {
        Camera camera = Camera.main;
        Vector2 pointerPos = Pointer.current.position.ReadValue();

        // Process camera move event
        Vector2 cameraMove = Vector2.zero;
        if (pointerPos.x < 0 + screenEdgeThreshold && pointerPos.x >= 0)
        {
            cameraMove.x -= 1;
        }
        else if (pointerPos.x > camera.pixelWidth - screenEdgeThreshold && pointerPos.x <= camera.pixelWidth)
        {
            cameraMove.x += 1;
        }

        if (pointerPos.y < 0 + screenEdgeThreshold && pointerPos.y >= 0)
        {
            cameraMove.y -= 1;
        }
        else if (pointerPos.y > camera.pixelHeight - screenEdgeThreshold && pointerPos.y <= camera.pixelHeight)
        {
            cameraMove.y += 1;
        }

        if (moveDirection != cameraMove)
        {
            moveDirection = cameraMove;
            screenEdgeMove.Invoke(cameraMove);
        }
    }
}
