using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : Singleton<UIController>
{
    public UISetting UISetting => FindObjectOfType<UISetting>();
    //public UIWin UIWin => FindObjectOfType<UIWin>();
    //public UIInGame UIInGame => FindObjectOfType<UIInGame>();
}