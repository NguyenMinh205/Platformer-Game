using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
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

    private bool isDead = false;

    private void Update()
    {
        if (GameManager.Instance.State != StateGame.Playing || isDead) return;

        horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))
        {
            if (isGrounded)
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
        if (isDead) return;

        playerMovement.Move(horizontal, moveSpeed);
        playerAnim.PlayAnimRun(horizontal);
        playerAnim.PlayAnimJumpAndFall(playerMovement.Rb.velocity.y);
        isGrounded = CheckGround();

        if (isGrounded)
        {
            playerAnim.Animator.SetBool("IsGrounded", true);
            playerAnim.Animator.SetBool("DoubleJump", false);
        }
        else
        {
            playerAnim.Animator.SetBool("IsGrounded", false);
        }
    }

    public void PlayerDie()
    {
        if (isDead) return;
        isDead = true;

        playerMovement.enabled = false;

        if (playerAnim != null && playerAnim.Animator != null)
        {
            playerAnim.Animator.SetTrigger("IsHitting");
        }

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.simulated = false;
            rb.bodyType = RigidbodyType2D.Kinematic;
            collider2D.enabled = false;
        }

        transform.DOMoveY(transform.position.y + 1.5f, 0.3f)
                 .SetEase(Ease.OutQuad)
                 .OnComplete(() =>
                 {
                     transform.DOMoveY(transform.position.y - 5f, 1f)
                              .SetEase(Ease.InQuad);
                 });

        DOVirtual.DelayedCall(2f, () =>
        {
            isDead = false;
        });
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
