using System;
using System.Collections;
using System.IO;
using Cinemachine;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class GameDataManager : MonoBehaviour
{
    private GameData gameData = new GameData();

    private ILevelManager manager;

    private IEnumerator saveRoutine;
    private IEnumerator loadRoutine;

    public static bool gameDataLoaded;

    //THIS CLASS WILL MANAGE ALL THE SAVINGS AND THE LOADINGS OF GAME DATA

    public void StartSaveLevelGameData(int level)
    {
        saveRoutine = SaveLevelGameData(level);
        StartCoroutine(saveRoutine);
    }

    private IEnumerator SaveLevelGameData(int level)
    {
        string address = Application.absoluteURL == string.Empty
            ? StaticDb.serverAddressEditor
            : Application.absoluteURL.TrimEnd('/');

        //GET THE CORRECT LEVELMANAGER
        manager = SetLevelManager();

        //GET GAMEDATA FROM THE MANAGER
        gameData = manager.GetGameData();

        ////IF IS FIRST LAUNCH SET TO FALSE
        //if (gameData.firstLaunch)
        //    gameData.firstLaunch = false;

        //SET LONG DATE VALUE
        gameData.longDate = gameData.date.ToFileTimeUtc();

        //SET LEVEL INDEX
        gameData.levelIndex = level;

        //SET CAMERA ZOOM VALUE
        CinemachineVirtualCamera virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        gameData.cameraZoom  = virtualCamera.m_Lens.OrthographicSize;
        Debug.Log(gameData.cameraZoom);


        //SET ALL THE serializableAiController
        foreach (TimeEvent timeEvent in gameData.timeEventList)
        {
            if (timeEvent.threat.aiController != null)
            {
                timeEvent.threat.serializableAiController = new SerializableAiController(timeEvent.threat.aiController);

                Debug.Log(timeEvent.threat.aiController);
            }

            //Debug.Log("FOREACH");
        }

        //PARSE GAMEDATA INSTANCE INTO JSON
        string data = JsonUtility.ToJson(gameData, true);

        //CREATE NEW WWWFORM FOR SENDING DATA
        WWWForm formData = new WWWForm();

        //ADD FIELD TO FORM
        formData.AddField("mode", "w");
        formData.AddField("mainDataFolder", StaticDb.mainDataFolder);
        formData.AddField("playerFolder", StaticDb.player.folderName);
        formData.AddField("saveFolder", StaticDb.saveFolder);
        //formData.AddField("saveFileName", StaticDb.slotName + manager.GetGameData().indexSlot + StaticDb.slotExt);
        formData.AddField("saveFileName", StaticDb.gameSaveName + StaticDb.gameSaveExt);
        formData.AddField("saveContent", data);

        //UPLOAD JSON DATA FROM GAMEDATA CLASS
        using (UnityWebRequest www =
            UnityWebRequest.Post(
                Path.Combine(address,
                    Path.Combine(StaticDb.phpFolder, StaticDb.playerSaveManagerScript)), formData))
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

    public void StartLoadLevelGameData()
    {
        //SET THE FLAG TO CHECK IF GAME DATA HAS BEEN LOADED TO FALSE
        gameDataLoaded = false;

        if (StaticDb.isNewGame)
        {
            gameDataLoaded = true;
            return;
        }

        //START CORUTINE TO LOAD GAMEDATA FROM SERVER
        loadRoutine = LoadLevelGameData();
        StartCoroutine(loadRoutine);
    }

    private IEnumerator LoadLevelGameData()
    {
        string address = Application.absoluteURL == string.Empty
            ? StaticDb.serverAddressEditor
            : Application.absoluteURL.TrimEnd('/');

        //GET THE CORRECT LEVELMANAGER
        manager = SetLevelManager();

        //CREATE NEW WWWFORM FOR GETTING DATA
        WWWForm form = new WWWForm();

        //Debug.Log("manager.GetGameData().indexSlot: " + manager.GetGameData().indexSlot);

        //ADD FIELD TO FORM
        form.AddField("mode", "r");
        form.AddField("mainDataFolder", StaticDb.mainDataFolder);
        form.AddField("playerFolder", StaticDb.player.folderName);
        form.AddField("saveFolder", StaticDb.saveFolder);
        //form.AddField("saveFileName", StaticDb.slotName + manager.GetGameData().indexSlot + StaticDb.slotExt);
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
                //Debug.Log(www.downloadHandler.text);
                
                if (www.downloadHandler.text == "Error Reading File")
                {
                    //file neither exits or couldn't open it
                }
                else
                {
                    //Get values to create game settings
                    string data = www.downloadHandler.text;

                    //SET GAMEDATA VARIABLE WITH JSON DATA DESERIALIZED
                    gameData = JsonUtility.FromJson<GameData>(data);

                    //SET DATE VALUE
                    gameData.date = DateTime.FromFileTimeUtc(gameData.longDate);

                    //SET MANAGER GAMEDATA WITH GAMEDATA VALUES RETRIEVED FROM SERVER
                    manager.SetGameData(gameData);
                }
            }
        }

        //GAME DATA LOADED SET THE FLAG TO TRUE
        gameDataLoaded = true;
    }

    private ILevelManager SetLevelManager()
    {
        ILevelManager iManager;
        if (SceneManager.GetActiveScene().buildIndex == StaticDb.level1SceneIndex)
        {
            iManager = FindObjectOfType<Level1Manager>();
        }
        else
        {
            iManager = FindObjectOfType<Level2Manager>();
        }

        return iManager;
    }

}
