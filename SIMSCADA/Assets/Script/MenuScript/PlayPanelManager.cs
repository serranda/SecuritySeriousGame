using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;

public class PlayPanelManager : MonoBehaviour
{

    [SerializeField] private List<Button> playBtnList = new List<Button>();

    [SerializeField] private List<Button> deleteBtnList = new List<Button>();

    [SerializeField] private List<Image> imageList = new List<Image>();

    [SerializeField] private List<TextMeshProUGUI> infoTxtList = new List<TextMeshProUGUI>();

    private IEnumerator checkSlotRoutine;
    private IEnumerator deleteSlotRoutine;

    private void OnEnable()
    {
        //get all the playButton and set the listener
        //if slotN is free=true --> play new game
        //else load slotN

        foreach(Button playButton in playBtnList)
        {
            int index = playBtnList.IndexOf(playButton);

            Image image = imageList[index];

            Button deleteButton = deleteBtnList[index];

            TextMeshProUGUI info = infoTxtList[index];
            
            StartCheckSlotSave(playButton, deleteButton, image, info, index);
        }
    }

    private void StartCheckSlotSave(Button playButton, Button deleteButton, Image image, TextMeshProUGUI info, int index)
    {
        checkSlotRoutine = CheckSlotSave(playButton, deleteButton, image, info, index);
        StartCoroutine(checkSlotRoutine);
    }

    private IEnumerator CheckSlotSave(Button playButton, Button deleteButton, Image image, TextMeshProUGUI info, int index)
    {
        string address = Application.absoluteURL == string.Empty
            ? StringDb.serverAddressEditor
            : StringDb.serverAddress;

        //CREATE NEW WWWFORM FOR GETTING DATA
        WWWForm form = new WWWForm();

        //ADD FIELD TO FORM
        form.AddField("mode", "r");
        form.AddField("mainDataFolder", StringDb.mainDataFolder);
        form.AddField("playerFolder", StringDb.player.folderName);
        form.AddField("saveFolder", StringDb.saveFolder);
        form.AddField("imageFileName", StringDb.slotName + index + StringDb.imageExt);

        //DOWNLOAD JSON DATA FOR GAMEDATA CLASS
        using (UnityWebRequest www =
            UnityWebRequest.Post(
                Path.Combine(address,
                    Path.Combine(StringDb.phpFolder, StringDb.imageSaveManagerScript)), form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                //Debug.Log(www.downloadHandler.text);

                //FILE DOESN'T EXIST, SLOT FREE
                if (www.downloadHandler.text == "Error Reading File")
                {
                    //SET TEXT OF BUTTON
                    playButton.GetComponentInChildren<TextMeshProUGUI>().text = "Gioca";

                    //SET LISTENER ON BUTTON TO START LEVEL 1
                    playButton.onClick.RemoveAllListeners();
                    playButton.onClick.AddListener(delegate
                    {
                        //Debug.Log(index);
                        //SET SLOT INDEX; THIS WILL BE USED IN SLOT FILE NAME TO PREVENT OVERWRITE BETWEEN SLOT
                        StringDb.indexSlot = index;
                        ClassDb.sceneLoader.StartLoadByIndex(StringDb.level1SceneIndex);
                    });

                    //DISABLE DELETE BUTTON
                    deleteButton.interactable = false;

                    //SET IMAGE SPRITE TO DEFAULT FREE SLOT IMAGE
                    image.sprite = Resources.Load<Sprite>("LoadPicture/EmptySlotImage");
                    image.preserveAspect = true;

                    //SET INFO TO NOTICE FREE SLOT
                    info.text = "SLOT DI SALVATAGGIO LIBERO";

                }
                //FILE EXISTS, SET IMAGE, BUTTON AND INFO
                else
                {
                    //SET TEXT OF BUTTON
                    playButton.GetComponentInChildren<TextMeshProUGUI>().text = "Carica";

                    //SET LISTENER ON BUTTON TO START LEVEL 1
                    playButton.onClick.RemoveAllListeners();
                    playButton.onClick.AddListener(delegate
                    {
                        //Debug.Log(index);
                        //SET SLOT INDEX; THIS WILL BE USED IN SLOT FILE NAME TO PREVENT OVERWRITE BETWEEN SLOT
                        StringDb.indexSlot = index;
                        ClassDb.sceneLoader.StartLoadByIndex(StringDb.level1SceneIndex);
                    });

                    //ENABLE DELETE BUTTON
                    deleteButton.interactable = true;

                    //SET LISTENER TO DELETE BUTTON TO DELETE SAVE SLOT
                    deleteButton.onClick.RemoveAllListeners();
                    deleteButton.onClick.AddListener(delegate
                    {
                        //SET LISTENER METHOD TO DELETE SLOT FILE AND IMAGE 
                        deleteSlotRoutine = DeleteSlotSave(playButton, deleteButton, image, info, index);
                        StartCoroutine(deleteSlotRoutine);

                    });

                    //GET IMAGE URL 
                    string imageURL = www.downloadHandler.text;

                    //CREATE NEW WHITE TEXTURE
                    Texture2D texture = Texture2D.whiteTexture;

                    using (UnityWebRequest textureWWW = UnityWebRequestTexture.GetTexture(imageURL))
                    {
                        yield return textureWWW.SendWebRequest();

                        if (textureWWW.isNetworkError || textureWWW.isHttpError)
                        {
                            Debug.Log(textureWWW.error);
                        }
                        else
                        {
                            //SET TEXTURE WITH ONE DOWNLOADED AT THE URL RETRIEVED BEFORE
                            texture = ((DownloadHandlerTexture)textureWWW.downloadHandler).texture;
                        }

                    }

                    //CREATE SPRITE AND APPLY PREVIOUS TEXTURE
                    Sprite sprite = Sprite.Create(
                        texture,
                        new Rect(0.0f, 0.0f, texture.width, texture.height),
                        new Vector2(0.5f, 0.5f),
                        63);

                    //SET IMAGE SPRITE WITH SCREENSHOT RETRIEVED FROM THE SERVER
                    image.sprite = sprite;
                    image.preserveAspect = true;

                    //CREATE FORM TO GET STATS ABOUT IMAGE FILE
                    WWWForm statForm = new WWWForm();

                    //ADD FIELD TO FORM
                    statForm.AddField("mainDataFolder", StringDb.mainDataFolder);
                    statForm.AddField("playerFolder", StringDb.player.folderName);
                    statForm.AddField("saveFolder", StringDb.saveFolder);
                    statForm.AddField("imageFileName", StringDb.slotName + index + StringDb.imageExt);

                    //DOWNLOAD JSON DATA FOR GAMEDATA CLASS
                    using (UnityWebRequest statWWW =
                        UnityWebRequest.Post(
                            Path.Combine(address,
                                Path.Combine(StringDb.phpFolder, StringDb.getFileInfoScript)), statForm))
                    {
                        yield return statWWW.SendWebRequest();

                        if (statWWW.isNetworkError || statWWW.isHttpError)
                        {
                            Debug.Log(statWWW.error);
                        }
                        else
                        {
                            //Debug.Log(statWWW.downloadHandler.text);

                            string unixTimeStamp = statWWW.downloadHandler.text;

                            DateTime date = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                            date = date.AddSeconds(int.Parse(unixTimeStamp)).ToLocalTime();
                            //SET INFO WITH DATETIME INFORMATION ABOUT SAVE FILE
                            info.text = date.ToString(StringDb.panelTimeFormat);
                        }
                    }


                }
            }
        }
    }

