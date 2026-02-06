using C_.ScriptableObjects;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyData normalData;
    [SerializeField] private EnemyData eliteData;
    
    [SerializeField] private float maxTtl = 5f;
    [SerializeField] private float distanceThreshold = 0.5f;
    
    private EnemyData _currentStats;
    private float _health;

    private float _currentTtl;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private Transform _playerTransform;
    private Rigidbody2D _rb;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
    }
    
    void OnEnable()
    {
        _currentTtl = maxTtl;
        _playerTransform = Game.Instance.player.transform;
        _animator.Play("Run", 0, 0f);
        _animator.speed = Random.Range(0.8f, 1.2f);
    }
    
    public void Initialize(bool isElite)
    {
        _currentStats = isElite ? eliteData : normalData;
        if (isElite) _spriteRenderer.sortingOrder++;
        
        _health = _currentStats.health;
        _spriteRenderer.color = _currentStats.spriteColor;
        transform.localScale = Vector3.one * _currentStats.scale;
    }

    void FixedUpdate()
    {
        _currentTtl -= Time.deltaTime;
        if (_currentTtl <= 0) Die();

        /*transform.position = Vector3.MoveTowards(
            transform.position, 
            _playerTransform.position, 
            _currentStats.speed * Time.deltaTime
        );*/
        
        Vector2 separation = ComputeSeparation();
        Vector2 moveDir = ((Vector2)_playerTransform.position - _rb.position).normalized;
        Vector2 finalVelocity = (moveDir + separation).normalized * _currentStats.speed;
        _rb.MovePosition(_rb.position + finalVelocity * Time.fixedDeltaTime);
        
        if (Vector3.Distance(transform.position, Game.Instance.player.transform.position) < distanceThreshold) Die();

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

    void Die()
    {
        Game.Instance.enemyManager.Release(this);
    }
}
