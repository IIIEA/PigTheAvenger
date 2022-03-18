using UnityEngine;
using UnityEngine.EventSystems;

public class TapHandlerJoystick : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] private bool _pressed;

    public bool Pressed => _pressed;

    public void OnPointerDown(PointerEventData eventData)
    {
        _pressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _pressed = false;
    }
}