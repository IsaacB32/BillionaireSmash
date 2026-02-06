using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerupUI : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private TextMeshProUGUI _description;
    [SerializeField] private Image _image;

    private Powerup _attached;

    public void AssignPowerup(Powerup p)
    {
        _attached = p;
        _title.text = p.powerup_name;
        _description.text = p.description;
        _image.sprite = p.icon;
    }

    public void Clicked()
    {
        Game.Instance.player.AttachPowerup(_attached);
        //hide menu
    }
}
