using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class UIAnimator : MonoBehaviour
{
    Animator animator;
    private string currentState;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void ChangeAnimationState(string state)
    {
        if (currentState == state) { return; }

        animator.Play(state);

        currentState = state;
    }

    public void RestartAnimationState()
    {
        animator.Play(null);
        animator.Play(currentState);
    }

    public bool IsAnimatorPlaying
    {
        get
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName(currentState))
            {
                return true;
            }

            return false;
        }
    }
}
