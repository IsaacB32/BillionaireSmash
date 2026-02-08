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
    public DynamicDifficult difficult;

    [SerializeField] private TextMeshProUGUI moneyTextUI;
    [SerializeField] private Cursor gameCursor;
    
    [Header("Powerup")]
    [SerializeField] private int _killedForPowerup = 20;

    [Header("Canvas Groups")]
    [SerializeField] private CanvasGroup mainMenuCanvas;
    [SerializeField] private CanvasGroup pauseMenuCanvas;
    [SerializeField] private CanvasGroup gameOverCanvas;
    
    [Header("Game Score Text")]
    [SerializeField] private TextMeshProUGUI highscoreText;
    [SerializeField] private TextMeshProUGUI scoreText;
    private int score = 0;

    public GameState state { private set; get; }

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);

        state = GameState.Menu;
        Time.timeScale = 1f;
        
        highscoreText.text = HighScore.Instance.GetScore().ToString();
    }
    
    public void UpdateMoneyUI(int val)
    {
        score = val;
        moneyTextUI.text = $"${val}";
    }

    public void Pause()
    {
        pauseMenuCanvas.alpha = 100f;
        pauseMenuCanvas.interactable = true;
        pauseMenuCanvas.blocksRaycasts = true;

        UnityEngine.Cursor.visible = true;
        gameCursor.gameObject.SetActive(false);
        
        Freeze();
    }

    public void Unpause()
    {
        pauseMenuCanvas.alpha = 0f;
        pauseMenuCanvas.interactable = false;
        pauseMenuCanvas.blocksRaycasts = false;

        UnityEngine.Cursor.visible = false;
        gameCursor.gameObject.SetActive(true);
        
        Unfreeze();
    }

    public void Freeze()
    {
        SwitchGameState(GameState.Paused);
        Time.timeScale = 0;
    }

    public void Unfreeze()
    {
        SwitchGameState(GameState.Playing);
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

        if (score > HighScore.Instance.GetScore())
        {
            highscoreText.text = score.ToString();
            HighScore.Instance.NewHighScore(score);
        }
        scoreText.text = score.ToString();

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
        mainMenuCanvas.interactable = false;
        mainMenuCanvas.blocksRaycasts = false;
        UnityEngine.Cursor.visible = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
