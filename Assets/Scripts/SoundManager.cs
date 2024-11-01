using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Networking;

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
    public string audioPath;
    public int activeSongIndex;
    void Start()
    {
        audioPath = Application.streamingAssetsPath + "/Music/";
        GetMusic();
    }
    /// <summary>
    /// Plays music from list of music, see editor for list and indexs
    /// </summary>
    /// <param name="songIndex"></param>
    public void PlayMusic(int songIndex)
    {
        activeSongIndex = songIndex;
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
    public void NextSong()
    {
        activeSongIndex += 1;
        if(activeSongIndex >= music.Count())
        {
            activeSongIndex = 0;
        }
        PlayMusic(activeSongIndex);
    }
    public void PrevSong()
    {
        activeSongIndex -= 1;
        if(activeSongIndex < 0)
        {
            activeSongIndex = music.Count() - 1;
        }
        PlayMusic(activeSongIndex);
    }
    /// <summary>
    /// Gets all user added music and adds them to the music list. 
    /// </summary>
    public void GetMusic()
    {
        string[] songs = Directory.GetFiles(audioPath,"*.mp3");
        foreach (string song in songs)
        {
            StartCoroutine(LoadAudioClip(song));
        }
    }
    /// <summary>
    /// Loads File using Unity WebRequest on internal File Structure. 
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    IEnumerator LoadAudioClip(string path)
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(path, AudioType.MPEG))
        {
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
                yield break;
            }
            AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
            string clipName = Path.GetFileNameWithoutExtension(path);
            clip.name = clipName;
            music.Add(clip);
        }
    }

    public void OpenMusicFolder()
    {
        Application.OpenURL(audioPath);
    }

   
}
