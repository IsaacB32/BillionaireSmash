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
    public float size { private set; get; }
    public float speed { private set; get; }
    public float pierce { private set; get; }
    public float fire { private set; get; }
    public float electric { private set; get; }
    public float slowing { private set; get; } 
    public float chain { private set; get; }
    public float explode { private set; get; }
}