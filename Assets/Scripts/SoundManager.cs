using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// Handels all sounds and music. 
/// </summary>
public class SoundManager : MonoBehaviour
{

    public List<AudioClip> music;
    public List<AudioClip> sfx;
    public AudioSource musicSource;
    public AudioSource sfxSource;
    public AudioSource contSFXSource;
    public AudioMixer mixer;
    /// <summary>
    /// Plays music from list of music, see editor for list and indexs
    /// </summary>
    /// <param name="songIndex"></param>
    public void PlayMusic(int songIndex)
    {
        if(songIndex < music.Count && songIndex >= 0)
        {
            musicSource.clip = music[songIndex];
            musicSource.Play();
            //Debug.Log("Playing " + music[songIndex].name);
        }
        else
        {
            Debug.Log("Song index out of range");
        }
    }
    /// <summary>
    /// Plays SFX from list of SFXs, see editor for list and indexs
    /// </summary>
    /// <param name="sfxIndex"></param>
    public void PlaySFX(int sfxIndex)
    {
        if(sfxIndex < sfx.Count && sfxIndex >= 0)
        {
            sfxSource.PlayOneShot(sfx[sfxIndex]);
            //Debug.Log("Playing one shot of " + sfx[sfxIndex].name);
        }
        else
        {
            Debug.Log("SFX index out of range");
        }
    }
    /// <summary>
    /// Plays continuing sfx from list of sfxs, see editor for list and indexs
    /// </summary>
    /// <param name="sfxIndex"></param>
    public void PlayContinuesSFX(int sfxIndex)
    {
        if(sfxIndex < sfx.Count && sfxIndex >= 0)
        {
            contSFXSource.clip = sfx[sfxIndex];
            contSFXSource.loop = true;
            contSFXSource.Play();
            //Debug.Log("Playing " + sfx[sfxIndex].name);
        }
        else
        {
            Debug.Log("SFX index out of range");
        }
    }
    /// <summary>
    /// Changes volume based on exposed group, and input value
    /// </summary>
    /// <param name="group"></param>
    /// <param name="value"></param>
    public void ChangeVolume(string group, float value)
    {
        mixer.SetFloat(group,value);
    }
}
