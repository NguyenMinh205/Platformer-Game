using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitionAnim : Singleton<SceneTransitionAnim>
{
    [SerializeField] private Animator animTransition;

    public virtual void Awake()
    {
        base.Awake();
        KeepAlive(true);
    }

    public void FadeInTransition()
    {
        animTransition.SetTrigger("FadeInAnim");
    }

    public void FadeOutTransition()
    {
        animTransition.SetTrigger("FadeOutAnim");
    }

    public void StartTransition()
    {
        this.gameObject.SetActive(true);
    }  
    
    public void EndTransition()
    {
        this.gameObject.SetActive(false);
    }    
}
