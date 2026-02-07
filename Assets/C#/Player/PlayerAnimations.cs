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
}
