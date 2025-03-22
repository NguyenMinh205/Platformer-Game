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
    
    public void PlayAnimJump (bool isJump)
    {
        this.isJump = isJump;
        animator.SetBool("isJumping", this.isJump);
        StartCoroutine(WaitForNextJump(0.5f));
    }

    public void PlayAnimDoubleJump()
    {
        animator.SetBool("DoubleJump", true);
        StartCoroutine(WaitForNextJump(0.7f));
    }

    private IEnumerator WaitForNextJump(float timeDelay)
    {
        yield return new WaitForSeconds(timeDelay);
        isJump = false;
        animator.SetBool("isJumping", false);
        animator.SetBool("DoubleJump", false);
    }    
}
