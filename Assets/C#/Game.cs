using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class Game : MonoBehaviour
{
    public enum GameState
    {
        Menu,
        Paused,
        Playing,
        Win,
        Lose
    }

    public void SwitchGameState(GameState s)
    {
        state = s;
    }
    
    public static Game Instance { get; private set; }

    [Header("Game State")]
    public EnemyManager enemyManager;
    public Player player;
    public PowerupManager powerup;

    [SerializeField] private TextMeshProUGUI moneyTextUI;
    
    [Header("Powerup")]
    [SerializeField] private int _killedForPowerup = 20;

    [Header("Canvas Groups")]
    [SerializeField] private CanvasGroup mainMenuCanvas;

    public GameState state { private set; get; }
    public int enemiesKilled { private set; get; }
    private int _totalKilled = 0;

    public void IncreaseEnemyKilled()
    {
        enemiesKilled++;
        _totalKilled++;
        if (enemiesKilled > _killedForPowerup)
        {
            powerup.ShowPowerupChoices();
            enemiesKilled = 0;
        }
    }

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);

        state = GameState.Menu;
    }

    public void UpdateMoneyUI(int val)
    {
        moneyTextUI.text = $"${val}";
    }

    public void Pause()
    {
        Time.timeScale = 0;
        SwitchGameState(GameState.Paused);
    }

    public void Unpause()
    {
        Time.timeScale = 1;
        SwitchGameState(GameState.Playing);
    }

    public void GameOver()
    {
        state = GameState.Lose;
    }

    public void StartGame()
    {
        state = GameState.Playing;

        
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
