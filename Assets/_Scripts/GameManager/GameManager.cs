using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private AnimController animController;
    [SerializeField] private GameObject sceneChoiceLevel;
    [SerializeField] private GameObject winLosePopup;
    public AnimController AnimController => animController;

    private StateGame state = StateGame.WaitingChoiceLevel;

    public StateGame State
    {
        get => state;
        set => state = value;
    }

    [SerializeField] private SpawnLevel spawnLevel;
    public SpawnLevel SpawnLevel => spawnLevel;

    private int curLevel = 1;    

    public void PlayGame(int level)
    {
        AudioManager.Instance.PlaySoundClickButton();
        if (level == 0) return;
        curLevel = level;
        spawnLevel.SpawnNewLevel(level);
        state = StateGame.Playing;
        AudioManager.Instance.StopMusic();
        DOVirtual.DelayedCall(1.5f, delegate
        {
            AudioManager.Instance.PlayMusicInGame();
        });
        //ObserverManager<GameEvent>.PostEvent(GameEvent.InPlaying);
    }    

    public void Win()
    {
        AudioManager.Instance.PlaySoundClickButton();
        state = StateGame.Win;
        AudioManager.Instance.StopMusic();
        curLevel += 1;

        DOVirtual.DelayedCall(0.5f, () =>
        {
            winLosePopup.SetActive(true);
        });
    }    

    public void Replay()
    {
        AudioManager.Instance.PlaySoundClickButton();
        if (state == StateGame.Win)
        {
            curLevel -= 1;
            state = StateGame.Playing;
        }
        spawnLevel.SpawnNewLevel(curLevel);
    }

    public void BackHome()
    {
        AudioManager.Instance.PlaySoundClickButton();
        if (state == StateGame.WaitingChoiceLevel)
        {
            SceneManager.LoadScene(0);
        }
        else 
        {
            EnableSceneChoiceLevel();
        }
    }   

    public void EnableSceneChoiceLevel()
    {
        sceneChoiceLevel.SetActive(true);
        state = StateGame.WaitingChoiceLevel;
        AudioManager.Instance.StopMusic();
        DOVirtual.DelayedCall(1.5f, delegate
        {
            AudioManager.Instance.PlayMusicBG();
        });
        spawnLevel.DestroyMap();
    }

    public void DisableSceneChoiceLevel(int level)
    {
        sceneChoiceLevel.SetActive(false);
        spawnLevel.SpawnNewLevel(level);
    }

    public void NextLevel()
    {
        AudioManager.Instance.PlaySoundClickButton();
        curLevel++;
        spawnLevel.SpawnNewLevel(curLevel);
        AudioManager.Instance.PlayMusicInGame();
    }    

    public void GameOver()
    {
        Debug.Log("Player Die");
        state = StateGame.Lose;
        AudioManager.Instance.StopMusic();
        DOVirtual.DelayedCall(0.5f, () =>
        {
            winLosePopup.SetActive(true);
        });
    }    
}