    private IEnumerator DeleteSlotSave(Button playButton, Button deleteButton, Image image, TextMeshProUGUI info, int index)
    {
        string address = Application.absoluteURL == string.Empty
            ? StringDb.serverAddressEditor
            : StringDb.serverAddress;

        //CREATE NEW WWWFORM FOR DELETE SAVE FILE
        WWWForm deleteSaveForm = new WWWForm();
        //ADD FIELD TO FORM
        deleteSaveForm.AddField("mode", "d");
        deleteSaveForm.AddField("mainDataFolder", StringDb.mainDataFolder);
        deleteSaveForm.AddField("playerFolder", StringDb.player.folderName);
        deleteSaveForm.AddField("saveFolder", StringDb.saveFolder);
        deleteSaveForm.AddField("saveFileName", StringDb.slotName + index + StringDb.slotExt);

        //SEND DELETE REQUEST TO SERVER
        using (UnityWebRequest deleteSaveWWW =
            UnityWebRequest.Post(
                Path.Combine(address,
                    Path.Combine(StringDb.phpFolder, StringDb.playerSaveManagerScript)),
                deleteSaveForm))
        {
            yield return deleteSaveWWW.SendWebRequest();

            if (deleteSaveWWW.isNetworkError || deleteSaveWWW.isHttpError)
            {
                Debug.Log(deleteSaveWWW.error);
            }
            else
            {
                Debug.Log(deleteSaveWWW.downloadHandler.text);
            }
        }

        //CREATE NEW WWWFORM FOR DELETE IMAGE
        WWWForm deleteImageForm = new WWWForm();
        //ADD FIELD TO FORM
        deleteImageForm.AddField("mode", "d");
        deleteImageForm.AddField("mainDataFolder", StringDb.mainDataFolder);
        deleteImageForm.AddField("playerFolder", StringDb.player.folderName);
        deleteImageForm.AddField("saveFolder", StringDb.saveFolder);
        deleteImageForm.AddField("imageFileName", StringDb.slotName + index + StringDb.imageExt);

        //SEND DELETE REQUEST TO SERVER
        using (UnityWebRequest deleteImageWWW =
            UnityWebRequest.Post(
                Path.Combine(address,
                    Path.Combine(StringDb.phpFolder, StringDb.imageSaveManagerScript)),
                deleteImageForm))
        {
            yield return deleteImageWWW.SendWebRequest();

            if (deleteImageWWW.isNetworkError || deleteImageWWW.isHttpError)
            {
                Debug.Log(deleteImageWWW.error);
            }
            else
            {
                Debug.Log(deleteImageWWW.downloadHandler.text);
            }
        }
        //REFRESH PLAY PANEL
        StartCheckSlotSave(playButton, deleteButton, image, info, index);


    }

}
