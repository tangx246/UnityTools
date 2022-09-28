using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragSelector : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Color dragColor = new Color(0f, 1f, 0.15f, 0.1f);
    public DragSelectEvent dragSelectEvent = new();
    public bool enableDragCollideEvents = true;
    public int maxNumDragColliders = 100;
    public LayerMask colliderMask = ~0;
    public float collideDepth = 1000f;
    [Tooltip("The z-position of the ground plane. Important for perspective cameras")] public float groundPlaneWorldZ = 0f;
    public DragCollideEvent dragCollideEvent = new();

    [SerializeField] private Image dragImage;

    [System.Serializable]
    public class DragSelectEvent : UnityEvent<Vector3[]> { } // Corners of the box, defined by RectTransform.GetWorldCorners()
    [Serializable]
    public class DragCollideEvent : UnityEvent<Collider[], int> { } // Colliders inside of the drag box, along with the number of results

    private Collider[] results;

    private void Awake()
    {
        results = new Collider[maxNumDragColliders];

        var dragImageParent = new GameObject();
        dragImageParent.transform.parent = transform;
        dragImage = dragImageParent.AddComponent<Image>();
        dragImage.enabled = false;
        dragImage.color = dragColor;
        dragImage.raycastTarget = false;
        dragImage.maskable = false;
        dragImage.rectTransform.anchorMin = new Vector2(0, 0);
        dragImage.rectTransform.anchorMax = new Vector2(0, 0);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }

        dragImage.enabled = true;
        dragImage.rectTransform.anchoredPosition = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }

        var mousePos = eventData.position;
        var anchorPos = dragImage.rectTransform.anchoredPosition;
        float anchorX;
        float anchorY;
        if (mousePos.x > anchorPos.x)
        {
            anchorX = 0;
        }
        else
        {
            anchorX = 1;
        }
        if (mousePos.y > anchorPos.y)
        {
            anchorY = 0;
        }
        else
        {
            anchorY = 1;
        }
        dragImage.rectTransform.pivot = new Vector2(anchorX, anchorY);

        var diagonal = anchorPos - mousePos;
        dragImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.Abs(diagonal.x));
        dragImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Mathf.Abs(diagonal.y));

        Vector3[] screenCorners = new Vector3[4];
        dragImage.rectTransform.GetWorldCorners(screenCorners);
        var camera = Camera.main;
        for (int i = 0; i < screenCorners.Length; i++)
        {
            // For perspective, we need to consider the camera frustum
            if (!camera.orthographic)
            {
                var plane = new Plane(Vector3.up, groundPlaneWorldZ);
                var ray = camera.ScreenPointToRay(screenCorners[i]);
                float z;
                plane.Raycast(ray, out z);
                screenCorners[i] = camera.ScreenToWorldPoint(new Vector3(screenCorners[i].x, screenCorners[i].y, z));
            }
            else
            {
                screenCorners[i] = camera.ScreenToWorldPoint(screenCorners[i]);
            }
        }

        dragSelectEvent.Invoke(screenCorners);

        if (enableDragCollideEvents)
        {
            DragCollide(screenCorners);
        }
    }

    private void DragCollide(Vector3[] corners)
    {
        if (corners.Length != 4)
        {
            Debug.LogError("Corners must be size 4");
            return;
        }

        Vector3 boxCenter = GetCenter(corners);
        var halfWidth = GetWidth(corners) / 2;
        var halfHeight = GetHeight(corners) / 2;
        var overlaps = Physics.OverlapBoxNonAlloc(boxCenter, new Vector3(halfWidth, halfHeight, collideDepth), results, Camera.main.transform.rotation, colliderMask);

        dragCollideEvent.Invoke(results, overlaps);
    }


    private static float GetHeight(Vector3[] corners)
    {
        return (corners[0] - corners[1]).magnitude;
    }

    private static float GetWidth(Vector3[] corners)
    {
        return (corners[1] - corners[2]).magnitude;
    }

    private static Vector3 GetCenter(Vector3[] corners)
    {
        var boxCenter = Vector3.zero;
        foreach (var corner in corners)
        {
            boxCenter += corner;
        }
        boxCenter /= corners.Length;
        return boxCenter;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }

        dragImage.enabled = false;
    }
}
