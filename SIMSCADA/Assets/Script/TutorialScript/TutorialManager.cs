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
    private static bool welcome1;
    private static bool welcome2;
    private static bool welcome3;
    private static bool welcome4;
    private static bool welcome5;
    private static bool welcome6;
    private static bool welcome7;
    private static bool interactiveObject;
    private static bool postWelcome;
    private static bool remoteAttackMessage;
    private static bool localAttackMessage;
    private static bool postAttackMessage;
    private static bool finalMessage;
    public static bool tutorialIsFinished;

    //bool for check if first click on room pc

    public static bool roomPcFirstTime;
    public static bool firstTimeHMIPanel;
    public static bool firstTimeMarket;

    //bool for check if first click on security check
    public static bool securityCheckFirstTime;

    //bool for check if first click on server pc
    public static bool serverPcFirstTime;

    //bool for check if first click on telephone and notebook
    public static bool telephoneFirstTime;
    public static bool notebookFirstTime;

    private HudManager hudManager;

    private IEnumerator welcome1Routine;
    private IEnumerator welcome2Routine;
    private IEnumerator welcome3Routine;
    private IEnumerator welcome4Routine;
    private IEnumerator welcome5Routine;
    private IEnumerator welcome6Routine;
    private IEnumerator welcome7Routine;
    private IEnumerator interactiveObjectRoutine;
    private IEnumerator postWelcomeRoutine;
    private IEnumerator remoteAttackMessageRoutine;
    private IEnumerator localAttackMessageRoutine;
    private IEnumerator postAttackMessageRoutine;
    private IEnumerator finalMessageRoutine;
    private IEnumerator startGameRoutine;

    private void Start()
    {
        roomPcFirstTime = true;
        firstTimeHMIPanel = true;
        firstTimeMarket = true;

        securityCheckFirstTime = true;

        serverPcFirstTime = true;

        telephoneFirstTime = true;

        notebookFirstTime = true;


        Debug.Log(notebookFirstTime + " Notebook first time");

        //spawn hud prefab
        ClassDb.level1Manager.SpawnHud();

        //set initial values ofr hud
        ClassDb.level1Manager.SetStartingValues();

        //start time in game
        ClassDb.level1Manager.StartTimeRoutine();

        tutorialIsFinished = false;

        //start coroutine for first message
        welcome1Routine = Welcome1Routine();
        StartCoroutine(welcome1Routine);
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
        interactiveObjectRoutine = InteractiveObjectRoutine();
        StartCoroutine(interactiveObjectRoutine);

        //display message
        ClassDb.tutorialMessageManager.Welcome7Message();

        //wait until dialog has been disabled
        yield return new WaitWhile(() => TutorialDialogBoxManager.dialogEnabled);

        //set flag to start next coroutine
        welcome7 = true;
    }

    private IEnumerator InteractiveObjectRoutine()
    {
        //wait unitl previous corutine has finished
        yield return new WaitUntil(() => welcome7);

        //wait for click on room pc
        yield return new WaitUntil(() => !roomPcFirstTime);

        //wait for click on scada screen
        yield return new WaitUntil(() => !firstTimeHMIPanel);
        yield return new WaitUntil(() => !TutorialRoomPcListener.scadaEnabled);

        //wait for click on store screen
        yield return new WaitUntil(() => !firstTimeMarket);
        yield return new WaitUntil(() => !TutorialRoomPcListener.storeEnabled);

        //wait for click on security check
        yield return new WaitUntil(() => !securityCheckFirstTime);

        //wait for click on server pc
        yield return new WaitUntil(() => !serverPcFirstTime);

        //wait for click on telephone
        yield return new WaitUntil(() => !telephoneFirstTime);

        //wait for click on notebook button
        yield return new WaitUntil(() => !notebookFirstTime);
        yield return new WaitUntil(() => !NotebookManager.noteBookEnabled);

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

        ////Move down dialog
        //ClassDb.tutorialDialogBoxManager.MoveDownDialog();

        ////Toggle time
        //ClassDb.timeManager.ToggleTime();

        //set flag to stop next coroutine
        finalMessage = false;

        //start next corotuine
        startGameRoutine = StartGame();
        StartCoroutine(startGameRoutine);

        //display message
        ClassDb.tutorialMessageManager.FinalMessage();

        //wait until dialog has been disabled
        yield return new WaitWhile(() => TutorialDialogBoxManager.dialogEnabled);

        //set flag to start next coroutine
        finalMessage = true;
        
    }

    private IEnumerator StartGame()
    {
        tutorialIsFinished = false;

        //wait unitl previous corutine has finished
        yield return new WaitUntil(() => finalMessage);

        StopAllCoroutines();

        //set initial values ofr hud
        ClassDb.level1Manager.SetStartingValues();

        //Start routine for the game components
        ClassDb.level1Manager.StartAllCoroutines();

        tutorialIsFinished = true;

        SetInteractiveSprite();
    }

    private void SetInteractiveSprite()
    {
        List<TutorialInteractiveSprite> interactiveObjects = FindObjectsOfType<TutorialInteractiveSprite>().ToList();

        foreach (TutorialInteractiveSprite sprite in interactiveObjects)
        {
            sprite.gameObject.AddComponent<InteractiveSprite>();
            Destroy(sprite);
        }

        SetListener();
    }

    private void SetListener()
    {
        List<TutorialTelephoneListener> telephones = FindObjectsOfType<TutorialTelephoneListener>().ToList();
        List<TutorialSecurityListener> securityChecks = FindObjectsOfType<TutorialSecurityListener>().ToList();
        List<TutorialServerPcListener> servers = FindObjectsOfType<TutorialServerPcListener>().ToList();
        List<TutorialRoomPcListener> pcs = FindObjectsOfType<TutorialRoomPcListener>().ToList();

        foreach (TutorialTelephoneListener telephone in telephones)
        {
            telephone.gameObject.AddComponent<TelephoneListener>();
            Destroy(telephone);
        }

        foreach (TutorialSecurityListener securityCheck in securityChecks)
        {
            securityCheck.gameObject.AddComponent<SecurityListener>();
            Destroy(securityCheck);
        }

        foreach (TutorialServerPcListener server in servers)
        {
            server.gameObject.AddComponent<ServerPcListener>();
            Destroy(server);
        }

        foreach (TutorialRoomPcListener pc in pcs)
        {
            pc.gameObject.AddComponent<RoomPcListener>();
            Destroy(pc);
        }
    }

}
