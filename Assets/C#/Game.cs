using UnityEngine;

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

    public EnemySpawner enemySpawner;
    public Player player;
    
    private GameState _state;
    
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }
}
