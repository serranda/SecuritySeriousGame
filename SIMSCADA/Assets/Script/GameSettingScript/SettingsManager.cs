using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class SettingsManager: MonoBehaviour
{    
    private Canvas pauseMenu;
    private PrefabManager prefabManager;

    public Resolution[] resolutions;
    public AudioSource audioSource;

    public static GameSettings gameSettings;

    [SerializeField] private GameSettings settingsRef;


    public void Awake()
    {

#if UNITY_WEBGL

        StartCoroutine(CheckWebSettingsFileRoutine());

#else
        GetResolutionValues();

        CheckLocalSettingsFile();
#endif
    }

    private void Update()
    {
        settingsRef = gameSettings;
    }

    //UNITY WEBGL METHOD
    //-----------------------------------------------------------------------------------

    private IEnumerator CheckWebSettingsFileRoutine()
    {

        WWWForm form = new WWWForm();

        form.AddField("mode", "r");
        form.AddField("folderName", StringDb.player.folderName);
        form.AddField("settingsFolder", StringDb.settingsWebFolderPath);
        form.AddField("settingFileName", StringDb.settingName + StringDb.settingExt);

        using (UnityWebRequest www =
            UnityWebRequest.Post(Path.Combine(StringDb.serverAddress, Path.Combine(StringDb.phpFolder, StringDb.settingsFileManagerScript)), form))
        {


            yield return www.SendWebRequest();  

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                GameSettings settings;

                //Debug.Log(www.downloadHandler.text);

                if (www.downloadHandler.text == "Error Reading File")
                {
                    //File not existing, need to create the file
                    settings = CreateSettingsWebValues();
                }
                else
                {
                    //Get values to create game settings
                    string jsonData = www.downloadHandler.text;
                    settings = LoadSettingsWebFile(jsonData);
                }

                if (settings != null)
                    ApplySettingsWebValues(settings);
            }
        }
    }

    private GameSettings CreateSettingsWebValues()
    {
        try
        {
            GameSettings settings = new GameSettings(false, 0.8f);
            //Debug.Log("SETTINGS CREATED\n" + gameSettings.printSettings());
            StartCoroutine(SaveSettingsWebFile(settings));

            return settings;

        }
        catch (Exception e)
        {
            Debug.Log("CANNOT CREATE SETTINGS: " + e);
            return null;

        }
    }

    public IEnumerator SaveSettingsWebFile(GameSettings settings)
    {
        string jsonData = JsonUtility.ToJson(settings, true);

        WWWForm form = new WWWForm();

        form.AddField("mode", "w");

        form.AddField("folderName", StringDb.player.folderName);
        form.AddField("settingsFolder", StringDb.settingsWebFolderPath);
        form.AddField("settingFileName", StringDb.settingName + StringDb.settingExt);
        form.AddField("settingsContent", jsonData);


        using (UnityWebRequest www =
            UnityWebRequest.Post(Path.Combine(StringDb.serverAddress, Path.Combine(StringDb.phpFolder, StringDb.settingsFileManagerScript)), form))
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

    private GameSettings LoadSettingsWebFile(string jsonData)
    {
        try
        {
            GameSettings settings = JsonUtility.FromJson<GameSettings>(jsonData);
            //Debug.Log("SETTINGS LOADED\n" + gameSettings.printSettings());

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

        gameSettings = settings;
    }

    //-----------------------------------------------------------------------------------


    //UNITY WIN METHOD
    //-----------------------------------------------------------------------------------

    private void GetResolutionValues()
    {
        resolutions = Screen.resolutions;
    }

    private void CheckLocalSettingsFile()
    {
        GameSettings settings = !File.Exists(StringDb.settingsLocalFilePath) ? 
            CreateSettingsLocalValues() : 
            LoadSettingsLocalFile();

        if(settings != null)
            ApplySettingsLocalValues(settings);
    }

    private GameSettings CreateSettingsLocalValues()
    {
        try
        {
            GameSettings settings = new GameSettings(true, 0.8f, Screen.resolutions.Length - 1);

            //Debug.Log("SETTINGS CREATED\n" + gameSettings.printSettings());

            //the actual settings file doesn't exist, we have to create it
            SaveSettingsLocalFile(settings);

            return settings;
        }
        catch (Exception e)
        {
            Debug.Log("CANNOT CREATE SETTINGS: " + e);

            return null;
        }

    }

    public void SaveSettingsLocalFile(GameSettings settings)
    {
        try
        {
            string jsonData = JsonUtility.ToJson(settings, true);
            File.WriteAllText(StringDb.settingsWebFilePath, jsonData);
            //Debug.Log("SETTINGS SAVED\n" + gameSettings.printSettings());
        }
        catch (DirectoryNotFoundException dirExc)
        {
            Debug.Log("CANNOT SAVE SETTINGS: " + dirExc);
            Directory.CreateDirectory(StringDb.settingsWebFolderPath);
            SaveSettingsLocalFile(settings);
        }
        catch (Exception e)
        {
            Debug.Log("CANNOT SAVE SETTINGS: " + e);
        }
    }

    private GameSettings LoadSettingsLocalFile()
    {
        try
        {

            string jsonData = File.ReadAllText(StringDb.settingsWebFilePath);
            GameSettings settings = JsonUtility.FromJson<GameSettings>(jsonData);
            //Debug.Log("SETTINGS LOADED\n" + gameSettings.printSettings());

            return settings;
        }
        catch (Exception e)
        {
            Debug.Log("CANNOT LOAD SETTINGS: " + e);

            return null;
        }

    }

    private void ApplySettingsLocalValues(GameSettings settings)
    {
        audioSource.volume = settings.volume;
        Screen.fullScreen = settings.fullScreen;
        Screen.SetResolution(resolutions[settings.resolutionIndex].width, resolutions[settings.resolutionIndex].height, settings.fullScreen);

        gameSettings = settings;
    }

    //-----------------------------------------------------------------------------------
    
    public void SaveByBtn()
    {
#if UNITY_WEBGL

        StartCoroutine(SaveSettingsWebFile(gameSettings));

#else

        SaveSettingsLocalFile(gameSettings));

#endif
    }
}

