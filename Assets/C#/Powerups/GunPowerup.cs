using UnityEngine;

public class GunPowerup : Powerup
{
    [Header("Gun Style")]
    public GunStyleType style;
    public float rateFire;

    [Tooltip("charge attack only")] public float chargeGrowthRate = 2.3f;
}
