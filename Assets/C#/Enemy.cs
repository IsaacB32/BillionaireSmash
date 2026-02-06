using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float maxTtl = 5f;

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
    }

    void Die()
    {
        Game.Instance.enemyManager.Release(gameObject);
    }
}
