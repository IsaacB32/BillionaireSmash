using UnityEngine;

[CreateAssetMenu(menuName = "BulletPowerup", fileName = "BulletPowerup")]
public class BulletPowerup : Powerup
{
    [Header("Bullet Stats")]
    public BulletStats stats;
}


[System.Serializable]
public struct BulletStats
{
    public float size;
    public float speed;
    public int pierce;
    public float fire;
    public float electric;
    public float slowing; 
    public int chain;
    public float explode;
}