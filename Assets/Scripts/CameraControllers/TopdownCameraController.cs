using Cinemachine;
using UnityEngine;
using UnityTools;

public class TopdownCameraController : MonoBehaviour
{
    public bool stopScrollingOnLostAppFocus = true;
    public float scrollSpeed = 10f;
    public float minZoom = 2.5f;
    public float maxZoom = 7f;
    public float cameraTiltDegrees = 35.3f;
    public float rotationSpeed = 100f;
    public Bounds cameraBounds;
    public GameObjectEventEmitter environmentFocusEvent;
    public EventEmitter<Vector3> cameraCenterEvent;

    [SerializeField] private Vector2 moveDirection;
    [SerializeField] private float rotateDirection;
    private new Camera camera;
    private CinemachineVirtualCamera cinemachineVc;

    private void OnValidate()
    {
        if (transform.parent.GetComponentInChildren<CinemachineVirtualCamera>() == null)
        {
            Debug.LogError("Need CinemachineVirtualCamera as a sibling", this);
        }
    }

    private void Awake()
    {
        cinemachineVc = transform.parent.GetComponentInChildren<CinemachineVirtualCamera>();
        cameraBounds.extents = Vector3.positiveInfinity;
    }

    private void OnEnable()
    {
        if (environmentFocusEvent != null)
        {
            environmentFocusEvent.eventEmitter.AddListener(SetBounds);
        }

        if (cameraCenterEvent != null)
        {
            cameraCenterEvent.eventEmitter.AddListener(SetPosition);
        }
    }

    private void OnDisable()
    {
        if (environmentFocusEvent != null)
        {
            environmentFocusEvent.eventEmitter.RemoveListener(SetBounds);
        }

        if (cameraCenterEvent != null)
        {
            cameraCenterEvent.eventEmitter.RemoveListener(SetPosition);
        }
    }

    private void Update()
    {
        if (stopScrollingOnLostAppFocus && !Application.isFocused)
        {
            Move(Vector2.zero);
            Rotate(0f);
            return;
        }

        if (camera == null)
        {
            camera = Camera.main;

            if (camera == null)
            {
                Debug.LogWarning("Camera.main is null");
                return;
            }
        }

        var cameraRelativeDirection = camera.transform.rotation * moveDirection;
        var cameraProjection = Vector3.ProjectOnPlane(cameraRelativeDirection, Vector3.up).normalized;

        SetPosition(transform.position + scrollSpeed * Time.unscaledDeltaTime * cameraProjection);

        var cameraRot = new Vector3(cameraTiltDegrees, cinemachineVc.transform.eulerAngles.y + rotationSpeed * rotateDirection * Time.unscaledDeltaTime, 0);
        cinemachineVc.transform.eulerAngles = cameraRot;
    }

    public void SetPosition(Vector3 pos)
    {
        transform.position = cameraBounds.ClosestPoint(pos);
    }

    public void SetBounds(GameObject parentWithRenderers)
    {
        Bounds bounds = new();
        var renderers = parentWithRenderers.GetComponentsInChildren<Renderer>();
        foreach (var renderer in renderers)
        {
            bounds.Encapsulate(renderer.bounds);
        }

        var terrains = parentWithRenderers.GetComponentsInChildren<Terrain>();
        foreach (var terrain in terrains)
        {
            var terrainBounds = terrain.terrainData.bounds;
            terrainBounds.center = terrainBounds.center + terrain.transform.position; // Shift to world position
            bounds.Encapsulate(terrainBounds);
        }

        cameraBounds = bounds;
    }

    [ContextMenu("Move Up")]
    private void MoveUp()
    {
        Move(Vector2.up);
    }

    [ContextMenu("Move Down")]
    private void MoveDown()
    {
        Move(Vector2.down);
    }

    [ContextMenu("Move Left")]
    private void MoveLeft()
    {
        Move(Vector2.left);
    }

    [ContextMenu("Move Right")]
    private void MoveRight()
    {
        Move(Vector2.right);
    }

    [ContextMenu("Rotate Clockwise")]
    private void RotateClockwise()
    {
        Rotate(1f);
    }

    [ContextMenu("Rotate Counterclockwise")]
    private void RotateCounterclockwise()
    {
        Rotate(-1f);
    }

    public void Move(Vector2 direction)
    {
        moveDirection = direction.normalized;
    }

    public void Rotate(float direction)
    {
        rotateDirection = Mathf.Clamp(direction, -1, 1);
    }

    public void Zoom(float value)
    {
        if (Camera.main.orthographic)
        {
            cinemachineVc.m_Lens.OrthographicSize = GetZoom(cinemachineVc.m_Lens.OrthographicSize, value);
        } else
        {
            var body = cinemachineVc.GetCinemachineComponent<CinemachineFramingTransposer>();
            if (body != null)
            {
                body.m_CameraDistance = GetZoom(body.m_CameraDistance, value);
            }
        }
    }

    private float GetZoom(float currentValue, float delta)
    {
        return Mathf.Clamp(currentValue - (delta / 100), minZoom, maxZoom);
    }
}
