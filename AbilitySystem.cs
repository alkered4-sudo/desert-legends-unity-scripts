using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Damageable))]
public class AbilitySystem : MonoBehaviour
{
public HeroStats stats;
public Transform weaponSocket;

public LayerMask enemyLayerMask;
public float targetSearchInterval = 0.15f;

private float _attackTimer;
private float _specialTimer;
private float _searchTimer;
private Damageable _currentTarget;
private Damageable _selfDamageable;

public System.Action OnBasicAttackFired;
public System.Action OnSpecialCast;

public float SpecialCooldownRemaining => Mathf.Max(0f, _specialTimer);
public float SpecialCooldownPercent => stats != null && stats.specialCooldown > 0 ? Mathf.Clamp01(_specialTimer / stats.specialCooldown) : 0f;

void Awake()
{
_selfDamageable = GetComponent<Damageable>();
}

void Update()
{
if (_attackTimer > 0f) _attackTimer -= Time.deltaTime;
if (_specialTimer > 0f) _specialTimer -= Time.deltaTime;

_searchTimer -= Time.deltaTime;
if (_searchTimer <= 0f)
{
_searchTimer = targetSearchInterval;
_currentTarget = FindNearestEnemy(stats.attackRange * 3f);
}
}

public void TryBasicAttack()
{
if (_attackTimer > 0f) return;
var target = FindNearestEnemy(stats.attackRange);
if (target == null) return;

target.TakeDamage(stats.attackDamage, _selfDamageable.team);
OnBasicAttackFired?.Invoke();
_attackTimer = stats.attackCooldown;
}

public void TrySpecialAbility()
{
if (_specialTimer > 0f) return;

switch (stats.specialType)
{
case AbilityType.GroundSlam: CastGroundSlam(); break;
case AbilityType.ShadowDash: CastShadowDash(); break;
case AbilityType.AreaBlast: CastAreaBlast(); break;
case AbilityType.MultiShot: CastMultiShot(); break;
}

OnSpecialCast?.Invoke();
_specialTimer = stats.specialCooldown;
}

private void CastGroundSlam()
{
var enemies = FindEnemiesInRadius(transform.position, stats.specialRadius);
foreach (var e in enemies)
e.TakeDamage(stats.specialDamage, _selfDamageable.team);
}

private void CastShadowDash()
{
var target = FindNearestEnemy(stats.specialRadius);
if (target == null) return;

Vector3 dir = (target.transform.position - transform.position);
dir.y = 0f;
float dashDistance = Mathf.Min(dir.magnitude - 1.5f, stats.specialRadius * 0.6f);
if (dashDistance > 0f)
transform.position += dir.normalized * dashDistance;

target.TakeDamage(stats.specialDamage, _selfDamageable.team);
StartCoroutine(TemporarySpeedBoost(1.8f, 1.5f));
}

private void CastAreaBlast()
{
var target = FindNearestEnemy(stats.specialRadius * 2f);
Vector3 center = target != null ? target.transform.position : transform.position;

var enemies = FindEnemiesInRadius(center, stats.specialRadius);
foreach (var e in enemies)
e.TakeDamage(stats.specialDamage, _selfDamageable.team);
}

private void CastMultiShot()
{
var enemies = FindEnemiesInRadius(transform.position, stats.specialRadius).OrderBy(e => Vector3.Distance(transform.position, e.transform.position)).Take(3);

foreach (var e in enemies)
e.TakeDamage(stats.specialDamage, _selfDamageable.team);
}

private System.Collections.IEnumerator TemporarySpeedBoost(float multiplier, float duration)
{
stats.speedMultiplier = multiplier;
yield return new WaitForSeconds(duration);
stats.speedMultiplier = 1f;
}

private Damageable FindNearestEnemy(float range)
{
return FindEnemiesInRadius(transform.position, range).OrderBy(e => Vector3.Distance(transform.position, e.transform.position)).FirstOrDefault();
}

private List<Damageable> FindEnemiesInRadius(Vector3 center, float radius)
{
var result = new List<Damageable>();
var hits = Physics.OverlapSphere(center, radius, enemyLayerMask);
foreach (var hit in hits)
{
var dmg = hit.GetComponent<Damageable>();
if (dmg != null && dmg.IsAlive && dmg.team != _selfDamageable.team)
result.Add(dmg);
}
return result;
}
}
