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
    public DynamicDifficult difficult;

    [SerializeField] private TextMeshProUGUI moneyTextUI;
    
    public GameState state { private set; get; }

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    public void UpdateMoneyUI(int val)
    {
        // moneyTextUI.text = $"${val}";
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
        
    }
}
