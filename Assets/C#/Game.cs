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
    
    public static Game Instance { get; private set; }

    [Header("Game State")]
    public EnemyManager enemyManager;
    public Player player;

    [SerializeField] private TextMeshProUGUI moneyTextUI;
    
    private GameState _state;
    
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    public void UpdateMoneyUI(int val)
    {
        moneyTextUI.text = $"${val}";
    }
}
