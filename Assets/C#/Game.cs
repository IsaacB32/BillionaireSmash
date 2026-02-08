using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
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
        if (state != GameState.Playing)
        {
            gameCursor.HideCursor();
        }
        else gameCursor.ShowCursor();
    }
    
    public static Game Instance { get; private set; }

    [Header("Game State")]
    public EnemyManager enemyManager;
    public Player player;
    public PowerupManager powerup;
    public DynamicDifficult difficult;
    public AudioManager audioManager;
    public TextReader textReader;
    public CameraShake2D cameraShake;
    public TransitionController transition;

    [SerializeField] private TextMeshProUGUI moneyTextUI;
    [SerializeField] private Cursor gameCursor;

    [Header("Canvas Groups")]
    [SerializeField] private CanvasGroup mainMenuCanvas;
    [SerializeField] private CanvasGroup pauseMenuCanvas;
    [SerializeField] private CanvasGroup gameOverCanvas;
    [SerializeField] private CanvasGroup storyCanvas;
    
    [Header("Game Score Text")]
    [SerializeField] private TextMeshProUGUI highscoreText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI healthText;
    private int score = 0;

    [Header("Misc")]
    public Material damageMaterial;
    
    private float _voiceLineInterval = 20f;
    private float _voiceLineTimer = 0f;
    
    public GameState state { private set; get; }

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);

        SwitchGameState(GameState.Menu);
        Time.timeScale = 1f;
    }

    private void Start()
    {
        highscoreText.text = HighScore.Instance.GetScore().ToString();
    }

    void Update()
    {
        if (state == GameState.Playing)
        {
            _voiceLineTimer += Time.deltaTime;
            if (_voiceLineTimer >= _voiceLineInterval)
            {
                Instance.audioManager.PlayVoiceLineRandom();
                _voiceLineTimer = 0f;
            }

            float playerHealthPercent = Instance.player.currentHealth / Instance.player.maxHealth;
            const float startThreshold = 0.4f;
            float t = Mathf.InverseLerp(startThreshold, 0f, playerHealthPercent);
            float target = Mathf.Lerp(0f, 3f, t);
            damageMaterial.SetFloat("_VignetteIntensity", target);

            healthText.text = $"{Instance.player.currentHealth}";
        }
    }
    
    public void UpdateMoneyUI(int val)
    {
        score = val;
        moneyTextUI.text = $"${val}";
    }

    public void Pause()
    {
        SwitchGameState(GameState.Paused);

        Instance.audioManager.PlayClick();
        
        pauseMenuCanvas.alpha = 100f;
        pauseMenuCanvas.interactable = true;
        pauseMenuCanvas.blocksRaycasts = true;

        UnityEngine.Cursor.visible = true;
        gameCursor.gameObject.SetActive(false);
        
        SwitchGameState(GameState.Paused);
        Freeze();
    }

    public void Unpause()
    {
        SwitchGameState(GameState.Playing);

        Instance.audioManager.PlayClick();
        
        pauseMenuCanvas.alpha = 0f;
        pauseMenuCanvas.interactable = false;
        pauseMenuCanvas.blocksRaycasts = false;

        UnityEngine.Cursor.visible = false;
        gameCursor.gameObject.SetActive(true);
        
        SwitchGameState(GameState.Playing);
        Unfreeze();
    }

    public void Freeze()
    {
        Time.timeScale = 0;
    }

    public void Unfreeze()
    {
        Time.timeScale = 1;
    }
    public void GameOver()
    {
        SwitchGameState(GameState.Lose);

        Game.Instance.cameraShake.PlayExtreme();
            
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
        healthText.text = "0";

        Time.timeScale = 0f;
        
        Instance.audioManager.PlayPlayerDie();
        Instance.StartCoroutine(GameOverVO());
    }

    private IEnumerator GameOverVO()
    {
        yield return new WaitForSecondsRealtime(2f);
        Instance.audioManager.PlayVoiceLineIndex(0);
    }

    public void Restart()
    {
        SwitchGameState(GameState.Menu);
        Instance.audioManager.PlayClick();
        damageMaterial.SetFloat("_VignetteIntensity", 0.0f);
        transition.ResetGame();
    }
    
    public void StartGame()
    {
        if (CutsceneController.Instance.GetCutscene())
        {
            storyCanvas.alpha = 1f;
            storyCanvas.blocksRaycasts = true;
            storyCanvas.interactable = true;
            LeanTween.alphaCanvas(mainMenuCanvas, 0f, 0.5f);
            Instance.audioManager.PlayClick();
            
            mainMenuCanvas.interactable = false;
            mainMenuCanvas.blocksRaycasts = false;

            textReader.BeginReading();
        }
        else
        {
            storyCanvas.blocksRaycasts = false;
            storyCanvas.interactable = false;
            
            SwitchGameState(GameState.Playing);
            
            Instance.audioManager.PlayClick();
            LeanTween.alphaCanvas(mainMenuCanvas, 0f, 0.5f);
            LeanTween.alphaCanvas(storyCanvas, 0f, 0.5f);
            mainMenuCanvas.interactable = false;
            mainMenuCanvas.blocksRaycasts = false;
            UnityEngine.Cursor.visible = false;
            StartCoroutine(Instance.audioManager.PlayMusic());
        }
    }

    public void QuitGame()
    {
        Instance.audioManager.PlayClick();
        Application.Quit();
    }
}
