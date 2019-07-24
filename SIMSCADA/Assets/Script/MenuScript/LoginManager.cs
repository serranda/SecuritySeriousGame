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

    private void Start()
    {
        //add listener to registration button
        registrationBtn.onClick.AddListener(delegate
        {
            if (playerUserNameR.text == string.Empty ||
                playerName.text == string.Empty ||
                playerSurname.text == string.Empty ||
                passwordR.text == string.Empty
            )
            {
                Debug.Log("Completa tutti i campi");

                //DIALOG BOX MESSAGE INFORMING TO COMPLETE ALL THE FIELDS
                ClassDb.loginMessageManager.StartCompleteAllField();
                return;
            }

            StartCoroutine(CheckRegisteringPlayerListRoutine());
        });

        //add listener to login button
        loginBtn.onClick.AddListener(delegate
        {
            if (playerUserNameL.text == string.Empty ||
                passwordL.text == string.Empty)
            {
                Debug.Log("Credenziali Incomplete");

                //DIALOG FOR USER NOT REGISTERED
                ClassDb.loginMessageManager.StartPlayerNotRegistered();
                return;
            }
            StartCoroutine(CheckLoggingPlayerListRoutine());
        });
    }

    private IEnumerator CheckRegisteringPlayerListRoutine()
    {
        //create player instance with registration credential
        Player player = new Player(playerUserNameR.text, passwordR.text, playerName.text, playerSurname.text);

        Debug.Log("PlayerInfo: " + player);

        //instanciate new wwwform
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

                //instanciate new playerlist
                PlayerList players = new PlayerList();

                //CHECK WWW RESPONSE; ABSOLUTE FIRST PLAYER REGISTERED
                if (www.downloadHandler.text == "File Created" ||
                    www.downloadHandler.text == "File Empty")
                { 
                   //NO PLAYER WITH PICKED USERNAME; UPDATING PLAYER LIST
                   StartCoroutine(UpdatePlayerList(player, players));
                }
                //CHECK WWW RESPONSE; THERE ARE OTHER PLAYER REGISTERED
                else
                {
                    //CHECK IF OTHER PLAYER WITH PICKED USERNAME EXIST
                    string jsonData = www.downloadHandler.text;

                    players = JsonUtility.FromJson<PlayerList>(jsonData);

                    foreach (Player p in players.list)
                    {
                        if (p.username != player.username) continue;

                        //NOTIFY OTHER PLAYER WITH PICKED USERNAME EXIST; EXIT
                        Debug.Log("Nome utente già in uso");
                        //DIALOG FOR USER ALREADY EXISTING
                        ClassDb.loginMessageManager.StartExistingPlayer();

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
        Player player = new Player(playerUserNameL.text, passwordL.text);

        Debug.Log("PlayerInfo: " + player);

        //instanciate new wwwform
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
                Debug.Log(www.downloadHandler.text);

                //CHECK WWW RESPONSE; THERE ARE REGISTERED PLAYER
                if (www.downloadHandler.text == "File Created" || www.downloadHandler.text == "File Empty")
                {
                    //CHECK WWW RESPONSE; THERE ARE NO REGISTERED PLAYER
                    Debug.Log("Giocatore non registrato");
                    //DIALOG FOR USER NOT REGISTERED
                    ClassDb.loginMessageManager.StartPlayerNotRegistered();
                }
                else
                {
                    //GET JSON PLAYERLIST FROM SERVER AND THEN CREATE PLAYERLIST INSTANCE
                    string jsonData = www.downloadHandler.text;

                    PlayerList players = JsonUtility.FromJson<PlayerList>(jsonData);

                    foreach (Player p in players.list)
                    {
                        if (p.username == player.username && p.password == player.password)
                        {
                            //PLAYER IS REGISTERED, USERNAME AND PASSWORD CORRESPOND; GET ALL THE INFO
                            Debug.Log("Login Eseguito con successo");

                            //get all the actual player information
                            player = p;

                            //LOGIN
                            LoginToMenu(player);
                        }
                        else
                        {
                            //CHECK WWW RESPONSE; THERE ARE NO REGISTERED PLAYER
                            Debug.Log("Giocatore non registrato");
                            //DIALOG FOR USER NOT REGISTERED
                            ClassDb.loginMessageManager.StartPlayerNotRegistered();
                        }
                    }
                }
            }
        }
    }



private IEnumerator UpdatePlayerList(Player player, PlayerList players)
    {
        //instanciate new wwwform
        WWWForm form = new WWWForm();

        //UPDATING PLAYERLIST WITH NEW PLAYER INFO
        players.list.Add(player);

        //Debug.Log(players);

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
                //CHECK WWW RESPONSE; COULDN'T OPEN FILE AND WRITE THE INFORMATION
                if (www.downloadHandler.text == "Error Reading File")
                {
                    Debug.Log("Error Updating List");
                }
                //CHECK WWW RESPONSE; FILE UPDATED CORRECTLY
                else
                {
                    Debug.Log("Player List Updated");
                    //CREATE FOLDER WHERE WILL BE STORED PLAYER LOG, INFO AND SETTINGS
                    StartCoroutine(CreatePlayerFolderRoutine(player));
                }
            }
        }
    }

    private IEnumerator CreatePlayerFolderRoutine(Player player)
    {
        //instanciate new wwwform
        WWWForm form = new WWWForm();

        //ADD PLAYER FOLDERNAME TO FORM FIELD
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
                //Debug.Log(www.downloadHandler.text);

                //CHECK WWW RESPONSE; COULDN'T CREATE THE FOLDER
                if (www.downloadHandler.text == "Error Creating Folder")
                {
                    Debug.Log("Error Creating Folder");
                }
                //CHECK WWW RESPONSE; FOLDER CREATED CORRECTLY
                else
                {
                    //LOGIN
                    LoginToMenu(player);
                }
            }
        }
    }

    private void LoginToMenu(Player player)
    {
        //SET PLAYER REFERENCE WITH LOGGED PLAYER VALUES
        StringDb.player = player;

        //SET STRING VALUES FOR LOCAL SAVE
        StringDb.settingsLocalFolderPath = Path.Combine(Application.persistentDataPath, Path.Combine(StringDb.playerDataFolder, Path.Combine(player.folderName, StringDb.settingFolder)));
        StringDb.settingsLocalFilePath = Path.Combine(StringDb.settingsLocalFolderPath, StringDb.settingName + StringDb.settingExt);

        //UPDATE PLAYER LOG WITH LOGIN DATA
        ClassDb.logManager.StartWritePlayerLogRoutine(player, StringDb.logEvent.SystemEvent, string.Concat(player.username, " has connected."));

        //LOAD START MENU SCENE
        ClassDb.sceneLoader.StartLoadByIndex(StringDb.menuSceneIndex);
    }



}