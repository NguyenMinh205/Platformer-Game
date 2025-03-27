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

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))
        {
            if (isGrounded == true)
            {
                playerMovement.Jump(jumpForce);
                canDoubleJump = true;
                isGrounded = false;
            }
            else if (canDoubleJump)
            {
                playerAnim.PlayAnimDoubleJump();
                playerMovement.Jump(jumpForce * 1.15f);
                canDoubleJump = false;
            }
        }
    }

    private void FixedUpdate()
    {
        playerMovement.Move(horizontal, moveSpeed);
        playerAnim.PlayAnimRun(horizontal);
        playerAnim.PlayAnimJumpAndFall(playerMovement.Rb.velocity.y);
        isGrounded = CheckGround();

        if (isGrounded == true)
        {
            playerAnim.Animator.SetBool("IsGrounded", true);
            playerAnim.Animator.SetBool("DoubleJump", false);
        }
        else
        {
            playerAnim.Animator.SetBool("IsGrounded", false);
        }
    }

    private bool CheckGround()
    {
        RaycastHit2D hit = Physics2D.BoxCast(collider2D.bounds.center, collider2D.bounds.size, 0f, Vector2.down, extraHeight, groundLayer);
        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        if (TryGetComponent(out Collider2D collider))
        {
            Vector2 origin = (Vector2)collider2D.bounds.center;
            Vector2 size = collider.bounds.size;
            Vector2 direction = Vector2.down;
            float distance = extraHeight;

            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(origin, size);

            Vector2 newPosition = origin + direction * distance;
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(newPosition, size);
        }
    }
}
