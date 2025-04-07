using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLevel : MonoBehaviour
{
    [SerializeField] private List<Transform> levels;
    private Transform currentLevel;

    public void SpawnNewLevel(int index)
    {
        if (index > levels.Count || index <= 0)
        {
            GameManager.Instance.BackHome();
            return;
        }
        StartCoroutine(Spawn(index));

        ObserverManager<GameEvent>.PostEvent(GameEvent.StartPlaying);
    }

    private IEnumerator Spawn(int index)
    {
        DestroyMap();
        yield return new WaitForSeconds(0.1f);
        currentLevel = Instantiate(levels[index - 1], transform.position, Quaternion.identity, transform);
    }

    public void DestroyMap()
    {
        if (currentLevel != null)
        {
            Destroy(currentLevel.gameObject);
            currentLevel = null;
        }
        ObserverManager<GameEvent>.PostEvent(GameEvent.StopPlaying);
    }
}
