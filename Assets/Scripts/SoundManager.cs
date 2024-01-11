using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.Rendering;

public class SoundManager : MonoBehaviour
{
    private const string PLAYER_PREFS_SOUND_VOLUME = "SoundVolume";

    public static SoundManager Instance { get; private set; }

    public enum Sound
    {
        BuildingDamaged,
        BuildingDestroyed,
        BuildingPlaced,
        EnemyDie,
        EnemyHit,
        EnemyWaveStarting,
        GameOver,
    }

    private AudioSource audioSource;
    private Dictionary<Sound, AudioClip> soundAudioClipDictionary;
    private float volume = .5f;

    private void Awake()
    {
        Instance = this;

        audioSource = GetComponent<AudioSource>();

        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_VOLUME, .5f);

        soundAudioClipDictionary = new Dictionary<Sound, AudioClip>();
        foreach(Sound sound in System.Enum.GetValues(typeof(Sound)))
        {
            soundAudioClipDictionary[sound] = Resources.Load<AudioClip>(sound.ToString());
        }
    }

    public void PlaySound(Sound sound)
    {
        audioSource.PlayOneShot(soundAudioClipDictionary[sound], volume);
    }

    public void IncreaseVolume()
    {
        volume += .1f;
        volume = Mathf.Clamp01(volume);

        PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_VOLUME, volume);
    }

    public void DecreaseVolume()
    {
        volume -= .1f;
        volume = Mathf.Clamp01(volume);

        PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_VOLUME, volume);
    }

    public float GetVolume() => volume;
}
