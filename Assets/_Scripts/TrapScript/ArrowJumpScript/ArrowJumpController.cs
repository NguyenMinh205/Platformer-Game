using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ArrowJumpController : MonoBehaviour
{

    [SerializeField] private float jumpForce, timeDelay;
    [SerializeField] private Animator anim;
    private bool isActive = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && isActive)
        {
            collision.GetComponent<PlayerMovement>().Jump(jumpForce);
            transform.DOPunchScale(Vector3.one * 0.2f, 0.2f, 5);
            StartCoroutine(DelayJumpNextTime());
        }
    }

    private IEnumerator DelayJumpNextTime()
    {
        isActive =false;
        anim.SetBool("IsHit", true);
        yield return new WaitForSeconds(timeDelay);
        isActive = true;
        anim.SetBool("IsHit", false);
    }
}
