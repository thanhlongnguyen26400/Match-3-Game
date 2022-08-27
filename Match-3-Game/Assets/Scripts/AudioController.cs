using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [Header("Main Settings: ")]
    [Range(0, 1)]
    public float musicVolume;

    public AudioSource musicAus;

    [Header("Game Sound And Musics")]
    public AudioClip isMatch;
    public AudioClip[] bgmusics;


    public void Start()
    {
        PlayMusic(bgmusics);
    }
    public void PlaySound(AudioClip sound, AudioSource aus = null)
    {
        if (!aus)
        {
            aus = musicAus;
        }
        if (aus)
        {
            aus.PlayOneShot(sound, musicVolume);
        }
    }


    public void PlayMusic(AudioClip music, bool loop = true)
    {
        if (musicAus)
        {
            musicAus.clip = music;
            musicAus.loop = loop;
            musicAus.volume = musicVolume;
            musicAus.Play();
        }

    }
    public void PlayMusic(AudioClip[] music, bool loop = true)
    {
        if (musicAus)
        {
            int rangIdx = Random.Range(0, music.Length);

            if (music[rangIdx] != null)
            {
                musicAus.clip = music[rangIdx];
                musicAus.loop = loop;
                musicAus.volume = musicVolume;
                musicAus.Play();
            }
        }
    }
}
