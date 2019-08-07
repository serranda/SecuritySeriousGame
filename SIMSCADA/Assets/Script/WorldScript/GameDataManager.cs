using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using BayatGames.SaveGameFree;
using BayatGames.SaveGameFree.Serializers;
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

    public void StartSaveLevelGameData(byte[] bytes)
    {
        saveRoutine = SaveLevelGameData(bytes);
        StartCoroutine(saveRoutine);
    }

    private IEnumerator SaveLevelGameData(byte[] bytes)
    {
        string address = Application.absoluteURL == string.Empty
            ? StringDb.serverAddressEditor
            : StringDb.serverAddress;

        //GET THE CORRECT LEVELMANAGER
        manager = SetLevelManager();

        //GET GAMEDATA FROM THE MANAGER
        gameData = manager.GetGameData();

        //IF IS FIRST LAUNCH SET TO FALSE
        if (gameData.firstLaunch)
            gameData.firstLaunch = false;

        //SET LONG DATE VALUE
        gameData.longDate = gameData.date.ToFileTimeUtc();

        //PARSE GAMEDATA INSTANCE INTO JSON
        string data = JsonUtility.ToJson(gameData, true);

        //CREATE NEW WWWFORM FOR SENDING DATA
        WWWForm formData = new WWWForm();

        //ADD FIELD TO FORM
        formData.AddField("mode", "w");
        formData.AddField("mainDataFolder", StringDb.mainDataFolder);
        formData.AddField("playerFolder", StringDb.player.folderName);
        formData.AddField("saveFolder", StringDb.saveFolder);
        formData.AddField("saveFileName", StringDb.slotName + manager.GetGameData().indexSlot + StringDb.slotExt);
        formData.AddField("saveContent", data);

        //UPLOAD JSON DATA FROM GAMEDATA CLASS
        using (UnityWebRequest www =
            UnityWebRequest.Post(
                Path.Combine(address,
                    Path.Combine(StringDb.phpFolder, StringDb.playerSaveManagerScript)), formData))
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

        //GET SCREEN CAPTURE
        byte[] imageBytes = bytes;

        //CREATE NEW WWWFORM FOR SENDING IMAGE
        WWWForm formImage = new WWWForm();

        //ADD FIELD AND BINARY DATA TO FORM
        formImage.AddField("mode", "w");
        formImage.AddField("mainDataFolder", StringDb.mainDataFolder);
        formImage.AddField("playerFolder", StringDb.player.folderName);
        formImage.AddField("saveFolder", StringDb.saveFolder);
        formImage.AddField("imageFileName", StringDb.slotName + manager.GetGameData().indexSlot + StringDb.imageExt);
        formImage.AddBinaryData("imageFile", imageBytes);

        //UPLOAD JSON DATA FROM GAMEDATA CLASS
        using (UnityWebRequest www =
            UnityWebRequest.Post(
                Path.Combine(address,
                    Path.Combine(StringDb.phpFolder, StringDb.imageSaveManagerScript)), formImage))
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
        //SET THE FLAG TO CHECK IF GAME DATAT HAS BEEN LOADED TO FALSE
        gameDataLoaded = false;

        //START CORUTINE TO LOAD GAMEDATA FROM SERVER
        loadRoutine = LoadLevelGameData();
        StartCoroutine(loadRoutine);
    }

    private IEnumerator LoadLevelGameData()
    {
        string address = Application.absoluteURL == string.Empty
            ? StringDb.serverAddressEditor
            : StringDb.serverAddress;

        //GET THE CORRECT LEVELMANAGER
        manager = SetLevelManager();

        //CREATE NEW WWWFORM FOR GETTING DATA
        WWWForm form = new WWWForm();

        //ADD FIELD TO FORM
        form.AddField("mode", "r");
        form.AddField("mainDataFolder", StringDb.mainDataFolder);
        form.AddField("playerFolder", StringDb.player.folderName);
        form.AddField("saveFolder", StringDb.saveFolder);
        form.AddField("saveFileName", StringDb.slotName + manager.GetGameData().indexSlot + StringDb.slotExt);

        //DOWNLOAD JSON DATA FOR GAMEDATA CLASS
        using (UnityWebRequest www =
            UnityWebRequest.Post(
                Path.Combine(address,
                    Path.Combine(StringDb.phpFolder, StringDb.playerSaveManagerScript)), form))
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
        if (SceneManager.GetActiveScene().buildIndex == StringDb.level1SceneIndex)
            iManager = FindObjectOfType<Level1Manager>();
        else
            iManager = FindObjectOfType<Level2Manager>();

        return iManager;
    }

}
