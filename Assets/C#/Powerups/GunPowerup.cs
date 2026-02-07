using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GunPowerup", menuName = "GunPowerup")]
public class GunPowerup : Powerup
{
    private void OnValidate()
    {
        type = PowerupType.GunModifier;
    }

    [Header("Gun Style")]
    public GunStyleType style;
    public float rateFire;

    [Tooltip("charge attack only")] public float chargeGrowthRate = 2.3f;
}
