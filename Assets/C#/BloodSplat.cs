using System;
using UnityEngine;

public class BloodSplat : MonoBehaviour
{
    [SerializeField] private int _lifetime = 5;
    
    private void Start()
    {
        Invoke(nameof(Die), _lifetime);
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
