using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckWin : MonoBehaviour
{
    private bool canWin;
    void Start()
    {
        ObserverManager<GameEvent>.AddRegisterEvent(GameEvent.AllFruitsCollected, SetWinTrue);
        ObserverManager<GameEvent>.AddRegisterEvent(GameEvent.StartPlaying, SetWinFalse);
    }

    private void OnEnable()
    {
        ObserverManager<GameEvent>.AddRegisterEvent(GameEvent.AllFruitsCollected, SetWinTrue);
        ObserverManager<GameEvent>.AddRegisterEvent(GameEvent.StartPlaying, SetWinFalse);
    }

    public void SetWinTrue(object param)
    {
        canWin = true;
    } 
    
    public void SetWinFalse(object param)
    {
        canWin = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canWin)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Debug.Log("Win");
                GameManager.Instance.Win ();
            }    
        }   
    }

    private void OnDestroy()
    {
        ObserverManager<GameEvent>.RemoveAddListener(GameEvent.FruitCollected, SetWinTrue);
        ObserverManager<GameEvent>.RemoveAddListener(GameEvent.StartPlaying, SetWinFalse);
    }
}
