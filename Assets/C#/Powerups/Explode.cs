using System;
using System.Collections;
using UnityEngine;

public class Explode : MonoBehaviour
{
    [SerializeField] private float _lifetime = 1f;
    public void ExplodeBomb(float radius)
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, radius, Game.Instance.enemyManager.enemyMask);
        foreach (Collider2D enemy in enemies)
        {
            Enemy e = enemy.GetComponent<Enemy>();
            StartCoroutine(e.Die());
        }
        Invoke(nameof(WaitDeath), _lifetime);
    }

    private void WaitDeath()
    {
        Destroy(gameObject);
    }
}
