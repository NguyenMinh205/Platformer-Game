using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    public Rigidbody2D Rb => rb;

    private Coroutine playMoveSoundCoroutine;
    [SerializeField] private float moveSoundDelay = 0.25f;

    private bool isJump;

    public void IsJump(bool isGround) => isJump = !isGround;

    public void IsStanding()
    {
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            if (rb.bodyType != RigidbodyType2D.Kinematic)
                rb.bodyType = RigidbodyType2D.Kinematic;
        }
    }

    public void ResumeControl()
    {
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.simulated = true;
        }
    }

    public void Move(float direction, float moveSpeed)
    {
        if (rb == null || rb.bodyType == RigidbodyType2D.Kinematic) return;

        if (Mathf.Abs(direction) > 0.01f)
        {
            Flip(direction);

            if (!isJump && playMoveSoundCoroutine == null)
            {
                playMoveSoundCoroutine = StartCoroutine(PlayMoveSound());
            }
        }
        else
        {
            StopMoveSound();
        }

        rb.velocity = new Vector2(direction * moveSpeed, rb.velocity.y);
    }

    public void Jump(float jumpForce)
    {
        if (rb == null || rb.bodyType == RigidbodyType2D.Kinematic) return;

        rb.velocity = Vector2.up * jumpForce * Mathf.Sign(rb.gravityScale);
        AudioManager.Instance.PlaySFXJump();
    }

    private IEnumerator PlayMoveSound()
    {
        AudioManager.Instance.PlaySFXMovement();
        yield return new WaitForSeconds(moveSoundDelay);
        playMoveSoundCoroutine = null;
    }

    private void StopMoveSound()
    {
        if (playMoveSoundCoroutine != null)
        {
            StopCoroutine(playMoveSoundCoroutine);
            playMoveSoundCoroutine = null;
        }
    }

    private void Flip(float direction)
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (direction > 0 ? 1 : -1);
        transform.localScale = scale;
    }
}
