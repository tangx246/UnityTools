using Cinemachine;
using UnityEngine;
using UnityTools;

public class TopdownCameraController : MonoBehaviour
{
    public float scrollSpeed = 1f;
    public float minZoom = 2.5f;
    public float maxZoom = 7f;
    public Bounds cameraBounds;
    public GameObjectEventEmitter environmentFocusEvent;
    public EventEmitter<Vector3> cameraCenterEvent;

    [SerializeField] private Vector2 moveDirection;
    private new Camera camera;
    private CinemachineVirtualCamera cinemachineVc;

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
        if (!Application.isFocused)
        {
            Move(Vector2.zero);
            return;
        }

        if (camera == null)
        {
            camera = Camera.main;
        }

        var cameraRelativeDirection = camera.transform.rotation * moveDirection;
        var cameraProjection = Vector3.ProjectOnPlane(cameraRelativeDirection, Vector3.up).normalized;

        SetPosition(transform.position + scrollSpeed * Time.unscaledDeltaTime * cameraProjection);
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
            bounds.Encapsulate(terrain.terrainData.bounds);
        }

        cameraBounds = bounds;
    }

    public void Move(Vector2 direction)
    {
        moveDirection = direction.normalized;
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
