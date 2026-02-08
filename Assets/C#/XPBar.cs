using UnityEngine;
using UnityEngine.UI;

public class XPBar : MonoBehaviour
{
    [SerializeField] private Image xpFillImage;
    [SerializeField] private float fillSpeed = 6f;
    
    private float targetFill;

    private void Update()
    {
        xpFillImage.fillAmount =
            Mathf.Lerp(xpFillImage.fillAmount, targetFill, Time.deltaTime * fillSpeed);
    }

    public void UpdateXPBar(int currentXP, int xpRequired)
    {
        targetFill = Mathf.Clamp01((float)currentXP / xpRequired);
    }
}