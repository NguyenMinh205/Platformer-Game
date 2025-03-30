using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AudioManager : Singleton<AudioManager>
{
    [Header("Audio Source")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource soundSource;

    [Header("Audio Click")]
    [SerializeField] private AudioClip buttonClick;

    [Space]
    [Header("Audio in Game")]
    [SerializeField] private AudioClip musicBG;
    [SerializeField] private AudioClip musicInGame;
    [SerializeField] private AudioClip soundWin;
    [SerializeField] private AudioClip soundFail;

    [Space]
    [Header("Audio in GamePlay")]
    [SerializeField] private AudioClip playerFall;
    [SerializeField] private AudioClip playerJump;
    [SerializeField] private AudioClip playerMoving;
    [SerializeField] private AudioClip playerGetItem;

    void Start()
    {
        SetMuteSounds();
        SetMuteMusic();
    }

    public void SetMuteSounds()
    {
        if (UIController.Instance.UISetting.IsMuteSound)
        {
            soundSource.mute = true;
            return;
        }
        soundSource.mute = false;
    }
    public void SetMuteMusic()
    {
        if (UIController.Instance.UISetting.IsMuteMusic)
        {
            musicSource.mute = true;
            return;
        }
        musicSource.mute = false;
    }

    public void PlayMusicInGame()
    {
        if (!musicSource.isPlaying)
        {
            if (musicInGame != null)
            {
                musicSource.clip = musicInGame;
                musicSource.DOFade(1f, 0.5f).OnPlay(() =>
                {
                    musicSource.Play();
                }).SetUpdate(true);
            }
        }
    }

    public void PlayMusicBG()
    {
        if (!musicSource.isPlaying)
        {
            if (musicInGame != null)
            {
                musicSource.clip = musicBG;
                musicSource.DOFade(1f, 0.5f).OnPlay(() =>
                {
                    musicSource.Play();
                }).SetUpdate(true);
            }
        }
    }

    public void StopMusic()
    {
        if (musicSource.isPlaying)
        {
            musicSource.DOFade(0f, 0.5f).OnComplete(() =>
            {
                musicSource.Stop();
            }).SetUpdate(true);
        }
    }

    public void PlaySFX(AudioClip sound, bool repeat = false)
    {
        if (UIController.Instance.UISetting.IsMuteSound) return;

        if (sound != null)
        {
            if (repeat)
            {
                soundSource.loop = true;
                soundSource.clip = sound;   
                soundSource.Play();
            }
            else
            {
                soundSource.loop = false;
                soundSource.PlayOneShot(sound);
            }
        }
    }

    public void PlaySoundClickButton()
    {
        PlaySFX(buttonClick);
    }

    public void PlaySFXMovement()
    {
        PlaySFX(playerMoving);
    }   
    
    public void PlaySFXJump()
    {
        PlaySFX(playerJump);
    }    

    public void PlaySFXFall()
    {
        PlaySFX(playerFall);
    }    

    public void PlaySFXGetItem()
    {
        PlaySFX(playerGetItem);
    }    

    public void PlaySoundWin()
    {
        PlaySFX(soundWin);
    }    

    public void PlaySoundFail()
    {
        PlaySFX(soundFail);
    }    
}
