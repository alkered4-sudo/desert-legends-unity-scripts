using UnityEngine;

[RequireComponent(typeof(Damageable))]
public class BaseCore : MonoBehaviour
{
private Damageable _damageable;

void Awake()
{
_damageable = GetComponent<Damageable>();
_damageable.OnDied += HandleBaseDestroyed;
}

void OnDestroy()
{
if (_damageable != null)
_damageable.OnDied -= HandleBaseDestroyed;
}

private void HandleBaseDestroyed(Damageable self)
{
Team losingTeam = self.team;
GameManager.Instance?.DeclareWinner(losingTeam == Team.Blue ? Team.Red : Team.Blue);
}
}
