using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    [SerializeField] private Slider masterVolume;
    [SerializeField] private Slider musicVolume;
    [SerializeField] private Slider sfxVolume;

    void OnEnable()
    {
        float master = PlayerPrefs.GetFloat("MasterVolume",1f);
        float music = PlayerPrefs.GetFloat("MusicVolume",1f);
        float sfx = PlayerPrefs.GetFloat("SfxVolume",1f);

        masterVolume.SetValueWithoutNotify(master);
        musicVolume.SetValueWithoutNotify(music);
        sfxVolume.SetValueWithoutNotify(sfx);

        AudioManager.Instance.SetMasterVolume(master);
        AudioManager.Instance.SetMusicVolume(music);
        AudioManager.Instance.SetSfxVolume(sfx);
    }

    public void OnMasterVolumeChanged(float value)
    {
        AudioManager.Instance.SetMasterVolume(value);
    }
    public void OnMusicVolumeChanged(float value)
    {
        AudioManager.Instance.SetMusicVolume(value);
    }
    public void OnSfxVolumeChanged(float value)
    {
        AudioManager.Instance.SetSfxVolume(value);
    }


    public void OnBackPressed()
    {
        PlayerPrefs.Save();
        UIManager.Instance.ShowMainMenu();
    }
}