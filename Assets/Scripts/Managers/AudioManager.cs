using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    // MANAGERS
    private GameManager GM;

    private bool inGame;

    enum audioModes
    {
        Beeps = 0,
        Bit8 = 1,
        HD = 2,
        HDEnd = 3,
        None = 4
    }

    private audioModes audioMode;
    private audioModes activeAudioMode;

    // MIXERS
    [Header("Mixers")]
    [SerializeField]
    private AudioMixer globalMixer;

    [Header("Music Audio Sources")]
    [SerializeField]
    private AudioSource musicIntroBeepsSource;
    [SerializeField]
    private AudioSource musicBeepsSource;
    [SerializeField]
    private AudioSource musicBit8Source;
    [SerializeField]
    private AudioSource musicHDSource;
    [SerializeField]
    private AudioSource musicHDEndSource;

    [Header("Music Snapshots")]
    [SerializeField]
    private AudioMixerSnapshot musicNone;
    [SerializeField]
    private AudioMixerSnapshot musicBeeps;
    [SerializeField]
    private AudioMixerSnapshot musicBit8;
    [SerializeField]
    private AudioMixerSnapshot musicHD;
    [SerializeField]
    private AudioMixerSnapshot musicHDEnd;

    private float musicCrossFadeDuration = 1.5f;

    [Header("Voice Over Music Snapshots")]
    [SerializeField]
    private AudioMixerSnapshot noVoiceOver;
    [SerializeField]
    private AudioMixerSnapshot voiceOver;

    [Header("Pause Music Snapshot")]
    [SerializeField]
    private AudioMixerSnapshot pauseModeMusic;

    private float voiceOverCrossFadeDuration = .7f;

    private int musicVolume;
    private int voicesVolume;
    private int sfxVolume;

    private void Start()
    {
        GM = this.GetComponent<GameManager>();

        GM.LoadFile("config");

        // set audio volumes
        musicVolume = GM.GetMusicVolume();
        voicesVolume = GM.GetVoicesVolume();
        sfxVolume = GM.GetSfxVolume();

        SetVolume("MusicVolume", musicVolume);
        SetVolume("VoicesVolume", voicesVolume);
        SetVolume("SfxVolume", sfxVolume);

        audioMode = audioModes.Beeps;
    }

    private void Update()
    {
        if (inGame == true)
        {
            // get audio mode
            activeAudioMode = (audioModes)System.Enum.Parse(typeof(audioModes), GM.GetAudioMode());
        }
        else
        {
            audioMode = audioModes.Beeps;
        }

        if (audioMode != activeAudioMode)
        {
            SetAudioMode(activeAudioMode.ToString());
        }

        // check if volume change
        if (musicVolume != GM.GetMusicVolume())
        {
            musicVolume = GM.GetMusicVolume();
            SetVolume("MusicVolume", musicVolume);
        }

        if (voicesVolume != GM.GetVoicesVolume())
        {
            voicesVolume = GM.GetVoicesVolume();
            SetVolume("VoicesVolume", voicesVolume);
        }

        if (sfxVolume != GM.GetSfxVolume())
        {
            sfxVolume = GM.GetSfxVolume();
            SetVolume("SfxVolume", sfxVolume);
        }
    }

    public void SetInGame(bool state)
    {
        inGame = state;
    }

    public void SetVolume(string param, float volume)
    {
        // volume conversion to match log formula
        if (volume == 0)
        {
            volume = 0.001f;
        }
        else
        {
            volume = volume / 5;
        }

        float newVolume = Mathf.Log(volume) * 20;
        globalMixer.SetFloat(param, newVolume);
    }

    public void SetAudioMode(string mode)
    {
        audioMode = (audioModes)System.Enum.Parse(typeof(audioModes), mode);
        MusicCrossFade();
        GM.SetAudioMode(mode);
    }

    public string GetAudioMode()
    {
        return audioMode.ToString();
    }

    public void PlayStopAllMusicSource(bool playState)
    {
        if (playState == true)
        {
            musicBeepsSource.Play();
            musicBit8Source.Play();
            musicHDSource.Play();
            musicHDEndSource.Play();
        }
        else
        {
            StartCoroutine(StopMusicCoroutine());
        }
    }

    private IEnumerator StopMusicCoroutine()
    {
        audioMode = audioModes.None;
        MusicCrossFade();

        yield return new WaitForSecondsRealtime(musicCrossFadeDuration);

        musicBeepsSource.Stop();
        musicBit8Source.Stop();
        musicHDSource.Stop();
        musicHDEndSource.Stop();
    }

    public void IntroMusicStart()
    {
        StartCoroutine(IntroMusicCoroutine());
    }

    private IEnumerator IntroMusicCoroutine()
    {
        audioMode = audioModes.Beeps;
        float introMusicLength = musicIntroBeepsSource.clip.length;

        musicIntroBeepsSource.Play();
        MusicCrossFade();
        yield return new WaitForSecondsRealtime(introMusicLength);

        PlayStopAllMusicSource(true);
    }

    private void MusicCrossFade()
    {
        switch (audioMode)
        {
            case audioModes.None:
                musicNone.TransitionTo(musicCrossFadeDuration);
                break;
            case audioModes.Bit8:
                musicBit8.TransitionTo(musicCrossFadeDuration);
                break;
            case audioModes.HD:
                musicHD.TransitionTo(musicCrossFadeDuration);
                break;
            case audioModes.HDEnd:
                musicHDEnd.TransitionTo(musicCrossFadeDuration);
                break;
            // case audioModes.Beeps:
            default:
                musicBeeps.TransitionTo(musicCrossFadeDuration);
                break;
        }
    }

    public void PauseModeMusicCrossFade(bool state)
    {
        if (state == true)
        {
            pauseModeMusic.TransitionTo(musicCrossFadeDuration);
        }
        else
        {
            noVoiceOver.TransitionTo(musicCrossFadeDuration);
        }
    }

    public void VoiceCrossFade(bool voiceOverState)
    {
        if (voiceOverState == true)
        {
            voiceOver.TransitionTo(voiceOverCrossFadeDuration);
        }
        else
        {
            noVoiceOver.TransitionTo(voiceOverCrossFadeDuration);
        }
    }
}

