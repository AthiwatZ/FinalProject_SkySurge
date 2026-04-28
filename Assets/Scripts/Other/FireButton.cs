using UnityEngine;
using UnityEngine.EventSystems;

public class FireButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool IsHolding { get; private set; }

    public void OnPointerDown(PointerEventData eventData)
    {
        IsHolding = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        IsHolding = false;
    }
}
