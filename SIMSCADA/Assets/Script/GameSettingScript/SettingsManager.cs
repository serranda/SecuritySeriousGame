using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsManager: MonoBehaviour
{    
    private Canvas pauseMenu;
    private PrefabManager prefabManager;
    public static GameSettings gameSettings;

    public static Resolution[] resolutions;
    public static AudioSource audioSource;

    public void Awake()
    {
        gameSettings = new GameSettings();

        GetAudioAndResValues();

        if (File.Exists(StringDb.settingsFilePath))
        {
            LoadSettings();
        }
        else
        {
            CreateSettings();
            //the actual settings file doesn't exist, we have to create it
            SaveSettings();
        }

        ApplySettings(gameSettings);
    }
    private void LoadSettings()
    {
        try
        {
            string jsonData = File.ReadAllText(StringDb.settingsFilePath);
            gameSettings = JsonUtility.FromJson<GameSettings>(jsonData);
            //Debug.Log("SETTINGS LOADED\n" + gameSettings.printSettings());

        }
        catch (Exception e)
        {
            Debug.Log("CANNOT LOAD SETTINGS: " + e);
        }

    }
    private void CreateSettings()
    {
        try
        {
            gameSettings.fullScreen = true;
            gameSettings.resolutionIndex = Screen.resolutions.Length - 1;
            gameSettings.volume = 0.8f;
            //Debug.Log("SETTINGS CREATED\n" + gameSettings.printSettings());
        }
        catch (Exception e)
        {
            Debug.Log("CANNOT CREATE SETTINGS: " + e);
        }

    }
    public void SaveByBtn()
    {
        SaveSettings();
    }
    public void SaveSettings()
    {
        try
        {
            string jsonData = JsonUtility.ToJson(gameSettings, true);
            File.WriteAllText(StringDb.settingsFilePath, jsonData);
            //Debug.Log("SETTINGS SAVED\n" + gameSettings.printSettings());
        }
        catch (DirectoryNotFoundException dirExc)
        {
            Debug.Log("CANNOT SAVE SETTINGS: " + dirExc);
            Directory.CreateDirectory(StringDb.settingsFolderPath);
            SaveSettings();
        }
        catch (Exception e)
        {
            Debug.Log("CANNOT SAVE SETTINGS: " + e);
        }
    }
    private void ApplySettings(GameSettings settings)
    {
        audioSource.volume = settings.volume;
        Screen.fullScreen = settings.fullScreen;
        Screen.SetResolution(resolutions[settings.resolutionIndex].width, resolutions[settings.resolutionIndex].height, settings.fullScreen);
    }
    private void GetAudioAndResValues()
    {
        try
        {
            audioSource = GameObject.Find(StringDb.gameAudio[SceneManager.GetActiveScene().buildIndex])
                .GetComponent<AudioSource>();
        }
        catch (IndexOutOfRangeException)
        {
            audioSource = GameObject.Find(StringDb.gameAudio[1]).GetComponent<AudioSource>();
        }
        catch (Exception exception)
        {
            Debug.Log(exception.Message);
        }
        resolutions = Screen.resolutions;
    }
}

