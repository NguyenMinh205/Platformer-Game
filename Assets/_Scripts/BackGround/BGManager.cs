using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGManager : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float loopDistance = 12f;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void FixedUpdate()
    {
        transform.position += Vector3.up * speed * Time.deltaTime;

        if (transform.position.y - startPosition.y >= loopDistance)
        {
            transform.position = startPosition;
        }
    }
}
