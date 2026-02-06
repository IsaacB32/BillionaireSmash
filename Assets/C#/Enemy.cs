using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float maxTtl = 5f;
    [SerializeField] private float speed = 0.1f;
    [SerializeField] private float distanceThreshold = 0.5f;

    private float _currentTtl;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        _currentTtl = maxTtl;
    }

    // Update is called once per frame
    void Update()
    {
        _currentTtl -= Time.deltaTime;
        if (_currentTtl <= 0) Die();

        transform.position = Vector3.MoveTowards(
            transform.position, Game.Instance.player.transform.position, speed);
        
        if (Vector3.Distance(transform.position, Game.Instance.player.transform.position) < distanceThreshold) Die();
    }

    void Die()
    {
        Game.Instance.enemyManager.Release(gameObject);
    }
}
