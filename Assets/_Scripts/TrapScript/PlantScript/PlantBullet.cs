using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantBullet : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody2D rb;
    private Vector2 movement = Vector2.zero;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Init(Vector2 movement)
    {
        this.movement = movement;
    }

    private void FixedUpdate()
    {
        rb.velocity = movement * speed;

        if (GameManager.Instance.State != StateGame.Playing)
        {
            PoolingManager.Despawn(this.gameObject);
        }
    }

    public void Hit(GameObject target)
    {
        PoolingManager.Despawn(this.gameObject);
        if (target.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player Die");
            GameManager.Instance.GameOver(target);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        this.Hit(collision.gameObject);
    }
}
