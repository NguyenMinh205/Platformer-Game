using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIWinLose : UISetting
{
    [SerializeField] private Button nextLevelBtn;
    [SerializeField] private TextMeshProUGUI title;
    public TextMeshProUGUI Title => title;

    public void DisplayPopupWinLose(bool isOn)
    {
        base.DisplaySetting(isOn);
        if (isOn == false)
        {
            return;
        }
        if (GameManager.Instance.State == StateGame.Win)
        {
            nextLevelBtn.gameObject.SetActive(true);
        }
        else
        {
            nextLevelBtn.gameObject.SetActive(false);
        }
    }
}
