using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager instance;

    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

     void Start()
     {
        Play("Theme");
    }

    void Update()
    {
        VolumeUpdate("Theme");
    }

    // Update is called once per frame
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        if (KeyController.GameIsPaused)
        {
            //s.source.pitch *= .5f;
        }

        s.source.Play();
    }

    public void VolumeUpdate(string name)
    {
        if (VolumeController.musicVolumeChanged)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            switch (s.soundType)
            {
                case SoundType.MUSIC_VOLUME:
                    VolumeController.musicVolumeChanged = false;
                    s.source.volume = PlayerPrefs.GetFloat("musicVolume");
                    break;
                case SoundType.SFX_VOLUME:
                    s.source.volume = PlayerPrefs.GetFloat("sfxVolume");
                    break;
                default:
                    break;
            }
        }
    }
}
