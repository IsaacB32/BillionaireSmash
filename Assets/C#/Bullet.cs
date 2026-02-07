using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 1.5f;
    [SerializeField] private float maxLifeTime = 1f;
    
    [Header("Bullet Tweeks")]
    [SerializeField] private float _chainRadius = 2f;
    
    //powerup 
    private int _pierceCounter;
    private int _chain;

    private float _timer;
    private Gun _gun;

    private float _defaultSpeed;
    private float _defaultSize;
    
    #region Pool
    public void Init(Gun gub)
    {
        _gun = gub;
        _timer = maxLifeTime;

        speed = _gun.GetStats().speed;
        transform.localScale = _gun.GetStats().size * Vector3.one;
        _pierceCounter = _gun.GetStats().pierce;
        _chain = _gun.GetStats().chain;
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

    // public void SetStats(BulletStats stats)
    // {
    //     speed = stats.speed == 0 ? speed : stats.speed;
    //     transform.localScale = Vector3.one * (stats.size == 0 ? transform.localScale.x : stats.size);
    //     //TODO stats
    // }

    public void OverrideSizeSpeed(float sizeOverride, float speedOverride)
    {
        _defaultSize = transform.localScale.x;
        _defaultSpeed = speed;
        
        transform.localScale += Vector3.one * sizeOverride;
        speed += Mathf.Clamp(speedOverride, 0.1f, 100f);
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
            ChainAttack();
            Enemy e = other.gameObject.GetComponent<Enemy>();
            if (!e.isDying)
            {
                e.isDying = true;
                Game.Instance.StartCoroutine(e.Die());
            }
        }
        if (hit && _pierceCounter-- == 0) Release();
    }

    private void ChainAttack()
    {
        while (_chain != 0)
        {
            //Physics2D.OverlapCircle(transform.position, _chainRadius, );
        } 
    }
}
