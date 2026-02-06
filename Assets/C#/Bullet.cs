using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 1.5f;
    [SerializeField] private float maxLifeTime = 1f;

    private float _timer;
    private Gun _gun;
    
    public void Init(Gun gub)
    {
        _gun = gub;
        _timer = maxLifeTime;
    }
    
    private void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0f) Release();
        
        transform.position += transform.right * speed * Time.deltaTime * 10;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //get hurt
        Release();
    }

    private void Release()
    {
        if (gameObject.activeSelf && _gun != null)
        {
            _gun.Release(this);
        }    
    }
}
