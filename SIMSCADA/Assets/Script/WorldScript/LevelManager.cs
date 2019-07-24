using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
// ReSharper disable IteratorNeverReturns

public class LevelManager : MonoBehaviour
{
    //structure for time event
    [SerializeField] public List<TimeEvent> timeEventList;

    [SerializeField] public List<Threat> deployedThreatList;

    [SerializeField] public List<Threat> remoteThreats;
    [SerializeField] public List<Threat> localThreats;
    [SerializeField] public List<Threat> threatDetectedList;

    private HudManager hudManager;

    private int previousMonth;

    private IEnumerator newMinuteRoutine;
    private IEnumerator newThreatRoutine;
    private IEnumerator remoteIdsRoutine;
    private IEnumerator localIdsRoutine;
    private IEnumerator threatManagementRoutine;

    [SerializeField] public bool isMoneyLoss;
    public Dictionary<StringDb.ThreatAttack, float> moneyLossList;
    [SerializeField] private float totalMoneyLossPerMinute;

    [SerializeField] public float totalMoneyEarnPerMinute;

    [SerializeField] public float totalCostPerMinute;

    [SerializeField] public int simulationSpeedMultiplier;

    [SerializeField] [Range(100, 10000)] private int millisecondsPerMinutes = 500;

    [SerializeField] [Range(500, 3000)] private int threatSpawnBaseTime = 2000;

    public static bool hasReplayDeployed;
    public static bool hasDosDeployed;
    public static bool hasPhishingDeployed;
    public static bool hasMitmDeployed;
    public static bool hasMalwareDeployed;
    public static bool hasStuxnetDeployed;
    public static bool hasDragonflyDeployed;

    public static bool hasPlantChecked;
    public static bool hasMalwareChecked;

    public static bool hasThreatManaged;

