using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;


public class GUISettingManager : MonoBehaviour
{
    //private Toggle fullScreenToggle;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private TextMeshProUGUI resolutionDropdownLabel;

    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Button fullScreenButton;


    private void OnEnable()
    {

        volumeSlider.onValueChanged.RemoveAllListeners();
        volumeSlider.onValueChanged.AddListener(delegate { OnVolumeChange(); });

        PauseManager.pauseEnabled = true;

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

    public void SetGuiElement()
    {
        GetSpriteFromBool(SettingsManager.gameSettings.fullScreen);
        resolutionDropdown.value = SettingsManager.gameSettings.resolutionIndex;
        volumeSlider.value = SettingsManager.gameSettings.volume;
    }

    public void OnFullScreenChange()
    {
        bool isOn = GetBoolFromSprite();
        Screen.fullScreen = isOn;
        SettingsManager.gameSettings.fullScreen = isOn;

        Debug.Log(isOn);
    }

    private bool GetBoolFromSprite()
    {
        //CHECK WHICH SPRITE IS CURRENTLY DISPLAYED; SET BOOL
        return fullScreenButton.image.sprite.name.Contains(StringDb.fsButtonOff);
    }

    public void GetSpriteFromBool(bool isOn)
    {
        if (isOn)
        {
            Sprite[] sprites = Resources.LoadAll<Sprite>(Path.Combine(StringDb.fsButtonFolder, StringDb.fsButtonOn));
            fullScreenButton.image.sprite = sprites[0];

            SpriteState buttonSpriteState = new SpriteState
            {
                highlightedSprite = sprites[1],
                pressedSprite = sprites[2],
                disabledSprite = sprites[3]
            };

            fullScreenButton.spriteState = buttonSpriteState;
        }
        else
        {
            Sprite[] sprites = Resources.LoadAll<Sprite>(Path.Combine(StringDb.fsButtonFolder, StringDb.fsButtonOff));
            fullScreenButton.image.sprite = sprites[0];

            SpriteState buttonSpriteState = new SpriteState
            {
                highlightedSprite = sprites[1],
                pressedSprite = sprites[2],
                disabledSprite = sprites[3]
            };

            fullScreenButton.spriteState = buttonSpriteState;
        }
    }


    public void OnVolumeChange()
    {
        float value = volumeSlider.value;
        ClassDb.settingsManager.audioSource.volume = value;
        SettingsManager.gameSettings.volume = value;
    }

    public void OnResolutionChange()
    {
        Screen.SetResolution(ClassDb.settingsManager.resolutions[resolutionDropdown.value].width, ClassDb.settingsManager.resolutions[resolutionDropdown.value].height, SettingsManager.gameSettings.fullScreen);
        SettingsManager.gameSettings.resolutionIndex = resolutionDropdown.value;
    }

    public void FillDropDownResolution()
    {
        //resolutions = SettingsManager.resolutions;
        foreach (Resolution resolution in ClassDb.settingsManager.resolutions)
        {
            resolutionDropdown.options.Add(new TMP_Dropdown.OptionData(resolution.ToString().Replace(" ", string.Empty)));
        }
    }
}
