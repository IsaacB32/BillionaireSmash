using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 1.5f;
    [SerializeField] private float maxLifeTime = 1f;
    
    [Header("Bullet Tweeks")]
    [SerializeField] private float _chainRadius = 200f;
    
    //powerup 
    private int _pierceCounter;
    private int _chain;
    private float _explode;

    private float _timer;
    private Gun _gun;

    private float _defaultSpeed;
    private float _defaultSize;
    private int _defaultPierce;
    
    #region Pool
    public void Init(Gun gub)
    {
        _gun = gub;
        _timer = maxLifeTime;

        speed = _gun.GetStats().speed;
        transform.localScale = _gun.GetStats().size * Vector3.one;
        _pierceCounter = _gun.GetStats().pierce;
        _chain = _gun.GetStats().chain;
        _explode = _gun.GetStats().explode;
    }

    private void Release()
    {
        if (gameObject.activeSelf && _gun != null)
        {
            transform.localScale = Vector3.one * _defaultSize;
            speed = _defaultSpeed;
            _gun.Release(this);
        }    
    }
    #endregion

    public void OverrideSizeSpeed(float sizeOverride, float speedOverride, int pierceOverride)
    {
        _defaultSize = transform.localScale.x;
        _defaultSpeed = speed;
        _defaultPierce = _pierceCounter;
        
        transform.localScale += Vector3.one * sizeOverride;
        speed += Mathf.Clamp(speedOverride, 0.1f, 100f);
        _pierceCounter += pierceOverride;
    }
    
    private void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0f) Release();
        
        transform.position += transform.right * speed * Time.deltaTime * 10;
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        bool hit = false;
        if (other.gameObject.CompareTag("Enemy"))
        {
            hit = true;
            ChainAttack(other.collider);
            ExplodeAttack();
            Enemy e = other.gameObject.GetComponent<Enemy>();
            if (!e.isDying)
            {
                e.isDying = true;
                Game.Instance.StartCoroutine(e.Die());
            }
        }
        if (_chain == 0 && hit && --_pierceCounter <= 0) Release();
    }

    private void ChainAttack(Collider2D hit)
    {
        if (_chain == 0) return;
        List<Collider2D> hitObjects = new List<Collider2D>();
        hitObjects.Add(hit);
        
        Collider2D[] enemy = Physics2D.OverlapCircleAll(transform.position, _chainRadius, Game.Instance.enemyManager.enemyMask);
        for (int i = 0; i < enemy.Length; i++)
        {
            if (enemy[i] != null && !hitObjects.Contains(enemy[i]))
            {
                Vector2 targetPos = enemy[i].transform.position;
                Vector2 currentPos = transform.position;
                Vector2 targetDirection = targetPos - currentPos;
                float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
                    
                Vector3 rotation_temp = transform.localEulerAngles;
                rotation_temp.z = angle;
                transform.localEulerAngles = rotation_temp;
                _chain--;
                break;
            }
        }
    }

    private void ExplodeAttack()
    {
        if (_explode == 0) return;
        GameObject o = Instantiate(Game.Instance.player.GetExplode(), transform.position, Quaternion.identity);
        Explode e = o.GetComponent<Explode>();
        o.transform.localScale = Vector3.one * _explode;
        e.ExplodeBomb(_explode);
    }
}
