using UnityEngine;
using UnityEngine.UI;

public enum SliderSoundType
{
    MUSIC_VOLUME,
    SFX_VOLUME
}

public class VolumeController : MonoBehaviour
{
    [SerializeField] private SliderSoundType soundType;
    [SerializeField] Slider musicVolumeSlider;

    public static bool musicVolumeChanged = false;
    public static bool sfxVolumeChanged = false;

    void Start()
    {
        if(!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 0.15f);
            load();
            
        }
        else
        {
            load();
        }
    }

    private void load()
    {
        musicVolumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
        musicVolumeChanged = true;
    }

    public void changeVolume()
    {
        switch (soundType)
        {
            case SliderSoundType.MUSIC_VOLUME:
                PlayerPrefs.SetFloat("musicVolume", musicVolumeSlider.value);
                musicVolumeChanged = true;
                break;
            case SliderSoundType.SFX_VOLUME:
                PlayerPrefs.SetFloat("sfxVolume", 0);
                sfxVolumeChanged = true;
                break;
            default:
                break;
        }
    }
}