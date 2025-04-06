using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitManager : MonoBehaviour
{
    private int totalFruits;
    private int collectedFruits;

    //private void Start()
    //{
    //    totalFruits = FindObjectsOfType<FruitController>().Length;
    //    ObserverManager<GameEvent>.AddRegisterEvent(GameEvent.FruitCollected, OnFruitCollected);
    //    collectedFruits = 0;
    //}

    private void OnEnable()
    {
        totalFruits = FindObjectsOfType<FruitController>().Length;
        ObserverManager<GameEvent>.AddRegisterEvent(GameEvent.FruitCollected, OnFruitCollected);
        collectedFruits = 0;
    }

    private void OnFruitCollected(object param)
    {
        collectedFruits++;
        Debug.Log($"Đã thu thập {collectedFruits}/{totalFruits} quả");

        if (collectedFruits >= totalFruits)
        {
            ObserverManager<GameEvent>.PostEvent(GameEvent.AllFruitsCollected);
        }
    }

    private void OnDestroy()
    {
        ObserverManager<GameEvent>.RemoveAddListener(GameEvent.FruitCollected, OnFruitCollected);
    }
}
