using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    private IEnumerator createPlayerDataFolder;
    private IEnumerator checkRegisteringPlayer;
    private IEnumerator checkLoggingPlayer;
    private IEnumerator updatePlayerList;
    private IEnumerator createPlayerFolder;

    private RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

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
        ClassDb.menuMessageManager.StartWelcomePlayer();

        string address = Application.absoluteURL == string.Empty
            ? StaticDb.serverAddressEditor
            : Application.absoluteURL.TrimEnd('/');

        Debug.Log("Address: " + address);

        //FORCE FULL SCREEN OFF
        Screen.fullScreen = false;

        //START COROUTINE TO CREATE PLAYER DATA FOLDER ON THE SERVER
        createPlayerDataFolder = CreatePlayerDataFolder();
        StartCoroutine(createPlayerDataFolder);

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
                ClassDb.menuMessageManager.StartCompleteAllField();
                return;
            }

            checkRegisteringPlayer = CheckRegisteringPlayerListRoutine();
            StartCoroutine(checkRegisteringPlayer);
        });

        //add listener to login button
        loginBtn.onClick.AddListener(delegate
        {
            if (playerUserNameL.text == string.Empty ||
                passwordL.text == string.Empty)
            {
                Debug.Log("Credenziali Incomplete");

                //DIALOG FOR USER NOT REGISTERED
                ClassDb.menuMessageManager.StartPlayerNotRegistered();
                return;
            }

            checkLoggingPlayer = CheckLoggingPlayerListRoutine();
            StartCoroutine(checkLoggingPlayer);
        });
    }

    private IEnumerator CreatePlayerDataFolder()
    {
        string address = Application.absoluteURL == string.Empty
            ? StaticDb.serverAddressEditor
            : Application.absoluteURL.TrimEnd('/');

        //NEW WWWFORM FOR WEB REQUEST
        WWWForm form = new WWWForm();

        //ADD INFO FOR PLAYER DATA FOLDER
        form.AddField("mainDataFolder", "Data");

        //SEND REQUEST
        using (UnityWebRequest www =
            UnityWebRequest.Post(
                Path.Combine(address, Path.Combine(StaticDb.phpFolder, StaticDb.createMainDataFolderScript)),
                form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                //CHECK WWW RESPONSE; COULDN'T CREATE THE FOLDER
                if (www.downloadHandler.text == "Error Creating Folder")
                {
                    Debug.Log("Error Creating Folder");
                }
                else
                {
                    Debug.Log("Folder Created");
                }
            }
        }
    }

    private IEnumerator CheckRegisteringPlayerListRoutine()
    {
        string address = Application.absoluteURL == string.Empty
            ? StaticDb.serverAddressEditor
            : Application.absoluteURL.TrimEnd('/');

        //GENERATE SALT AND HASH CODE FOR PASSWORD
        byte[] hash;
        byte[] salt;
        using (SHA256 mySha256 = SHA256.Create())
        {
            salt = new byte[5];
            rng.GetBytes(salt);

            byte[] saltPassword = salt.Concat(Encoding.ASCII.GetBytes(passwordR.text)).ToArray();

            hash = mySha256.ComputeHash(saltPassword);

            //Debug.Log("salt: " + string.Join(" ", salt));
            //Debug.Log("passwordToByte: " + string.Join(" ", Encoding.ASCII.GetBytes(passwordR.text)));
            //Debug.Log("saltPassword: " + string.Join(" ", saltPassword));
            //Debug.Log("hash: " + string.Join(" ", hash));
        }

        //create player instance with registration credential
        Player player = new Player(playerUserNameR.text, hash, playerName.text, playerSurname.text, salt);

        Debug.Log("PlayerInfo: " + player);

        //instanciate new wwwform
        WWWForm form = new WWWForm();

        //add field to form
        form.AddField("mode", "r");
        form.AddField("mainDataFolder", StaticDb.mainDataFolder);
        form.AddField("playerListName", StaticDb.playerListName);

        using (UnityWebRequest www =
            UnityWebRequest.Post(
                Path.Combine(address, Path.Combine(StaticDb.phpFolder, StaticDb.playersListManager)),
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
                   updatePlayerList = UpdatePlayerList(player, players);
                   StartCoroutine(updatePlayerList);
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
                        ClassDb.menuMessageManager.StartExistingPlayer();

                        yield break;
                    }


                    //NO PLAYER WITH PICKED USERNAME; UPDATING PLAYER LIST
                    updatePlayerList = UpdatePlayerList(player, players);
                    StartCoroutine(updatePlayerList);
                }
            }
        }
    }

    private IEnumerator CheckLoggingPlayerListRoutine()
    {
        string address = Application.absoluteURL == string.Empty
            ? StaticDb.serverAddressEditor
            : Application.absoluteURL.TrimEnd('/');

        //instanciate new wwwform
        WWWForm form = new WWWForm();

        //add field to form
        form.AddField("mode", "r");
        form.AddField("mainDataFolder", StaticDb.mainDataFolder);
        form.AddField("playerListName", StaticDb.playerListName);

        using (UnityWebRequest www =
            UnityWebRequest.Post(
                Path.Combine(address, Path.Combine(StaticDb.phpFolder, StaticDb.playersListManager)),
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

                //CHECK WWW RESPONSE; THERE ARE REGISTERED PLAYER
                if (www.downloadHandler.text == "File Created" || www.downloadHandler.text == "File Empty")
                {
                    //CHECK WWW RESPONSE; THERE ARE NO REGISTERED PLAYER
                    Debug.Log("Giocatore non registrato");
                    //DIALOG FOR USER NOT REGISTERED
                    ClassDb.menuMessageManager.StartPlayerNotRegistered();
                }
                else
                {
                    ////create player instance with credential
                    //Player player = new Player(playerUserNameL.text, passwordL.text);

                    //Debug.Log("PlayerInfo: " + player);

                    //GET JSON PLAYERLIST FROM SERVER AND THEN CREATE PLAYERLIST INSTANCE
                    string jsonData = www.downloadHandler.text;

                    PlayerList players = JsonUtility.FromJson<PlayerList>(jsonData);

                    bool playerFound = false;

                    foreach (Player p in players.list)
                    {


                        //CALCUATE HASH CODE FOR PASSWORD; BEFORE INSERT SALT SAVED FOR THAT PLAYER
                        byte[] hash;
                        using (SHA256 mySha256 = SHA256.Create())
                        {
                            byte[] saltPassword = p.salt.Concat(Encoding.ASCII.GetBytes(passwordL.text)).ToArray();

                            hash = mySha256.ComputeHash(saltPassword);

                            //Debug.Log("salt: " + string.Join(" ", p.salt));
                            //Debug.Log("passwordToByte: " + string.Join(" ", Encoding.ASCII.GetBytes(passwordL.text)));
                            //Debug.Log("saltPassword: " + string.Join(" ", saltPassword));
                            //Debug.Log("hash: " + string.Join(" ", hash));
                            //Debug.Log("p.hash: " + string.Join(" ", p.hash));                 
                        }
                        bool hashEqual = p.hash.SequenceEqual(hash);

                        //Debug.Log(hashEqual);

                        if (p.username != playerUserNameL.text || !hashEqual) continue;

                        playerFound = true;
                        //PLAYER IS REGISTERED, USERNAME AND PASSWORD CORRESPOND; GET ALL THE INFO
                        Debug.Log("Login Eseguito con successo");

                        //LOGIN
                        LoginToMenu(p);

                        break;
                    }

                    if (playerFound) yield break;
                    Debug.Log("Giocatore non registrato");
                    //DIALOG FOR USER NOT REGISTERED
                    ClassDb.menuMessageManager.StartPlayerNotRegistered();
                }
            }
        }
    }

    private IEnumerator UpdatePlayerList(Player player, PlayerList players)
    {
        string address = Application.absoluteURL == string.Empty
            ? StaticDb.serverAddressEditor
            : Application.absoluteURL.TrimEnd('/');

        //instanciate new wwwform
        WWWForm form = new WWWForm();

        //UPDATING PLAYERLIST WITH NEW PLAYER INFO
        players.list.Add(player);

        //Debug.Log(players);

        //create json data
        string playerListData = JsonUtility.ToJson(players);

        Debug.Log("playerListData: " + playerListData);

        //add filed and json data to form
        form.AddField("mode", "w");
        form.AddField("mainDataFolder", StaticDb.mainDataFolder);
        form.AddField("playerListName", StaticDb.playerListName);
        form.AddField("playerListData", playerListData);

        using (UnityWebRequest www =
            UnityWebRequest.Post(
                Path.Combine(address, Path.Combine(StaticDb.phpFolder, StaticDb.playersListManager)),
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
                    createPlayerFolder = CreatePlayerFolderRoutine(player);
                    StartCoroutine(createPlayerFolder);
                }
            }
        }
    }

    private IEnumerator CreatePlayerFolderRoutine(Player player)
    {
        string address = Application.absoluteURL == string.Empty
            ? StaticDb.serverAddressEditor
            : Application.absoluteURL.TrimEnd('/');

        //instanciate new wwwform
        WWWForm form = new WWWForm();

        //ADD VALUES TO FORM FIELD
        form.AddField("mainDataFolder", StaticDb.mainDataFolder);
        form.AddField("playerFolder", player.folderName);

        using (UnityWebRequest www =
            UnityWebRequest.Post(
                Path.Combine(address,
                    Path.Combine(StaticDb.phpFolder, StaticDb.createPlayerFolderScript)), form))
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
        StaticDb.player = player;

        //UPDATE PLAYER LOG WITH LOGIN DATA
        ClassDb.logManager.StartWritePlayerLogRoutine(player, StaticDb.logEvent.SystemEvent, string.Concat(player.username, " has connected."));

        //LOAD START MENU SCENE
        ClassDb.sceneLoader.StartLoadByIndex(StaticDb.menuSceneIndex);
    }

}