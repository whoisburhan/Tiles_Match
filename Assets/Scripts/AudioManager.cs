using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public const string MUSIC_VOLUME_KEY = "MUSIC_VOLUME_KEY";
    public const string SOUND_VOLUME_KEY = "SOUND_VOLUME_KEY";


    [Header("Audio Sources")]
    public AudioSource backgroundAudio;
    public AudioSource[] audioSource;

    [Header("Audio Clip")]
    public AudioClip[] backgroundClip;
    public AudioClip[] sounds;

    private void Awake()
    {
        if (Instance != null)
            Destroy(this.gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }


    /// <summary>
    /// Set and Play Background Music
    /// </summary>
    /// <param name="_audioClipIndex"></param>
    public void BackgroundAudioFunc(int _audioClipIndex, bool _isLoop = true /*,float _pitch = 1f*/)
    {
        if (backgroundAudio.isPlaying)
        {
            backgroundAudio.Stop();
        }

        //backgroundAudio.pitch = _pitch;
        // backgroundAudio.volume = _volume;
        backgroundAudio.loop = _isLoop;
        backgroundAudio.clip = backgroundClip[_audioClipIndex];
        backgroundAudio.Play();
    }

    public void StopBackgroundMusic()
    {
        if (backgroundAudio.isPlaying)
        {
            backgroundAudio.Stop();
        }
    }

    /// <summary>
    /// Set and play Sounds
    /// </summary>
    /// <param name="_audioSource"></param>
    /// <param name="_audioClipIndex"></param>
    public void AudioChangeFunc(int _audioSource, int _audioClipIndex, bool _isLoop = false/*, float _pitch = 1f, float _volume = 1f*/)
    {
        if (audioSource[_audioSource].isPlaying)
        {
            audioSource[_audioSource].Stop();
        }

        //  audioSource[_audioSource].pitch = _pitch;
        // audioSource[_audioSource].volume = _volume;
        audioSource[_audioSource].loop = _isLoop;
        audioSource[_audioSource].clip = sounds[_audioClipIndex];
        audioSource[_audioSource].Play();
    }

    public void MusicVolumeIncrese()
    {
        if (backgroundAudio.volume < 1f)
        {
            backgroundAudio.volume += .1f;
        }
    }

    public void MusicVolumeDecrese()
    {
        if (backgroundAudio.volume > 0f)
        {
            backgroundAudio.volume -= .1f;
        }
    }

    public void SoundVolumeIncrese()
    {
        if (audioSource[0].volume < 1f)
        {
            foreach (AudioSource _audioSouce in audioSource)
            {
                _audioSouce.volume += .1f;
            }
        }
    }

    public void SoundVolumeDecrese()
    {
        if (audioSource[0].volume > 0f)
        {
            foreach (AudioSource _audioSouce in audioSource)
            {
                _audioSouce.volume -= .1f;
            }
        }
    }

    public void ResetAudio()
    {
        if (backgroundAudio.isPlaying)
        {
            backgroundAudio.Stop();
        }

        foreach (var _audioSource in audioSource)
        {
            if (_audioSource.isPlaying)
            {
                _audioSource.Stop();
            }
        }
    }

    public void SoundVolume(float volume)
    {
        foreach (var x in audioSource)
        {
            x.volume = volume;
        }

        PlayerPrefs.SetFloat(SOUND_VOLUME_KEY, volume);
    }

    public void MusicVolume(float volume)
    {
        backgroundAudio.volume = volume;

        PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, volume);
    }

    public void Mute(AudioSource _audioSource)
    {
        _audioSource.mute = true;
    }

    public void UnMute(AudioSource _audioSource)
    {
        _audioSource.mute = false;
    }
}