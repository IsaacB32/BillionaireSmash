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
                _player.SetUpgradeStats(sp.speed,sp.health,sp.fireRate);
                break;
            case PowerupType.BulletModifer:
                break;
            case PowerupType.GunModifier:
                break;
        }
    }
}
