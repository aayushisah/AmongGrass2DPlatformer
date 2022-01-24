using UnityEngine.Audio;
using System;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public Sound[] sounds;
    // Start is called before the first frame update
    void Awake()
    {
        
        
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
        
        foreach ( Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();

            s.source.clip = s.clip;
            s.source.volume = s.vol;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.spatialize = s.spatialize;
        }
        FindObjectOfType<AudioManager>().Play("mainmenu");
    }

    
    public void Play (string name)
    {
        Sound s = Array.Find( sounds, sound=>sound.name == name);
        s.source.Play();
    }


    public void Pause (string name)
    {
        Sound s = Array.Find( sounds, sound=>sound.name == name);
        s.source.Pause();
    }

}