using UnityEngine.Audio;
using System;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioSource bgsource;
    public Sound[] sounds;
    void Awake()
    { 
        Debug.Log("awake");
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        int i =0; 
        bgsource = GetComponent<AudioSource>();
        foreach ( Sound s in sounds)
        {
            i++;
            s.source = gameObject.AddComponent<AudioSource>();

            s.source.clip = s.clip;
            s.source.volume = s.vol;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.spatialize = s.spatialize;
            s.source.priority = i;
            
        }
        //FindObjectOfType<AudioManager>().Play("bgmusic");
        //PlayMusic(Resources.Load("Twitch Prime OST") as AudioClip);
        //PlayMusic(Resources.Load("mainmenu") as AudioClip);
        bgsource.clip = Resources.Load("Twitch Prime OST") as AudioClip;
        bgsource.volume = 0.5f;
        bgsource.pitch = 1;
        bgsource.loop = true;
        bgsource.Play();
        Debug.Log("start");
    }

    public void PlayMusic(AudioClip clip)
    {
        AudioSource.PlayClipAtPoint(clip, Vector3.zero, volume:1);
    }

    public void Play (string name)
    {
        if (name == "Twitch Prime OST" )
        {
            bgsource.Play();
            return;
        }
        Sound s = Array.Find( sounds, sound=>sound.name == name);
        s.source.Play();
    }

    public void Pause (string name)
    {
        if (name == "Twitch Prime OST" )
        {
            bgsource.Pause();
            return;
        }
        Sound s = Array.Find( sounds, sound=>sound.name == name);
        s.source.Pause();
    }
}