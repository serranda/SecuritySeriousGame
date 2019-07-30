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
    private GameData gameData;

    private ILevelManager manager;

    private IEnumerator saveRoutine;
    private IEnumerator loadRoutine;

    //THIS CLASS WILL MANAGE ALL THE SAVINGS AND THE LOADINGS OF GAME DATA

    public void StartSaveLevelGameData()
    {
        saveRoutine = SaveLevelGameData();
        StartCoroutine(saveRoutine);
    }

    private IEnumerator SaveLevelGameData()
    {
        //GET THE CORRECT LEVELMANAGER
        manager = SetLevelManager();

        //GET GAMEDATA FROM THE MANAGER
        gameData = manager.GetGameData();

        //PARSE GAMEDATA INSTANCE INTO JSON
        string data = JsonUtility.ToJson(gameData, true);

        //CREATE NEW WWWFORM FOR SENDING DATA
        WWWForm formData = new WWWForm();

        //ADD FIELD TO FORM
        formData.AddField("mode", "w");
        formData.AddField("playerFolder", StringDb.player.folderName);
        formData.AddField("saveFolder", StringDb.saveFolder);
        formData.AddField("saveFileName", StringDb.slotName + manager.GetGameData().slotIndex + StringDb.slotExt);
        formData.AddField("saveContent", data);

        //UPLOAD JSON DATA FROM GAMEDATA CLASS
        using (UnityWebRequest www =
            UnityWebRequest.Post(
                Path.Combine(StringDb.serverAddress,
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

        yield return new WaitForEndOfFrame();

        //CREATE SCREEN CAPTURE
        //TODO CREATE SCREEN CAPTURE BEFORE EXIT MESSAGE/PAUSE MENU
        Texture2D texture2D = ScreenCapture.CaptureScreenshotAsTexture();
        byte[] imageFile = texture2D.EncodeToPNG();

        //CREATE NEW WWWFORM FOR SENDING IMAGE
        WWWForm formImage = new WWWForm();

        //ADD FIELD AND BINARY DATA TO FORM
        formImage.AddField("mode", "w");
        formImage.AddField("playerFolder", StringDb.player.folderName);
        formImage.AddField("saveFolder", StringDb.saveFolder);
        formImage.AddBinaryData("imageFile", imageFile, StringDb.slotName + manager.GetGameData().slotIndex + StringDb.imageExt, "image/png");

        //UPLOAD JSON DATA FROM GAMEDATA CLASS
        using (UnityWebRequest www =
            UnityWebRequest.Post(
                Path.Combine(StringDb.serverAddress,
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
        loadRoutine = LoadLevelGameData();
        StartCoroutine(loadRoutine);
    }

    private IEnumerator LoadLevelGameData()
    {
        //GET THE CORRECT LEVELMANAGER
        manager = SetLevelManager();

        //CREATE NEW WWWFORM FOR GETTING DATA
        WWWForm form = new WWWForm();

        //ADD FIELD TO FORM
        form.AddField("mode", "r");
        form.AddField("playerFolder", StringDb.player.folderName);
        form.AddField("saveFolder", StringDb.saveFolder);
        form.AddField("saveFileName", StringDb.slotName + manager.GetGameData().slotIndex + StringDb.slotExt);

        //DOWNLOAD JSON DATA FOR GAMEDATA CLASS
        using (UnityWebRequest www =
            UnityWebRequest.Post(
                Path.Combine(StringDb.serverAddress,
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

                    //TODO SET MANAGER GAMEDATA WITH PREVIOUS GAMEDATA VALUES
                }
            }
        }
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
