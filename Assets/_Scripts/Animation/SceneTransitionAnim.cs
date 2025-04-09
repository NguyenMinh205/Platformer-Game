using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitionAnim : Singleton<SceneTransitionAnim>
{
    [SerializeField] private Animator animTransition;

    public virtual void Awake()
    {
        base.Awake();
        KeepAlive(false);
    }

    public void StartTransition()
    {
        animTransition.SetTrigger("StartTransition");
    }

    public void EndTransition()
    {
        animTransition.SetTrigger("EndTransition");
    }
}
