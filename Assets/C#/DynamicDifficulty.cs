using C_.ScriptableObjects;
using TMPro;
using UnityEngine;

public class DynamicDifficulty : MonoBehaviour
{
    [Header("Leveling")]
    [SerializeField] private int baseKillsPerLevel = 20;
    [SerializeField] private float earlyGrowth = 1.25f;   // for very fast early levels
    [SerializeField] private float lateGrowth = 0.15f;    // increase for strong late game taper
    [SerializeField] private TextMeshProUGUI levelText;

    [Header("Enemy Spawning")]
    [SerializeField] private float startingSpawnInterval = 0.5f;
    [SerializeField] private float minSpawnInterval = 0.03f;
    [SerializeField] private float spawnCurveStrength = 0.42f;
    [Tooltip("the number of levels before increasing enemy stats")] [SerializeField] private int _enemyStatBoostRate = 3;

    private int level = 1;
    private int killsThisLevel;
    private int killsRequired;

    private int totalKills;

    public (float, float) GetEnemyStats(float speed, float health)
    {
        if (level % _enemyStatBoostRate == 0)
        {
            health += (level > 15) ? (level > 28) ? 3 : 2 : 1;
            speed += 0.8f;
        }
        return (speed, health);
    } 

    private void Start()
    {
        RecalculateLevelData();
        ApplySpawnRate();
        UpdateLevelText();
    }

    public void OnEnemyKilled()
    {
        killsThisLevel++;
        totalKills++;

        Game.Instance.XpBar.UpdateXPBar(killsThisLevel, killsRequired);
        
        if (killsThisLevel >= killsRequired)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        level++;
        killsThisLevel = 0;

        RecalculateLevelData();
        ApplySpawnRate();
        UpdateLevelText();
        
        StartCoroutine(Game.Instance.powerup.ShowPowerupChoices());
    }

    private void RecalculateLevelData()
    {
        // early levels are exponential while later levels are logarithmic with taper
        float early = Mathf.Pow(level, earlyGrowth);
        float late = Mathf.Log(level + 1) * lateGrowth;

        killsRequired = Mathf.RoundToInt(baseKillsPerLevel * (early + late));
    }

    private void ApplySpawnRate()
    {
        // this will decrease smoothly but never reach 0
        float curve = Mathf.Log(level + 1) * spawnCurveStrength;
        float interval = startingSpawnInterval / (1f + curve);

        Game.Instance.enemyManager.spawnInterval =
            Mathf.Max(interval, minSpawnInterval);
    }

    private void UpdateLevelText()
    {
        levelText.text = $"Level {level}";
    }
    
    public void RaiseMaxLevel()
    {
        level = 999;
        killsThisLevel = 0;
        
        RecalculateLevelData();
        ApplySpawnRate();
        UpdateLevelText();
    }

    #if UNITY_EDITOR
    private void OnValidate()
    {
        baseKillsPerLevel = Mathf.Max(1, baseKillsPerLevel);
        earlyGrowth = Mathf.Max(1f, earlyGrowth);
        lateGrowth = Mathf.Max(0f, lateGrowth);
    }
    #endif
}
