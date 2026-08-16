using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
public RectTransform joystickBase;
public RectTransform joystickHandle;

public float handleRange = 60f;

public Vector2 InputDirection { get; private set; } = Vector2.zero;

private Vector2 _basePosition;

void Start()
{
if (joystickBase != null)
_basePosition = joystickBase.position;
}

public void OnPointerDown(PointerEventData eventData)
{
OnDrag(eventData);
}

public void OnDrag(PointerEventData eventData)
{
Vector2 direction = eventData.position - _basePosition;
InputDirection = (direction.magnitude > handleRange) ? direction.normalized : direction / handleRange;

if (joystickHandle != null)
joystickHandle.anchoredPosition = InputDirection * handleRange;
}

public void OnPointerUp(PointerEventData eventData)
{
InputDirection = Vector2.zero;
if (joystickHandle != null)
joystickHandle.anchoredPosition = Vector2.zero;
}
}
