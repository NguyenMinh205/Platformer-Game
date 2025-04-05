using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Space]
    [Header("Setting Animation")]
    public Ease scaleEase;         // Kiểu easing khi scale
    public float scaleOnHover;      // Kích thước khi hover
    public float scaleTransition;   // Thời gian scale

    public float hoverPunchAngle;   // Góc rung khi hover
    public float hoverTransition;   // Thời gian rung

    [Space] [Header("Setting Level")] 
    [SerializeField] private Button button;

    [SerializeField] private int level;
    public bool Lock = true;

    public bool IsLock
    {
        set
        {
            PlayerPrefs.SetInt("Level_" + level, value ? 1 : 0);
        }
        get => PlayerPrefs.GetInt("Level_" + level, 0) == 1;
    }


    private void Start()
    {
        if (button != null)
        {
            button.onClick.AddListener(StartLevel);
        }
    }

    private void OnEnable()
    {
        if (!Lock)
        {
            IsLock = true;
        }
    }

    private void StartLevel()
    {
        if(!IsLock) return;
        GameManager.Instance.PlayGame(level);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!IsLock) return;
        transform.DOScale(scaleOnHover, scaleTransition).SetEase(scaleEase);
        DOTween.Kill(2, true);
        transform.DOPunchRotation(Vector3.forward * hoverPunchAngle, hoverTransition, 20, 1).SetId(2);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(!IsLock) return;
        DOTween.Kill(2, true);
        transform.DOScale(1, scaleTransition).SetEase(scaleEase);
        transform.DOPunchRotation(Vector3.forward * -hoverPunchAngle, hoverTransition, 20, 1).SetId(2);
    }
}
