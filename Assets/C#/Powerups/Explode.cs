using System;
using System.Collections;
using UnityEngine;

public class Explode : MonoBehaviour
{
    [SerializeField] private float _lifetime = 1f;
    private ParticleSystem _particles;

    private void Awake()
    {
        _particles.transform.localScale = .42f * transform.localScale;
    }

    public void ExplodeBomb(float radius)
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, radius, Game.Instance.enemyManager.enemyMask);
        foreach (Collider2D enemy in enemies)
        {
            if (!enemy.gameObject.CompareTag("Enemy")) continue;
            Enemy e = enemy.GetComponent<Enemy>();
            if (!e.isDying && e.DecreaseHealth(2) <= 0) StartCoroutine(e.Die());
        }
        Invoke(nameof(WaitDeath), _lifetime);
    }

    private void WaitDeath()
    {
        Destroy(gameObject);
    }
}
