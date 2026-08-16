using UnityEngine;

public class GameManager : MonoBehaviour
{
public static GameManager Instance { get; private set; }

public GameObject winPanel;
public GameObject losePanel;

public Team playerTeam = Team.Blue;

public bool IsGameOver { get; private set; }

void Awake()
{
if (Instance != null && Instance != this)
{
Destroy(gameObject);
return;
}
Instance = this;
}

public void DeclareWinner(Team winningTeam)
{
if (IsGameOver) return;
IsGameOver = true;

Time.timeScale = 0f;

bool playerWon = winningTeam == playerTeam;
if (winPanel != null) winPanel.SetActive(playerWon);
if (losePanel != null) losePanel.SetActive(!playerWon);
}

public void RestartMatch()
{
Time.timeScale = 1f;
UnityEngine.SceneManagement.SceneManager.LoadScene(
UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
}
}
