using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class StringDb
{
    //string values for language setting

    //string vale for data format in log file
    public static string dataTimeFormat = "[dd/MMM/yyyy:hh:mm:ss zzz]";

    //string values for php script and server folder
    public static string serverAddress = "http://192.168.1.118/TestLoginBuildWebGL";
    public static string phpFolder = "PHP";
    public static string mainDataFolder = "Data";
    public static string createPlayerFolderScript = "createPlayerFolder.php";
    public static string createMainDataFolderScript = "createMainDataFolder.php";
    public static string playersListManager = "playersListManager.php";
    public static string writePlayerLogScript = "writePlayerLog.php";
    public static string playerSettingsManagerScript = "playerSettingsManager.php";
    public static string playerSaveManagerScript = "playerSaveManager.php";
    public static string imageSaveManagerScript = "imageSaveManager.php";
    public static string getFileInfoScript = "getFileInfo.php";

    //player object reference
    //DEBUG
    public static Player player = new Player("aaa", "aaa", "aaa", "aaa");

    //enum for logEvent types
    public enum logEvent
    {
        SystemEvent,
        GameEvent
    }

    //string values for setting folder
    public static string settingExt = ".ini";
    public static string settingFolder = "settings";
    public static string settingName = "gamesettings";
    public static string settingsWebFolderPath = settingFolder;

    //string values for element of dialog box prefab
    public static string dialogBoxTitle = "DB_Title";
    public static string dialogBoxMessage = "DB_MessageText";
    public static string dialogBoxBtnNext = "DB_ButtonNext";
    public static string dialogBoxBtnBack = "DB_ButtonBack";
    public static string dialogBoxBtnLeft = "DB_ButtonLeft";
    public static string dialogBoxBtnRight = "DB_ButtonRight";

    //string values for element of tutorial dialog box prefab
    public static string tutorialDialogBoxMessage = "TDB_MessageText";
    public static string tutorialDialogBoxBtn = "TDB_Button";
    public static string tutorialPanel = "TDB_Template";

    //string values for element of pause menu prefab
    public static string pauseToggle = "FullScreenToggle";
    public static string pauseSlider = "VolumeSlider";
    public static string pauseDropdown = "ResolutionDropdown";
    public static string pauseBtnResume = "Resume";
    //public static string pauseBtnLesson = "Lessons";
    public static string pauseBtnExit = "Exit";

    //string values for login message files in resources folder
    public static string completeField = "LoginMessages/CompleteField";
    public static string existingPlayer = "LoginMessages/ExistingPlayer";
    public static string playerNotRegistered = "LoginMessages/PlayerNotRegistered";

    //string values for level message files in resources folder
    public static string purchase = "Messages/ConfirmPurchase";
    public static string exitTxt = "Messages/Exit";
    public static string failedCorruption = "Messages/FailedCorruption";
    public static string idsCleaned = "Messages/IdsCleaned";
    public static string idsInterception = "Messages/IdsInterception";
    public static string internalMessage = "Messages/InternalEmployee";
    public static string moneyEarn = "Messages/MoneyEarn";
    public static string fakeLocalMoneyLoss = "Messages/MoneyLossFakeLocal";
    public static string localMoneyLoss = "Messages/MoneyLossLocal";
    public static string remoteMoneyLoss = "Messages/MoneyLossRemote";
    public static string suspiciousAi = "Messages/SuspiciousAi";
    public static string localDeployed = "Messages/ThreatDeployedLocal";
    public static string remoteDeployed = "Messages/ThreatDeployedRemote";
    //public static string localThreatRecap = "Messages/ThreatRecapLocal";
    //public static string remoteThreatRecap = "Messages/ThreatRecapRemote";
    public static string fakeLocalStopped = "Messages/ThreatStoppedFakeLocal";
    public static string localStopped = "Messages/ThreatStoppedLocal";
    public static string remoteStopped = "Messages/ThreatStoppedRemote";
    public static string welcomeTxt = "Messages/Welcome";
    public static string plantCheck = "Messages/PlantCheck";
    public static string malwareCheck = "Messages/MalwareCheck";
    public static string threatManagementResult = "Messages/ThreatManagementResult";
    public static string newTrustedEmployees = "Messages/NewTrustedEmployees";
    public static string newEmployeesHired = "Messages/NewEmployeesHired";
    public static string tutorialExit = "Messages/TutorialExit";
    public static string showLessonFirstTime = "Messages/ShowLessonFirstTime";
    public static string researchReport = "Messages/ResearchReport";
    public static string endGameWin = "Messages/EndGameWin";
    public static string endGameLoss = "Messages/EndGameLoss";

    //string values for tutorial message files in resources folder
    public static string tutorialWelcome1 = "TutorialMessages/Welcome1";
    public static string tutorialWelcome2 = "TutorialMessages/Welcome2";
    public static string tutorialWelcome3 = "TutorialMessages/Welcome3";
    public static string tutorialWelcome4 = "TutorialMessages/Welcome4";
    public static string tutorialWelcome5 = "TutorialMessages/Welcome5";
    public static string tutorialWelcome6 = "TutorialMessages/Welcome6";
    public static string tutorialWelcome7 = "TutorialMessages/Welcome7";
    public static string tutorialTelephone1 = "TutorialMessages/Telephone1";
    public static string tutorialTelephone2 = "TutorialMessages/Telephone2";
    public static string tutorialServer = "TutorialMessages/Server";
    public static string tutorialSecurityCheck = "TutorialMessages/SecurityCheck";
    public static string tutorialRemoteAttack = "TutorialMessages/RemoteAttack";
    public static string tutorialPostWelcome = "TutorialMessages/PostWelcome";
    public static string tutorialPostAttack = "TutorialMessages/PostAttack";
    public static string tutorialMarketPanel1 = "TutorialMessages/MarketPanel1";
    public static string tutorialMarketPanel2 = "TutorialMessages/MarketPanel2";
    public static string tutorialLocalAttack = "TutorialMessages/LocalAttack";
    public static string tutorialInteractiveObject = "TutorialMessages/InteractiveObject";
    public static string tutorialHMIPC = "TutorialMessages/HMIPC";
    public static string tutorialHMIPanel = "TutorialMessages/HMIPanel";
    public static string tutorialFinalMessage = "TutorialMessages/FinalMessage";
    public static string notebookMessage = "TutorialMessages/Notebook";

    //string value for employees notes on idcard
    public static string attacker = "EmployeesNotes/Attacker";
    public static string attacker2 = "EmployeesNotes/Attacker2";
    public static string attacker3 = "EmployeesNotes/Attacker3";
    public static string employee = "EmployeesNotes/Employee";
    public static string employee2 = "EmployeesNotes/Employee2";
    public static string employee3 = "EmployeesNotes/Employee3";

    //string values for element of action menu prefab
    public static string actionMenuName = "ActionButton(Clone)";

    //string values for element of notebook prefab
    public static string noteBookBtnNext = "NextBtn";
    public static string noteBookBtnPrevious = "PreviousBtn";
    public static string noteBookBtnBack = "BackBtn";
    public static string noteBookBtnList = "BtnList";
    public static string noteBookPageL = "PageL";
    public static string noteBookPageR = "PageR";
    public static string noteBookTitleL = "TitleL";
    public static string noteBookTitleR = "TitleR";
    public static string noteBookToggleGroup = "BtnListToggleGroup";

    //string values for tore screen prefab
    public static string storeScreenName = "StoreScreen(Clone)";

    //string values for element of id card prefab
    public static string idCardName = "NameTMPro";
    public static string idCardSurname = "SurnameTMPro";
    public static string idCardJob = "JobTMPro";
    public static string idCardAttackerNote = "AttackerTMPro";
    public static string idCardTrusted = "TrustedTMPro";
    public static string idCardBtnBack = "BackBtn";
    public static string idCardAttackerLbl = "AttackerLBL";
    public static string idCardAiImage = "AiImage";

    ////values for marks image on id card
    //public static string marksPrefix = "Marks";
    //public static string marksFolder = "Marks";

    //string values for save files and folder
    public static string slotName = "slot";
    public static string slotExt = ".ini";
    public static string imageExt = ".png";
    public static string saveFolder = "saves";

    //string values for interactive sprite in the scene
    public const string telephoneTag = "Telephone";
    public const string roomPcTag = "RoomPc";
    public const string securityCheckTag = "SecurityCheck";
    public const string serverPcTag = "ServerPc";

    //string value for interactive sprite resources sprite
    public static string rscIntSpriteFolder = "InteractObj";

    //string values for characters sprite in resources folder
    public static string rscSpriteFolder = "CharacterSprite";
    public static string rscAiSpriteFolder = "AI";
    public static string rscPlSpriteFolder = "Player";
    public static string rscAiSpritePrefix = "AI";
    public static string rscHlSpriteSuffix = "HL";
    public static string rscPrSpriteSuffix = "PR";
    public static string rscPlSpritePrefix = "PL";

    //string values for full screen button
    public static string fsButtonFolder = "FSButton";
    public static string fsButtonOn = "FSButtonOn";
    public static string fsButtonOff = "FSButtonOff";

    //string values for server room security level toggle
    public const string strictToggle = "StrictToggle";
    public const string mediumToggle = "MediumToggle";
    public const string looseToggle = "LooseToggle";

    //string values for time velocity toggle
    public const string toggleX1 = "toggleX1";
    public const string toggleX2 = "toggleX2";
    public const string toggleX5 = "toggleX5";
    public const string toggleX10 = "toggleX10";

    ////values for speed multiplier
    //public static int speedMultiplier = 1;

    //values for floor tile map
    public static string floorTileMap = "TM_Floor";

    //string values for ai job
    public static string[] trustedJobs =
    {
        "Operatore SCADA",
        "Operatore HMI",
        "Programmatore PLC",
        "Supervisore"
    };

    //string values for ai job
    public static string[] normalJobs =
    {
        "Stagista",
        "Ospite",
        "Operatore Server"
    };

    //char value for alphabet replacing

    //string value for hud
    public static string hudName = "Hud(Clone)";

    //string value for player prefab name
    public static string playerPrefabName = "Player";

    //string value for ai prefab name
    public static string aiPrefabName = "AI";

    //string value for audio objects in scenes
    public static string[] gameAudio = {"MenuAudio", "GameAudio"};

    //values for threat
    public enum ThreatType
    {
        local,
        remote,
        fakeLocal,
        timeEvent
    }

    public enum ThreatAttacker
    {
        intern,
        external,
        timeEvent
    }

    public enum ThreatStatus
    {
        deployed,
        unarmed,
        timeEvent
    }
    public enum ThreatAttack
    {
        dos,            //remote simple
        phishing,       //remote simple
        replay,         //remote complex
        mitm,           //hybrid simple
        stuxnet,        //hybrid complex
        dragonfly,      //hybrid complex
        malware,        //local simple
        createRemote,   //local complex
        fakeLocal,      //fakeLocal
        timeEvent       //timeEvent
    }

    public enum ThreatDanger
    {
        low,
        medium,
        high,
        fakeLocal,
        timeEvent
    }

    //values for setting server security level
    public enum ServerSecurity
    {
        strict,
        medium,
        loose
    }


    //values for defining ai danger resistance
    public enum AiDangerResistance
    {
        low,
        medium,
        high,
        veryHigh,
        external
    }

    //value for timeEvent standard threat
    public static Threat timeEventThreat = new Threat(-1, ThreatType.timeEvent, -1,
        ThreatAttacker.timeEvent, ThreatDanger.timeEvent, ThreatAttack.timeEvent, null, -1);

    //empty DialogBoxMessage
    public static DialogBoxMessage emptyMessage = new DialogBoxMessage
    {
        head = string.Empty,
        bodyPath = string.Empty,
        backBtn = string.Empty,
        nextBtn = string.Empty
    };

    //Vector3 which define player spawn location
    public static Vector3Int playerSpawn = new Vector3Int(0, 0, 0);

    //Vector3 which define ai spawn location
    public static Vector3Int aiSpawn = new Vector3Int(-16, 4, 0);

    //Vector3 which define destination of interactive sprites
    public static Vector3Int pcDest = new Vector3Int(2, -4, 0);
    public static Vector3Int telDest = new Vector3Int(-7, 2, 0);
    public static Vector3Int serverInDest = new Vector3Int(-3, -15, 0);
    public static Vector3Int serverOutDest = new Vector3Int(-6, -15, 0);

    //Values for sacene index
    public static int loginSceneIndex = 0;
    public static int menuSceneIndex = 1;
    public static int tutorialSceneIndex = 2;
    public static int level1SceneIndex = 3;
    public static int level2SceneIndex = 4;

    //parameters for money earn
    public static float baseEarn = 0.1f;
    //public static float baseEarn = 0f; //DEBUG

    //parameters for firewall cost
    public static float firewallCost = 0.03f;

    //parameters for ids cost
    public static float idsCost = 0.03f;

    //parameters for local security cost
    public static float localSecurityCost = 0.03f;




}