    public static bool isFirewallActive;
    public static bool isRemoteIdsActive;
    public static bool isLocalIdsActive;

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == StringDb.menuSceneIndex ||
            SceneManager.GetActiveScene().buildIndex == StringDb.tutorialSceneIndex)
            return;

        //initializing list for managing threat and time event
        timeEventList = new List<TimeEvent>();
        deployedThreatList = new List<Threat>();
        remoteThreats = new List<Threat>();
        localThreats = new List<Threat>();
        threatDetectedList = new List<Threat>();

        moneyLossList = new Dictionary<StringDb.ThreatAttack, float>()
        {
            {StringDb.ThreatAttack.dos, 0f},
            {StringDb.ThreatAttack.phishing, 0f},
            {StringDb.ThreatAttack.replay, 0f},
            {StringDb.ThreatAttack.mitm, 0f},
            {StringDb.ThreatAttack.stuxnet, 0f},
            {StringDb.ThreatAttack.dragonfly, 0f},
            {StringDb.ThreatAttack.malware, 0f}
        };

        SpawnHud();

        //isMoneyEarn = false;
        isMoneyLoss = false;

        hasDosDeployed = false;
        hasPhishingDeployed = false;
        hasReplayDeployed = false;
        hasMitmDeployed = false;
        hasMalwareDeployed = false;
        hasStuxnetDeployed = false;
        hasDragonflyDeployed = false;

        hasPlantChecked = true;
        hasMalwareChecked = true;

        hasThreatManaged = false;

        //check for data saves and eventually load it
        ClassDb.dataLoader.StartDataLoader();

        //start and set gui setting parameters
        if (GameData.firstLaunch)
        {
            SetStartingValues();

            ClassDb.levelMessageManager.StartWelcome();
        }

        StartTimeRoutine();

        //DEBUG
        //---------------------------------------------------------------------------------------------------------------------

        //EVENT TO SET A THREAT TENDENCIES; IF RESEARCH REPORT IS ACTIVE DISPLAY MESSAGE 
        SetMonthlyThreatAttack();

        StartAllCoroutines();

        //---------------------------------------------------------------------------------------------------------------------
    }

    private void Update()
    {
        simulationSpeedMultiplier = StringDb.speedMultiplier;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ClassDb.pauseManager.TogglePauseMenu();
        }

        ////DEBUG
        ////---------------------------------------------------------------------------------------------------------------------
        //if (Input.GetKeyDown(KeyCode.N))
        //    StartCoroutine(GetRemoteThreats());

        //if (Input.GetKeyDown(KeyCode.B))
        //    StartCoroutine(GetLocalThreats());

        //if (Input.GetKeyDown(KeyCode.M))
        //{
        //    Threat threat = ClassDb.threatManager.NewRandomThreat();
        //    InstantiateNewThreat(threat);
        //}

        //if (Input.GetKeyDown(KeyCode.V))
        //    StartCoroutine(GetFakeLocalThreats());
        ////---------------------------------------------------------------------------------------------------------------------

        if (Input.GetMouseButtonDown(1) && ActionButtonManager.buttonEnabled)
        {
            Canvas actionMenu = GameObject.Find(StringDb.actionMenuName).GetComponent<Canvas>();
            ClassDb.prefabManager.ReturnPrefab(actionMenu.gameObject, PrefabManager.actionIndex);
        }

        hudManager.UpdateHud(GameData.date, GameData.money, GameData.successfulThreat,
            GameData.totalThreat, GameData.trustedEmployees, GameData.totalEmployees, GameData.reputation);

        UpdateMinutes();

        GameData.threatSpawnRate = threatSpawnBaseTime / (float)GameData.totalEmployees;
        if (GameData.threatSpawnRate < 10)
        {
            GameData.threatSpawnRate = 10;
        }

    }

    ////DEBUG
    ////---------------------------------------------------------------------------------------------------------------------
    //private IEnumerator GetRemoteThreats()
    //{
    //    for (int i = 0; i < 1; i++)
    //    {
    //        Threat threat = ClassDb.threatManager.NewRemoteThreat();

    //        InstantiateNewThreat(threat);

    //        yield return new WaitForSeconds(3);
    //    }
    //}

    //private IEnumerator GetLocalThreats()
    //{
    //    for (int i = 0; i < 1; i++)
    //    {
    //        Threat threat = ClassDb.threatManager.NewLocalThreat();
    //        InstantiateNewThreat(threat);

    //        yield return new WaitForSeconds(1);
    //    }
    //}

    //private IEnumerator GetFakeLocalThreats()
    //{
    //    for (int i = 0; i < 1; i++)
    //    {
    //        Threat threat = ClassDb.threatManager.NewFakeLocalThreat();
    //        InstantiateNewThreat(threat);

    //        yield return new WaitForSeconds(1);
    //    }
    //}
    ////---------------------------------------------------------------------------------------------------------------------

    public void SpawnHud()
    {
        //instancing the hud
        ClassDb.prefabManager.GetPrefab(ClassDb.prefabManager.prefabHud.gameObject, PrefabManager.hudIndex).GetComponent<Canvas>();
        hudManager = GameObject.Find(StringDb.hudName).GetComponent<HudManager>();
    }

    public void SetStartingValues()
    {
        GameData.date = new DateTime(2019, 01, 01, 08, 00, 00);
        GameData.money = 1000;
        GameData.successfulThreat = 0;
        GameData.totalThreat = 0;
        GameData.trustedEmployees = 10;
        GameData.totalEmployees = 50;
        GameData.reputation = 35;

        GameData.serverSecurity = StringDb.ServerSecurity.medium;
    }

    public void StartTimeRoutine()
    {
        newMinuteRoutine = OnNewMinute();
        StartCoroutine(newMinuteRoutine);
    }

    public void StartAllCoroutines()
    {

        //DEBUG
        //---------------------------------------------------------------------------------------------------------------------

        remoteIdsRoutine = RemoteIdsCheckRoutine();
        StartCoroutine(remoteIdsRoutine);

        localIdsRoutine = LocalIdsCheckRoutine();
        StartCoroutine(localIdsRoutine);

        newThreatRoutine = CreateNewThreat();
        StartCoroutine(newThreatRoutine);

        //---------------------------------------------------------------------------------------------------------------------
        Debug.Log("COROUTINES STARTED");

    }

    private void UpdateMinutes()
    {
        GameData.minutePercentage += simulationSpeedMultiplier * 10000 * Time.fixedDeltaTime * Time.timeScale / millisecondsPerMinutes;
    }

    private IEnumerator OnNewMinute()
    {
        for(;;)
        {
            yield return new WaitUntil(() => GameData.minutePercentage > 1);

            //Debug.Log("DeltaTime: " + Time.fixedDeltaTime + " MinutePercentage: " + StringDb.minutePercentage);

            timeEventList = ClassDb.timeEventManager.UpdateTimeEventList(timeEventList);

            previousMonth = GameData.date.Month;

            GameData.date = GameData.date.AddMinutes(1.0);

            if (GameData.date.Month != previousMonth)
            {
                OnNewMonth();
            }

            totalMoneyEarnPerMinute = StringDb.baseEarn * GameData.totalEmployees;

            if (isMoneyLoss)
            {
                totalMoneyLossPerMinute = moneyLossList.Values.Sum();
                isMoneyLoss = false;
            }

            totalCostPerMinute = 0;

            if (isFirewallActive)
            {
                totalCostPerMinute += StringDb.firewallCost * GameData.totalEmployees;
            }

            if (isRemoteIdsActive)
            {
                totalCostPerMinute += StringDb.idsCost * GameData.totalEmployees;
            }

            if (isLocalIdsActive)
            {
                totalCostPerMinute += StringDb.localSecurityCost * GameData.totalEmployees;
            }

            GameData.money += totalMoneyEarnPerMinute - totalCostPerMinute - totalMoneyLossPerMinute;

            GameData.remoteIdsCheckTime += GameData.minutePercentage;

            GameData.localIdsCheckTime += GameData.minutePercentage;

            GameData.threatSpawnTime += GameData.minutePercentage;

            if (GameData.reputation >= 85 && GameData.money >= 15000)
            {
                GameData.winCounter += GameData.minutePercentage;
            }
            else if (GameData.money <= -10000)
            {
                GameData.lossCounter += GameData.minutePercentage;
            }
            else
            {
                GameData.winCounter = 0;
                GameData.lossCounter = 0;
            }

            CheckEndgame();

            GameData.minutePercentage = 0;
        }
    }

    private void CheckEndgame()
    {
        if (SceneManager.GetActiveScene().buildIndex == StringDb.menuSceneIndex ||
            SceneManager.GetActiveScene().buildIndex == StringDb.tutorialSceneIndex)
            return;

        //if (DialogBoxManager.dialogEnabled) return;

        if (GameData.winCounter >= 150)
        {
            GameData.isGameWon = true;
            ClassDb.levelMessageManager.StartEndGame();
            StopAllCoroutines();
        }

        if (GameData.lossCounter >= 150)
        {
            GameData.isGameWon = false;
            ClassDb.levelMessageManager.StartEndGame();
            StopAllCoroutines();
        }
    }

    private void OnNewMonth()
    {
        //RANDOM EVENTS TO GIVE MONEY
        if (Random.Range(0, 100) < GameData.reputation)
        {
            //Message to inform boss has given more money
            ClassDb.levelMessageManager.StartMoneyEarn(UpdateMoney());
        }


        //EVENT TO SET A THREAT TENDENCIES; IF RESEARCH REPORT IS ACTIVE DISPLAY MESSAGE 
        SetMonthlyThreatAttack();
    }

    private void SetMonthlyThreatAttack()
    {
        StringDb.ThreatAttack attack;
        do
        {
            attack = (StringDb.ThreatAttack) Random.Range(0, 8);
        } while (GameData.monthlyThreat == attack);

        GameData.monthlyThreat = attack;

        if(GameData.researchUpgrade)
            ClassDb.levelMessageManager.StartShowReport(attack.ToString().ToUpper());

        Debug.Log(attack);

        SetRandomizer();

    }

    private void SetRandomizer()
    {
        List<StringDb.ThreatAttack> keys = GameData.weights.Keys.ToList();
        foreach (StringDb.ThreatAttack key in keys)
        {
            if (key == GameData.monthlyThreat)
            {
                GameData.weights[key] = 0.65f;
            }
            else
            {
                GameData.weights[key] = 0.05f;
            }
        }

        GameData.threatRandomizer.AddOrUpdateWeight((int)StringDb.ThreatAttack.dos, GameData.weights[StringDb.ThreatAttack.dos]);
        GameData.threatRandomizer.AddOrUpdateWeight((int)StringDb.ThreatAttack.phishing, GameData.weights[StringDb.ThreatAttack.phishing]);
        GameData.threatRandomizer.AddOrUpdateWeight((int)StringDb.ThreatAttack.replay, GameData.weights[StringDb.ThreatAttack.replay]);
        GameData.threatRandomizer.AddOrUpdateWeight((int)StringDb.ThreatAttack.mitm, GameData.weights[StringDb.ThreatAttack.mitm]);
        GameData.threatRandomizer.AddOrUpdateWeight((int)StringDb.ThreatAttack.stuxnet, GameData.weights[StringDb.ThreatAttack.stuxnet]);
        GameData.threatRandomizer.AddOrUpdateWeight((int)StringDb.ThreatAttack.dragonfly, GameData.weights[StringDb.ThreatAttack.dragonfly]);
        GameData.threatRandomizer.AddOrUpdateWeight((int)StringDb.ThreatAttack.malware, GameData.weights[StringDb.ThreatAttack.malware]);
        GameData.threatRandomizer.AddOrUpdateWeight((int)StringDb.ThreatAttack.createRemote, GameData.weights[StringDb.ThreatAttack.createRemote]);

    }

    private float UpdateMoney()
    {
        float deltaMoney = Random.Range(1f,5f) * GameData.money / 100;
        GameData.money += deltaMoney;

        return deltaMoney;
    }

    private IEnumerator CreateNewThreat()
    {
        //yield return new WaitForSeconds(5);
        for (;;)
        {
            GameData.threatSpawnTime = 0;

            yield return new WaitUntil(() => GameData.threatSpawnTime > GameData.threatSpawnRate);

            yield return new WaitWhile(() =>
                hasDosDeployed ||
                hasPhishingDeployed ||
                hasReplayDeployed ||
                hasMitmDeployed ||
                hasMalwareDeployed ||
                hasStuxnetDeployed ||
                hasDragonflyDeployed);

            //Debug.Log(StringDb.threatSpawnRate);

            NewThreat();
        }
    }

    private void NewThreat()
    {
        Threat threat = ClassDb.threatManager.NewRandomThreat();
        InstantiateNewThreat(threat);
    }

    public void InstantiateNewThreat(Threat threat)
    {
        switch (threat.threatType)
        {
            case StringDb.ThreatType.local:
                //local threat
                StartLocalThreat(threat);
                break;
            case StringDb.ThreatType.remote:
                //remote threat
                StartRemoteThreat(threat);
                break;
            case StringDb.ThreatType.fakeLocal:
                //fake local threat
                StartFakeLocalThreat(threat);
                //moneyEarnList.Add(threat.aiController,threat.moneyLossPerMinute);
                break;
            case StringDb.ThreatType.timeEvent:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        hudManager.UpdateHud(GameData.money, GameData.successfulThreat,
            GameData.totalThreat, GameData.totalEmployees, GameData.reputation);
    }

    private void StartLocalThreat(Threat threat)
    {
        //create aiPrefab and attaching to threat
        threat.aiController = ClassDb.spawnCharacter.SpawnLocalAttackerAi(GameData.lastAiId++);

        TimeEvent timeEvent = ClassDb.timeEventManager.NewTimeEventFromThreat(threat, threat.aiController.gameObject, true, false);

        //add timeEvent created from threat in the timeEventList
        timeEventList.Add(timeEvent);

        threat.aiController.timeEvent = timeEvent;
        threat.aiController.AfterEnable();

        localThreats.Add(threat);

        Debug.Log(threat);
    }

    private void StartRemoteThreat(Threat threat)
    {
        threat.aiController = ClassDb.spawnCharacter.SpawnRemoteAi(GameData.lastAiId++);

        TimeEvent timeEvent = ClassDb.timeEventManager.NewTimeEventFromThreat(threat, threat.aiController.gameObject, false, false);

        //add timeEvent created from threat in the timeEventList
        timeEventList.Add(timeEvent);

        threat.aiController.timeEvent = timeEvent;
        threat.aiController.AfterEnable();

        remoteThreats.Add(threat);

        Debug.Log(threat);
    }

    private void StartFakeLocalThreat(Threat threat)
    {
        //create aiPrefab and attaching to threat
        threat.aiController = ClassDb.spawnCharacter.SpawnLocalNormalAi(GameData.lastAiId++);

        TimeEvent timeEvent = ClassDb.timeEventManager.NewTimeEventFromThreat(threat, threat.aiController.gameObject, true, false);

        //add timeEvent created from threat in the timeEventList
        timeEventList.Add(timeEvent);

        threat.aiController.timeEvent = timeEvent;
        threat.aiController.AfterEnable();

        localThreats.Add(threat);

        Debug.Log(threat);
    }

    public IEnumerator DeployThreat(Threat threat)
    {
        threat.aiController.BeforeDeploy();

        yield return new WaitWhile(() => threat.aiController.pathUpdated);

        //CHECK IF PLAYER IS NEAR OBJECTIVE; IF NOT THREAT NOT ELIGIBLE TO DEPLOY
        if (threat.aiController.ThreatDeployEligibility())
        {
            //DEBUG
            //---------------------------------------------------------------------------------------------------------------------
            //THREAT WITH INTERN ATTACKER; CHECK IF CORRUPTION ATTEMPT IS SUCCESSFUL
            if (threat.threatAttacker == StringDb.ThreatAttacker.intern &&
                (threat.aiController.isTrusted || (int)threat.threatDanger < (int)threat.aiController.dangerResistance))
            {
                if (threat.threatType == StringDb.ThreatType.remote)
                    remoteThreats.Remove(threat);
                else
                    localThreats.Remove(threat);

                GameData.totalThreat += 1;
                UpdateReputation(threat, StringDb.ThreatStatus.unarmed);

                ClassDb.spawnCharacter.RemoveAi(threat.aiController.gameObject);
                ClassDb.levelMessageManager.StartFailedCorruption();
                yield break;
            }

            //REMOTE THREAT; CHECK IF FIREWALL INTERCEPT BEFORE DEPLOY
            if (threat.threatType == StringDb.ThreatType.remote &&
                Random.Range(0, 100) < GameData.firewallSuccessRate && isFirewallActive)
            {
                remoteThreats.Remove(threat);

                GameData.totalThreat += 1;
                UpdateReputation(threat, StringDb.ThreatStatus.unarmed);

                ClassDb.spawnCharacter.RemoveAi(threat.aiController.gameObject);
                ClassDb.levelMessageManager.StartThreatStopped(threat);
                yield break;
            }

            //---------------------------------------------------------------------------------------------------------------------

            bool deployed = BeforeDeployThreat(threat);

            //wait for closing dialog box
            yield return new WaitWhile(() => DialogBoxManager.dialogEnabled);

            if (!deployed) yield break;

            float moneyLoss;

            switch (threat.threatAttack)
            {
                case StringDb.ThreatAttack.dos:
                    //flag to inform about dos attack
                    if (!hasDosDeployed)
                        hasDosDeployed = true;
                    
                    //Stop all time events before setting money loss
                    ClassDb.timeEventManager.StopTimeEventList(timeEventList);

                    ////Stop earning until rebooting is executed
                    //totalMoneyEarnPerMinute = 0f;

                    //Set money loss for rebooting and check flag to start money loss
                    moneyLossList[StringDb.ThreatAttack.dos] += threat.moneyLossPerMinute * totalMoneyEarnPerMinute;
                    isMoneyLoss = true;

                    //Set flag to start evaluate threat management result
                    hasThreatManaged = false;
                    StartThreatManagementResultData(threat);

                    if (GameData.isFirstDos)
                    {
                        GameData.isFirstDos = false;
                        //SHOW THE CORRISPONDENT LESSON
                    }

                    break;

                case StringDb.ThreatAttack.phishing:
                    //flag to inform about dos attack
                    if (!hasPhishingDeployed)
                        hasPhishingDeployed = true;

                    //Calculate money loss according to the formula moneyLossPerMinute * 60 * threat number of hour  
                    moneyLoss = threat.moneyLossPerMinute * 60 * threat.deployTime;

                    //Inform how much money has been lost
                    ClassDb.levelMessageManager.StartMoneyLoss(threat.threatType, moneyLoss);

                    //Decreasing money by moneyloss amount
                    GameData.money -= moneyLoss;

                    hasPhishingDeployed = false;

                    if (GameData.isFirstPhishing)
                    {
                        GameData.isFirstPhishing = false;
                        //SHOW THE CORRISPONDENT LESSON
                        ClassDb.levelMessageManager.StartShowLessonFirstTime(threat);
                        //wait for closing dialog box
                        yield return new WaitWhile(() => DialogBoxManager.dialogEnabled);
                    }
                    break;

                case StringDb.ThreatAttack.replay:
                    //flag to inform about replay attack
                    if (!hasReplayDeployed)
                        hasReplayDeployed = true;

                    //Stop all time events before setting money loss
                    ClassDb.timeEventManager.StopTimeEventList(timeEventList);

                    ////Stop earning until checking is executed
                    //totalMoneyEarnPerMinute = 0f;

                    //flag to inform about check plant by phone
                    hasPlantChecked = false;

                    //set money loss until check network cfg has been executed and check flag to start money loss
                    moneyLossList[StringDb.ThreatAttack.replay] += threat.moneyLossPerMinute * totalMoneyEarnPerMinute;
                    isMoneyLoss = true;

                    //Calculate money loss according to the formula moneyLossPerMinute * 60 * threat number of hour  
                    moneyLoss = threat.moneyLossPerMinute * 60 * threat.deployTime;

                    //Add threat to replyThreat dictionary in order to show information about money loss when check plant
                    TelephoneListener.replayThreats.Add(threat, moneyLoss);

                    //Set flag to start evaluate threat management result
                    hasThreatManaged = false;
                    StartThreatManagementResultData(threat);

                    if (GameData.isFirstReplay)
                    {
                        GameData.isFirstReplay = false;
                        //SHOW THE CORRISPONDENT LESSON
                        ClassDb.levelMessageManager.StartShowLessonFirstTime(threat);
                        //wait for closing dialog box
                        yield return new WaitWhile(() => DialogBoxManager.dialogEnabled);
                    }
                    break;

                case StringDb.ThreatAttack.mitm:
                    //flag to inform about mitm attack
                    if (!hasMitmDeployed)
                        hasMitmDeployed = true;

                    //Stop all time events before setting money loss
                    ClassDb.timeEventManager.StopTimeEventList(timeEventList);

                    ////Stop earning until checking is executed
                    //totalMoneyEarnPerMinute = 0f;

                    //set money loss until check network cfg has been executed and check flag to start money loss
                    moneyLossList[StringDb.ThreatAttack.mitm] += threat.moneyLossPerMinute * totalMoneyEarnPerMinute;
                    isMoneyLoss = true;

                    //Set flag to start evaluate threat management result
                    hasThreatManaged = false;
                    StartThreatManagementResultData(threat);

                    if (GameData.isFirstMitm)
                    {
                        GameData.isFirstMitm = false;
                        //SHOW THE CORRISPONDENT LESSON
                        ClassDb.levelMessageManager.StartShowLessonFirstTime(threat);
                        //wait for closing dialog box
                        yield return new WaitWhile(() => DialogBoxManager.dialogEnabled);
                    }
                    break;

                case StringDb.ThreatAttack.stuxnet:
                    //flag to inform about stuxnet attack
                    if (!hasStuxnetDeployed)
                        hasStuxnetDeployed = true;

                    //Stop all time events before setting money loss
                    ClassDb.timeEventManager.StopTimeEventList(timeEventList);

                    ////Stop earning until scan is executed
                    //totalMoneyEarnPerMinute = 0f;

                    //flag to inform about check plant by phone
                    hasPlantChecked = false;

                    //set money loss until scan has been executed and check flag to start money loss
                    moneyLossList[StringDb.ThreatAttack.stuxnet] += threat.moneyLossPerMinute * totalMoneyEarnPerMinute;
                    isMoneyLoss = true;

                    //Calculate money loss according to the formula moneyLossPerMinute * 60 * threat number of hour  
                    moneyLoss = threat.moneyLossPerMinute * 60 * threat.deployTime;

                    //Add threat to replyThreat dictionary in order to show information about money loss when check plant
                    TelephoneListener.stuxnetThreats.Add(threat, moneyLoss);

                    //Set flag to start evaluate threat management result
                    hasThreatManaged = false;
                    StartThreatManagementResultData(threat);

                    if (GameData.isFirstStuxnet)
                    {
                        GameData.isFirstStuxnet = false;
                        //SHOW THE CORRISPONDENT LESSON
                        ClassDb.levelMessageManager.StartShowLessonFirstTime(threat);
                        //wait for closing dialog box
                        yield return new WaitWhile(() => DialogBoxManager.dialogEnabled);
                    }
                    break;

                case StringDb.ThreatAttack.dragonfly:
                    //flag to inform about dragonfly attack
                    if (!hasDragonflyDeployed)
                        hasDragonflyDeployed = true;

                    //Stop all time events before setting money loss
                    ClassDb.timeEventManager.StopTimeEventList(timeEventList);

                    ////Stop earning until scan is executed
                    //totalMoneyEarnPerMinute = 0f;

                    //flag to inform about check malware
                    hasMalwareChecked = false;

                    //set money loss until scan has been executed and check flag to start money loss
                    moneyLossList[StringDb.ThreatAttack.dragonfly] += threat.moneyLossPerMinute * totalMoneyEarnPerMinute;
                    isMoneyLoss = true;

                    //Calculate money loss according to the formula moneyLossPerMinute * 60 * threat number of hour  
                    moneyLoss = threat.moneyLossPerMinute * 60 * threat.deployTime;

                    //Inform how much money has been lost
                    ClassDb.levelMessageManager.StartMoneyLoss(threat.threatType, moneyLoss);

                    //wait for closing dialog box
                    yield return new WaitWhile(() => DialogBoxManager.dialogEnabled);

                    //Decreasing money by moneyloss amount
                    GameData.money -= moneyLoss;

                    //Set flag to start evaluate threat management result
                    hasThreatManaged = false;
                    StartThreatManagementResultData(threat);

                    if (GameData.isFirstDragonfly)
                    {
                        GameData.isFirstDragonfly = false;
                        //SHOW THE CORRISPONDENT LESSON
                        ClassDb.levelMessageManager.StartShowLessonFirstTime(threat);
                        //wait for closing dialog box
                        yield return new WaitWhile(() => DialogBoxManager.dialogEnabled);
                    }
                    break;

                case StringDb.ThreatAttack.malware:
                    //flag to inform about malware attack
                    if (!hasMalwareDeployed)
                        hasMalwareDeployed = true;

                    //Stop all time events before setting money loss
                    ClassDb.timeEventManager.StopTimeEventList(timeEventList);

                    ////Stop earning until scan is executed
                    //totalMoneyEarnPerMinute = 0f;

                    //set money loss until scan has been executed and check flag to start money loss
                    moneyLossList[StringDb.ThreatAttack.malware] += threat.moneyLossPerMinute * totalMoneyEarnPerMinute;
                    isMoneyLoss = true;

                    //Calculate money loss according to the formula moneyLossPerMinute * 60 * threat number of hour 
                    moneyLoss = threat.moneyLossPerMinute * 60 * threat.deployTime;

                    //Inform how much money has been lost
                    ClassDb.levelMessageManager.StartMoneyLoss(threat.threatType, moneyLoss);

                    //wait for closing dialog box
                    yield return new WaitWhile(() => DialogBoxManager.dialogEnabled);

                    //Decreasing money by moneyloss amount
                    GameData.money -= moneyLoss;

                    //Set flag to start evaluate threat management result
                    hasThreatManaged = false;
                    StartThreatManagementResultData(threat);

                    if (GameData.isFirstMalware)
                    {
                        GameData.isFirstMalware = false;
                        //SHOW THE CORRISPONDENT LESSON
                        ClassDb.levelMessageManager.StartShowLessonFirstTime(threat);
                        //wait for closing dialog box
                        yield return new WaitWhile(() => DialogBoxManager.dialogEnabled);
                    }
                    break;

                case StringDb.ThreatAttack.createRemote:

                    ClassDb.spawnCharacter.RemoveAi(threat.aiController.gameObject);

                    //WAIT BEFORE CREATE NEW THREAT
                    yield return new WaitWhile(() => threat.aiController.pathUpdated);

                    localThreats.Remove(threat);

                    Threat newThreat = ClassDb.threatManager.NewRemoteThreat();

                    InstantiateNewThreat(newThreat);
                    yield break;

                case StringDb.ThreatAttack.timeEvent:
                    break;

                case StringDb.ThreatAttack.fakeLocal:

                    ClassDb.spawnCharacter.RemoveAi(threat.aiController.gameObject);

                    localThreats.Remove(threat);

                    //Calculate money earn according to the formula moneyLossPerMinute * 60 * threat number of hour  
                    int moneyEarn = (int)(threat.moneyLossPerMinute * 60 * threat.deployTime);

                    GameData.money += moneyEarn;

                    //moneyEarnList.Remove(threat.aiController);

                    //isMoneyEarn = true;

                    yield break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            AfterDeployThreat(threat);

        }
        else
        {
            switch (threat.threatType)
            {
                case StringDb.ThreatType.local:
                    threat.aiController.onClickAi = false;

                    StopLocalThreat(threat);

                    ClassDb.spawnCharacter.RemoveAi(threat.aiController.gameObject);

                    ClassDb.levelMessageManager.StartThreatStopped(threat);
                    yield break;

                case StringDb.ThreatType.remote:
                    yield break;

                case StringDb.ThreatType.fakeLocal:
                    threat.aiController.onClickAi = false;

                    StopLocalThreat(threat);

                    ClassDb.spawnCharacter.RemoveAi(threat.aiController.gameObject);

                    ClassDb.levelMessageManager.StartThreatStopped(threat);
                    yield break;

                case StringDb.ThreatType.timeEvent:
                    yield break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    //REMOVE AI AND POP UP A DIALOG BOX
    public bool BeforeDeployThreat(Threat threat)
    {
        float threatSuccessRate = Random.Range(0, 100);

        if (threat.threatType == StringDb.ThreatType.remote && !isFirewallActive)
        {
            return true;
        }

        switch (threat.threatAttack)
        {
            case StringDb.ThreatAttack.dos:
                if (!(threatSuccessRate < GameData.defenseDos)) break;
                ClassDb.spawnCharacter.RemoveAi(threat.aiController.gameObject);
                ClassDb.levelMessageManager.StartThreatStopped(threat);
                return false;

            case StringDb.ThreatAttack.phishing:
                if (!(threatSuccessRate < GameData.defensePhishing)) break;
                ClassDb.spawnCharacter.RemoveAi(threat.aiController.gameObject);
                ClassDb.levelMessageManager.StartThreatStopped(threat);
                return false;

            case StringDb.ThreatAttack.replay:
                if (!(threatSuccessRate < GameData.defenseReplay)) break;
                ClassDb.spawnCharacter.RemoveAi(threat.aiController.gameObject);
                ClassDb.levelMessageManager.StartThreatStopped(threat);
                return false;

            case StringDb.ThreatAttack.mitm:
                if (!(threatSuccessRate < GameData.defenseMitm)) break;
                ClassDb.spawnCharacter.RemoveAi(threat.aiController.gameObject);
                ClassDb.levelMessageManager.StartThreatStopped(threat);
                return false; 

            case StringDb.ThreatAttack.stuxnet:
                if (!(threatSuccessRate < GameData.defenseStuxnet)) break;
                ClassDb.spawnCharacter.RemoveAi(threat.aiController.gameObject);
                ClassDb.levelMessageManager.StartThreatStopped(threat);
                return false;

            case StringDb.ThreatAttack.dragonfly:
                if (!(threatSuccessRate < GameData.defenseDragonfly)) break;
                ClassDb.spawnCharacter.RemoveAi(threat.aiController.gameObject);
                ClassDb.levelMessageManager.StartThreatStopped(threat);
                return false;

            case StringDb.ThreatAttack.malware:
                if (!(threatSuccessRate < GameData.defenseMalware)) break;
                ClassDb.spawnCharacter.RemoveAi(threat.aiController.gameObject);
                ClassDb.levelMessageManager.StartThreatStopped(threat);
                return false;

            case StringDb.ThreatAttack.createRemote:
                if (!(threatSuccessRate < GameData.defenseCreateRemote)) break;
                ClassDb.spawnCharacter.RemoveAi(threat.aiController.gameObject);
                ClassDb.levelMessageManager.StartThreatStopped(threat);
                return false;

            case StringDb.ThreatAttack.fakeLocal:
                break;

            case StringDb.ThreatAttack.timeEvent:
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
        threat.aiController.onClickAi = false;
        ClassDb.spawnCharacter.RemoveAi(threat.aiController.gameObject);
        ClassDb.levelMessageManager.StartThreatDeployed(threat);

        return true;
    }

    public void AfterDeployThreat(Threat threat)
    {
        //THREAT TO DEPLOY
        GameData.successfulThreat += 1;
        GameData.totalThreat += 1;

        UpdateReputation(threat, StringDb.ThreatStatus.deployed);

        if (threat.threatAttacker == StringDb.ThreatAttacker.intern)
        {
            GameData.totalEmployees -= 1;
        }

        hudManager.UpdateLastThreat(threat);
    }

    public void StopLocalThreat(Threat threat)
    {
        try
        {
            localThreats.Remove(threat);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }

        switch (threat.threatType)
        {
            case StringDb.ThreatType.local:
                GameData.totalThreat += 1;
                UpdateReputation(threat, StringDb.ThreatStatus.unarmed);
                if (threat.threatAttacker == StringDb.ThreatAttacker.intern)
                {
                    GameData.totalEmployees -= 1;
                }
                break;

            case StringDb.ThreatType.remote:
                break;

            case StringDb.ThreatType.fakeLocal:
                UpdateReputation(threat, StringDb.ThreatStatus.deployed);
                GameData.totalEmployees -= 1;
                break;

            case StringDb.ThreatType.timeEvent:
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void UpdateReputation(Threat threat, StringDb.ThreatStatus threatStatus)
    {
        float deltaReputation = threat.deployTime * GameData.reputation / 50;
        switch (threatStatus)
        {
            case StringDb.ThreatStatus.deployed:
                GameData.reputation -= deltaReputation;
                break;
            case StringDb.ThreatStatus.unarmed:
                GameData.reputation += deltaReputation;
                break;
            case StringDb.ThreatStatus.timeEvent:
                break;
            default:
                throw new ArgumentOutOfRangeException("threatStatus", threatStatus, null);
        }
    }

    private IEnumerator RemoteIdsCheckRoutine()
    {
        for (;;)
        {
            yield return new WaitWhile(() => !isRemoteIdsActive);

            Debug.Log("IDS ACTIVE");

            yield return new WaitWhile(() => ServerPcListener.isThreatDetected);
            yield return new WaitUntil(() => remoteThreats.Count > 0);
            GameData.remoteIdsCheckTime = 0;

            yield return new WaitUntil(() => GameData.remoteIdsCheckTime > GameData.remoteIdsCheckRate);
            RemoteIdsCheck();
        }
    }

    private void RemoteIdsCheck()
    {
        threatDetectedList.Clear();

        foreach (Threat threat in remoteThreats)
        {
            if(Random.Range(1, 100) < GameData.remoteIdsSuccessRate)
                threatDetectedList.Add(threat);
        }

        if (threatDetectedList.Count <= 0) return;

        ClassDb.levelMessageManager.StartIdsInterception();
        
        ServerPcListener.isThreatDetected = true;
    }

    private IEnumerator LocalIdsCheckRoutine()
    {
        for (;;)
        {
            yield return new WaitWhile(() => !isLocalIdsActive);

            Debug.Log("LOCAL SECURITY ACTIVE");

            yield return new WaitUntil(() => localThreats.Count > 0);
            GameData.localIdsCheckTime = 0;

            yield return new WaitUntil(() => GameData.localIdsCheckTime > GameData.localIdsCheckRate);
            LocalIdsCheck();
        }
    }

    private void LocalIdsCheck()
    {
        if (GameData.localIdsUpgraded)
        {
            foreach (Threat threat in localThreats)
            {
                if (threat.threatType != StringDb.ThreatType.local) continue;
                if (threat.aiController.wrongDestinationCounter <= GameData.localIdsWrongCounter ||
                    threat.aiController.isSuspected) continue;
                threat.aiController.PointOutThreat();
            }
        }
        else
        {
            foreach (Threat threat in localThreats)
            {
                if (threat.aiController.wrongDestinationCounter <= GameData.localIdsWrongCounter ||
                    threat.aiController.isSuspected) continue;
                threat.aiController.PointOutThreat();
            }
        }
    }

    public void StartThreatManagementResultData(Threat threat)
    {
        threatManagementRoutine = ThreatManagementResultData(threat);
        StartCoroutine(threatManagementRoutine);
    }

    public IEnumerator ThreatManagementResultData(Threat threat)
    {
        DateTime deployGameTime = GameData.date;
        DateTime deployRealTime = DateTime.Now;

        yield return new WaitWhile(() => !hasThreatManaged);

        DateTime managedGameTime = GameData.date;
        DateTime managedRealTime = DateTime.Now;

        TimeSpan elapsedGameTime = managedGameTime - deployGameTime;
        TimeSpan elapsedRealTime = managedRealTime - deployRealTime;

        float moneyLoss = (float) elapsedGameTime.TotalMinutes * threat.moneyLossPerMinute;

        ClassDb.levelMessageManager.StartThreatManagementResult(elapsedGameTime, moneyLoss);
        ClassDb.dataCollector.GetThreatData(threat, (float) elapsedRealTime.TotalSeconds);

        //wait to close dialog to continue
        yield return new WaitUntil(() => DialogBoxManager.dialogEnabled);

    }

    public void SetFirewallActive(bool active)
    {
        isFirewallActive = active;
    }

    public void SetRemoteIdsActive(bool active)
    {
        isRemoteIdsActive = active;
    }

    public void SetLocalIdsActive(bool active)
    {
        isLocalIdsActive = active;
    }
}
