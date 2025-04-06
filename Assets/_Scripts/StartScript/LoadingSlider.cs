using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class LoadingSlider : MonoBehaviour
{
    [SerializeField] private GameObject panelLoading;
    [SerializeField] private GameObject popUpStartGame;
    [SerializeField] private Slider slider;
    [SerializeField] private float duration;
    private const float maxValue = 100;
    private float value;


    void Start()
    {
        slider.maxValue = maxValue;
        DOTween.To(() => value, x => value = x, 100f, duration).OnUpdate(() =>
        {
            slider.value = value;
        }).OnComplete(() =>
        {
            panelLoading.SetActive(false);
            popUpStartGame.gameObject.SetActive(true);
            popUpStartGame.gameObject.transform.localScale = Vector3.zero;
            popUpStartGame.transform.DOScale(1f, 0.35f).SetEase(Ease.OutBounce);
        });
    }

    public void Play()
    {
        //StartCoroutine(GameManager.Instance.DoSceneTransition(() =>
        //{
        //    SceneManager.LoadScene(1);
        //    GameManager.Instance.State = StateGame.WaitingChoiceLevel;
        //}));
        SceneManager.LoadScene(1);
        GameManager.Instance.State = StateGame.WaitingChoiceLevel;
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");

        Application.Quit();
    }    
}
