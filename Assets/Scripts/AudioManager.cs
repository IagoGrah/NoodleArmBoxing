using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] AudioClip[] punchSounds;
    [SerializeField] int minDamagePunchSound = 7;
    [SerializeField] int maxDamagePunchSound = 20;
    [SerializeField] float minVolumeFactor = 0.2f;

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
        source.PlayOneShot(sound, volume);
    }
}
