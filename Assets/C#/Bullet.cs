using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 1.5f;
    [SerializeField] private float maxLifeTime = 1f;

    private float _timer;
    private Gun _gun;
    
    #region Pool
    public void Init(Gun gub)
    {
        _gun = gub;
        _timer = maxLifeTime;
    }

    private void Release()
    {
        if (gameObject.activeSelf && _gun != null)
        {
            _gun.Release(this);
        }    
    }
    #endregion

    public void SetStats(BulletStats stats)
    {
        speed = stats.speed == 0 ? speed : stats.speed;
        transform.localScale = Vector3.one * (stats.size == 0 ? transform.localScale.x : stats.size);
        //TODO stats
    }
    
    private void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0f) Release();
        
        transform.position += transform.right * speed * Time.deltaTime * 10;
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Enemy e = other.gameObject.GetComponent<Enemy>();
            if (!e.isDying)
            {
                e.isDying = true;
                Game.Instance.StartCoroutine(e.Die());
            }
        }
        Release();
    }
}
