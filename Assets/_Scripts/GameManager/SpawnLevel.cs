using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLevel : MonoBehaviour
{
    [SerializeField] private List<Transform> levels;
    private Transform currentLevel;

    public void SpawnNewLevel(int index)
    {
        if(index > levels.Count) return;
        DestroyMap();
        currentLevel = Instantiate(levels[index - 1], transform.position, Quaternion.identity, transform);
    }

    public void DestroyMap()
    {
        if (currentLevel != null)
        {
            Destroy(currentLevel.gameObject);
            currentLevel = null;
        }
    }
}
