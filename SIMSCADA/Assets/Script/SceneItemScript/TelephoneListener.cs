using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TelephoneListener : MonoBehaviour, IObjectListener
{
    private InteractiveSprite interactiveSprite;

    private IEnumerator getMoneyRoutine;
    private IEnumerator checkPlantRoutine;
    private IEnumerator coolDownRoutine;

    private ILevelManager manager;

    private void Start()
    {
        manager = SetLevelManager();

        interactiveSprite = GetComponent<InteractiveSprite>();
    }

    private ILevelManager SetLevelManager()
    {
        ILevelManager iManager;
        if (SceneManager.GetActiveScene().buildIndex == StaticDb.level1SceneIndex)
            iManager = FindObjectOfType<Level1Manager>();
        else
            iManager = FindObjectOfType<Level2Manager>();

        return iManager;
    }

    public void SetListeners()
    {
        List<Button> buttons;

        if (manager.GetGameData().hasThreatDeployed)
        {
            buttons = interactiveSprite.actionButtonManager.GetActiveCanvasGroup(3);

            buttons[0].GetComponentInChildren<TextMeshProUGUI>().text = "Richiedi fondi";
            buttons[0].onClick.RemoveAllListeners();
            buttons[0].onClick.AddListener(delegate
            {
                StartGetMoney();
                interactiveSprite.ToggleMenu();
            });

            buttons[1].GetComponentInChildren<TextMeshProUGUI>().text = "Vai alle lezioni";
            buttons[1].onClick.RemoveAllListeners();
            buttons[1].onClick.AddListener(delegate
            {
                ClassDb.notebookManager.ToggleNoteBook();
                interactiveSprite.ToggleMenu();
            });

            buttons[2].GetComponentInChildren<TextMeshProUGUI>().text = "Check dell'impianto";
            buttons[2].onClick.RemoveAllListeners();
            buttons[2].onClick.AddListener(delegate
            {
                StartCheckPlant();
                interactiveSprite.ToggleMenu();
            });
        }
        else
        {
            buttons = interactiveSprite.actionButtonManager.GetActiveCanvasGroup(2);

            buttons[0].GetComponentInChildren<TextMeshProUGUI>().text = "Richiedi fondi";
            buttons[0].onClick.RemoveAllListeners();
            buttons[0].onClick.AddListener(delegate
            {
                StartGetMoney();
                interactiveSprite.ToggleMenu();
            });

            buttons[1].GetComponentInChildren<TextMeshProUGUI>().text = "Vai alle lezioni";
            buttons[1].onClick.RemoveAllListeners();
            buttons[1].onClick.AddListener(delegate
            {
                ClassDb.notebookManager.ToggleNoteBook();
                interactiveSprite.ToggleMenu();
            });
        }


        foreach (Button button in buttons)
        {
            button.interactable = true;
        }
    }

    private void StartGetMoney()
    {
        interactiveSprite.SetInteraction(false);

        TimeEvent progressEvent = ClassDb.timeEventManager.NewTimeEvent(
            manager.GetGameData().telephoneMoneyTime, interactiveSprite.gameObject, true, true,
            StaticDb.getMoneyRoutine);

        manager.GetGameData().timeEventList.Add(progressEvent);

        getMoneyRoutine = GetMoney(progressEvent);
        StartCoroutine(getMoneyRoutine);
    }

    public void RestartGetMoney(TimeEvent progressEvent)
    {
        interactiveSprite.SetInteraction(false);

        getMoneyRoutine = GetMoney(progressEvent);
        StartCoroutine(getMoneyRoutine);
    }

    private IEnumerator GetMoney(TimeEvent progressEvent)
    {
        //WRITE LOG
        ClassDb.logManager.StartWritePlayerLogRoutine(StaticDb.player, StaticDb.logEvent.UserEvent, "STARTED MONEY REQUEST");

        yield return new WaitWhile(() => manager.GetGameData().timeEventList.Contains(progressEvent));

        int successRate = Random.Range(0, 1);

        if (!(successRate >= manager.GetGameData().reputation))
        {
            float deltaMoney = Random.Range(0f, 5f) * manager.GetGameData().money / 100;
            manager.GetGameData().money += deltaMoney;
            ClassDb.levelMessageManager.StartMoneyEarnTrue(deltaMoney);

            //WRITE LOG
            ClassDb.logManager.StartWritePlayerLogRoutine(StaticDb.player, StaticDb.logEvent.GameEvent, "MONEY REQUEST FINE");
        }
        else
        {
            //Debug.Log("NO MONEY");
            ClassDb.levelMessageManager.StartMoneyEarnFalse();

            //WRITE LOG
            ClassDb.logManager.StartWritePlayerLogRoutine(StaticDb.player, StaticDb.logEvent.GameEvent, "MONEY REQUEST NOT FINE");
        }

        StartCoolDown();
    }

    private void StartCoolDown()
    {
        TimeEvent progressEvent = ClassDb.timeEventManager.NewTimeEvent(
            manager.GetGameData().telephoneMoneyCoolDown * 60, interactiveSprite.gameObject, true, false,
            StaticDb.coolDownRoutine);

        manager.GetGameData().timeEventList.Add(progressEvent);

        coolDownRoutine = CoolDown(progressEvent);
        StartCoroutine(coolDownRoutine);
    }

    public void RestartCoolDown(TimeEvent progressEvent)
    {
        interactiveSprite.SetInteraction(false);

        coolDownRoutine = CoolDown(progressEvent);
        StartCoroutine(coolDownRoutine);
    }

    private IEnumerator CoolDown(TimeEvent progressEvent)
    {
        //WRITE LOG
        ClassDb.logManager.StartWritePlayerLogRoutine(StaticDb.player, StaticDb.logEvent.GameEvent, "STARTED MONEY REQUEST COOL DOWN");

        yield return new WaitWhile(() => manager.GetGameData().timeEventList.Contains(progressEvent));

        interactiveSprite.SetInteraction(true);
    }

    private void StartCheckPlant()
    {
        interactiveSprite.SetInteraction(false);

        //REGISTER THE USER ACTION AND CHECK IF THE ACTION IS CORRECT OR WRONG RELATIVELY TO THE THREAT DEPLOYED
        ClassDb.userActionManager.RegisterThreatSolution(new UserAction(StaticDb.ThreatSolution.plantCheck), manager.GetGameData().lastThreatDeployed, false);

        TimeEvent progressEvent = ClassDb.timeEventManager.NewTimeEvent(
            manager.GetGameData().telephoneCheckPlantTime, interactiveSprite.gameObject, true, true,
            StaticDb.checkPlantRoutine);

        manager.GetGameData().timeEventList.Add(progressEvent);

        checkPlantRoutine = CheckPlant(progressEvent);

        StartCoroutine(checkPlantRoutine);
    }

    public void RestartCheckPlant(TimeEvent progressEvent)
    {
        interactiveSprite.SetInteraction(false);

        checkPlantRoutine = CheckPlant(progressEvent);
        StartCoroutine(checkPlantRoutine);
    }


    private IEnumerator CheckPlant(TimeEvent progressEvent)
    {
        //WRITE LOG
        ClassDb.logManager.StartWritePlayerLogRoutine(StaticDb.player, StaticDb.logEvent.UserEvent, "STARTED CHECK PLANT");

        yield return new WaitWhile(() => manager.GetGameData().timeEventList.Contains(progressEvent));

        interactiveSprite.SetInteraction(true);

        float success = Random.Range(0, 100);

        float moneyLoss;

        if (!manager.GetGameData().hasThreatDeployed) yield break;

        if (manager.GetGameData().lastThreatDeployed.threatAttack != StaticDb.ThreatAttack.replay &&
            manager.GetGameData().lastThreatDeployed.threatAttack != StaticDb.ThreatAttack.stuxnet)
        {
            moneyLoss = 0;
            ClassDb.levelMessageManager.StartPlantReport(true, moneyLoss);
            yield break;
        }

        if (!(success > manager.GetGameData().defensePlantResistance))
        {
            moneyLoss = 0;
            ClassDb.levelMessageManager.StartPlantReport(true, moneyLoss);
        }
        else
        {
            //Inform how much money has been lost
            moneyLoss = manager.GetGameData().lastThreatDeployed.moneyLossPerMinute * 60 *
                        manager.GetGameData().lastThreatDeployed.deployTime;

            ClassDb.levelMessageManager.StartPlantReport(false, moneyLoss);
        }

        //wait for closing dialog box
        yield return new WaitWhile(() => manager.GetGameData().dialogEnabled);

        manager.GetGameData().money -= moneyLoss;

        manager.GetGameData().hasPlantChecked = true;
    }
}