using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Damageable))]
public class MinionController : MonoBehaviour
{
public Transform[] waypoints;
public float moveSpeed = 3f;
private int _waypointIndex;

public float engageRange = 4f;
public float attackRange = 1.8f;
public float attackDamage = 6f;
public float attackCooldown = 1f;
public LayerMask enemyLayerMask;

private float _attackTimer;
private Damageable _selfDamageable;
private Damageable _currentTarget;

void Awake()
{
_selfDamageable = GetComponent<Damageable>();
}

void Update()
{
if (!_selfDamageable.IsAlive) return;

if (_attackTimer > 0f) _attackTimer -= Time.deltaTime;

_currentTarget = FindNearestEnemy();

if (_currentTarget != null)
{
float dist = Vector3.Distance(transform.position, _currentTarget.transform.position);
if (dist <= attackRange)
{
if (_attackTimer <= 0f)
{
_currentTarget.TakeDamage(attackDamage, _selfDamageable.team);
_attackTimer = attackCooldown;
}
return;
}
else
{
MoveTowards(_currentTarget.transform.position);
return;
}
}

FollowWaypoints();
}

private void FollowWaypoints()
{
if (waypoints == null || waypoints.Length == 0) return;
if (_waypointIndex >= waypoints.Length) return;

Transform wp = waypoints[_waypointIndex];
MoveTowards(wp.position);

if (Vector3.Distance(transform.position, wp.position) < 0.3f)
_waypointIndex++;
}

private void MoveTowards(Vector3 point)
{
Vector3 dir = (point - transform.position);
dir.y = 0f;
if (dir.magnitude < 0.05f) return;

transform.position += dir.normalized * moveSpeed * Time.deltaTime;
transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 8f * Time.deltaTime);
}

private Damageable FindNearestEnemy()
{
var hits = Physics.OverlapSphere(transform.position, engageRange, enemyLayerMask);
return hits
.Select(h => h.GetComponent<Damageable>())
.Where(d => d != null && d.IsAlive && d.team != _selfDamageable.team)
.OrderBy(d => Vector3.Distance(transform.position, d.transform.position))
.FirstOrDefault();
}
}
