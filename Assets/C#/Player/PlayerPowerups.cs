using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPowerups : MonoBehaviour
{
    private Player _player;
    private List<Powerup> _powerupList = new List<Powerup>();
    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    public void AttachPowerup(Powerup p)
    {
        _powerupList.Add(p);
        switch (p.type)
        {
            case PowerupType.StatBoost:
                StatPowerup sp = (StatPowerup)p;
                _player.SetUpgradeStats(sp.speed,sp.health);
                break;
            case PowerupType.BulletModifer:
                BulletPowerup bp = (BulletPowerup)p;
                _player.UpgradeBullets(bp.stats);
                break;
            case PowerupType.GunModifier:
                GunPowerup gp = (GunPowerup)p;
                _player.UpgradeGun(gp.style, gp.rateFire, gp.chargeGrowthRate);
                break;
        }
    }
}
