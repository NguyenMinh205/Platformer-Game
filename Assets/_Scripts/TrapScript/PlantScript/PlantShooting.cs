using System.Collections;
using UnityEngine;

public class PlantShooting : MonoBehaviour
{
    [SerializeField] private PlantBullet bulletPrefab;
    [SerializeField] private float timeDelay, timeStart;
    [SerializeField] private Animator animator;

    private Coroutine shootingCoroutine;

    private void Start()
    {
        shootingCoroutine = StartCoroutine(StartShooting());
    }

    private void OnEnable()
    {
        shootingCoroutine = StartCoroutine(StartShooting());
    }

    private IEnumerator StartShooting()
    {
        yield return new WaitForSeconds(timeStart);
        if (GameManager.Instance.State != StateGame.Playing)
        {
            StopCoroutine(shootingCoroutine);
            shootingCoroutine = null;
        }
        while (true)
        {
            Debug.Log("Shoot");
            animator.SetTrigger("Shoot");
            yield return new WaitForSeconds(0.8f);
            PlantBullet newBullet = PoolingManager.Spawn<PlantBullet>(bulletPrefab, this.transform.position + new Vector3(0.3f, 0.3f, 0), Quaternion.identity);
            newBullet.Init(Vector2.left);
            animator.SetTrigger("Shoot");
            yield return new WaitForSeconds(timeDelay);
        }
    }

    private void OnDisable()
    {
        if (shootingCoroutine != null)
        {
            StopCoroutine(shootingCoroutine);
            shootingCoroutine = null;
        }
    }
}
