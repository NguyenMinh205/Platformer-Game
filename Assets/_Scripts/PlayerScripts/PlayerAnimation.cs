using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    public Animator Animator => animator;
    private bool isJump;
    
    public void PlayAnimRun (float horizontal)
    {
        if (isJump)
            return;
        animator.SetFloat("Speed", Mathf.Abs(horizontal));
    }

    public void PlayAnimJump(float yVelocity)
    {
        animator.SetFloat("yVelocity", yVelocity);
    }

    public void PlayAnimDoubleJump()
    {
        animator.SetBool("DoubleJump", true);
    }
}
