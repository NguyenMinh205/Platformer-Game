using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimController : MonoBehaviour
{
    [SerializeField] private Transform objPool;
    [SerializeField] private Animator collectAnim;
    [SerializeField] private Animator flagAnim;

    private void Start()
    {
        ObserverManager<GameEvent>.AddRegisterEvent(GameEvent.FruitCollected, OnFruitCollected);
        ObserverManager<GameEvent>.AddRegisterEvent(GameEvent.AllFruitsCollected, CollectAllFruits);
    }

    private void OnFruitCollected(object param)
    {
        StartCoroutine(CollectAnim());
    }    

    private void CollectAllFruits(object param)
    {
        flagAnim.SetBool("Win", true);
    }    

    private IEnumerator CollectAnim()
    {
        PoolingManager.Spawn(collectAnim, this.transform.position, Quaternion.identity, objPool);
        yield return new WaitForSeconds(0.5f);
        PoolingManager.Despawn(collectAnim.gameObject);
    }

    private void OnDestroy()
    {
        ObserverManager<GameEvent>.RemoveAddListener(GameEvent.FruitCollected, OnFruitCollected);
        ObserverManager<GameEvent>.RemoveAddListener(GameEvent.AllFruitsCollected, CollectAllFruits);
    }
}
