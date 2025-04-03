using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWinLose : UISetting
{
    [SerializeField] private Button nextLevelBtn;

    public void DisplayPopupWinLose()
    {
        base.DisplaySetting(true);
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
