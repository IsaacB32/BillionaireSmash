using UnityEngine;
using System;
using UnityEngine.Pool;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
    public LayerMask enemyMask;
    
    [Header("Spawn Settings")]
    public float spawnInterval;
    [SerializeField] private int maxEnemies;
    [SerializeField] private float eliteWeight = 0.1f;
    [SerializeField] private float innerRadius;
    [SerializeField] private float outerRadius;
    
    [Header("Object Pooler")]
    [SerializeField] private Enemy glopPrefab;
    [SerializeField] private Enemy klopPrefab;
    [SerializeField, Range(0f, 1f)] private float glopWeight = 0.7f;
    
    [SerializeField] private int objectPoolDefaultCapacity = 100;
    [SerializeField] private int objectPoolMaxCapacity = 500;

    private float _spawnTimer = 0f;
    private int _activeEnemies = 0;
    private IObjectPool<Enemy> _poolA;
    private IObjectPool<Enemy> _poolB;
    
    IObjectPool<Enemy> CreatePool(Enemy prefab)
    {
        return new ObjectPool<Enemy>(
            () => Instantiate(prefab),
            e => e.gameObject.SetActive(true),
            e => e.gameObject.SetActive(false),
            e => Destroy(e.gameObject),
            true,
            objectPoolDefaultCapacity,
            objectPoolMaxCapacity
        );
    }
    
    void Awake()
    {
        _poolA = CreatePool(glopPrefab);
        _poolB = CreatePool(klopPrefab);
    }
    
    void Update()
    {
        if (Game.Instance.state != Game.GameState.Playing) return;
        
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
        bool useA = Random.value < glopWeight;
        IObjectPool<Enemy> pool = useA ? _poolA : _poolB;
        
        Enemy enemy = pool.Get();

        bool isElite = Random.value < eliteWeight;
        enemy.Initialize(isElite);
        
        Vector2 spawnPos = GetRandomPointInDonut(innerRadius, outerRadius);
        enemy.transform.position = spawnPos;
        
        enemy.SetOwningPool(pool); 
        
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
        if (!enemy.gameObject.activeSelf) return;
        {
            enemy.ReleaseToPool();
            _activeEnemies--;
        }
    } 
    
    #endregion
}
