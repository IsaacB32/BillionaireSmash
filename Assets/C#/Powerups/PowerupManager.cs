using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour
{
    public List<Powerup> powerupList;
    [SerializeField] private GameObject _powerupUI;

    public void ShowPowerupChoices()
    {
        _powerupUI.SetActive(true);
        PowerupUI[] p_ui = _powerupUI.GetComponentsInChildren<PowerupUI>();
        foreach (PowerupUI p in p_ui)
        {
            p.AssignPowerup(ChoosePowerup());
        }
        Game.Instance.Freeze();
    }

    private Powerup ChoosePowerup()
    {
        float totalWeight = 0f;
        foreach (var powerup in powerupList)  totalWeight += powerup.rarity;
        float roll = Random.Range(0f, totalWeight);
        
        foreach (var powerup in powerupList)
        {
            roll -= powerup.rarity;

            if (roll <= 0f)
            {
                if (powerup.type == PowerupType.GunModifier)
                {
                    var gunPowerup = (GunPowerup)powerup;
                    if (gunPowerup.style == Game.Instance.player.GetStyle())
                    {
                        // reject and retry
                        return ChoosePowerup();
                    }
                }
                return powerup;
            }
        }
        return null; 
    }

    public void Hide()
    {
        Game.Instance.Unfreeze();
        _powerupUI.SetActive(false);
    }
}
