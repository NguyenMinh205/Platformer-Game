using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitionAnim : Singleton<SceneTransitionAnim>
{
    [SerializeField] private Animator animTransition;
    [SerializeField] private string startTransitionTriggerName = "StartTransition";
    [SerializeField] private string endTransitionTriggerName = "EndTransition";

    public virtual void Awake()
    {
        base.Awake();
        KeepAlive(false);
    }

    public void StartTransition()
    {
        animTransition.SetTrigger(startTransitionTriggerName);
    }

    public void EndTransition()
    {
        animTransition.SetTrigger(endTransitionTriggerName);
    }
}
