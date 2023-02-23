using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private Slider volumeMasterSlider;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private TMP_Dropdown graphicsDropdown;

    public void Start()
    {
        if (MainManager.Instance != null)
        {
            volumeMasterSlider.value = MainManager.Instance.volumeMaster;
            graphicsDropdown.value = MainManager.Instance.qualityIndex;
        }
    }

    public void SetVolumeMaster(float volume)
    {
        audioMixer.SetFloat("volumeMaster", volume);
        MainManager.Instance.volumeMaster = volume;
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        MainManager.Instance.qualityIndex = qualityIndex;
    }
}