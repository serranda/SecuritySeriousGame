using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class TutorialManager : MonoBehaviour
{
    private bool welcome1;
    private bool welcome2;
    private bool welcome3;
    private bool welcome4;
    private bool welcome5;
    private bool welcome6;
    private bool welcome7;
    private bool interactiveObject;
    private bool idsFirewallIps;
    private bool postWelcome;
    private bool remoteAttackMessage;
    private bool localAttackMessage;
    private bool postAttackMessage;
    private bool finalMessage;
    public bool tutorialIsFinished;

    //bool for check if first click on room pc

    public bool roomPcFirstTime = true;
    public bool firstTimeHMIPanel = true;
    public bool firstTimeMarket = true;

    //bool for check if first click on security check
    public bool securityCheckFirstTime = true;

    //bool for check if first click on servveer pc
    public bool serverPcFirstTime = true;

    //bool for check if first click on telephone and notebook
    public bool telephoneFirstTime = true;
    public bool notebookFirstTime = true;

    private HudManager hudManager;

    private IEnumerator newMinuteRoutine;
    private IEnumerator welcome1Routine;
    private IEnumerator welcome2Routine;
    private IEnumerator welcome3Routine;
    private IEnumerator welcome4Routine;
    private IEnumerator welcome5Routine;
    private IEnumerator welcome6Routine;
    private IEnumerator welcome7Routine;
    private IEnumerator interactiveObjectRoutine;
    private IEnumerator idsFirewallIpsRoutine;
    private IEnumerator postWelcomeRoutine;
    private IEnumerator remoteAttackMessageRoutine;
    private IEnumerator localAttackMessageRoutine;
    private IEnumerator postAttackMessageRoutine;
    private IEnumerator finalMessageRoutine;
    private IEnumerator startGameRoutine;

    public TutorialGameData tutorialGameData;

    private void Awake()
    {
        //instancing data for tutorial
        tutorialGameData = new TutorialGameData();
    }

    private void Start()
    {


        //spawn hud prefab
        SpawnHud();

        //start time in game
        StartTimeRoutine();

        //start coroutine for first message
        welcome1Routine = Welcome1Routine();

        StartCoroutine(welcome1Routine);
    }

    private void Update()
    {
        if (hudManager == null) return;
        //gameData.simulationSpeedMultiplier = StaticDb.speedMultiplier;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (tutorialGameData.dialogEnabled)
            {
                //SHOW MESSAGE TO CLOSE ALL THE DIALOG BOX BEFORE GO TO PAUSE
                ClassDb.levelMessageManager.StartCloseDialog();
                return;
            }
            ClassDb.pauseManager.TogglePauseMenu();
        }

        if (Input.GetMouseButtonDown(1) && tutorialGameData.buttonEnabled)
        {
            Canvas actionMenu = GameObject.Find(StaticDb.actionMenuName).GetComponent<Canvas>();
            ClassDb.prefabManager.ReturnPrefab(actionMenu.gameObject, PrefabManager.actionIndex);
        }

        hudManager.UpdateHud(tutorialGameData.date, tutorialGameData.money, tutorialGameData.successfulThreat,
            tutorialGameData.totalThreat, tutorialGameData.trustedEmployees, tutorialGameData.totalEmployees, tutorialGameData.reputation);

        UpdateMinutes();
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

    public void UpdateMinutes()
    {
        tutorialGameData.minutePercentage += tutorialGameData.simulationSpeedMultiplier * 10000 * Time.fixedDeltaTime * Time.timeScale / tutorialGameData.millisecondsPerMinutes;
    }

    public IEnumerator OnNewMinute()
    {
        for (; ; )
        {
            yield return new WaitUntil(() => tutorialGameData.minutePercentage > 1);

            //Debug.Log("DeltaTime: " + Time.fixedDeltaTime + " MinutePercentage: " + StaticDb.minutePercentage);

            tutorialGameData.date = tutorialGameData.date.AddMinutes(1.0);

            tutorialGameData.totalMoneyEarnPerMinute = StaticDb.baseEarn * tutorialGameData.totalEmployees;


            tutorialGameData.totalCostPerMinute = 0;


            if (tutorialGameData.isFirewallActive)
            {
                tutorialGameData.totalCostPerMinute += StaticDb.firewallCost * tutorialGameData.totalEmployees;
            }

            if (tutorialGameData.isRemoteIdsActive)
            {
                tutorialGameData.totalCostPerMinute += StaticDb.idsCost * tutorialGameData.totalEmployees;
            }

            if (tutorialGameData.isLocalIdsActive)
            {
                tutorialGameData.totalCostPerMinute += StaticDb.localSecurityCost * tutorialGameData.totalEmployees;
            }

            tutorialGameData.money += tutorialGameData.totalMoneyEarnPerMinute - tutorialGameData.totalCostPerMinute;

            tutorialGameData.minutePercentage = 0;
        }
    }

    private IEnumerator Welcome1Routine()
    {
        //set flag to stop next coroutine
        welcome1 = false;
        
        //start next corotuine
        welcome2Routine = Welcome2Routine();
        StartCoroutine(welcome2Routine);

        //display message
        ClassDb.tutorialMessageManager.Welcome1Message();

        //wait until dialog has been disabled
        yield return new WaitWhile(() => TutorialDialogBoxManager.dialogEnabled);

        //set flag to start next coroutine
        welcome1 = true;


    }

    private IEnumerator Welcome2Routine()
    {
        //wait unitl previous corutine has finished
        yield return new WaitUntil(() => welcome1);

        //set flag to stop next coroutine
        welcome2 = false;

        //start next corotuine
        welcome3Routine = Welcome3Routine();
        StartCoroutine(welcome3Routine);

        //display message
        ClassDb.tutorialMessageManager.Welcome2Message();

        //wait until dialog has been disabled
        yield return new WaitWhile(() => TutorialDialogBoxManager.dialogEnabled);

        //set flag to start next coroutine
        welcome2 = true;


    }

    private IEnumerator Welcome3Routine()
    {
        //wait unitl previous corutine has finished
        yield return new WaitUntil(() => welcome2);

        //Move up dialog
        ClassDb.tutorialDialogBoxManager.MoveUpDialog();

        //Toggle time
        ClassDb.timeManager.ToggleTime();

        //set flag to stop next coroutine
        welcome3 = false;

        //start next corotuine
        welcome4Routine = Welcome4Routine();
        StartCoroutine(welcome4Routine);

        //display message
        ClassDb.tutorialMessageManager.Welcome3Message();

        //wait until dialog has been disabled
        yield return new WaitWhile(() => TutorialDialogBoxManager.dialogEnabled);

        //set flag to start next coroutine
        welcome3 = true;
    }

    private IEnumerator Welcome4Routine()
    {
        //wait until previous corutine has finished
        yield return new WaitUntil(() => welcome3);

        //set flag to stop next coroutine
        welcome4 = false;

        //start next corotuine
        welcome5Routine = Welcome5Routine();
        StartCoroutine(welcome5Routine);

        //display message
        ClassDb.tutorialMessageManager.Welcome4Message();

        //wait until dialog has been disabled
        yield return new WaitWhile(() => TutorialDialogBoxManager.dialogEnabled);

        //set flag to start next coroutine
        welcome4 = true;
    }

    private IEnumerator Welcome5Routine()
    {
        //wait unitl previous corutine has finished
        yield return new WaitUntil(() => welcome4);

        //set flag to stop next coroutine
        welcome5 = false;

        //start next corotuine
        welcome6Routine = Welcome6Routine();
        StartCoroutine(welcome6Routine);

        //display message
        ClassDb.tutorialMessageManager.Welcome5Message();

        //wait until dialog has been disabled
        yield return new WaitWhile(() => TutorialDialogBoxManager.dialogEnabled);

        //set flag to start next coroutine
        welcome5 = true;
    }

    private IEnumerator Welcome6Routine()
    {
        //wait unitl previous corutine has finished
        yield return new WaitUntil(() => welcome5);

        //set flag to stop next coroutine
        welcome6 = false;

        //start next corotuine
        welcome7Routine = Welcome7Routine();
        StartCoroutine(welcome7Routine);

        //display message
        ClassDb.tutorialMessageManager.Welcome6Message();

        //wait until dialog has been disabled
        yield return new WaitWhile(() => TutorialDialogBoxManager.dialogEnabled);

        //set flag to start next coroutine
        welcome6 = true;
    }

    private IEnumerator Welcome7Routine()
    {
        //wait unitl previous corutine has finished
        yield return new WaitUntil(() => welcome6);

        //Move down dialog
        ClassDb.tutorialDialogBoxManager.MoveDownDialog();

        ////Toggle time
        //ClassDb.timeManager.ToggleTime();

        //set flag to stop next coroutine
        welcome7 = false;

        ////start next corotuine
        idsFirewallIpsRoutine = IdsFirewallIps();
        StartCoroutine(idsFirewallIpsRoutine);


        //display message
        ClassDb.tutorialMessageManager.Welcome7Message();

        //wait until dialog has been disabled
        yield return new WaitWhile(() => TutorialDialogBoxManager.dialogEnabled);

        //set flag to start next coroutine
        welcome7 = true;
    }

    private IEnumerator IdsFirewallIps()
    {
        //wait unitl previous corutine has finished
        yield return new WaitUntil(() => welcome7);

        //wait for click on room pc
        yield return new WaitUntil(() => !roomPcFirstTime);

        //wait for click on scada screen
        yield return new WaitUntil(() => !firstTimeHMIPanel);
        yield return new WaitUntil(() => !tutorialGameData.scadaEnabled);

        //wait for click on store screen
        yield return new WaitUntil(() => !firstTimeMarket);
        yield return new WaitUntil(() => !tutorialGameData.storeEnabled);

        //wait for click on security check
        yield return new WaitUntil(() => !securityCheckFirstTime);

        //wait for click on server pc
        yield return new WaitUntil(() => !serverPcFirstTime);

        //wait for click on telephone
        yield return new WaitUntil(() => !telephoneFirstTime);

        //wait for click on notebook button
        yield return new WaitUntil(() => !notebookFirstTime);
        yield return new WaitUntil(() => !tutorialGameData.noteBookEnabled);

        ////Toggle time
        //ClassDb.timeManager.ToggleTime();

        List<TutorialInteractiveSprite> sprites = FindObjectsOfType<TutorialInteractiveSprite>().ToList();

        foreach (TutorialInteractiveSprite sprite in sprites)
        {
            if (sprite.hasMenu)
            {
                sprite.ToggleMenu();
            }
        }
 
        //set flag to stop next coroutine
        idsFirewallIps = false;

        //start next corotuine
        interactiveObjectRoutine = InteractiveObjectRoutine();
        StartCoroutine(interactiveObjectRoutine);

        //display message
        ClassDb.tutorialMessageManager.IdsFirewallIpsMessage();

        //wait until dialog has been disabled
        yield return new WaitWhile(() => TutorialDialogBoxManager.dialogEnabled);

        //set flag to start next coroutine
        idsFirewallIps = true;
    }

    private IEnumerator InteractiveObjectRoutine()
    {
        //wait unitl previous corutine has finished
        yield return new WaitUntil(() => idsFirewallIps);

        //set flag to stop next coroutine
        interactiveObject = false;

        //start next corotuine
        postWelcomeRoutine = PostWelcomeRoutine();
        StartCoroutine(postWelcomeRoutine);

        //display message
        ClassDb.tutorialMessageManager.InteractiveObjectMessage();

        //wait until dialog has been disabled
        yield return new WaitWhile(() => TutorialDialogBoxManager.dialogEnabled);

        //set flag to start next coroutine
        interactiveObject = true;
    }

    private IEnumerator PostWelcomeRoutine()
    {
        //wait unitl previous corutine has finished
        yield return new WaitUntil(() => interactiveObject);

        ////Move down dialog
        //ClassDb.tutorialDialogBoxManager.MoveDownDialog();

        ////Toggle time
        //ClassDb.timeManager.ToggleTime();

        //set flag to stop next coroutine
        postWelcome = false;

        //start next corotuine
        remoteAttackMessageRoutine = RemoteAttackMessageRoutine();
        StartCoroutine(remoteAttackMessageRoutine);

        //display message
        ClassDb.tutorialMessageManager.PostWelcomeMessage();

        //wait until dialog has been disabled
        yield return new WaitWhile(() => TutorialDialogBoxManager.dialogEnabled);

        //set flag to start next coroutine
        postWelcome = true;
    }

    private IEnumerator RemoteAttackMessageRoutine()
    {
        //wait unitl previous corutine has finished
        yield return new WaitUntil(() => postWelcome);

        ////Move down dialog
        //ClassDb.tutorialDialogBoxManager.MoveDownDialog();

        ////Toggle time
        //ClassDb.timeManager.ToggleTime();

        //set flag to stop next coroutine
        remoteAttackMessage = false;

        //start next corotuine
        localAttackMessageRoutine = LocalAttackMessageRoutine();
        StartCoroutine(localAttackMessageRoutine);

        //display message
        ClassDb.tutorialMessageManager.RemoteAttackMessage();

        //wait until dialog has been disabled
        yield return new WaitWhile(() => TutorialDialogBoxManager.dialogEnabled);

        //set flag to start next coroutine
        remoteAttackMessage = true;
    }

    private IEnumerator LocalAttackMessageRoutine()
    {
        //wait unitl previous corutine has finished
        yield return new WaitUntil(() => remoteAttackMessage);

        ////Move down dialog
        //ClassDb.tutorialDialogBoxManager.MoveDownDialog();

        ////Toggle time
        //ClassDb.timeManager.ToggleTime();

        //set flag to stop next coroutine
        localAttackMessage = false;

        //start next corotuine
        postAttackMessageRoutine = PostAttackMessageRoutine();
        StartCoroutine(postAttackMessageRoutine);

        //display message
        ClassDb.tutorialMessageManager.LocalAttackMessage();

        //wait until dialog has been disabled
        yield return new WaitWhile(() => TutorialDialogBoxManager.dialogEnabled);

        //set flag to start next coroutine
        localAttackMessage = true;
    }

    private IEnumerator PostAttackMessageRoutine()
    {
        //wait unitl previous corutine has finished
        yield return new WaitUntil(() => localAttackMessage);

        ////Move down dialog
        //ClassDb.tutorialDialogBoxManager.MoveDownDialog();

        ////Toggle time
        //ClassDb.timeManager.ToggleTime();

        //set flag to stop next coroutine
        postAttackMessage = false;

        //start next corotuine
        finalMessageRoutine = FinalMessageRoutine();
        StartCoroutine(finalMessageRoutine);

        //display message
        ClassDb.tutorialMessageManager.PostAttackMessage();

        //wait until dialog has been disabled
        yield return new WaitWhile(() => TutorialDialogBoxManager.dialogEnabled);

        //set flag to start next coroutine
        postAttackMessage = true;
    }

    private IEnumerator FinalMessageRoutine()
    {
        //wait unitl previous corutine has finished
        yield return new WaitUntil(() => postAttackMessage);

        //set flag to stop next coroutine
        finalMessage = false;

        //display message
        ClassDb.tutorialMessageManager.FinalMessage();

        //wait until dialog has been disabled
        yield return new WaitWhile(() => TutorialDialogBoxManager.dialogEnabled);

        //set flag to start next coroutine
        finalMessage = true;

        //exit from tutorial and load level 1
        ClassDb.sceneLoader.StartLoadByIndex(StaticDb.level1SceneIndex);
    }

    public void SetFirewallActive(bool active)
    {
        tutorialGameData.isFirewallActive = active;
    }

    public void SetRemoteIdsActive(bool active)
    {
        tutorialGameData.isRemoteIdsActive = active;
    }

    public void SetLocalIdsActive(bool active)
    {
        tutorialGameData.isLocalIdsActive = active;
    }
}
