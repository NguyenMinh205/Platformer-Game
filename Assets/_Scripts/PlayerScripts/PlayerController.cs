using DG.Tweening;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Collider2D collider2D;
    [SerializeField] private float moveSpeed, jumpForce, extraHeight;

    [Header("Animation")]
    [SerializeField] private PlayerAnimation playerAnim;
    public PlayerAnimation PlayerAnim => playerAnim;

    [Header("Ground Detection")]
    [SerializeField] private LayerMask groundLayer;

    private float horizontal;
    private bool canDoubleJump;
    private bool isGrounded;
    private bool isDead = false;
    private bool wasPaused = false;

    private void Update()
    {
        if (!IsPlaying())
        {
            horizontal = 0;
            if (!wasPaused)
            {
                playerMovement.IsStanding();
                wasPaused = true;
            }
            return;
        }

        // Resume once after unpausing
        if (wasPaused)
        {
            playerMovement.ResumeControl();
            collider2D.enabled = true;
            wasPaused = false;
        }

        HandleInput();
    }

    private void FixedUpdate()
    {
        if (!IsPlaying()) return;

        playerMovement.Move(horizontal, moveSpeed);
        playerAnim.PlayAnimRun(horizontal);
        playerAnim.PlayAnimJumpAndFall(playerMovement.Rb.velocity.y);

        UpdateGroundState();
    }

    private void HandleInput()
    {
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

    private void UpdateGroundState()
    {
        isGrounded = CheckGround();
        playerMovement.IsJump(isGrounded);

        playerAnim.Animator.SetBool("IsGrounded", isGrounded);
        if (isGrounded)
        {
            playerAnim.Animator.SetBool("DoubleJump", false);
        }
    }

    private bool IsPlaying()
    {
        return GameManager.Instance.State == StateGame.Playing && !isDead;
    }

    public void PlayerDie()
    {
        if (isDead) return;

        isDead = true;
        playerMovement.enabled = false;

        playerAnim?.Animator?.SetTrigger("IsHitting");

        Rigidbody2D rb = playerMovement.Rb;
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.simulated = false;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

        collider2D.enabled = false;

        transform.DOMoveY(transform.position.y + 1.5f, 0.3f)
                 .SetEase(Ease.OutQuad)
                 .OnComplete(() =>
                 {
                     transform.DOMoveY(transform.position.y - 5f, 1f)
                              .SetEase(Ease.InQuad);
                 });

        DOVirtual.DelayedCall(5f, () =>
        {
            isDead = false;
            playerMovement.enabled = true;
        });
    }

    private bool CheckGround()
    {
        RaycastHit2D hit = Physics2D.BoxCast(
            collider2D.bounds.center,
            collider2D.bounds.size,
            0f,
            Vector2.down,
            extraHeight,
            groundLayer);

        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        if (collider2D == null) return;

        Gizmos.color = Color.red;
        Vector2 origin = collider2D.bounds.center + Vector3.down * extraHeight;
        Gizmos.DrawWireCube(origin, collider2D.bounds.size);
    }
}
