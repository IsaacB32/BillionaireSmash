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
