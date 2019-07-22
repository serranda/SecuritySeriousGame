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

    public static Resolution[] resolutions;
    public static AudioSource audioSource;

    public static GameSettings gameSettings;

    [SerializeField] private GameSettings settingsRef;


    public void Awake()
    {

#if UNITY_WEBGL

        GetAudioValues();

        StartCoroutine(CheckWebSettingsFileRoutine());

#else
        GetAudioAndResValues();

        CheckLocalSettingsFile();
#endif
    }

    private void Update()
    {
        settingsRef = gameSettings;
    }

    //UNITY WEBGL METHOD
    //-----------------------------------------------------------------------------------

    private void GetAudioValues()
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
    }

    private IEnumerator CheckWebSettingsFileRoutine()
    {
        GameSettings settings = new GameSettings();

        WWWForm form = new WWWForm();

        form.AddField("mode", "r");
        form.AddField("folderName", StringDb.playerFolderName);
        form.AddField("settingsFolder", StringDb.settingsWebFolderPath);
        form.AddField("settingFileName", StringDb.settingName + StringDb.settingExt);

        using (UnityWebRequest www =
            UnityWebRequest.Post(Path.Combine(StringDb.serverAddress, Path.Combine(StringDb.phpFolder, StringDb.settingsFileManagerScript)), form))
        {
           

            yield return www.SendWebRequest(); ; 

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);

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
            }
        }

        if (settings != null)
            ApplySettingsWebValues(settings);

    }

    private GameSettings CreateSettingsWebValues()
    {
        try
        {
            GameSettings settings = new GameSettings()
            {
                fullScreen = false,
                volume = 0.8f
            };
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

        form.AddField("folderName", StringDb.playerFolderName);
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

        //TODO SET STATIC VALUES
        gameSettings = settings;
    }

    //-----------------------------------------------------------------------------------


    //UNITY WIN METHOD
    //-----------------------------------------------------------------------------------

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
            GameSettings settings = new GameSettings
            {
                fullScreen = true,
                resolutionIndex = Screen.resolutions.Length - 1,
                volume = 0.8f
            };

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

