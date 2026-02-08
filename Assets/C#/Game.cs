using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    [SerializeField] private Cursor gameCursor;
    
    [Header("Powerup")]
    [SerializeField] private int _killedForPowerup = 20;

    [Header("Canvas Groups")]
    [SerializeField] private CanvasGroup mainMenuCanvas;
    [SerializeField] private CanvasGroup pauseMenuCanvas;
    [SerializeField] private CanvasGroup gameOverCanvas;

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
        Time.timeScale = 1f;
    }

    void Update()
    {

    }
    
    public void UpdateMoneyUI(int val)
    {
        moneyTextUI.text = $"${val}";
    }

    public void Pause()
    {
        SwitchGameState(GameState.Paused);

        pauseMenuCanvas.alpha = 100f;
        pauseMenuCanvas.interactable = true;
        pauseMenuCanvas.blocksRaycasts = true;

        UnityEngine.Cursor.visible = true;
        gameCursor.gameObject.SetActive(false);
        
        Time.timeScale = 0;
    }

    public void Unpause()
    {
        SwitchGameState(GameState.Playing);

        pauseMenuCanvas.alpha = 0f;
        pauseMenuCanvas.interactable = false;
        pauseMenuCanvas.blocksRaycasts = false;

        UnityEngine.Cursor.visible = false;
        gameCursor.gameObject.SetActive(true);
        
        Time.timeScale = 1;
    }

    public void GameOver()
    {
        state = GameState.Lose;

        LeanTween.alphaCanvas(gameOverCanvas, 100f, 0.5f);
        gameOverCanvas.interactable = true;
        gameOverCanvas.blocksRaycasts = true;
        
        UnityEngine.Cursor.visible = true;
        gameCursor.gameObject.SetActive(false);

        Time.timeScale = 0f;
    }

    public void Restart()
    {
        state = GameState.Menu;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void StartGame()
    {
        state = GameState.Playing;

        LeanTween.alphaCanvas(mainMenuCanvas, 0f, 0.5f);
        UnityEngine.Cursor.visible = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
