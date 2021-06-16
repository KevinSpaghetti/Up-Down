using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISettingsScreen : UIScreen
{

    public Slider volumeSlider;

    public override void OnAppear()
    {
        Debug.Log(PlayerPrefs.GetFloat("volume", 0.5f));
        base.OnAppear();

        volumeSlider.value = PlayerPrefs.GetFloat("volume", 0.5f);
        volumeSlider.onValueChanged.AddListener((value) => { PlayerPrefs.SetFloat("volume", value); });
    }

    public override void OnDisappear()
    {
        Debug.Log("Saved volumes in player prefs");
        PlayerPrefs.Save();
        base.OnDisappear();
    }
}
