using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider efxVolumeSlider;
    [SerializeField] private Dropdown graphicDropdown;
    [SerializeField] private AudioSource menuAudioSource;

    private void Awake()
    {
        musicVolumeSlider.value = SaveGame.GetMusicVolume();
        efxVolumeSlider.value = SaveGame.GetEffectsVolume();
        graphicDropdown.value = SaveGame.GetGraphicQuality();
        QualitySettings.SetQualityLevel(graphicDropdown.value);
    }

    public void SaveGraphicQuality(int qualityIndex)
    {
        Debug.Log("Ciao!");
        QualitySettings.SetQualityLevel(qualityIndex);
        SaveGame.SaveGraphicQuality(qualityIndex);
    }

    public void SaveMusicVolume()
    {
        menuAudioSource.volume = musicVolumeSlider.value;
        SaveGame.SaveMusicVolume(musicVolumeSlider.value);
    }

    public void SaveEffectsVolume()
    {
        SaveGame.SaveEffectsVolume(efxVolumeSlider.value);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("MainLevel");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
