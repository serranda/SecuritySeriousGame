using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class LogManager : MonoBehaviour
{
    private string filename = "playerLog";

    private IEnumerator writeRoutine;

    public void StartWritePlayerLogRoutine(Player player, StringDb.logEvent logEvent, string content)
    {
        writeRoutine = WritePlayerLogRoutine(player, logEvent, content);
        StartCoroutine(writeRoutine);
    }

    private IEnumerator WritePlayerLogRoutine(Player player, StringDb.logEvent logEvent, string content)
    {
        WWWForm form = new WWWForm();

        string fieldContent = string.Concat(DateTime.Now.ToString(StringDb.dataTimeFormat), " [", logEvent.ToString().ToUpper() , "] ", content,"\n");

        form.AddField("folderName", player.folderName);
        form.AddField("fileName", filename);
        form.AddField("fileContent", fieldContent);


        using (UnityWebRequest www =
            UnityWebRequest.Post(
                Path.Combine(StringDb.serverAddress, Path.Combine(StringDb.phpFolder, StringDb.writePlayerLogScript)),
                form))
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
