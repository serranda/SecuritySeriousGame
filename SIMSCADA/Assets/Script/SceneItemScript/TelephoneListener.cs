using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TelephoneListener : MonoBehaviour
{
    private InteractiveSprite interactiveSprite;

    private IEnumerator moneyRoutine;
    private IEnumerator checkPlantRoutine;

    public bool telephoneOnCoolDown;


    public static Dictionary<Threat, float> replayThreats;
    public static Dictionary<Threat, float> stuxnetThreats;

    private void Start()
    { 
        interactiveSprite = GetComponent<InteractiveSprite>();

        replayThreats = new Dictionary<Threat, float>();
        stuxnetThreats = new Dictionary<Threat, float>();

        telephoneOnCoolDown = false;
    }

    public void SetTelephoneListeners()
    {
        List<Button> buttons;

        if (LevelManager.hasDosDeployed ||
            LevelManager.hasPhishingDeployed ||
            LevelManager.hasReplayDeployed ||
            LevelManager.hasMitmDeployed ||
            LevelManager.hasMalwareDeployed ||
            LevelManager.hasStuxnetDeployed ||
            LevelManager.hasDragonflyDeployed)
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

        int successRate = Random.Range(0, 100);

        TimeEvent progressEvent = ClassDb.timeEventManager.NewTimeEvent(
            GameData.telephoneMoneyTime, interactiveSprite.gameObject, true, true);

        ClassDb.levelManager.timeEventList.Add(progressEvent);

        moneyRoutine = GetMoney(progressEvent, successRate);
        StartCoroutine(moneyRoutine);
    }

    private IEnumerator GetMoney(TimeEvent progressEvent, int successRate)
    {

        yield return new WaitWhile(() => ClassDb.levelManager.timeEventList.Contains(progressEvent));

        if (!(successRate >= GameData.reputation))
        {
            float deltaMoney = Random.Range(0f, 5f) * GameData.money / 100;
            GameData.money += deltaMoney;
            ClassDb.messageManager.StartMoneyEarn(deltaMoney);
        }

        TimeEvent coolDownEvent = ClassDb.timeEventManager.NewTimeEvent(
            GameData.telephoneMoneyCoolDown * 60, interactiveSprite.gameObject, true, false);

        ClassDb.levelManager.timeEventList.Add(coolDownEvent);

        telephoneOnCoolDown = true;

        yield return new WaitWhile(() => ClassDb.levelManager.timeEventList.Contains(coolDownEvent));

        telephoneOnCoolDown = false;

        interactiveSprite.SetInteraction(true);
    }

    private void StartCheckPlant()
    {
        interactiveSprite.SetInteraction(false);

        TimeEvent progressEvent = ClassDb.timeEventManager.NewTimeEvent(
            GameData.telephoneCheckPlantTime, interactiveSprite.gameObject, true, true);

        ClassDb.levelManager.timeEventList.Add(progressEvent);

        checkPlantRoutine = CheckPlant(progressEvent);

        StartCoroutine(checkPlantRoutine);
    }

    public IEnumerator CheckPlant(TimeEvent progressEvent)
    {
        yield return new WaitWhile(() => ClassDb.levelManager.timeEventList.Contains(progressEvent));


        if (LevelManager.hasReplayDeployed)
        {
            foreach (KeyValuePair<Threat, float> pair in replayThreats)
            {
                float success = Random.Range(0, 100);

                if (!(success > GameData.defensePlantResistance)) continue;
                //Inform how much money has been lost
                ClassDb.messageManager.StartMoneyLoss(pair.Key.threatType, pair.Value);

                //wait for closing dialog box
                yield return new WaitWhile(() => DialogBoxManager.dialogEnabled);

                GameData.money -= pair.Value;

            }

            replayThreats.Clear();

        }


        if (LevelManager.hasStuxnetDeployed)
        {
            foreach (KeyValuePair<Threat, float> pair in stuxnetThreats)
            {
                float success = Random.Range(0, 100);

                if (!(success > GameData.defensePlantResistance))
                {
                    ClassDb.messageManager.StartThreatStopped(pair.Key);

                    Debug.Log(success);

                    continue;
                }
                //Inform how much money has been lost
                ClassDb.messageManager.StartMoneyLoss(pair.Key.threatType, pair.Value);

                //wait for closing dialog box
                yield return new WaitWhile(() => DialogBoxManager.dialogEnabled);

                GameData.money -= pair.Value;

            }

            stuxnetThreats.Clear();

        }

        interactiveSprite.SetInteraction(true);

        LevelManager.hasPlantChecked = true;
    }
}
