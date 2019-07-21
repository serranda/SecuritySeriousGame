using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    private Button button;
    private TMP_InputField folderName;
    private TMP_InputField fileContent;

    private string username;
    private string filename = "data";
    private string content;


    private void Start()
    {
        button = GetComponentInChildren<Button>();
        folderName = GameObject.Find("FolderName").GetComponent<TMP_InputField>();
        fileContent = GameObject.Find("FileContent").GetComponent<TMP_InputField>();


        button.onClick.AddListener(delegate
        {
            if (folderName.text == "")
            {
                Debug.Log("SCRIVI UN NOME NEL TEXTFIELD");

                return;
            }

            username = folderName.text;

            StartCoroutine(CreatePlayerFolderRoutine());
        });
    }

    private IEnumerator CreatePlayerFolderRoutine()
    {

        WWWForm form = new WWWForm();


        form.AddField("folderName", username);


        using (UnityWebRequest www =
            UnityWebRequest.Post("http://192.168.1.117/TestLoginBuildWebGL/PHP/createPlayerFolder.php", form))
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

        StartCoroutine(WritePlayerLogRoutine());

    }

    private IEnumerator WritePlayerLogRoutine()
    {
        WWWForm form = new WWWForm();

        content = string.Concat(DateTime.Now.ToString("[dd/MMM/yyyy:hh:mm:ss zzz]"), " ", fileContent.text, "\n");

        form.AddField("folderName", username);
        form.AddField("fileName", filename);
        form.AddField("fileContent", content);


        using (UnityWebRequest www =
            UnityWebRequest.Post("http://192.168.1.117/TestLoginBuildWebGL/PHP/writePlayerLog.php", form))
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
