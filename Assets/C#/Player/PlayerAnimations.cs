using System;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private static readonly int Moving = Animator.StringToHash("moving");
    [SerializeField] private Animator animator;

    public void UpdateState(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.Moving:
                animator.SetFloat(Moving, 2);
                break;
            case PlayerState.Idle:
                animator.SetFloat(Moving, 0);
                break;
        }
    }

    private void Awake()
    {
        SwitchGunArt(GunStyleType.Default);
    }

    [Space]
    [SerializeField] private GameObject shotgun;
    [SerializeField] private GameObject charge;
    [SerializeField] private GameObject pistol;

    public void SwitchGunArt(GunStyleType type)
    {
        charge.SetActive(false);
        pistol.SetActive(false);
        shotgun.SetActive(false);
        
        switch (type)
        {
            case GunStyleType.ChargeGun:
                charge.SetActive(true);
                break;
            case GunStyleType.Shotgun:
            case GunStyleType.DuelBarrel:
                shotgun.SetActive(true);
                break;
            default:
                pistol.SetActive(true);
                break;
        }
    }
}
