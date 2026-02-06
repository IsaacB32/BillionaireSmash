using System;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private static readonly int Moving = Animator.StringToHash("moving");
    private static readonly int Idle = Animator.StringToHash("idle");
    [SerializeField] private Animator animator;

    public void UpdateState(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.Moving:
                animator.SetFloat(Moving, 2);
                animator.SetBool(Idle, false);
                break;
            case PlayerState.Idle:
                animator.SetBool(Idle, true);
                break;
            case PlayerState.Standing:
                animator.SetFloat(Moving, 0);
                break;
        }
    }
}
