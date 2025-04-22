using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawController : MonoBehaviour
{
    [SerializeField] private List<Transform> waypoints = new List<Transform>();
    [SerializeField] private float speed;
    [SerializeField] private float timeDelay;
    [SerializeField] private Rigidbody2D rb;
    private Sequence moveSequence;
    private Coroutine moveCoroutine;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (waypoints == null || waypoints.Count == 0)
        {
            Debug.LogError("Waypoints list is empty! Saw will not move.");
            return;
        }

/*        moveCoroutine = StartCoroutine(MoveAlongWaypoints());
*/    }

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        if (waypoints.Count > 0 && moveCoroutine == null)
        {
            MoveAlongWaypoints();
        }
    }

    private void MoveAlongWaypoints()
    {
        moveSequence = DOTween.Sequence();

        for (int i = 0; i < waypoints.Count; i++)
        {
            int nextIndex = (i + 1) % waypoints.Count; // Quay vòng khi đến điểm cuối
            float moveTime = Vector2.Distance(waypoints[i].position, waypoints[nextIndex].position) / speed;

            moveSequence.Append(transform.DOMove(waypoints[nextIndex].position, moveTime)
                .SetEase(Ease.Linear));

            moveSequence.AppendInterval(timeDelay); // Dừng tại waypoint
        }

        moveSequence.SetLoops(-1);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.GameOver(collision.gameObject);
        }
    }

    private void OnDisable()
    {
        if (moveCoroutine != null)
        {
            moveSequence.Kill();
            moveCoroutine = null;
        }
    }
}
