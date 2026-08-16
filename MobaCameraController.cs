using UnityEngine;

public class MobaCameraController : MonoBehaviour
{
public Transform target;

public Vector3 offset = new Vector3(0f, 14f, -10f);
public float smoothTime = 0.18f;

public float minZoomOffset = 8f;
public float maxZoomOffset = 20f;
private float _currentZoom;

private Vector3 _velocity = Vector3.zero;

void Start()
{
_currentZoom = offset.magnitude;
transform.LookAt(target != null ? target.position : Vector3.zero);
}

void LateUpdate()
{
if (target == null) return;

HandlePinchZoom();

Vector3 desiredPosition = target.position + offset.normalized * _currentZoom;
transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref _velocity, smoothTime);
transform.LookAt(target.position + Vector3.up * 1.2f);
}

private void HandlePinchZoom()
{
if (Input.touchCount == 2)
{
Touch t0 = Input.GetTouch(0);
Touch t1 = Input.GetTouch(1);

Vector2 t0Prev = t0.position - t0.deltaPosition;
Vector2 t1Prev = t1.position - t1.deltaPosition;

float prevDist = (t0Prev - t1Prev).magnitude;
float curDist = (t0.position - t1.position).magnitude;
float delta = (prevDist - curDist) * 0.02f;

_currentZoom = Mathf.Clamp(_currentZoom + delta, minZoomOffset, maxZoomOffset);
}
}
}
