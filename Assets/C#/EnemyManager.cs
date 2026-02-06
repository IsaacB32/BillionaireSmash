using UnityEngine;
using System;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private float spawnInterval;
    [SerializeField] private int maxEnemies;
    [SerializeField] private float eliteWeight = 0.1f;
    [SerializeField] private float innerRadius;
    [SerializeField] private float outerRadius;

    [Header("Object Pooler")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int objectPoolDefaultCapacity = 100;
    [SerializeField] private int objectPoolMaxCapacity = 500;

    private float _spawnTimer = 0f;
    private int _activeEnemies = 0;
    private IObjectPool<Enemy> _pool;
    
    void Awake() {
        _pool = new ObjectPool<Enemy>(
            () => Instantiate(enemyPrefab).GetComponent<Enemy>(),
            e => e.gameObject.SetActive(true),
            e => e.gameObject.SetActive(false),
            e => Destroy(e.gameObject),
            true,
            objectPoolDefaultCapacity,
            objectPoolMaxCapacity
        );
    }
    
    void Update()
    {
        _spawnTimer += Time.deltaTime;

        if (CanSpawn())
        {
            _spawnTimer = 0f;
            Spawn();
        }
    }

    #region SpawnMethods
    bool CanSpawn()
    {
        return _spawnTimer >= spawnInterval && _activeEnemies < maxEnemies;
    }

    void Spawn()
    {
        Enemy enemy = _pool.Get();

        bool isElite = Random.value < eliteWeight;
        enemy.Initialize(isElite);
        
        Vector2 spawnPos = GetRandomPointInDonut(innerRadius, outerRadius);
        enemy.transform.position = spawnPos;
        
        _activeEnemies++;
    }

    Vector2 GetRandomPointInDonut(float i, float o)
    {
        float theta = Random.Range(0f, 2f * (float)Math.PI);
        float dist = (float)Math.Sqrt(Random.Range(i * i, o * o));
        return Game.Instance.player.transform.position +
               new Vector3((float)(Math.Cos(theta) * dist), (float)(Math.Sin(theta) * dist), 0f);
    }
    #endregion
    
    #region ObjectPoolerMethods
    
    public void Release(Enemy enemy)
    {
        if (enemy.gameObject.activeSelf && _pool != null)
        {
            _pool.Release(enemy);
            _activeEnemies--;
        }
    } 
    
    #endregion
}
