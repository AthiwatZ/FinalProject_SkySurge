using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public RectTransform handle;
    public float radius = 80f;

    public Vector2 Direction { get; private set; }

    RectTransform baseRect;

    void Awake()
    {
        baseRect = GetComponent<RectTransform>();
        Direction = Vector2.zero;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            baseRect,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPoint
        );

        Vector2 clamped = Vector2.ClampMagnitude(localPoint, radius);
        handle.anchoredPosition = clamped;

        Direction = clamped / radius;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Direction = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
    }
}
