using UnityEngine;

[CreateAssetMenu(fileName = "StatBoost", menuName = "StatPowerup")]
public class StatPowerup : Powerup
{
    [Header("Stat Boost")]
    public int health;
    public float speed;
    public float fireRate;
}
