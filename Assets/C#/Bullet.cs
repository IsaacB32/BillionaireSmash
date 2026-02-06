using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 1.5f;
    [SerializeField] private float bulletLifeTime = 1f;

    private void Start()
    {
        Invoke(nameof(Destory), bulletLifeTime);
    }

    private void FixedUpdate()
    {
        transform.position += transform.right * speed * Time.deltaTime * 10;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //get hurt
        Destory();
    }

    private void Destory()
    {
        //do pooling
    }
}
