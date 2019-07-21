using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class OnScreenSettingManager : MonoBehaviour
{
    private Toggle fullScreenToggle;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private TextMeshProUGUI resolutionDropdownLabel;
    private Slider volumeSlider;
    private AudioSource gameAudioSource;

    private void OnEnable()
    {
        //get UI component reference
        fullScreenToggle = GameObject.Find(StringDb.pauseToggle).GetComponent<Toggle>();
        volumeSlider = GameObject.Find(StringDb.pauseSlider).GetComponent<Slider>();
        //get audio reference from settings
        gameAudioSource = SettingsManager.audioSource;

        //set the listener on the GUI component
        fullScreenToggle.onValueChanged.RemoveAllListeners();
        fullScreenToggle.onValueChanged.AddListener(delegate { OnFullScreenToggle(); });

        volumeSlider.onValueChanged.RemoveAllListeners();
        volumeSlider.onValueChanged.AddListener(delegate { OnVolumeChange(); });

        //create setting class for store the values
        PauseManager.pauseEnabled = true;

        SetGuiElement();


        //DEBUG
        //-------------------------------------------------------------------------------------------------------------------
#if UNITY_WEBGL

        resolutionDropdownLabel.gameObject.SetActive(false);
        resolutionDropdown.gameObject.SetActive(false);

#else

        resolutionDropdownLabel.gameObject.SetActive(true);
        resolutionDropdown.gameObject.SetActive(true);

        resolutionDropdown.onValueChanged.RemoveAllListeners();
        resolutionDropdown.onValueChanged.AddListener(delegate { OnResolutionChange(); });

        FillDropDownResolution();

#endif
        //-------------------------------------------------------------------------------------------------------------------

    }
    private void OnDisable()
    {
        PauseManager.pauseEnabled = false;
    }
    private void SetGuiElement()
    { 
        fullScreenToggle.isOn = SettingsManager.gameSettings.fullScreen;
        resolutionDropdown.value = SettingsManager.gameSettings.resolutionIndex;
        volumeSlider.value = SettingsManager.gameSettings.volume;
    }
    public void OnVolumeChange()
    {
        float value = volumeSlider.value;
        gameAudioSource.volume = value;
        SettingsManager.gameSettings.volume = value;
    }
    public void OnFullScreenToggle()
    {
        bool isOn = fullScreenToggle.isOn;
        Screen.fullScreen = isOn;
        SettingsManager.gameSettings.fullScreen = isOn;
    }
    public void OnResolutionChange()
    {
        Screen.SetResolution(SettingsManager.resolutions[resolutionDropdown.value].width, SettingsManager.resolutions[resolutionDropdown.value].height, SettingsManager.gameSettings.fullScreen);
        SettingsManager.gameSettings.resolutionIndex = resolutionDropdown.value;
    }
    public void FillDropDownResolution()
    {
        //resolutions = SettingsManager.resolutions;
        foreach (Resolution resolution in SettingsManager.resolutions)
        {
            resolutionDropdown.options.Add(new TMP_Dropdown.OptionData(resolution.ToString().Replace(" ", string.Empty)));
        }
    }
}
