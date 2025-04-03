using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISetting : MonoBehaviour
{
    [SerializeField] private Image dime;
    [SerializeField] private GameObject popup;
    [SerializeField] private Button home;
    [SerializeField] private Button replay;

    public bool IsMuteMusic
    {
        get => PlayerPrefs.GetInt("IsMuteMusic", 0) == 1;
        set
        {
            if (value == false)
            {
                PlayerPrefs.SetInt("IsMuteMusic", 0);
            }
            else
            {
                PlayerPrefs.SetInt("IsMuteMusic", 1);
            }
        }
    }

    public bool IsMuteSound
    {
        get => PlayerPrefs.GetInt("IsMuteSound", 0) == 1;
        set
        {
            if (value == false)
            {
                PlayerPrefs.SetInt("IsMuteSound", 0);
            }
            else
            {
                PlayerPrefs.SetInt("IsMuteSound", 1);
            }
        }
    }

    public void ButtonMusicClick()
    {
        AudioManager.Instance.PlaySoundClickButton();
        IsMuteMusic = !IsMuteMusic;
        AudioManager.Instance.SetMuteMusic();
    }

    public void ButtonSoundClick()
    {
        AudioManager.Instance.PlaySoundClickButton();
        IsMuteSound = !IsMuteSound;
        AudioManager.Instance.SetMuteSounds();
    }

    public virtual void DisplaySetting(bool isOn)
    {
        dime.gameObject.SetActive(isOn);
        popup.gameObject.SetActive(isOn);
    }    

    public void OnButtonSettingClick()
    {
        AudioManager.Instance.PlaySoundClickButton();
        DisplaySetting(true);
        if (GameManager.Instance.State == StateGame.Playing)
        {
            GameManager.Instance.State = StateGame.Pause;
            replay.gameObject.SetActive(true);
            home.gameObject.SetActive(true);
        }
        else
        {
            replay.gameObject.SetActive(false);
            home.gameObject.SetActive(false);
        }
    }

    public void OnButtonCloseClick()
    {
        AudioManager.Instance.PlaySoundClickButton();
        DisplaySetting(false);
        if (GameManager.Instance.State == StateGame.Pause)
        {
            GameManager.Instance.State = StateGame.Playing;
        }
    }
}
