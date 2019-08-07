using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Networking;


public class GameSettingManager : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Button fullScreenButton;

    private AudioSource audioSource;
    [SerializeField] private List<Button> applyButtons;

    public GameSettings gameSettings;

    private IEnumerator checkRoutine;
    private IEnumerator saveRoutine;


    private void OnEnable()
    {
        volumeSlider.onValueChanged.RemoveAllListeners();
        volumeSlider.onValueChanged.AddListener(delegate
        {
            OnVolumeChange();
        });

        foreach (Button applyButton in applyButtons)
        {
            applyButton.onClick.RemoveAllListeners();
            applyButton.onClick.AddListener(delegate
            {
                StartSaveSettingsWebFile(gameSettings);
            });

            //Debug.Log(applyButton.name + " LISTENER");
        }

        audioSource = FindObjectOfType<AudioSource>();
    }

    public void StartCheckWebSettingsFileRoutine()
    {
        checkRoutine = CheckWebSettingsFileRoutine();
        StartCoroutine(checkRoutine);
    }

    private IEnumerator CheckWebSettingsFileRoutine()
    {
        string address = Application.absoluteURL == string.Empty
            ? StringDb.serverAddressEditor
            : StringDb.serverAddress;

        WWWForm form = new WWWForm();

        form.AddField("mode", "r");
        form.AddField("mainDataFolder", StringDb.mainDataFolder);
        form.AddField("playerFolder", StringDb.player.folderName);
        form.AddField("settingsFolder", StringDb.settingsWebFolderPath);
        form.AddField("settingFileName", StringDb.settingName + StringDb.settingExt);

        using (UnityWebRequest www =
            UnityWebRequest.Post(
                Path.Combine(address,
                    Path.Combine(StringDb.phpFolder, StringDb.playerSettingsManagerScript)), form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                //Debug.Log(www.downloadHandler.text);

                if (www.downloadHandler.text == "Error Reading File")
                {
                    //File not existing, need to create the file
                    gameSettings = CreateSettingsWebValues();
                }
                else
                {
                    //Get values to create game settings
                    string jsonData = www.downloadHandler.text;
                    gameSettings = LoadSettingsWebFile(jsonData);
                }

                if (gameSettings != null)
                {
                    ApplySettingsWebValues(gameSettings);
                }
            }
        }
    }

    private GameSettings CreateSettingsWebValues()
    {
        try
        {
            GameSettings settings = new GameSettings(false, 0.8f);
            StartSaveSettingsWebFile(settings);

            return settings;
        }
        catch (Exception e)
        {
            Debug.Log("CANNOT CREATE SETTINGS: " + e);
            return null;
        }
    }

    private GameSettings LoadSettingsWebFile(string jsonData)
    {
        try
        {
            GameSettings settings = JsonUtility.FromJson<GameSettings>(jsonData);

            return settings;
        }
        catch (Exception e)
        {
            Debug.Log("CANNOT LOAD SETTINGS: " + e);

            return null;
        }
    }

    private void ApplySettingsWebValues(GameSettings settings)
    {
        audioSource.volume = settings.volume;
        Screen.fullScreen = settings.fullScreen;

        SetGuiElement(settings);
    }

    public void SetGuiElement(GameSettings settings)
    {
        GetSpriteFromBool(settings.fullScreen);
        volumeSlider.value = settings.volume;
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

    private bool GetBoolFromSprite()
    {
        //CHECK WHICH SPRITE IS CURRENTLY DISPLAYED; SET BOOL
        return fullScreenButton.image.sprite.name.Contains(StringDb.fsButtonOff);
    }

    public void OnFullScreenChange()
    {
        bool isOn = GetBoolFromSprite();
        Screen.fullScreen = isOn;
        gameSettings.fullScreen = isOn;

        //Debug.Log(isOn);
    }

    public void OnVolumeChange()
    {
        float value = volumeSlider.value;
        audioSource.volume = value;
        gameSettings.volume = value;
    }

    public void StartSaveSettingsWebFile(GameSettings settings)
    {
        saveRoutine = SaveSettingsWebFile(settings);
        StartCoroutine(saveRoutine);
    }

    public IEnumerator SaveSettingsWebFile(GameSettings settings)
    {
        string address = Application.absoluteURL == string.Empty
            ? StringDb.serverAddressEditor
            : StringDb.serverAddress;

        string jsonData = JsonUtility.ToJson(settings, true);

        WWWForm form = new WWWForm();

        form.AddField("mode", "w");

        form.AddField("mainDataFolder", StringDb.mainDataFolder);
        form.AddField("playerFolder", StringDb.player.folderName);
        form.AddField("settingsFolder", StringDb.settingsWebFolderPath);
        form.AddField("settingFileName", StringDb.settingName + StringDb.settingExt);
        form.AddField("settingsContent", jsonData);


        using (UnityWebRequest www =
            UnityWebRequest.Post(
                Path.Combine(address,
                    Path.Combine(StringDb.phpFolder, StringDb.playerSettingsManagerScript)), form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
            }
        }
    }
}