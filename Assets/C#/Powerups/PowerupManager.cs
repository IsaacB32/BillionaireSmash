using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour
{
    public List<Powerup> powerupList;
    [SerializeField] private GameObject _powerupUI;

    public IEnumerator ShowPowerupChoices()
    {
        Game.Instance.audioManager.PlayPowerUp();

        yield return StartCoroutine(ScaleTime(1f, 0f, 0.5f));

        LeanTween.moveLocal(_powerupUI, Vector3.zero, 0.5f)
            .setIgnoreTimeScale(true);

        PowerupUI[] p_ui = _powerupUI.GetComponentsInChildren<PowerupUI>();
        foreach (PowerupUI p in p_ui)
        {
            p.AssignPowerup(ChoosePowerup());
        }
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
        LeanTween.moveLocal(_powerupUI, new Vector3(0f, -1000f, 0f), 0.5f).setIgnoreTimeScale(true);
        StartCoroutine(ScaleTime(0f, 1f, 0.5f));
    }
    
    IEnumerator ScaleTime(float startScale, float endScale, float duration)
    {
        float counter = 0f;
        
        while (counter < duration)
        {
            counter += Time.unscaledDeltaTime; 
            
            Time.timeScale = Mathf.Lerp(startScale, endScale, counter / duration);
            
            yield return null; 
        }
        Time.timeScale = endScale;
    }
}
