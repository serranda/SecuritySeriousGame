using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TelephoneListener : MonoBehaviour
{
    private InteractiveSprite interactiveSprite;

    private IEnumerator getMoneyRoutine;
    private IEnumerator checkPlantRoutine;
    private IEnumerator coolDownRoutine;

    public static Dictionary<Threat, float> replayThreats;
    public static Dictionary<Threat, float> stuxnetThreats;

    private ILevelManager manager;


    private void Start()
    {
        manager = SetLevelManager();

        interactiveSprite = GetComponent<InteractiveSprite>();

        replayThreats = new Dictionary<Threat, float>();
        stuxnetThreats = new Dictionary<Threat, float>();

    }

    private ILevelManager SetLevelManager()
    {
        ILevelManager iManager;
        if (SceneManager.GetActiveScene().buildIndex == StringDb.level1SceneIndex)
            iManager = FindObjectOfType<Level1Manager>();
        else
            iManager = FindObjectOfType<Level2Manager>();

        return iManager;
    }

    public void SetTelephoneListeners()
    {
        List<Button> buttons;

        if (manager.GetGameData().hasDosDeployed ||
            manager.GetGameData().hasPhishingDeployed ||
            manager.GetGameData().hasReplayDeployed ||
            manager.GetGameData().hasMitmDeployed ||
            manager.GetGameData().hasMalwareDeployed ||
            manager.GetGameData().hasStuxnetDeployed ||
            manager.GetGameData().hasDragonflyDeployed)
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
            manager.GetGameData().telephoneMoneyTime, interactiveSprite.gameObject, true, true, StringDb.getMoneyRoutine);

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
        yield return new WaitWhile(() => manager.GetGameData().timeEventList.Contains(progressEvent));

        int successRate = Random.Range(0, 100);

        if (!(successRate >= manager.GetGameData().reputation))
        {
            float deltaMoney = Random.Range(0f, 5f) * manager.GetGameData().money / 100;
            manager.GetGameData().money += deltaMoney;
            ClassDb.levelMessageManager.StartMoneyEarn(deltaMoney);
        }
        else
        {
            Debug.Log("NO MONEY");
        }

        StartCoolDown();
    }

    private void StartCoolDown()
    {
        TimeEvent progressEvent = ClassDb.timeEventManager.NewTimeEvent(
            manager.GetGameData().telephoneMoneyCoolDown * 60, interactiveSprite.gameObject, true, false, StringDb.coolDownRoutine);

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
        yield return new WaitWhile(() => manager.GetGameData().timeEventList.Contains(progressEvent));

        interactiveSprite.SetInteraction(true);
    }

    private void StartCheckPlant()
    {
        interactiveSprite.SetInteraction(false);

        TimeEvent progressEvent = ClassDb.timeEventManager.NewTimeEvent(
            manager.GetGameData().telephoneCheckPlantTime, interactiveSprite.gameObject, true, true, StringDb.checkPlantRoutine);

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
        yield return new WaitWhile(() => manager.GetGameData().timeEventList.Contains(progressEvent));

        if (manager.GetGameData().hasReplayDeployed)
        {
            foreach (KeyValuePair<Threat, float> pair in replayThreats)
            {
                float success = Random.Range(0, 100);

                if (!(success > manager.GetGameData().defensePlantResistance)) continue;
                //Inform how much money has been lost
                ClassDb.levelMessageManager.StartMoneyLoss(pair.Key.threatType, pair.Value);

                //wait for closing dialog box
                yield return new WaitWhile(() => manager.GetGameData().dialogEnabled);

                manager.GetGameData().money -= pair.Value;

            }

            replayThreats.Clear();

        }


        if (manager.GetGameData().hasStuxnetDeployed)
        {
            foreach (KeyValuePair<Threat, float> pair in stuxnetThreats)
            {
                float success = Random.Range(0, 100);

                if (!(success > manager.GetGameData().defensePlantResistance))
                {
                    ClassDb.levelMessageManager.StartThreatStopped(pair.Key);

                    Debug.Log(success);

                    continue;
                }
                //Inform how much money has been lost
                ClassDb.levelMessageManager.StartMoneyLoss(pair.Key.threatType, pair.Value);

                //wait for closing dialog box
                yield return new WaitWhile(() => manager.GetGameData().dialogEnabled);

                manager.GetGameData().money -= pair.Value;

            }

            stuxnetThreats.Clear();

        }

        interactiveSprite.SetInteraction(true);

        manager.GetGameData().hasPlantChecked = true;
    }
}
