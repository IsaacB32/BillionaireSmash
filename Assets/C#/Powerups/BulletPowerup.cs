using UnityEngine;

[CreateAssetMenu(menuName = "BulletPowerup", fileName = "BulletPowerup")]
public class BulletPowerup : Powerup
{
    private void OnValidate()
    {
        type = PowerupType.BulletModifer;
    }
    [Header("Bullet Stats")]
    public BulletStats stats;
}


[System.Serializable]
public struct BulletStats
{
    public float size;
    public float speed;
    public int pierce;
    public int chain;
    public float explode;
    public int damage;
}