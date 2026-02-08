using UnityEngine;

[CreateAssetMenu(fileName = "StatBoost", menuName = "StatPowerup")]
public class StatPowerup : Powerup
{
    [Header("Stat Boost")]
    public int health;
    public float speed;
    [Tooltip("make the dash faster")] public int dashForce;
    [Tooltip("make the dash longer, also increases invicibility")] public float dashTime;
    [Tooltip("how long between dashes")] public float dashCooldown;
}
