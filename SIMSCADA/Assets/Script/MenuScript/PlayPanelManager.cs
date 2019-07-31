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

    [SerializeField] private List<Button> slotBtnList = new List<Button>();

    [SerializeField] private List<Image> slotImageList = new List<Image>();

    [SerializeField] private List<TextMeshProUGUI> slotInfoList = new List<TextMeshProUGUI>();

    private IEnumerator checkSlotRoutine;


    private void OnEnable()
    {
        //get all the button and set the listener
        //if slotN is free=true --> play new game
        //else load slotN

        foreach(Button button in slotBtnList)
        {
            int index = slotBtnList.IndexOf(button);
            Image image = slotImageList[index];
            TextMeshProUGUI info = slotInfoList[index];
            
            StartCheckSlotSave(button, image, info, index);
        }
    }

    private void StartCheckSlotSave(Button button, Image image, TextMeshProUGUI info, int index)
    {
        checkSlotRoutine = CheckSlotSave(button, image, info, index);
        StartCoroutine(checkSlotRoutine);
    }

    private IEnumerator CheckSlotSave(Button button, Image image, TextMeshProUGUI info, int index)
    {
        //CREATE NEW WWWFORM FOR GETTING DATA
        WWWForm form = new WWWForm();

        //ADD FIELD TO FORM
        form.AddField("mode", "r");
        form.AddField("playerFolder", StringDb.player.folderName);
        form.AddField("saveFolder", StringDb.saveFolder);
        form.AddField("imageFileName", StringDb.slotName + index + StringDb.imageExt);

        //DOWNLOAD JSON DATA FOR GAMEDATA CLASS
        using (UnityWebRequest www =
            UnityWebRequest.Post(
                Path.Combine(StringDb.serverAddress,
                    Path.Combine(StringDb.phpFolder, StringDb.imageSaveManagerScript)), form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);

                //FILE DOESN'T EXIST, SLOT FREE
                if (www.downloadHandler.text == "Error Reading File")
                {
                    //SET TEXT OF BUTTON
                    button.GetComponentInChildren<TextMeshProUGUI>().text = "Gioca";

                    //SET LISTENER ON BUTTON TO START LEVEL 1
                    button.onClick.RemoveAllListeners();
                    button.onClick.AddListener(delegate
                    {
                        //Debug.Log(index);
                        //TODO SET SLOT INDEX; THIS WILL BE USED IN SLOT FILE NAME TO PREVENT OVERWRITE BETWEEN SLOT
                        //GameData.slotIndex = index;
                        ClassDb.sceneLoader.StartLoadByIndex(StringDb.level1SceneIndex);
                    });

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
                    button.GetComponentInChildren<TextMeshProUGUI>().text = "Carica";

                    //SET LISTENER ON BUTTON TO START LEVEL 1
                    button.onClick.RemoveAllListeners();
                    button.onClick.AddListener(delegate
                    {
                        //Debug.Log(index);
                        //TODO SET SLOT INDEX; THIS WILL BE USED IN SLOT FILE NAME TO PREVENT OVERWRITE BETWEEN SLOT
                        //GameData.slotIndex = index;
                        ClassDb.sceneLoader.StartLoadByIndex(StringDb.level1SceneIndex);
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
                    statForm.AddField("playerFolder", StringDb.player.folderName);
                    statForm.AddField("saveFolder", StringDb.saveFolder);
                    statForm.AddField("imageFileName", StringDb.slotName + index + StringDb.imageExt);

                    //DOWNLOAD JSON DATA FOR GAMEDATA CLASS
                    using (UnityWebRequest statWWW =
                        UnityWebRequest.Post(
                            Path.Combine(StringDb.serverAddress,
                                Path.Combine(StringDb.phpFolder, StringDb.getFileInfoScript)), statForm))
                    {
                        yield return statWWW.SendWebRequest();

                        if (statWWW.isNetworkError || statWWW.isHttpError)
                        {
                            Debug.Log(statWWW.error);
                        }
                        else
                        {
                            Debug.Log(statWWW.downloadHandler.text);

                            string unixTimeStamp = statWWW.downloadHandler.text;

                            DateTime date = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                            date = date.AddSeconds(int.Parse(unixTimeStamp)).ToLocalTime();
                            //SET INFO WITH DATETIME INFORMATION ABOUT SAVE FILE
                            info.text = date.ToString("dd/MM/yyyy HH:mm");
                        }
                    }


                }
            }
        }
    }

}
