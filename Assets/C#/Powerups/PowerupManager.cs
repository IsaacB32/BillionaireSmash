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
        Game.Instance.Pause();
    }

    private Powerup ChoosePowerup()
    {
        int random = 0;
        bool lookForPowerup = true;
        while (lookForPowerup)
        {
            random = Random.Range(0, powerupList.Count - 1);
            if (powerupList[random].type == PowerupType.GunModifier)
            {
                if (((GunPowerup)powerupList[random]).style != Game.Instance.player.GetStyle())
                {
                    return powerupList[random];
                }
            }
            else lookForPowerup = false;
        }

        return powerupList[random];
    }

    public void Hide()
    {
        Game.Instance.Unpause();
        _powerupUI.SetActive(false);
    }
}
