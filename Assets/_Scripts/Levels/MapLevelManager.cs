using System.Collections.Generic;
using UnityEngine;

public class MapLevelManager : Singleton<MapLevelManager>
{
    protected override void Awake()
    {
        base.KeepAlive(false);
        base.Awake();
    }

    [SerializeField] private List<ButtonController> listBtn;
    public List<ButtonController> ListBtn => listBtn;
}