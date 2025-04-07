using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            AudioManager.Instance.PlaySFXGetItem();
            GameManager.Instance.AnimController.OnFruitCollected(this.gameObject);
            this.gameObject.SetActive(false);
            ObserverManager<GameEvent>.PostEvent(GameEvent.FruitCollected, this);
        }
    }
}
