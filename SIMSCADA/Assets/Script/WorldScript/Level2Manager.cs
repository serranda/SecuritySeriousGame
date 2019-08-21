using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;
// ReSharper disable IteratorNeverReturns

public class Level2Manager : MonoBehaviour, ILevelManager
{
    private HudManager hudManager;

    private IEnumerator starterRoutine;
    private IEnumerator newMinuteRoutine;
    private IEnumerator newThreatRoutine;
    private IEnumerator remoteIdsRoutine;
    private IEnumerator localIdsRoutine;
    private IEnumerator threatManagementRoutine;

    public GameData gameData = new GameData();

    private void Start()
    {
        starterRoutine = StarterCoroutine();
        StartCoroutine(starterRoutine);
    }

    private void Update()
    {
        if (!GameDataManager.gameDataLoaded || hudManager == null) return;
        //gameData.simulationSpeedMultiplier = StaticDb.speedMultiplier;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameData.dialogEnabled)
            {
                //SHOW MESSAGE TO CLOSE ALL THE DIALOG BOX BEFORE GO TO PAUSE
                ClassDb.levelMessageManager.StartCloseDialog();
                return;
            }
            ClassDb.pauseManager.TogglePauseMenu();
        }

        //DEBUG
        //---------------------------------------------------------------------------------------------------------------------
        if (Input.GetKeyDown(KeyCode.V))
            StartCoroutine(NewFakeLocalThreats());

        if (Input.GetKeyDown(KeyCode.B))
            StartCoroutine(NewLocalThreats());

        if (Input.GetKeyDown(KeyCode.N))
            StartCoroutine(NewRemoteThreats());

        if (Input.GetKeyDown(KeyCode.M))
        {
            Threat threat = ClassDb.threatManager.NewRandomLevel2Threat();
            InstantiateNewThreat(threat);
        }



        //---------------------------------------------------------------------------------------------------------------------

        if (Input.GetMouseButtonDown(1) && gameData.buttonEnabled)
        {
            Canvas actionMenu = GameObject.Find(StaticDb.actionMenuName).GetComponent<Canvas>();
            ClassDb.prefabManager.ReturnPrefab(actionMenu.gameObject, PrefabManager.actionIndex);
        }

        hudManager.UpdateHud(gameData.date, gameData.money, gameData.successfulThreat,
            gameData.totalThreat, gameData.trustedEmployees, gameData.totalEmployees, gameData.reputation);

        UpdateMinutes();

        gameData.threatSpawnRate = gameData.threatSpawnBaseTime / (float)gameData.totalEmployees;
        if (gameData.threatSpawnRate < 10)
        {
            gameData.threatSpawnRate = 10;
        }

