using UnityEngine.Audio;
using UnityEngine;


[System.Serializable] 

public class Sound 
{
    
  public string name;
  public bool loop;
  
  public AudioClip clip;

  [Range(0f,1f)]
  public float vol;

  [Range(0.3f,1f)]
  public float pitch;

  public bool spatialize;

  [HideInInspector]
  public AudioSource source;

}
