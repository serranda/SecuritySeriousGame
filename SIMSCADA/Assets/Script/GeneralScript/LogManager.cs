using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class LogManager : MonoBehaviour
{
    private string logName = "playerLog";

    private IEnumerator writeRoutine;

    public void StartWritePlayerLogRoutine(Player player, StringDb.logEvent logEvent, string content)
    {
        writeRoutine = WritePlayerLogRoutine(player, logEvent, content);
        StartCoroutine(writeRoutine);
    }

    private IEnumerator WritePlayerLogRoutine(Player player, StringDb.logEvent logEvent, string content)
    {
        string address = Application.absoluteURL == string.Empty
            ? StringDb.serverAddressEditor
            : StringDb.serverAddress;

        WWWForm form = new WWWForm();

        string logContent = string.Concat(DateTime.Now.ToString(StringDb.logTimeFormat), " [", logEvent.ToString().ToUpper() , "] ", content,"\n");

        //ADD FIELD TO WWWFORM
        form.AddField("mainDataFolder", StringDb.mainDataFolder);
        form.AddField("playerFolder", player.folderName);
        form.AddField("logName", logName);
        form.AddField("logContent", logContent);


        using (UnityWebRequest www =
            UnityWebRequest.Post(
                Path.Combine(address, Path.Combine(StringDb.phpFolder, StringDb.writePlayerLogScript)),
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
