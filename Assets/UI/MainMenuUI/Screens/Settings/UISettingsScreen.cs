using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISettingsScreen : UIScreen
{

    public Slider volumeSlider;

    public VolumeManager volumeManager;

    private float volumeLevel;
    public override void OnAppear()
    {
        Debug.Log(PlayerPrefs.GetFloat("volume", 0.5f));
        base.OnAppear();

        volumeSlider.value = PlayerPrefs.GetFloat("volume", 0.5f);
        volumeLevel = volumeSlider.value;
        volumeSlider.onValueChanged.AddListener((value) =>
        {
            volumeLevel = value;
            PlayerPrefs.SetFloat("volume", volumeLevel);
        });
    }

    public override void OnDisappear()
    {
        Debug.Log("Saved volumes in player prefs");
        PlayerPrefs.Save();
        Debug.Log($"Global volume set to {volumeLevel}");
        volumeManager.SetGlobalVolume(volumeLevel);
        base.OnDisappear();
    }
}
