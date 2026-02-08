using System;
using UnityEngine;

public class DynamicDifficult : MonoBehaviour
{
    [Header("Stats")] 
    public int startingKilledForPowerup = 40;
    public float startingEnemySpawnRate = 0.5f;
    
    private int _enemiesKilled;
    private int _totalKilled;
    
    private int _level = 1;

    private int _killedForPowerup;
    
    
    private void Start()
    {
        SetLevel(startingKilledForPowerup, startingEnemySpawnRate);
    }

    public void IncreaseLevel()
    {
        _level++;
        int killedLevel = Mathf.FloorToInt(Mathf.Pow(2.65f, _level) + 19);
        float spawnRate = (_level != 1) ? 1.0f / (1 + _level) : startingEnemySpawnRate;
        SetLevel(killedLevel, spawnRate);
    }

    public void SetLevel(int killedLevel, float spawnRate)
    {
        _killedForPowerup = killedLevel; 
        Game.Instance.enemyManager.spawnInterval = spawnRate;
    }
    
    public void IncreaseEnemyKilled()
    {
        _enemiesKilled++;
        _totalKilled++;
        if (_enemiesKilled >= _killedForPowerup)
        {
            StartCoroutine(Game.Instance.powerup.ShowPowerupChoices());
            _enemiesKilled = 0;
            IncreaseLevel();
        }
    }
}
