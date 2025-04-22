using System.Collections;
using UnityEngine;

public class SpikeHeadController : MonoBehaviour
{
    //public enum MoveDirection { Horizontal, Vertical } 
    //[SerializeField] private MoveDirection moveDirection = MoveDirection.Horizontal;
    [SerializeField] private float speed; 
    [SerializeField] private float stopTime; 
    [SerializeField] private LayerMask groundLayer; 
    [SerializeField] private Vector2 moveDir;
    [SerializeField] private Animator anim;

    private bool isMoving = true;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        //moveDir = (moveDirection == MoveDirection.Horizontal) ? Vector2.right : Vector2.up;

        StartCoroutine(MoveSpike());
    }

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();

        //moveDir = (moveDirection == MoveDirection.Horizontal) ? Vector2.right : Vector2.up;

        StartCoroutine(MoveSpike());
    }

    private IEnumerator MoveSpike()
    {
        while (true)
        {
            if (isMoving)
            {
                rb.velocity = moveDir * speed;
            }
            else
            {
                rb.velocity = Vector2.zero;
            }
            yield return null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            if (isMoving)
            {
                StartCoroutine(ChangeDirectionAfterDelay());
            }
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.GameOver(collision.gameObject);
        }
    }

    private IEnumerator ChangeDirectionAfterDelay()
    {
        isMoving = false;
        anim.SetTrigger("Hit");
        yield return new WaitForSeconds(stopTime);
        moveDir *= -1;
        anim.SetTrigger("Hit");
        isMoving = true; 
    }

    private void OnDisable()
    {
        StopCoroutine(MoveSpike());
    }
}
