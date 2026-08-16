using UnityEngine;

public class MinionSpawner : MonoBehaviour
{
[System.Serializable]
public class LaneSpawnInfo
{
public string laneName;
public Transform blueSpawnPoint;
public Transform redSpawnPoint;
public Transform[] blueWaypoints;
public Transform[] redWaypoints;
}

public LaneSpawnInfo[] lanes;

public GameObject blueMinionPrefab;
public GameObject redMinionPrefab;

public float waveInterval = 8f;
private float _timer;

void Update()
{
_timer += Time.deltaTime;
if (_timer >= waveInterval)
{
_timer = 0f;
SpawnWave();
}
}

private void SpawnWave()
{
foreach (var lane in lanes)
{
SpawnMinion(blueMinionPrefab, lane.blueSpawnPoint, lane.blueWaypoints, Team.Blue);
SpawnMinion(redMinionPrefab, lane.redSpawnPoint, lane.redWaypoints, Team.Red);
}
}

private void SpawnMinion(GameObject prefab, Transform spawnPoint, Transform[] waypoints, Team team)
{
if (prefab == null || spawnPoint == null) return;

GameObject obj = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);

var dmg = obj.GetComponent<Damageable>();
if (dmg != null) dmg.team = team;

var minion = obj.GetComponent<MinionController>();
if (minion != null) minion.waypoints = waypoints;
}
}
