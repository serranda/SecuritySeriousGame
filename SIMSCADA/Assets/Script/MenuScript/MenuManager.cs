using System;
using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager: MonoBehaviour
{    
    [SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] private GameSettingManager gameSettingManager;

    [SerializeField] private Button playButton;
    [SerializeField] private Button loadButton;

    private IEnumerator checkGameSaveRoutine;

    public int levelIndex;


    private void Awake()
    {
        playerName.text =string.Concat("Benvenuto ", StaticDb.player.username.ToUpper()) ;

        gameSettingManager.StartCheckWebSettingsFileRoutine();
    }

    private void OnEnable()
    {
        StartCheckGameSave();
    }

    private void Update()
    {
        gameSettingManager.GetSpriteFromBool(Screen.fullScreen);
    }

    private void StartCheckGameSave()
    {
        checkGameSaveRoutine = CheckGameSave();
        StartCoroutine(checkGameSaveRoutine);
    }

    private IEnumerator CheckGameSave()
    {
        string address = Application.absoluteURL == string.Empty
            ? StaticDb.serverAddressEditor
            : Application.absoluteURL.TrimEnd('/');

        //CREATE NEW WWWFORM FOR GETTING DATA
        WWWForm form = new WWWForm();

        //ADD FIELD TO FORM
        form.AddField("mode", "r");
        form.AddField("mainDataFolder", StaticDb.mainDataFolder);
        form.AddField("playerFolder", StaticDb.player.folderName);
        form.AddField("saveFolder", StaticDb.saveFolder);
        form.AddField("saveFileName", StaticDb.gameSaveName + StaticDb.gameSaveExt);

        //DOWNLOAD JSON DATA FOR GAMEDATA CLASS
        using (UnityWebRequest www =
            UnityWebRequest.Post(
                Path.Combine(address,
                    Path.Combine(StaticDb.phpFolder, StaticDb.playerSaveManagerScript)), form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                //CHECK IF FILE EXIST AND SET LOAD BUTTON INTERACTABLE
                loadButton.interactable = www.downloadHandler.text != "Error Reading File";

                if (loadButton.interactable)
                {
                    string data = www.downloadHandler.text;

                    //SET GAMEDATA VARIABLE WITH JSON DATA DESERIALIZED
                    GameData gameData = JsonUtility.FromJson<GameData>(data);

                    levelIndex = gameData.levelIndex;

                    playButton.onClick.RemoveAllListeners();
                    playButton.onClick.AddListener(delegate
                    {
                        ClassDb.menuMessageManager.StartWarningNewGame(); 
                    });

                    loadButton.onClick.RemoveAllListeners();
                    loadButton.onClick.AddListener(delegate
                    {
                        ClassDb.sceneLoader.StartLoadGame();
                    });
                }
                else
                {
                    playButton.onClick.RemoveAllListeners();
                    playButton.onClick.AddListener(delegate
                    {
                        ClassDb.sceneLoader.StartNewGame();
                    });

                    loadButton.onClick.RemoveAllListeners();
                }

                //Get values to create game settings
            }
        }
    }


}

