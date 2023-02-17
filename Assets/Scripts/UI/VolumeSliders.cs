using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSliders : MonoBehaviour
{
    [SerializeField] TMP_Text musicVolumeLabel;
    [SerializeField] Slider musicVolumeSlider;

    [SerializeField] TMP_Text effectsVolumeLabel;
    [SerializeField] Slider effectsVolumeSlider;

    private void Start()
    {
        musicVolumeSlider.value = AudioManager.Instance.GetMusicVolume();
        effectsVolumeSlider.value = AudioManager.Instance.GetEffectsVolume();
        UpdateLabels();
    }

    public void SetMusicVolume(float volume)
    {
        AudioManager.Instance.SetMusicVolume(volume);
        UpdateLabels();
    }

    public void SetEffectsVolume(float volume)
    {
        AudioManager.Instance.SetEffectsVolume(volume);
        UpdateLabels();
    }

    private void UpdateLabels()
    {
        musicVolumeLabel.text = $"Music Volume: {Mathf.Round(AudioManager.Instance.GetMusicVolume() * 100)}%";
        effectsVolumeLabel.text = $"Effects Volume: {Mathf.Round(AudioManager.Instance.GetEffectsVolume() * 100)}%";
    }
}
