using System.Collections;
using C_.ScriptableObjects;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;

public class Enemy : MonoBehaviour
{
    public bool isDying;
    
    [SerializeField] private EnemyData normalData;
    [SerializeField] private EnemyData eliteData;
    
    [SerializeField] private float maxTtl = 5f;
    [SerializeField] private float distanceThreshold = 0.5f;
    
    public EnemyData currentStats;
    private float _health;
    private float _speed;
    
    private float _currentTtl;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private Transform _playerTransform;
    private Rigidbody2D _rb;

    private ParticleSystem _particleSystem;
    
    IObjectPool<Enemy> _owningPool;
    
    [SerializeField] private GameObject _splat;

    public void SetOwningPool(IObjectPool<Enemy> pool)
    {
        _owningPool = pool;
    }

    public void ReleaseToPool()
    {
        _owningPool?.Release(this);
    }
    
    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _particleSystem = GetComponentInChildren<ParticleSystem>();
    }
    
    void OnEnable()
    {
        isDying = false;
        _spriteRenderer.color = Color.white;
        _currentTtl = maxTtl;
        _playerTransform = Game.Instance.player.transform;
        _animator.Play("Run", 0, 0f);
        _animator.speed = Random.Range(0.8f, 1.2f);
    }
    
    public void Initialize(bool isElite)
    {
        currentStats = isElite ? eliteData : normalData;
        if (isElite) _spriteRenderer.sortingOrder++;

        (_speed, _health) = Game.Instance.difficulty.GetEnemyStats(currentStats.speed, currentStats.health);
        
        _spriteRenderer.color = currentStats.spriteColor;
        transform.localScale = Vector3.one * currentStats.scale;
    }

    void FixedUpdate()
    {
        _currentTtl -= Time.deltaTime;
        if (_currentTtl <= 0 && !isDying)
        {
            isDying = true;
            Game.Instance.StartCoroutine(Die());
        }

        /*transform.position = Vector3.MoveTowards(
            transform.position, 
            _playerTransform.position, 
            _currentStats.speed * Time.deltaTime
        );*/
        
        Vector2 separation = ComputeSeparation();
        Vector2 moveDir = ((Vector2)_playerTransform.position - _rb.position).normalized;
        Vector2 finalVelocity = (moveDir + separation).normalized * _speed;
        _rb.MovePosition(_rb.position + finalVelocity * Time.fixedDeltaTime);

        if (Vector3.Distance(transform.position, Game.Instance.player.transform.position) < distanceThreshold && !isDying)
        {
            isDying = true;
            Game.Instance.StartCoroutine(Die());
        }

        _spriteRenderer.flipX = _playerTransform.position.x < transform.position.x;
    }
    
    Vector2 ComputeSeparation()
    {
        Vector2 separationVec = Vector2.zero;
        Collider2D[] neighbors = Physics2D.OverlapCircleAll(_rb.position, 0.1f); 

        foreach (var neighbor in neighbors)
        {
            if (neighbor.gameObject == gameObject) continue;

            Vector2 diff = _rb.position - (Vector2)neighbor.transform.position;
            separationVec += diff.normalized / diff.magnitude;
        }
        return separationVec;
    }

    public IEnumerator Die()
    {
        Game.Instance.SpawnDrop(currentStats.dropPrefab, transform.position, currentStats.value);
        Game.Instance.enemyManager.CreateSplatter(transform.position);
        
        yield return Game.Instance.StartCoroutine(EnemyHit());

        Game.Instance.difficulty.OnEnemyKilled();
        Game.Instance.enemyManager.ReleaseEnemy(this);
    }

    private IEnumerator EnemyHit()
    {
        float hitDuration = 0.2f;
        Vector2 knockbackDir = (transform.position - _playerTransform.position).normalized;
        float knockbackForce = 2f;
        
        float elapsed = 0f;
        while (elapsed < hitDuration)
        {
            elapsed += Time.deltaTime;
            transform.Translate(knockbackDir * knockbackForce * Time.deltaTime);

            _spriteRenderer.color = (Mathf.FloorToInt(elapsed * 20) % 2 == 0) ? Color.white : Color.red;

            yield return null;
        }

        if (currentStats.isElite) _spriteRenderer.color = Color.red;
        
        Game.Instance.audioManager.PlayEnemyHit();
        Game.Instance.cameraShake.AddLightShake();
    }
    
    public float DecreaseHealth(float value)
    {
        _health -= value;
        Game.Instance.StartCoroutine(EnemyHit());
        return _health;
    }

    public void PlayParticles(Vector3 rot)
    {
        _particleSystem.gameObject.transform.localRotation = Quaternion.Euler(rot.x, rot.y, rot.z + 90);
        _particleSystem.Play();
    }
    
    
}
