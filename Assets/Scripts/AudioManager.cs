using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] AudioClip[] punchSounds;
    [SerializeField] int minDamagePunchSound = 7;
    [SerializeField] int maxDamagePunchSound = 20;
    [SerializeField] float minVolumeFactor = 0.2f;

    [SerializeField] AudioClip menuMusic;
    [SerializeField] AudioClip fightMusic;
    [SerializeField] AudioClip bellSound;

    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource effectsSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }

        musicSource.volume = GetMusicVolume();
        effectsSource.volume = GetEffectsVolume();
    }

    public void PlayMenuBGM()
    {
        musicSource.clip = menuMusic;
        musicSource.Play();
    }

    public void PlayFightBGM()
    {
        musicSource.clip = fightMusic;
        musicSource.Play();
    }

    public void PlayPunchSound(int dmg)
    {
        var sound = punchSounds[Random.Range(0, punchSounds.Length)];
        var volume = Mathf.Clamp(Mathf.InverseLerp(minDamagePunchSound, maxDamagePunchSound, Mathf.Max(dmg, minDamagePunchSound)), minVolumeFactor, 1f);
        effectsSource.PlayOneShot(sound, volume);
    }
    
    public void PlayBellSound()
    {
        effectsSource.PlayOneShot(bellSound);
    }

    public float GetMusicVolume()
    {
        return PlayerPrefs.GetFloat("musicVolume", 1f);
    }

    public float GetEffectsVolume()
    {
        return PlayerPrefs.GetFloat("effectsVolume", 1f);
    }

    public void SetMusicVolume(float volume)
    {
        volume = Mathf.Clamp01(volume);
        musicSource.volume = volume;
        PlayerPrefs.SetFloat("musicVolume", volume);
    }
    public void SetEffectsVolume(float volume)
    {
        volume = Mathf.Clamp01(volume);
        effectsSource.volume = volume;
        PlayerPrefs.SetFloat("effectsVolume", volume);
    }
}
