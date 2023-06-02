using FMOD.Studio;

using FMODUnity;

using UnityEngine;

public class VolumeController : MonoBehaviour
{
    private VCA master;
    private VCA music;
    private VCA sfx;

    #region Unity Event Functions

    private void Awake()
    {
        master = RuntimeManager.GetVCA("vca:/Master");
        music = RuntimeManager.GetVCA("vca:/Music");
        sfx = RuntimeManager.GetVCA("vca:/SFX");

    }

    private void Update()
    {
        master.setVolume(PlayerPrefs.GetFloat(SettingsMenu.MasterVolumeKey, SettingsMenu.DefaultMasterVolume));
        music.setVolume(PlayerPrefs.GetFloat(SettingsMenu.MusicVolumeKey, SettingsMenu.DefaultMusicVolume));
        sfx.setVolume(PlayerPrefs.GetFloat(SettingsMenu.SfxVolumeKey, SettingsMenu.DefaultSfxVolume));
        
    }

    #endregion
}
