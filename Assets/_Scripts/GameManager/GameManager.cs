using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private AnimController animController;
    [SerializeField] private GameObject sceneChoiceLevel;
    [SerializeField] private UIWinLose winLosePopup;
    [SerializeField] private UISetting settingPopup;
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

        StartCoroutine(DoSceneTransition(() =>
        {
            curLevel = level;
            DisableSceneChoiceLevel(level);
            state = StateGame.Playing;
            AudioManager.Instance.StopMusic();

            DOVirtual.DelayedCall(1f, delegate
            {
                AudioManager.Instance.PlayMusicInGame();
            });
        }));
    }


    public void Win()
    {
        AudioManager.Instance.PlaySoundWin();
        state = StateGame.Win;
        AudioManager.Instance.StopMusic();

        if (curLevel < MapLevelManager.Instance.ListBtn.Count)
        {
            MapLevelManager.Instance.ListBtn[curLevel].IsLock = true;
        }

        DOVirtual.DelayedCall(0.5f, () =>
        {
            winLosePopup.DisplayPopupWinLose(true);
            winLosePopup.Title.text = "YOU WIN";
        });
    }

    public void Replay()
    {
        AudioManager.Instance.PlaySoundClickButton();

        StartCoroutine(DoSceneTransition(() =>
        {
            if (state == StateGame.Win || state == StateGame.Lose)
            {
                winLosePopup.DisplayPopupWinLose(false);
            }
            else
            {
                settingPopup.DisplaySetting(false);
            }

            state = StateGame.Playing;
            spawnLevel.SpawnNewLevel(curLevel);
            AudioManager.Instance.PlayMusicInGame();
        }));
    }

    public void BackHome()
    {
        AudioManager.Instance.PlaySoundClickButton();

        StartCoroutine(DoSceneTransition(() =>
        {
            if (state == StateGame.Win || state == StateGame.Lose)
            {
                winLosePopup.DisplayPopupWinLose(false);
            }
            else
            {
                settingPopup.DisplaySetting(false);
            }
            if (state == StateGame.WaitingChoiceLevel)
            {
                SceneManager.LoadScene(0);
            }
            else
            {
                EnableSceneChoiceLevel();
            }
        }));
    }


    public void EnableSceneChoiceLevel()
    {
        sceneChoiceLevel.SetActive(true);
        state = StateGame.WaitingChoiceLevel;
        AudioManager.Instance.StopMusic();
        DOVirtual.DelayedCall(1f, delegate
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

        StartCoroutine(DoSceneTransition(() =>
        {
            curLevel++;
            winLosePopup.DisplayPopupWinLose(false);
            if (curLevel > MapLevelManager.Instance.ListBtn.Count)
            {
                EnableSceneChoiceLevel();
            }
            else
            {
                spawnLevel.SpawnNewLevel(curLevel);
                AudioManager.Instance.PlayMusicInGame();
            }
        }));
    }


    public void GameOver(GameObject player)
    {
        player.GetComponent<PlayerController>().PlayerDie();
        AudioManager.Instance.PlaySoundFail();
        state = StateGame.Lose;
        AudioManager.Instance.StopMusic();
        DOVirtual.DelayedCall(1f, () =>
        {
            winLosePopup.DisplayPopupWinLose(true);
            winLosePopup.Title.text = "YOU LOSE";
        });
    }

    public IEnumerator DoSceneTransition(Action onMidAction)
    {
        SceneTransitionAnim.Instance.StartTransition();
        yield return new WaitForSeconds(1f);

        onMidAction?.Invoke();

        yield return new WaitForSeconds(0.2f);

        SceneTransitionAnim.Instance.EndTransition();
    }

}
