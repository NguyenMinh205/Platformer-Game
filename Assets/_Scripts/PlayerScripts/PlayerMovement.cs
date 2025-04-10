using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    public Rigidbody2D Rb => rb;

    public void Move(float direction, float moveSpeed)
    {
        if (Mathf.Abs(direction) > 0.01f) Flip(direction);
        rb.velocity = new Vector2(direction * moveSpeed, rb.velocity.y);
        //AudioManager.Instance.PlaySFXMovement();
    }

    private void Flip(float direction)
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (direction > 0 ? 1 : -1);
        transform.localScale = scale;
    }
    public void Jump(float jumpForce)
    {
        rb.velocity = Vector2.up * jumpForce * Mathf.Sign(rb.gravityScale);
        AudioManager.Instance.PlaySFXJump();
    } 
}
