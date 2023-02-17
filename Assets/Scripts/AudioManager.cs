using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] AudioClip[] punchSounds;
    [SerializeField] int minDamagePunchSound = 7;
    [SerializeField] int maxDamagePunchSound = 20;
    [SerializeField] float minVolumeFactor = 0.2f;

    private float settingsMusicVolume = 1f;
    private float settingsEffectsVolume = 1f;

    private AudioSource source;

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

        source = GetComponent<AudioSource>();
    }

    public void PlayPunchSound(int dmg)
    {
        var sound = punchSounds[Random.Range(0, punchSounds.Length)];
        var volume = Mathf.Clamp(Mathf.InverseLerp(minDamagePunchSound, maxDamagePunchSound, Mathf.Max(dmg, minDamagePunchSound)), minVolumeFactor, 1f);
        source.PlayOneShot(sound, volume * settingsEffectsVolume);
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
        settingsMusicVolume = volume;
        PlayerPrefs.SetFloat("musicVolume", volume);
    }
    public void SetEffectsVolume(float volume)
    {
        volume = Mathf.Clamp01(volume);
        settingsEffectsVolume = volume;
        PlayerPrefs.SetFloat("effectsVolume", volume);
    }
}
