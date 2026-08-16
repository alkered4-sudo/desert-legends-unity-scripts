using System;
using UnityEngine;

public class Damageable : MonoBehaviour
{
public Team team = Team.Blue;

public float maxHp = 100f;
public float CurrentHp { get; private set; }
public bool IsAlive { get; private set; } = true;

public bool canRespawn = false;
public float respawnDelay = 6f;
public Transform respawnPoint;

public event Action<Damageable, float> OnDamaged;
public event Action<Damageable> OnDied;
public event Action<Damageable> OnRespawned;

void Awake()
{
CurrentHp = maxHp;
}

public void TakeDamage(float amount, Team attackerTeam)
{
if (!IsAlive || attackerTeam == team) return;

CurrentHp = Mathf.Max(0f, CurrentHp - amount);
OnDamaged?.Invoke(this, amount);

if (CurrentHp <= 0f)
Die();
}

private void Die()
{
IsAlive = false;
OnDied?.Invoke(this);

if (canRespawn)
Invoke(nameof(Respawn), respawnDelay);
else
gameObject.SetActive(false);
}

private void Respawn()
{
CurrentHp = maxHp;
IsAlive = true;
if (respawnPoint != null)
{
transform.position = respawnPoint.position;
transform.rotation = respawnPoint.rotation;
}
gameObject.SetActive(true);
OnRespawned?.Invoke(this);
}
}

public enum Team { Blue, Red, Neutral }
