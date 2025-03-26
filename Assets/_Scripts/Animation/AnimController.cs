using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimController : MonoBehaviour
{
    [SerializeField] private Transform objPool;
    [SerializeField] private GameObject collectAnim;
    [SerializeField] private Animator flagAnim;

    private void Start()
    {
        ObserverManager<GameEvent>.AddRegisterEvent(GameEvent.AllFruitsCollected, CollectAllFruits);
    }

    public void OnFruitCollected(GameObject fruit)
    {
        StartCoroutine(CollectAnim(fruit));
    }    

    public void CollectAllFruits(object param)
    {
        StartCoroutine(FlagWinAppear());
    }    

    private IEnumerator CollectAnim(GameObject fruit)
    {
        PoolingManager.Spawn(collectAnim, fruit.transform.position, Quaternion.identity, objPool);
        yield return new WaitForSeconds(2f);
        PoolingManager.Despawn(collectAnim);
    }

    private IEnumerator FlagWinAppear()
    {
        flagAnim.SetBool("AppearFlag", true);
        yield return new WaitForSeconds(1.275f);
        flagAnim.SetBool("AppearFlag", false);
        flagAnim.SetBool("Win", true);
    }

    private void OnDestroy()
    {
        ObserverManager<GameEvent>.RemoveAddListener(GameEvent.AllFruitsCollected, CollectAllFruits);
    }
}