        gameData.longDate = gameData.date.ToFileTimeUtc();
    }

    //DEBUG
    //---------------------------------------------------------------------------------------------------------------------
    private IEnumerator NewRemoteThreats()
    {
        for (int i = 0; i < 1; i++)
        {
            Threat threat = ClassDb.threatManager.NewRemoteThreat();

            InstantiateNewThreat(threat);

            yield return new WaitForSeconds(3);
        }
    }

    private IEnumerator NewLocalThreats()
    {
        for (int i = 0; i < 1; i++)
        {
            Threat threat = ClassDb.threatManager.NewLocalThreat();
            InstantiateNewThreat(threat);

            yield return new WaitForSeconds(1);
        }
    }

    private IEnumerator NewFakeLocalThreats()
    {
        for (int i = 0; i < 1; i++)
        {
            Threat threat = ClassDb.threatManager.NewFakeLocalThreat();
            InstantiateNewThreat(threat);

            yield return new WaitForSeconds(1);
        }
    }

    //---------------------------------------------------------------------------------------------------------------------

    public IEnumerator StarterCoroutine()
    {
        gameData.indexSlot = StaticDb.indexSlot;

        //check for data saves and eventually load it
        ClassDb.gameDataManager.StartLoadLevelGameData();

        yield return new WaitUntil(() => GameDataManager.gameDataLoaded);

        SpawnHud();


        //start and set gui setting parameters
        if (!gameData.firstLaunch && gameData.levelIndex == 2)
        {
            RestorePrefabs(gameData);
        }
        else
        {
            ClassDb.levelMessageManager.StartWelcome();

            //EVENT TO SET A THREAT TENDENCIES; IF RESEARCH REPORT IS ACTIVE DISPLAY MESSAGE 
            SetMonthlyThreatAttack();
        }


        StartTimeRoutine();

        //DEBUG
        //---------------------------------------------------------------------------------------------------------------------

        StartAllCoroutines();

        //---------------------------------------------------------------------------------------------------------------------
    }

    public void SpawnHud()
    {
        //instancing the hud
        ClassDb.prefabManager.GetPrefab(ClassDb.prefabManager.prefabHud.gameObject, PrefabManager.hudIndex).GetComponent<Canvas>();
        hudManager = GameObject.Find(StaticDb.hudName).GetComponent<HudManager>();
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

        //newThreatRoutine = CreateNewThreat();
        //StartCoroutine(newThreatRoutine);

        //---------------------------------------------------------------------------------------------------------------------
        Debug.Log("COROUTINES STARTED");

    }

    public void UpdateMinutes()
    {
        gameData.minutePercentage += gameData.simulationSpeedMultiplier * 10000 * Time.fixedDeltaTime * Time.timeScale / gameData.millisecondsPerMinutes;
    }

    public IEnumerator OnNewMinute()
    {
        for (; ; )
        {
            yield return new WaitUntil(() => gameData.minutePercentage > 1);

            //Debug.Log("DeltaTime: " + Time.fixedDeltaTime + " MinutePercentage: " + StaticDb.minutePercentage);

            gameData.timeEventList = ClassDb.timeEventManager.UpdateTimeEventList(gameData.timeEventList);

            gameData.previousMonth = gameData.date.Month;

            gameData.date = gameData.date.AddMinutes(1.0);

            if (gameData.date.Month != gameData.previousMonth)
            {
                OnNewMonth();
            }

            gameData.totalMoneyEarnPerMinute = StaticDb.baseEarn * gameData.totalEmployees;

            //if (gameData.isMoneyLoss)
            //{
            //    gameData.totalMoneyLossPerMinute = gameData.moneyLossList.Values.Sum();
            //    gameData.isMoneyLoss = false;
            //}

            gameData.totalMoneyLossPerMinute = gameData.moneyLossList.Values.Sum();


            gameData.totalCostPerMinute = 0;

            if (gameData.isFirewallActive)
            {
                gameData.totalCostPerMinute += StaticDb.firewallCost * gameData.totalEmployees;
            }

            if (gameData.isRemoteIdsActive)
            {
                gameData.totalCostPerMinute += StaticDb.idsCost * gameData.totalEmployees;
            }

            if (gameData.isLocalIdsActive)
            {
                gameData.totalCostPerMinute += StaticDb.localSecurityCost * gameData.totalEmployees;
            }

            gameData.money += gameData.totalMoneyEarnPerMinute - gameData.totalCostPerMinute - gameData.totalMoneyLossPerMinute;

            gameData.remoteIdsCheckTime += gameData.minutePercentage;

            gameData.localIdsCheckTime += gameData.minutePercentage;

            gameData.threatSpawnTime += gameData.minutePercentage;

            if (gameData.reputation >= 85 && gameData.money >= 15000)
            {
                gameData.winCounter += gameData.minutePercentage;
            }
            else if (gameData.money <= -10000)
            {
                gameData.lossCounter += gameData.minutePercentage;
            }
            else
            {
                gameData.winCounter = 0;
                gameData.lossCounter = 0;
            }

            CheckEndgame();

            gameData.minutePercentage = 0;
        }
    }

    public void CheckEndgame()
    {
        if (SceneManager.GetActiveScene().buildIndex == StaticDb.menuSceneIndex ||
            SceneManager.GetActiveScene().buildIndex == StaticDb.tutorialSceneIndex)
            return;

        //if (DialogBoxManager.dialogEnabled) return;

        if (gameData.winCounter >= 150)
        {
            gameData.isGameWon = true;
            ClassDb.levelMessageManager.StartEndGame();
            StopAllCoroutines();
        }

        if (gameData.lossCounter >= 150)
        {
            gameData.isGameWon = false;
            ClassDb.levelMessageManager.StartEndGame();
            StopAllCoroutines();
        }
    }

    public void OnNewMonth()
    {
        //RANDOM EVENTS TO GIVE MONEY
        if (Random.Range(0, 100) < gameData.reputation)
        {
            //Message to inform boss has given more money
            ClassDb.levelMessageManager.StartMoneyEarnTrue(UpdateMoney());
        }


        //EVENT TO SET A THREAT TENDENCIES; IF RESEARCH REPORT IS ACTIVE DISPLAY MESSAGE 
        SetMonthlyThreatAttack();
    }

    public void SetMonthlyThreatAttack()
    {
        StaticDb.ThreatAttack attack;
        do
        {
            attack = (StaticDb.ThreatAttack)Random.Range(0, 8);
        } while (gameData.monthlyThreat == attack);

        gameData.monthlyThreat = attack;

        if (gameData.researchUpgrade)
            ClassDb.levelMessageManager.StartShowReport(attack.ToString().ToUpper());

        Debug.Log(attack);

        SetRandomizer();

    }

    public void SetRandomizer()
    {
        List<StaticDb.ThreatAttack> keys = gameData.weights.Keys.ToList();
        foreach (StaticDb.ThreatAttack key in keys)
        {
            if (key == gameData.monthlyThreat)
            {
                gameData.weights[key] = 0.65f;
            }
            else
            {
                gameData.weights[key] = 0.05f;
            }
        }

        gameData.threatRandomizer.AddOrUpdateWeight((int)StaticDb.ThreatAttack.dos, gameData.weights[StaticDb.ThreatAttack.dos]);
        gameData.threatRandomizer.AddOrUpdateWeight((int)StaticDb.ThreatAttack.phishing, gameData.weights[StaticDb.ThreatAttack.phishing]);
        gameData.threatRandomizer.AddOrUpdateWeight((int)StaticDb.ThreatAttack.replay, gameData.weights[StaticDb.ThreatAttack.replay]);
        gameData.threatRandomizer.AddOrUpdateWeight((int)StaticDb.ThreatAttack.mitm, gameData.weights[StaticDb.ThreatAttack.mitm]);
        gameData.threatRandomizer.AddOrUpdateWeight((int)StaticDb.ThreatAttack.stuxnet, gameData.weights[StaticDb.ThreatAttack.stuxnet]);
        gameData.threatRandomizer.AddOrUpdateWeight((int)StaticDb.ThreatAttack.dragonfly, gameData.weights[StaticDb.ThreatAttack.dragonfly]);
        gameData.threatRandomizer.AddOrUpdateWeight((int)StaticDb.ThreatAttack.malware, gameData.weights[StaticDb.ThreatAttack.malware]);
        gameData.threatRandomizer.AddOrUpdateWeight((int)StaticDb.ThreatAttack.createRemote, gameData.weights[StaticDb.ThreatAttack.createRemote]);

    }

    public float UpdateMoney()
    {
        float deltaMoney = Random.Range(1f, 5f) * gameData.money / 100;
        gameData.money += deltaMoney;

        return deltaMoney;
    }

    public IEnumerator CreateNewThreat()
    {
        //yield return new WaitForSeconds(5);
        for (; ; )
        {
            gameData.threatSpawnTime = 0;

            yield return new WaitUntil(() => gameData.threatSpawnTime > gameData.threatSpawnRate);

            //yield return new WaitWhile(() => gameData.hasDosDeployed || gameData.hasPhishingDeployed || gameData.hasReplayDeployed || gameData.hasMitmDeployed || gameData.hasMalwareDeployed || gameData.hasStuxnetDeployed || gameData.hasDragonflyDeployed);
            yield return new WaitUntil(() =>gameData.hasThreatDeployed);

            //Debug.Log(StaticDb.threatSpawnRate);

            NewThreat();
        }
    }

    public void NewThreat()
    {
        Threat threat = ClassDb.threatManager.NewRandomLevel2Threat();
        InstantiateNewThreat(threat);
    }

    public void InstantiateNewThreat(Threat threat)
    {
        switch (threat.threatType)
        {
            case StaticDb.ThreatType.local:
                //local threat
                StartLocalThreat(threat);
                break;
            case StaticDb.ThreatType.remote:
                //remote threat
                StartRemoteThreat(threat);
                break;
            case StaticDb.ThreatType.fakeLocal:
                //fake local threat
                StartFakeLocalThreat(threat);
                //moneyEarnList.Add(threat.aiController,threat.moneyLossPerMinute);
                break;
            case StaticDb.ThreatType.timeEvent:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        hudManager.UpdateHud(gameData.money, gameData.successfulThreat,
            gameData.totalThreat, gameData.totalEmployees, gameData.reputation);
    }

    public void StartLocalThreat(Threat threat)
    {
        //create aiPrefab and attaching to threat
        threat.aiController = ClassDb.spawnCharacter.SpawnAi(++gameData.lastAiId, true);

        TimeEvent timeEvent = ClassDb.timeEventManager.NewTimeEventFromThreat(threat, threat.aiController.gameObject, true, false);

        //add timeEvent created from threat in the timeEventList
        gameData.timeEventList.Add(timeEvent);

        threat.aiController.timeEvent = timeEvent;
        threat.aiController.AfterEnable();

        gameData.localThreats.Add(threat);

        Debug.Log(threat);
    }

    public void StartRemoteThreat(Threat threat)
    {
        //create aiPrefab and attaching to threat
        threat.aiController = ClassDb.spawnCharacter.SpawnAi(++gameData.lastAiId, true);

        TimeEvent timeEvent = ClassDb.timeEventManager.NewTimeEventFromThreat(threat, threat.aiController.gameObject, false, false);

        //add timeEvent created from threat in the timeEventList
        gameData.timeEventList.Add(timeEvent);

        threat.aiController.timeEvent = timeEvent;
        threat.aiController.AfterEnable();

        gameData.remoteThreats.Add(threat);

        Debug.Log(threat);
    }

    public void StartFakeLocalThreat(Threat threat)
    {
        //create aiPrefab and attaching to threat
        threat.aiController = ClassDb.spawnCharacter.SpawnAi(++gameData.lastAiId, false);

        TimeEvent timeEvent = ClassDb.timeEventManager.NewTimeEventFromThreat(threat, threat.aiController.gameObject, true, false);

        //add timeEvent created from threat in the timeEventList
        gameData.timeEventList.Add(timeEvent);

        threat.aiController.timeEvent = timeEvent;
        threat.aiController.AfterEnable();

        gameData.localThreats.Add(threat);

        Debug.Log(threat);
    }

    public IEnumerator DeployThreat(Threat threat)
    {
        if (threat.threatType != StaticDb.ThreatType.remote)
            threat.aiController.BeforeDeploy();

        yield return new WaitWhile(() => threat.aiController.pathUpdated);

        //CHECK IF PLAYER IS NEAR OBJECTIVE; IF NOT THREAT NOT ELIGIBLE TO DEPLOY
        //BYPASS THIS CHECK IF THREAT IS REMOTE
        if (threat.aiController.ThreatDeployEligibility() || threat.threatType == StaticDb.ThreatType.remote)
        {
            //DEBUG
            //---------------------------------------------------------------------------------------------------------------------
            if (threat.threatType != StaticDb.ThreatType.fakeLocal)
            {
                //THREAT WITH INTERN ATTACKER; CHECK IF CORRUPTION ATTEMPT IS SUCCESSFUL
                if (threat.threatAttacker == StaticDb.ThreatAttacker.intern &&
                    (threat.aiController.isTrusted || (int)threat.threatDanger < (int)threat.aiController.dangerResistance))
                {
                    if (threat.threatType == StaticDb.ThreatType.remote)
                        gameData.remoteThreats.Remove(threat);
                    else
                        gameData.localThreats.Remove(threat);

                    gameData.totalThreat += 1;
                    UpdateReputation(threat, StaticDb.ThreatStatus.unarmed);

                    ClassDb.spawnCharacter.RemoveAi(threat.aiController.gameObject);
                    ClassDb.levelMessageManager.StartFailedCorruption();
                    yield break;
                }

                //REMOTE THREAT; CHECK IF FIREWALL INTERCEPT BEFORE DEPLOY
                if (threat.threatType == StaticDb.ThreatType.remote &&
                    Random.Range(0, 100) < gameData.firewallSuccessRate &&
                    gameData.isFirewallActive)
                {
                    gameData.remoteThreats.Remove(threat);

                    gameData.totalThreat += 1;
                    UpdateReputation(threat, StaticDb.ThreatStatus.unarmed);

                    ClassDb.spawnCharacter.RemoveAi(threat.aiController.gameObject);
                    ClassDb.levelMessageManager.StartThreatStopped(threat);
                    yield break;
                }
            }
            //---------------------------------------------------------------------------------------------------------------------

            bool deployed = BeforeDeployThreat(threat);

            //wait for closing dialog box
            yield return new WaitWhile(() => gameData.dialogEnabled);

            if (!deployed) yield break;

            //SET FLAGS TO INFORM ABOUT DEPLOYED THREAT
            gameData.hasThreatDeployed = true;
            gameData.lastThreatDeployed = threat;

            //Set flag to start evaluate threat management result
            gameData.hasThreatManaged = false;
            StartThreatManagementResultData(threat);

            float moneyLoss;

            switch (threat.threatAttack)
            {
                case StaticDb.ThreatAttack.dos:
                    ////flag to inform about dos attack
                    //if (!gameData.hasDosDeployed) gameData.hasDosDeployed = true;

                    //Stop all time events before setting money loss
                    ClassDb.timeEventManager.StopTimeEventList(gameData.timeEventList);

                    ////Stop earning until rebooting is executed
                    //totalMoneyEarnPerMinute = 0f;

                    //Set money loss for rebooting and check flag to start money loss
                    gameData.moneyLossList[StaticDb.ThreatAttack.dos] += threat.moneyLossPerMinute * gameData.totalMoneyEarnPerMinute;
                    //gameData.isMoneyLoss = true;

                    ////Set flag to start evaluate threat management result
                    //gameData.hasThreatManaged = false;
                    //StartThreatManagementResultData(threat);

                    if (gameData.isFirstDos)
                    {
                        gameData.isFirstDos = false;
                        //SHOW THE CORRISPONDENT LESSON
                        ClassDb.levelMessageManager.StartShowLessonFirstTime(threat);
                        //wait for closing dialog box
                        yield return new WaitWhile(() => gameData.dialogEnabled);
                    }

                    break;

                case StaticDb.ThreatAttack.phishing:
                    ////flag to inform about dos attack
                    //if (!gameData.hasPhishingDeployed) gameData.hasPhishingDeployed = true;

                    //Calculate money loss according to the formula moneyLossPerMinute * 60 * threat number of hour  
                    moneyLoss = threat.moneyLossPerMinute * 60 * threat.deployTime;

                    //Inform how much money has been lost
                    ClassDb.levelMessageManager.StartMoneyLoss(threat.threatType, moneyLoss);

                    //Decreasing money by moneyloss amount
                    gameData.money -= moneyLoss;

                    //gameData.hasPhishingDeployed = false;

                    if (gameData.isFirstPhishing)
                    {
                        gameData.isFirstPhishing = false;
                        //SHOW THE CORRISPONDENT LESSON
                        ClassDb.levelMessageManager.StartShowLessonFirstTime(threat);
                        //wait for closing dialog box
                        yield return new WaitWhile(() => gameData.dialogEnabled);
                    }
                    break;

                case StaticDb.ThreatAttack.replay:
                    ////flag to inform about replay attack
                    //if (!gameData.hasReplayDeployed) gameData.hasReplayDeployed = true;

                    //Stop all time events before setting money loss
                    ClassDb.timeEventManager.StopTimeEventList(gameData.timeEventList);

                    ////Stop earning until checking is executed
                    //totalMoneyEarnPerMinute = 0f;

                    //flag to inform about check plant by phone
                    gameData.hasPlantChecked = false;

                    //set money loss until check network cfg has been executed and check flag to start money loss
                    gameData.moneyLossList[StaticDb.ThreatAttack.replay] += threat.moneyLossPerMinute * gameData.totalMoneyEarnPerMinute;
                    //gameData.isMoneyLoss = true;

                    //Calculate money loss according to the formula moneyLossPerMinute * 60 * threat number of hour  
                    moneyLoss = threat.moneyLossPerMinute * 60 * threat.deployTime;

                    ////Add threat to replyThreat dictionary in order to show information about money loss when check plant
                    //TelephoneListener.replayThreats.Add(threat, moneyLoss);

                    ////TODO Set flag to start evaluate threat management result
                    //gameData.hasThreatManaged = false;
                    //StartThreatManagementResultData(threat);

                    if (gameData.isFirstReplay)
                    {
                        gameData.isFirstReplay = false;
                        //SHOW THE CORRISPONDENT LESSON
                        ClassDb.levelMessageManager.StartShowLessonFirstTime(threat);
                        //wait for closing dialog box
                        yield return new WaitWhile(() => gameData.dialogEnabled);
                    }
                    break;

                case StaticDb.ThreatAttack.mitm:
                    ////flag to inform about mitm attack
                    //if (!gameData.hasMitmDeployed) gameData.hasMitmDeployed = true;

                    //Stop all time events before setting money loss
                    ClassDb.timeEventManager.StopTimeEventList(gameData.timeEventList);

                    ////Stop earning until checking is executed
                    //totalMoneyEarnPerMinute = 0f;

                    //set money loss until check network cfg has been executed and check flag to start money loss
                    gameData.moneyLossList[StaticDb.ThreatAttack.mitm] += threat.moneyLossPerMinute * gameData.totalMoneyEarnPerMinute;
                    //gameData.isMoneyLoss = true;

                    ////TODO Set flag to start evaluate threat management result
                    //gameData.hasThreatManaged = false;
                    //StartThreatManagementResultData(threat);

                    if (gameData.isFirstMitm)
                    {
                        gameData.isFirstMitm = false;
                        //SHOW THE CORRISPONDENT LESSON
                        ClassDb.levelMessageManager.StartShowLessonFirstTime(threat);
                        //wait for closing dialog box
                        yield return new WaitWhile(() => gameData.dialogEnabled);
                    }
                    break;

                case StaticDb.ThreatAttack.stuxnet:
                    ////flag to inform about stuxnet attack
                    //if (!gameData.hasStuxnetDeployed) gameData.hasStuxnetDeployed = true;

                    //Stop all time events before setting money loss
                    ClassDb.timeEventManager.StopTimeEventList(gameData.timeEventList);

                    ////Stop earning until scan is executed
                    //totalMoneyEarnPerMinute = 0f;

                    //flag to inform about check plant by phone
                    gameData.hasPlantChecked = false;

                    //set money loss until scan has been executed and check flag to start money loss
                    gameData.moneyLossList[StaticDb.ThreatAttack.stuxnet] += threat.moneyLossPerMinute * gameData.totalMoneyEarnPerMinute;
                    //gameData.isMoneyLoss = true;

                    //Calculate money loss according to the formula moneyLossPerMinute * 60 * threat number of hour  
                    moneyLoss = threat.moneyLossPerMinute * 60 * threat.deployTime;

                    ////Add threat to replyThreat dictionary in order to show information about money loss when check plant
                    //TelephoneListener.stuxnetThreats.Add(threat, moneyLoss);

                    ////TODO Set flag to start evaluate threat management result
                    //gameData.hasThreatManaged = false;
                    //StartThreatManagementResultData(threat);

                    if (gameData.isFirstStuxnet)
                    {
                        gameData.isFirstStuxnet = false;
                        //SHOW THE CORRISPONDENT LESSON
                        ClassDb.levelMessageManager.StartShowLessonFirstTime(threat);
                        //wait for closing dialog box
                        yield return new WaitWhile(() => gameData.dialogEnabled);
                    }
                    break;

                case StaticDb.ThreatAttack.dragonfly:
                    ////flag to inform about dragonfly attack
                    //if (!gameData.hasDragonflyDeployed) gameData.hasDragonflyDeployed = true;

                    //Stop all time events before setting money loss
                    ClassDb.timeEventManager.StopTimeEventList(gameData.timeEventList);

                    ////Stop earning until scan is executed
                    //totalMoneyEarnPerMinute = 0f;

                    //flag to inform about check malware
                    gameData.hasMalwareChecked = false;

                    //set money loss until scan has been executed and check flag to start money loss
                    gameData.moneyLossList[StaticDb.ThreatAttack.dragonfly] += threat.moneyLossPerMinute * gameData.totalMoneyEarnPerMinute;
                    //gameData.isMoneyLoss = true;

                    //Calculate money loss according to the formula moneyLossPerMinute * 60 * threat number of hour  
                    moneyLoss = threat.moneyLossPerMinute * 60 * threat.deployTime;

                    //Inform how much money has been lost
                    ClassDb.levelMessageManager.StartMoneyLoss(threat.threatType, moneyLoss);

                    //wait for closing dialog box
                    yield return new WaitWhile(() => gameData.dialogEnabled);

                    //Decreasing money by moneyloss amount
                    gameData.money -= moneyLoss;

                    ////TODO Set flag to start evaluate threat management result
                    //gameData.hasThreatManaged = false;
                    //StartThreatManagementResultData(threat);

                    if (gameData.isFirstDragonfly)
                    {
                        gameData.isFirstDragonfly = false;
                        //SHOW THE CORRISPONDENT LESSON
                        ClassDb.levelMessageManager.StartShowLessonFirstTime(threat);
                        //wait for closing dialog box
                        yield return new WaitWhile(() => gameData.dialogEnabled);
                    }
                    break;

                case StaticDb.ThreatAttack.malware:
                    ////flag to inform about malware attack
                    //if (!gameData.hasMalwareDeployed) gameData.hasMalwareDeployed = true;

                    //Stop all time events before setting money loss
                    ClassDb.timeEventManager.StopTimeEventList(gameData.timeEventList);

                    ////Stop earning until scan is executed
                    //totalMoneyEarnPerMinute = 0f;

                    //set money loss until scan has been executed and check flag to start money loss
                    gameData.moneyLossList[StaticDb.ThreatAttack.malware] += threat.moneyLossPerMinute * gameData.totalMoneyEarnPerMinute;
                    //gameData.isMoneyLoss = true;

                    //Calculate money loss according to the formula moneyLossPerMinute * 60 * threat number of hour 
                    moneyLoss = threat.moneyLossPerMinute * 60 * threat.deployTime;

                    //Inform how much money has been lost
                    ClassDb.levelMessageManager.StartMoneyLoss(threat.threatType, moneyLoss);

                    //wait for closing dialog box
                    yield return new WaitWhile(() => gameData.dialogEnabled);

                    //Decreasing money by moneyloss amount
                    gameData.money -= moneyLoss;

                    ////TODO Set flag to start evaluate threat management result
                    //gameData.hasThreatManaged = false;
                    //StartThreatManagementResultData(threat);

                    if (gameData.isFirstMalware)
                    {
                        gameData.isFirstMalware = false;
                        //SHOW THE CORRISPONDENT LESSON
                        ClassDb.levelMessageManager.StartShowLessonFirstTime(threat);
                        //wait for closing dialog box
                        yield return new WaitWhile(() => gameData.dialogEnabled);
                    }
                    break;

                case StaticDb.ThreatAttack.createRemote:

                    ClassDb.spawnCharacter.RemoveAi(threat.aiController.gameObject);

                    //WAIT BEFORE CREATE NEW THREAT
                    yield return new WaitWhile(() => threat.aiController.pathUpdated);

                    gameData.localThreats.Remove(threat);

                    Threat newThreat = ClassDb.threatManager.NewRemoteThreat();

                    InstantiateNewThreat(newThreat);
                    yield break;

                case StaticDb.ThreatAttack.timeEvent:
                    break;

                case StaticDb.ThreatAttack.fakeLocal:

                    ClassDb.spawnCharacter.RemoveAi(threat.aiController.gameObject);

                    gameData.localThreats.Remove(threat);

                    //Calculate money earn according to the formula moneyLossPerMinute * 60 * threat number of hour  
                    int moneyEarn = (int)(threat.moneyLossPerMinute * 60 * threat.deployTime);

                    gameData.money += moneyEarn;

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
                case StaticDb.ThreatType.local:
                    threat.aiController.onClickAi = false;

                    StopLocalThreat(threat);

                    ClassDb.spawnCharacter.RemoveAi(threat.aiController.gameObject);

                    ClassDb.levelMessageManager.StartThreatStopped(threat);
                    yield break;

                case StaticDb.ThreatType.remote:
                    yield break;

                case StaticDb.ThreatType.fakeLocal:
                    threat.aiController.onClickAi = false;

                    StopLocalThreat(threat);

                    ClassDb.spawnCharacter.RemoveAi(threat.aiController.gameObject);

                    ClassDb.levelMessageManager.StartThreatStopped(threat);
                    yield break;

                case StaticDb.ThreatType.timeEvent:
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

        if (threat.threatType == StaticDb.ThreatType.remote && !gameData.isFirewallActive)
        {
            return true;
        }

        switch (threat.threatAttack)
        {
            case StaticDb.ThreatAttack.dos:
                if (!(threatSuccessRate < gameData.defenseDos)) break;
                ClassDb.spawnCharacter.RemoveAi(threat.aiController.gameObject);
                ClassDb.levelMessageManager.StartThreatStopped(threat);
                return false;

            case StaticDb.ThreatAttack.phishing:
                if (!(threatSuccessRate < gameData.defensePhishing)) break;
                ClassDb.spawnCharacter.RemoveAi(threat.aiController.gameObject);
                ClassDb.levelMessageManager.StartThreatStopped(threat);
                return false;

            case StaticDb.ThreatAttack.replay:
                if (!(threatSuccessRate < gameData.defenseReplay)) break;
                ClassDb.spawnCharacter.RemoveAi(threat.aiController.gameObject);
                ClassDb.levelMessageManager.StartThreatStopped(threat);
                return false;

            case StaticDb.ThreatAttack.mitm:
                if (!(threatSuccessRate < gameData.defenseMitm)) break;
                ClassDb.spawnCharacter.RemoveAi(threat.aiController.gameObject);
                ClassDb.levelMessageManager.StartThreatStopped(threat);
                return false;

            case StaticDb.ThreatAttack.stuxnet:
                if (!(threatSuccessRate < gameData.defenseStuxnet)) break;
                ClassDb.spawnCharacter.RemoveAi(threat.aiController.gameObject);
                ClassDb.levelMessageManager.StartThreatStopped(threat);
                return false;

            case StaticDb.ThreatAttack.dragonfly:
                if (!(threatSuccessRate < gameData.defenseDragonfly)) break;
                ClassDb.spawnCharacter.RemoveAi(threat.aiController.gameObject);
                ClassDb.levelMessageManager.StartThreatStopped(threat);
                return false;

            case StaticDb.ThreatAttack.malware:
                if (!(threatSuccessRate < gameData.defenseMalware)) break;
                ClassDb.spawnCharacter.RemoveAi(threat.aiController.gameObject);
                ClassDb.levelMessageManager.StartThreatStopped(threat);
                return false;

            case StaticDb.ThreatAttack.createRemote:
                if (!(threatSuccessRate < gameData.defenseCreateRemote)) break;
                ClassDb.spawnCharacter.RemoveAi(threat.aiController.gameObject);
                ClassDb.levelMessageManager.StartThreatStopped(threat);
                return false;

            case StaticDb.ThreatAttack.fakeLocal:
                break;

            case StaticDb.ThreatAttack.timeEvent:
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
        gameData.successfulThreat += 1;
        gameData.totalThreat += 1;

        UpdateReputation(threat, StaticDb.ThreatStatus.deployed);

        if (threat.threatAttacker == StaticDb.ThreatAttacker.intern)
        {
            gameData.totalEmployees -= 1;
        }

        hudManager.UpdateLastThreat(threat);
    }

    public void StopLocalThreat(Threat threat)
    {
        try
        {
            gameData.localThreats.Remove(threat);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }

        switch (threat.threatType)
        {
            case StaticDb.ThreatType.local:
                gameData.totalThreat += 1;
                UpdateReputation(threat, StaticDb.ThreatStatus.unarmed);
                if (threat.threatAttacker == StaticDb.ThreatAttacker.intern)
                {
                    gameData.totalEmployees -= 1;
                }
                break;

            case StaticDb.ThreatType.remote:
                break;

            case StaticDb.ThreatType.fakeLocal:
                UpdateReputation(threat, StaticDb.ThreatStatus.deployed);
                gameData.totalEmployees -= 1;
                break;

            case StaticDb.ThreatType.timeEvent:
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void UpdateReputation(Threat threat, StaticDb.ThreatStatus threatStatus)
    {
        float deltaReputation = threat.deployTime * gameData.reputation / 50;
        switch (threatStatus)
        {
            case StaticDb.ThreatStatus.deployed:
                gameData.reputation -= deltaReputation;
                break;
            case StaticDb.ThreatStatus.unarmed:
                gameData.reputation += deltaReputation;
                break;
            case StaticDb.ThreatStatus.timeEvent:
                break;
            default:
                throw new ArgumentOutOfRangeException("threatStatus", threatStatus, null);
        }
    }

    public IEnumerator RemoteIdsCheckRoutine()
    {
        for (; ; )
        {
            yield return new WaitWhile(() => !gameData.isRemoteIdsActive);

            Debug.Log("IDS ACTIVE");

            yield return new WaitWhile(() => ServerPcListener.isThreatDetected);
            yield return new WaitUntil(() => gameData.remoteThreats.Count > 0);
            gameData.remoteIdsCheckTime = 0;

            yield return new WaitUntil(() => gameData.remoteIdsCheckTime > gameData.remoteIdsCheckRate);
            RemoteIdsCheck();
        }
    }

    public void RemoteIdsCheck()
    {
        gameData.threatDetectedList.Clear();

        foreach (Threat threat in gameData.remoteThreats)
        {
            if (Random.Range(1, 100) < gameData.remoteIdsSuccessRate)
                gameData.threatDetectedList.Add(threat);
        }

        if (gameData.threatDetectedList.Count <= 0) return;

        ClassDb.levelMessageManager.StartIdsInterception();

        ServerPcListener.isThreatDetected = true;
    }

    public IEnumerator LocalIdsCheckRoutine()
    {
        for (; ; )
        {
            yield return new WaitWhile(() => !gameData.isLocalIdsActive);

            Debug.Log("LOCAL SECURITY ACTIVE");

            yield return new WaitUntil(() => gameData.localThreats.Count > 0);
            gameData.localIdsCheckTime = 0;

            yield return new WaitUntil(() => gameData.localIdsCheckTime > gameData.localIdsCheckRate);
            LocalIdsCheck();
        }
    }

    public void LocalIdsCheck()
    {
        if (gameData.localIdsUpgraded)
        {
            foreach (Threat threat in gameData.localThreats)
            {
                if (threat.threatType != StaticDb.ThreatType.local) continue;
                if (threat.aiController.wrongDestinationCounter <= gameData.localIdsWrongCounter ||
                    threat.aiController.isSuspected) continue;
                threat.aiController.PointOutThreat();
            }
        }
        else
        {
            foreach (Threat threat in gameData.localThreats)
            {
                if (threat.aiController.wrongDestinationCounter <= gameData.localIdsWrongCounter ||
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
        DateTime deployGameTime = gameData.date;
        DateTime deployRealTime = DateTime.Now;

        yield return new WaitWhile(() => !gameData.hasThreatManaged);

        DateTime managedGameTime = gameData.date;
        DateTime managedRealTime = DateTime.Now;

        TimeSpan elapsedGameTime = managedGameTime - deployGameTime;
        TimeSpan elapsedRealTime = managedRealTime - deployRealTime;

        float moneyLoss = (float)elapsedGameTime.TotalMinutes * threat.moneyLossPerMinute;

        ClassDb.levelMessageManager.StartThreatManagementResult(elapsedGameTime, moneyLoss);
        ClassDb.threatChartManager.GetThreatData(threat, (float)elapsedRealTime.TotalSeconds);

        //wait to close dialog to continue
        yield return new WaitUntil(() => gameData.dialogEnabled);

    }

    public void SetFirewallActive(bool active)
    {
        gameData.isFirewallActive = active;
    }

    public void SetRemoteIdsActive(bool active)
    {
        gameData.isRemoteIdsActive = active;
    }

    public void SetLocalIdsActive(bool active)
    {
        gameData.isLocalIdsActive = active;
    }

    public GameData GetGameData()
    {
        return gameData;
    }

    public void SetGameData(GameData data)
    {
        gameData = data;
    }

    public void RestorePrefabs(GameData data)
    {
        //CHECK WHICH PREFAB SHOULD BE RESTORED
        //PREFAB TO CHECK:

        //STORESCREEN
        if (data.storeEnabled)
        {
            data.storeEnabled = false;
            FindObjectOfType<RoomPcListener>().ToggleStoreScreen();
        }

        //SCADASCREEN
        if (data.scadaEnabled)
        {
            data.scadaEnabled = false;
            FindObjectOfType<RoomPcListener>().ToggleScadaScreen();
        }

        //TIMEVENTS, INCLUDED RESTORE THE PROGRESSBAR, REMOTE AI AND LOCAL AI
        if (data.timeEventList.Count > 0)
        {
            Debug.Log("RESTORING TIME EVENTS");

            List<TimeEvent> threatEvents = new List<TimeEvent>();
            List<TimeEvent> events = new List<TimeEvent>();

            foreach (TimeEvent timeEvent in data.timeEventList)
            {
                if (timeEvent.threat.threatType != StaticDb.ThreatType.timeEvent)
                {
                    threatEvents.Add(timeEvent);
                }
                else
                {
                    events.Add(timeEvent);
                }
            }

            foreach (TimeEvent threatEvent in threatEvents)
            {
                ClassDb.timeEventManager.RestoreThreatTimeEvent(threatEvent);

            }

            foreach (TimeEvent te in events)
            {
                ClassDb.timeEventManager.RestoreTimeEvent(te);

            }

        }

        //SECURITYSCREEN
        if (data.securityScreenEnabled)
        {
            data.securityScreenEnabled = false;
            FindObjectOfType<SecurityListener>().ToggleSecurityScreen();
        }

        //PRESSED ITEM MENU
        if (data.buttonEnabled)
        {
            data.buttonEnabled = false;
            GameObject.Find(data.pressedSprite).GetComponent<InteractiveSprite>().ToggleMenu();
        }

        //IDCARD
        if (data.idCardEnabled)
        {
            data.idCardEnabled = false;
            FindObjectOfType<IdCardManager>().ToggleIdCard();
        }

        //NOTEBOOK
        if (data.noteBookEnabled)
        {
            data.noteBookEnabled = false;
            FindObjectOfType<NotebookManager>().ToggleNoteBook();
        }

        //RESTORE RANDOMIZER WEIGHTS
        SetRandomizer();

        //RESTORE CAMERA ZOOM VALUE
        CinemachineVirtualCamera virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        Debug.Log(data.cameraZoom);
        virtualCamera.m_Lens.OrthographicSize = data.cameraZoom;

    }
}