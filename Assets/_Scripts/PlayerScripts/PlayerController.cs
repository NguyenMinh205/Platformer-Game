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

    [Space]
    [Header("Layer Mask")]
    [SerializeField] private LayerMask groundLayer;

    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        playerMovement.Move(horizontal, moveSpeed);
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && CheckGround())
        {
            playerMovement.Jump(jumpForce);
        }    
    }

    private bool CheckGround()
    {
        RaycastHit2D hit = Physics2D.BoxCast(collider2D.bounds.center, collider2D.bounds.size, 0f, Vector2.down, extraHeight, groundLayer);
        return hit.collider != null;
    }
}
