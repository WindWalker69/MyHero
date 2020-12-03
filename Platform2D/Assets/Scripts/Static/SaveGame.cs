using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveGame
{
    public static void SaveGraphicQuality(int qualityIndex)
    {
        PlayerPrefs.SetInt("QUALITY", qualityIndex);
        PlayerPrefs.Save();
    }

    public static void SaveMusicVolume(float volume)
    {
        PlayerPrefs.SetFloat("MUSIC", volume);
        PlayerPrefs.Save();
    }

    public static void SaveEffectsVolume(float volume)
    {
        PlayerPrefs.SetFloat("EFFECTS", volume);
        PlayerPrefs.Save();
    }

    public static int GetGraphicQuality()
    {
        return PlayerPrefs.GetInt("QUALITY");
    }

    public static float GetMusicVolume()
    {
        return PlayerPrefs.GetFloat("MUSIC");
    }

    public static float GetEffectsVolume()
    {
        return PlayerPrefs.GetFloat("EFFECTS");
    }
}
