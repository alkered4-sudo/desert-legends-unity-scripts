using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class HeroMovement : MonoBehaviour
{
public VirtualJoystick joystick;
public HeroStats stats;
public Animator animator;

public float rotationSpeed = 10f;

private CharacterController _controller;
private Vector3 _velocity;
public bool IsMoving { get; private set; }

void Awake()
{
_controller = GetComponent<CharacterController>();
}

void Update()
{
if (joystick == null || stats == null) return;

Vector2 input = joystick.InputDirection;
IsMoving = input.magnitude > 0.05f;

Vector3 moveDir = new Vector3(input.x, 0f, input.y);

float currentSpeed = stats.CurrentMoveSpeed;
_controller.SimpleMove(moveDir * currentSpeed);

if (IsMoving)
{
Quaternion targetRot = Quaternion.LookRotation(moveDir, Vector3.up);
transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
}

if (animator != null)
animator.SetBool("IsWalking", IsMoving);
}
}
