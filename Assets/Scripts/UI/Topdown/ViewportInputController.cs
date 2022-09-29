using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ViewportInputController : MonoBehaviour, IPointerClickHandler
{
    public ClickEvent selectEvent = new();
    public ClickEvent actionEvent = new();


    [Serializable]
    public class ClickEvent : UnityEvent<Vector3> { } // Viewport clicked on screen position

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.dragging)
        {
            return;
        }

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            selectEvent.Invoke(eventData.pressPosition);
        } else
        {
            actionEvent.Invoke(eventData.pressPosition);
        }
    }
}