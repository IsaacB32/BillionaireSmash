using System;
using UnityEngine;

public class DynamicDifficult : MonoBehaviour
{
    [Header("Stats")] 
    public int startingKilledForPowerup = 20;
    public float startingEnemySpawnRate = 0.5f;
    
    private int _killedLevel;
    private float _enemySpawnRate;
    
    private int _enemiesKilled;
    private int _killedForPowerup;
    private int _totalKilled;

    [Header("Function")]
    [SerializeField] private int _multiplier = 2;
    private int _level = 1;

    private void Start()
    {
        _killedLevel = startingKilledForPowerup;
        _enemySpawnRate = startingEnemySpawnRate;
        SetLevel();
    }

    public void IncreaseLevel()
    {
        _killedLevel = Mathf.FloorToInt(Mathf.Pow(1.75f, _level) + 19);
        SetLevel();
    }

    public void SetLevel()
    {
        _killedForPowerup = _killedLevel; 
        Game.Instance.enemyManager.spawnInterval = _enemySpawnRate;
    }
    
    public void IncreaseEnemyKilled()
    {
        _enemiesKilled++;
        _totalKilled++;
        if (_enemiesKilled >= _killedForPowerup)
        {
            Game.Instance.powerup.ShowPowerupChoices();
            _enemiesKilled = 0;
        }
        IncreaseLevel();
    }
}
