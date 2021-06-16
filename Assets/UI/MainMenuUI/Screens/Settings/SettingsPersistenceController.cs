using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsPersistenceController : MonoBehaviour
{
    public float volume;

    public void SetVolume(float vol)
    {
        volume = vol;
    }
    
    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("volume", volume);
    }

    public void LoadSettings()
    {
        volume = PlayerPrefs.GetFloat("volume", 0.5f);
    }
    
    
}
