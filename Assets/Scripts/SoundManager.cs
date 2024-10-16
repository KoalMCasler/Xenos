using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{

    public List<AudioClip> music;
    public List<AudioClip> sfx;
    public AudioSource musicSource;
    public AudioSource sfxSource;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayMusic(int songIndex)
    {
        if(songIndex < music.Count && songIndex >= 0)
        {
            musicSource.clip = music[songIndex];
            Debug.Log("Playing " + music[songIndex].name);
        }
        else
        {
            Debug.Log("Song index out of range");
        }
    }

    public void PlaySFX(int sfxIndex)
    {
        if(sfxIndex < sfx.Count && sfxIndex >= 0)
        {
            sfxSource.PlayOneShot(sfx[sfxIndex]);
            Debug.Log("Playing one shot of " + sfx[sfxIndex].name);
        }
        else
        {
            Debug.Log("SFX index out of range");
        }
    }
}
