using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance { get; private set; }

    [SerializeField] private float spawnInterval;
    [SerializeField] private int maxEnemies;
    [SerializeField] private float eliteWeight = 0.1f;

    [SerializeField] private float innerRadius;
    [SerializeField] private float outerRadius;

    private float _spawnTimer = 0f;
    private int _activeEnemies = 0;
    
    void Awake() {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }
    
    void Update()
    {
        _spawnTimer += Time.deltaTime;

        if (CanSpawn())
        {
            Spawn();
        }
    }

    bool CanSpawn()
    {
        return _spawnTimer >= spawnInterval && _activeEnemies < maxEnemies;
    }

    void Spawn()
    {
        Vector2 spawnPos = GetRandomPointInDonut(innerRadius, outerRadius);
    }

    Vector2 GetRandomPointInDonut(float i, float o)
    {
        float theta = Random.Range(0f, 2f * (float)Math.PI);
        float dist = (float)Math.Sqrt(Random.Range(i, o));
        return Game.Instance.player.transform.position +
               new Vector3((float)(Math.Cos(theta) * dist), (float)(Math.Sin(theta) * dist), 0f);
    }
}
