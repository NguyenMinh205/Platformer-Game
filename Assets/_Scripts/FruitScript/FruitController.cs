using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ObserverManager<GameEvent>.PostEvent(GameEvent.FruitCollected, this);
            this.gameObject.SetActive(false);
        }
    }
}
