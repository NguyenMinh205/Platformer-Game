using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimController : MonoBehaviour
{
    [Space]
    [Header("SourceForInGame")]
    //[SerializeField] private Transform objPool;
    [SerializeField] private GameObject collectAnim;
    [SerializeField] private Animator flagAnim;

    private void Start()
    {
        ObserverManager<GameEvent>.AddRegisterEvent(GameEvent.AllFruitsCollected, CollectAllFruits);
        //ObserverManager<GameEvent>.AddRegisterEvent(GameEvent.InPlaying, StartPlaying);
        //ObserverManager<GameEvent>.AddRegisterEvent(GameEvent.StopPlaying, StopPlaying);
    }

    private void OnEnable()
    {
        ObserverManager<GameEvent>.AddRegisterEvent(GameEvent.AllFruitsCollected, CollectAllFruits);
        ObserverManager<GameEvent>.AddRegisterEvent(GameEvent.InPlaying, StartPlaying);
        ObserverManager<GameEvent>.AddRegisterEvent(GameEvent.StopPlaying, StopPlaying);
    }

    public void StartPlaying(object param)
    {
        flagAnim = GameObject.FindGameObjectWithTag("Win").GetComponent<Animator>();
    }  
    
    public void StopPlaying(object param)
    {
        flagAnim = null;
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
        GameObject newCollectAnim = PoolingManager.Spawn(collectAnim, fruit.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(1f);
        PoolingManager.Despawn(newCollectAnim);
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
        ObserverManager<GameEvent>.RemoveAddListener(GameEvent.InPlaying, StartPlaying);
        ObserverManager<GameEvent>.RemoveAddListener(GameEvent.InPlaying, StopPlaying);
    }
}
