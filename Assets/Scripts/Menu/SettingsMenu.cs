using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    #region PlayerPref Keys

    public const string MasterVolumeKey = "Settings.Master.Volume";
    public const string MusicVolumeKey = "Settings.Volumes.Music";
    public const string SfxVolumeKey = "Settings.Volumes.SFX";

    
    #endregion

    #region Default Values

    public const float DefaultMasterVolume = 0.5f;
    public const float DefaultMusicVolume = 1f;
    public const float DefaultSfxVolume = 1f;

    
    
    #endregion
    
    #region Inspector

    [Header("Volume")]
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;


    #endregion

    #region Unity Event Functions

    private void Awake()
    {
        // Initialize volumes
        Initialize(masterVolumeSlider, MasterVolumeKey, DefaultMasterVolume);
        Initialize(musicVolumeSlider, MusicVolumeKey, DefaultMusicVolume);
        Initialize(sfxVolumeSlider, SfxVolumeKey, DefaultSfxVolume);

    }
    
    #endregion

    private void Initialize(Slider slider, string key, float defaultValue)
    {
        slider.SetValueWithoutNotify(PlayerPrefs.GetFloat(key, defaultValue));
        slider.onValueChanged.AddListener(
            (float sliderValue) =>
            {
                PlayerPrefs.SetFloat(key, sliderValue);
            }
            );
    }
    
    

    private void Initialize(Toggle toggle, string key, bool defaultValue)
    {
        toggle.SetIsOnWithoutNotify(GetBool(key, defaultValue));
        toggle.onValueChanged.AddListener(
            (bool toggleValue) =>
            {
                SetBool(key, toggleValue);
            }
        );
    }

    #region PlayerPrefs

    public static void SetBool(string key, bool value)
    {
        int intValue = value ? 1 : 0;
        PlayerPrefs.SetInt(key, intValue);
    }

    public static bool GetBool(string key, bool defaultValue = false)
    {
        int defaultIntValue = defaultValue ? 1 : 0;
        int intValue = PlayerPrefs.GetInt(key, defaultIntValue);
        return intValue != 0;
    }

    #endregion
    
}
