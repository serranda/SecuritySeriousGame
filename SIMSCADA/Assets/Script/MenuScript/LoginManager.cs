using System;
using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    [SerializeField] private Button registrationBtn;
    [SerializeField] private Button loginBtn;

    [SerializeField] private TMP_InputField playerUserNameR;
    [SerializeField] private TMP_InputField playerUserNameL;
    [SerializeField] private TMP_InputField passwordR;
    [SerializeField] private TMP_InputField passwordL;
    [SerializeField] private TMP_InputField playerName;
    [SerializeField] private TMP_InputField playerSurname;

    private string filename = "userData";
    private string content;


    private void Start()
    {
        registrationBtn.onClick.AddListener(delegate
        {
            if (playerUserNameR.text == string.Empty ||
                playerName.text == string.Empty ||
                playerSurname.text == string.Empty ||
                passwordR.text == string.Empty
            )
            {
                Debug.Log("Completa tutti i campi");

                //TODO ADD DIALOG BOX MESSAGE FOR COMPLETE ALL THE FIELDS

                return;
            }

            StartCoroutine(CheckRegisteringPlayerListRoutine());
        });

        loginBtn.onClick.AddListener(delegate
        {
            if (playerUserNameL.text == string.Empty ||
                passwordL.text == string.Empty)
                return;

            StartCoroutine(CheckLoggingPlayerListRoutine());
        });
    }

    private IEnumerator CheckRegisteringPlayerListRoutine()
    {
        //create player instance with credential
        Player player = new Player(playerUserNameR.text, passwordR.text, playerName.text, playerSurname.text);

        Debug.Log("PlayerInfo: " + player);

        PlayerList players = new PlayerList();

        WWWForm form = new WWWForm();

        using (UnityWebRequest www =
            UnityWebRequest.Post(
                Path.Combine(StringDb.serverAddress, Path.Combine(StringDb.phpFolder, StringDb.checkPlayerListScript)),
                form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                //Debug.Log(www.downloadHandler.text);

                if (www.downloadHandler.text == "File Created" ||
                    www.downloadHandler.text == "File Empty")
                {
                    //ABSOLUTE FIRST PLAYER REGISTERED
                    StartCoroutine(UpdatePlayerList(player, players));
                }
                else
                {
                    //CHECK IF OTHER PLAYER WITH PICKED USERNAME EXIST
                    string jsonData = www.downloadHandler.text;

                    players = JsonUtility.FromJson<PlayerList>(jsonData);

                    foreach (Player p in players.list)
                    {
                        if (p.username != player.username) continue;

                        Debug.Log("Nome utente già in uso");
                        //TODO ADD DIALOG FOR USER ALREADY EXISTING
                        yield break;
                    }


                    //NO PLAYER WITH PICKED USERNAME; UPDATING PLAYER LIST
                    StartCoroutine(UpdatePlayerList(player, players));
                }
            }
        }
    }

    private IEnumerator CheckLoggingPlayerListRoutine()
    {
        //create player instance with credential
        Player player = new Player(playerUserNameL.text, passwordL.text, playerName.text, playerSurname.text);

        Debug.Log("PlayerInfo: " + player);

        PlayerList players = new PlayerList();

        WWWForm form = new WWWForm();

        using (UnityWebRequest www =
            UnityWebRequest.Post(
                Path.Combine(StringDb.serverAddress, Path.Combine(StringDb.phpFolder, StringDb.checkPlayerListScript)),
                form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                //Debug.Log(www.downloadHandler.text);

                if (www.downloadHandler.text != "File Created" &&
                    www.downloadHandler.text != "File Empty")
                {
                    //THERE ARE PLAYER REGISTERED
                    string jsonData = www.downloadHandler.text;

                    players = JsonUtility.FromJson<PlayerList>(jsonData);

                    foreach (Player p in players.list)
                    {
                        if (p.username != player.username) continue;
                        if (p.password == player.password)
                        {
                            //LOGIN
                            Debug.Log("Login Eseguito con successo");
                            player = p;
                            LoginToMenu(player);
                            yield break;

                        }

                        Debug.Log("Giocatore non registrato");
                        //TODO ADD DIALOG FOR USER NOT REGISTERED
                        yield break;
                    }
                }
                else
                {
                    //THERE ARE NO PLAYER REGISTERED
                    Debug.Log("Giocatore non registrato");
                }
            }
        }
    }



private IEnumerator UpdatePlayerList(Player player, PlayerList players)
    {
        WWWForm form = new WWWForm();

        players.list.Add(player);

        Debug.Log(players);

        //create json data
        string playerListData = JsonUtility.ToJson(players);

        Debug.Log("playerListData: " + playerListData);

        //add json data to form
        form.AddField("playerListData", playerListData);

        using (UnityWebRequest www =
            UnityWebRequest.Post(
                Path.Combine(StringDb.serverAddress, Path.Combine(StringDb.phpFolder, StringDb.updatePlayerListScript)),
                form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                if (www.downloadHandler.text == "Error Reading File")
                {
                    Debug.Log("Error Updating List");
                }
                else
                {
                    Debug.Log("Player List Updated");
                    StartCoroutine(CreatePlayerFolderRoutine(player));
                }
            }
        }
    }

    private IEnumerator CreatePlayerFolderRoutine(Player player)
    {
        WWWForm form = new WWWForm();

        form.AddField("folderName", player.folderName);

        using (UnityWebRequest www =
            UnityWebRequest.Post(
                Path.Combine(StringDb.serverAddress,
                    Path.Combine(StringDb.phpFolder, StringDb.createPlayerFolderScript)), form))
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

        //LOGIN
        LoginToMenu(player);

        //StartCoroutine(WritePlayerLogRoutine());
    }

    private void LoginToMenu(Player player)
    {
        StringDb.player = player;

        StringDb.settingsLocalFolderPath = Path.Combine(Application.persistentDataPath, Path.Combine(StringDb.playerDataFolder, Path.Combine(player.folderName, StringDb.settingFolder)));
        StringDb.settingsLocalFilePath = Path.Combine(StringDb.settingsLocalFolderPath, StringDb.settingName + StringDb.settingExt);

        ClassDb.sceneLoader.StartLoadByIndex(StringDb.menuSceneIndex);
    }


    //METODO DA SPOSTARE IN CLASSE PER IL LOGGING

    private IEnumerator WritePlayerLogRoutine(Player player)
    {
        WWWForm form = new WWWForm();

        content = string.Concat(DateTime.Now.ToString(StringDb.dataTimeFormat), " ", "\n");

        //form.AddField("userName", playerUserName.text);
        form.AddField("fileName", filename);
        form.AddField("fileContent", content);


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