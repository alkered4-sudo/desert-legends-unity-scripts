using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Damageable))]
public class TurretController : MonoBehaviour
{
public float attackRange = 9f;
public float attackDamage = 22f;
public float attackCooldown = 0.9f;

public LayerMask enemyLayerMask;

private float _attackTimer;
private Damageable _selfDamageable;

public System.Action<Damageable> OnFired;

void Awake()
{
_selfDamageable = GetComponent<Damageable>();
}

void Update()
{
if (!_selfDamageable.IsAlive) return;

if (_attackTimer > 0f) _attackTimer -= Time.deltaTime;
if (_attackTimer > 0f) return;

var target = FindPriorityTarget();
if (target == null) return;

target.TakeDamage(attackDamage, _selfDamageable.team);
OnFired?.Invoke(target);
_attackTimer = attackCooldown;
}

private Damageable FindPriorityTarget()
{
var hits = Physics.OverlapSphere(transform.position, attackRange, enemyLayerMask);
var enemies = hits
.Select(h => h.GetComponent<Damageable>())
.Where(d => d != null && d.IsAlive && d.team != _selfDamageable.team)
.ToList();

if (enemies.Count == 0) return null;

var minion = enemies
.Where(d => d.GetComponent<MinionController>() != null)
.OrderBy(d => Vector3.Distance(transform.position, d.transform.position))
.FirstOrDefault();

return minion ?? enemies
.OrderBy(d => Vector3.Distance(transform.position, d.transform.position))
.First();
}

void OnDrawGizmosSelected()
{
Gizmos.color = Color.red;
Gizmos.DrawWireSphere(transform.position, attackRange);
}
}
