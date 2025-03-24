using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    [Space]
    [Header("Setting movement")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Collider2D collider2D;
    [SerializeField] private float moveSpeed, jumpForce, extraHeight;
    private float horizontal;
    private bool canDoubleJump;
    private bool isGrounded;

    [Space]
    [Header("Animation")]
    [SerializeField] private PlayerAnimation playerAnim;
    public PlayerAnimation PlayerAnim => playerAnim;

    [Space]
    [Header("Layer Mask")]
    [SerializeField] private LayerMask groundLayer;

    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        playerMovement.Move(horizontal, moveSpeed);
        playerAnim.PlayAnimRun(horizontal);
        playerAnim.PlayAnimJumpAndFall(playerMovement.Rb.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))
        {
            if (isGrounded)
            {
                playerMovement.Jump(jumpForce);
                playerAnim.Animator.SetBool("IsGrounded", false);
                canDoubleJump = true;
                isGrounded = false;
            }
            else if (canDoubleJump)
            {
                playerMovement.Jump(jumpForce);
                playerAnim.PlayAnimDoubleJump();
                canDoubleJump = false;
            }
        }
    }

    private void FixedUpdate()
    {
        isGrounded = CheckGround();

        if (isGrounded)
        {
            playerAnim.Animator.SetBool("IsGrounded", true);
            playerAnim.Animator.SetBool("DoubleJump", false);
        }
    }

    private bool CheckGround()
    {
        RaycastHit2D hit = Physics2D.BoxCast(collider2D.bounds.center, collider2D.bounds.size, 0f, Vector2.down, extraHeight, groundLayer);
        Debug.DrawRay(collider2D.bounds.center, Vector2.down * (collider2D.bounds.extents.y + extraHeight), Color.red);
        return hit.collider != null;
    }
}
