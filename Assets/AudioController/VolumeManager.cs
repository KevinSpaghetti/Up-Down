using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeManager : MonoBehaviour
{
    public void SetGlobalVolume(float volume)
    {
        AudioListener.volume = volume;
    }
}
