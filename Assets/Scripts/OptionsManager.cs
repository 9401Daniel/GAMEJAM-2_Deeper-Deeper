using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    [SerializeField] private Slider masterVolume;
    [SerializeField] private Slider musicVolume;
    [SerializeField] private Slider sfxVolume;

    void OnEnable()
    {
        masterVolume.SetValueWithoutNotify(PlayerPrefs.GetFloat("MasterVolume", 1f));
        musicVolume.SetValueWithoutNotify(PlayerPrefs.GetFloat("MusicVolume", 1f));
        sfxVolume.SetValueWithoutNotify(PlayerPrefs.GetFloat("SfxVolume", 1f));
    }

    public void OnMasterVolumeChanged(float value) => AudioManager.Instance.SetMasterVolume(value);
    public void OnMusicVolumeChanged(float value)  => AudioManager.Instance.SetMusicVolume(value);
    public void OnSfxVolumeChanged(float value)    => AudioManager.Instance.SetSfxVolume(value);

    public void OnBackPressed()
    {
        PlayerPrefs.Save();
        UIManager.Instance.ShowMainMenu();
    }
}