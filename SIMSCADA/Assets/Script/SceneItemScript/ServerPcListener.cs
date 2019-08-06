using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ServerPcListener : MonoBehaviour
{
    [SerializeField] private InteractiveSprite interactiveSprite;

    private IEnumerator beforeAntiMalwareScanRoutine;
    private IEnumerator antiMalwareScanRoutine;
    private IEnumerator idsCleanRoutine;
    private IEnumerator rebootServerRoutine;
    private IEnumerator beforeCheckNetworkRoutine;
    private IEnumerator checkNetworkCfgRoutine;

    public static bool isThreatDetected;
    public List<Threat> threatDetectedList;

    private ILevelManager manager;


    private void Start()
    {
        manager = SetLevelManager();

        isThreatDetected = false;

        interactiveSprite = GetComponent<InteractiveSprite>();
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

    public void SetSeverPcListeners()
    {
        List<Button> buttons;

        //DEBUG OVERRIDE TO DELETE
        manager.GetGameData().hasReplayDeployed = true;

        //DEBUG OVERRIDE TO DELETE
        manager.GetGameData().hasPlantChecked = false;

        if (manager.GetGameData().hasDosDeployed ||
            manager.GetGameData().hasPhishingDeployed ||
            manager.GetGameData().hasReplayDeployed ||
            manager.GetGameData().hasMitmDeployed ||
            manager.GetGameData().hasMalwareDeployed ||
            manager.GetGameData().hasStuxnetDeployed ||
            manager.GetGameData().hasDragonflyDeployed ||
            isThreatDetected)
        {
            buttons = interactiveSprite.actionButtonManager.GetActiveCanvasGroup(7);

            if (manager.GetGameData().isFirewallActive)
            {
                buttons[0].GetComponentInChildren<TextMeshProUGUI>().text = "Disattiva Firewall";
                buttons[0].onClick.RemoveAllListeners();
                buttons[0].onClick.AddListener(delegate
                {
                    manager.SetFirewallActive(false);
                    interactiveSprite.ToggleMenu();
                });
            }
            else
            {
                buttons[0].GetComponentInChildren<TextMeshProUGUI>().text = "Attiva Firewall";
                buttons[0].onClick.RemoveAllListeners();
                buttons[0].onClick.AddListener(delegate
                {
                    manager.SetFirewallActive(true);
                    interactiveSprite.ToggleMenu();
                });
            }


            if (manager.GetGameData().isRemoteIdsActive)
            {
                buttons[1].GetComponentInChildren<TextMeshProUGUI>().text = "Disattiva IDS";
                buttons[1].onClick.RemoveAllListeners();
                buttons[1].onClick.AddListener(delegate
                {
                    manager.SetRemoteIdsActive(false);
                    interactiveSprite.ToggleMenu();
                });
            }
            else
            {
                buttons[1].GetComponentInChildren<TextMeshProUGUI>().text = "Attiva IDS";
                buttons[1].onClick.RemoveAllListeners();
                buttons[1].onClick.AddListener(delegate
                {
                    manager.SetRemoteIdsActive(true);
                    interactiveSprite.ToggleMenu();
                });
            }

            if (manager.GetGameData().isLocalIdsActive)
            {
                buttons[2].GetComponentInChildren<TextMeshProUGUI>().text = "Disattiva Controlli Locali";
                buttons[2].onClick.RemoveAllListeners();
                buttons[2].onClick.AddListener(delegate
                {
                    manager.SetLocalIdsActive(false);
                    interactiveSprite.ToggleMenu();
                });
            }
            else
            {
                buttons[2].GetComponentInChildren<TextMeshProUGUI>().text = "Attiva Controlli Locali";
                buttons[2].onClick.RemoveAllListeners();
                buttons[2].onClick.AddListener(delegate
                {
                    manager.SetLocalIdsActive(true);
                    interactiveSprite.ToggleMenu();
                });
            }

            buttons[3].GetComponentInChildren<TextMeshProUGUI>().text = "Controlla eventi IDS";
            buttons[3].onClick.RemoveAllListeners();
            buttons[3].onClick.AddListener(delegate
            {
                StartIdsClean();
                interactiveSprite.ToggleMenu();
            });

            buttons[4].GetComponentInChildren<TextMeshProUGUI>().text = "Check configurazione di rete";
            buttons[4].onClick.RemoveAllListeners();
            buttons[4].onClick.AddListener(delegate
            {
                StartCheckNetworkCfg(interactiveSprite);
                interactiveSprite.ToggleMenu();
            });

            buttons[5].GetComponentInChildren<TextMeshProUGUI>().text = "Riavvia server";
            buttons[5].onClick.RemoveAllListeners();
            buttons[5].onClick.AddListener(delegate
            {
                StartRebootServer();
                interactiveSprite.ToggleMenu();
            });

            buttons[6].GetComponentInChildren<TextMeshProUGUI>().text = "Esegui scansione malware";
            buttons[6].onClick.RemoveAllListeners();
            buttons[6].onClick.AddListener(delegate
            {
                StartAntiMalwareScan(interactiveSprite);
                interactiveSprite.ToggleMenu();
            });
        }
        else
        {
            buttons = interactiveSprite.actionButtonManager.GetActiveCanvasGroup(3);

            if (manager.GetGameData().isFirewallActive)
            {
                buttons[0].GetComponentInChildren<TextMeshProUGUI>().text = "Disattiva Firewall";
                buttons[0].onClick.RemoveAllListeners();
                buttons[0].onClick.AddListener(delegate
                {
                    manager.SetFirewallActive(false);
                    interactiveSprite.ToggleMenu();
                });
            }
            else
            {
                buttons[0].GetComponentInChildren<TextMeshProUGUI>().text = "Attiva Firewall";
                buttons[0].onClick.RemoveAllListeners();
                buttons[0].onClick.AddListener(delegate
                {
                    manager.SetFirewallActive(true);
                    interactiveSprite.ToggleMenu();
                });
            }


            if (manager.GetGameData().isRemoteIdsActive)
            {
                buttons[1].GetComponentInChildren<TextMeshProUGUI>().text = "Disattiva IDS";
                buttons[1].onClick.RemoveAllListeners();
                buttons[1].onClick.AddListener(delegate
                {
                    manager.SetRemoteIdsActive(false);
                    interactiveSprite.ToggleMenu();
                });
            }
            else
            {
                buttons[1].GetComponentInChildren<TextMeshProUGUI>().text = "Attiva IDS";
                buttons[1].onClick.RemoveAllListeners();
                buttons[1].onClick.AddListener(delegate
                {
                    manager.SetRemoteIdsActive(true);
                    interactiveSprite.ToggleMenu();
                });
            }

            if (manager.GetGameData().isLocalIdsActive)
            {
                buttons[2].GetComponentInChildren<TextMeshProUGUI>().text = "Disattiva Controlli Locali";
                buttons[2].onClick.RemoveAllListeners();
                buttons[2].onClick.AddListener(delegate
                {
                    manager.SetLocalIdsActive(false);
                    interactiveSprite.ToggleMenu();
                });
            }
            else
            {
                buttons[2].GetComponentInChildren<TextMeshProUGUI>().text = "Attiva Controlli Locali";
                buttons[2].onClick.RemoveAllListeners();
                buttons[2].onClick.AddListener(delegate
                {
                    manager.SetLocalIdsActive(true);
                    interactiveSprite.ToggleMenu();
                });
            }
        }

        foreach (Button button in buttons)
        {
            button.interactable = true;
        }
    }

    public void StartAntiMalwareScan(InteractiveSprite sprite)
    {
        sprite.SetInteraction(false);

        TimeEvent progressEvent = ClassDb.timeEventManager.NewTimeEvent(
            manager.GetGameData().serverAntiMalwareTime, sprite.gameObject, true, true, StringDb.antiMalwareScanRoutine);

        beforeAntiMalwareScanRoutine = BeforeAntiMalwareScan(progressEvent, sprite);
        StartCoroutine(beforeAntiMalwareScanRoutine);
    }

    public void RestartAntiMalwareScan(TimeEvent progressEvent, InteractiveSprite sprite)
    {
        sprite.SetInteraction(false);

        if (!manager.GetGameData().hasPlantChecked)
        {
            //TODO : CHECK
            beforeAntiMalwareScanRoutine = BeforeAntiMalwareScan(progressEvent, sprite);
            StartCoroutine(beforeAntiMalwareScanRoutine);
        }
        else
        {
            antiMalwareScanRoutine = AntiMalwareScan(progressEvent, sprite);
            StartCoroutine(antiMalwareScanRoutine);
        }

    }

    private IEnumerator BeforeAntiMalwareScan(TimeEvent progressEvent, InteractiveSprite sprite)
    {

        if (manager.GetGameData().hasStuxnetDeployed)
        {
            if (!manager.GetGameData().hasPlantChecked)
            {
                //MESSAGE TO INFORM ABOUT CHECKING PLANT
                ClassDb.levelMessageManager.StartPlantCheck();

                //wait to close dialog to continue
                yield return new WaitUntil(() => manager.GetGameData().dialogEnabled);

            }

            yield return new WaitUntil(() => manager.GetGameData().hasPlantChecked);
        }

        manager.GetGameData().timeEventList.Add(progressEvent);

        antiMalwareScanRoutine = AntiMalwareScan(progressEvent, sprite);
        StartCoroutine(antiMalwareScanRoutine);
    }

    private IEnumerator AntiMalwareScan(TimeEvent progressEvent, InteractiveSprite sprite)
    {
        yield return new WaitWhile(() => manager.GetGameData().timeEventList.Contains(progressEvent));

        //Reset money loss due to replay attack
        manager.GetGameData().moneyLossList[StringDb.ThreatAttack.malware] = 0;
        manager.GetGameData().moneyLossList[StringDb.ThreatAttack.stuxnet] = 0;

        ////reset base money earn
        //ClassDb.worldManager.totalMoneyEarnPerMinute = StringDb.baseEarn;

        //restart money generation
        manager.GetGameData().isMoneyLoss = true;
        //ClassDb.worldManager.isMoneyEarn = true;
        
        //restart all time event
        ClassDb.timeEventManager.StartTimeEventList(manager.GetGameData().timeEventList);

        sprite.SetInteraction(true);

        //reset flag to restart threat generation
        manager.GetGameData().hasMalwareDeployed = false;
        manager.GetGameData().hasStuxnetDeployed = false;

        if (manager.GetGameData().hasDragonflyDeployed)
        {
            manager.GetGameData().hasMalwareChecked = true;
            yield break;
        }

        //MESSAGE FOR RECAP
        manager.GetGameData().hasThreatManaged = true;

    }

    private void StartIdsClean()
    {
        interactiveSprite.SetInteraction(false);

        //start duration: 90 min
        TimeEvent progressEvent = ClassDb.timeEventManager.NewTimeEvent(
            manager.GetGameData().serverIdsCleanTime, interactiveSprite.gameObject, true, true, StringDb.idsCleanRoutine);

        manager.GetGameData().timeEventList.Add(progressEvent);

        idsCleanRoutine = IdsClean(progressEvent);
        StartCoroutine(idsCleanRoutine);
    }

    public void RestartIdsClean(TimeEvent progressEvent)
    {
        interactiveSprite.SetInteraction(false);

        idsCleanRoutine = IdsClean(progressEvent);
        StartCoroutine(idsCleanRoutine);
    }

    private IEnumerator IdsClean(TimeEvent progressEvent)
    {
        threatDetectedList = manager.GetGameData().threatDetectedList;

        yield return new WaitWhile(() => manager.GetGameData().timeEventList.Contains(progressEvent));

        foreach (Threat threat in threatDetectedList)
        {
            manager.GetGameData().timeEventList.Remove(ClassDb.timeEventManager.GetThreatTimeEvent(threat));
            manager.GetGameData().remoteThreats.Remove(threat);
        }

        isThreatDetected = false;
        ClassDb.levelMessageManager.StartIdsClean();
        interactiveSprite.SetInteraction(true);

    }

    private void StartRebootServer()
    {
        interactiveSprite.SetInteraction(false);

        TimeEvent progressEvent = ClassDb.timeEventManager.NewTimeEvent(
            manager.GetGameData().serverRebootTime, interactiveSprite.gameObject, true, true, StringDb.rebootServerRoutine);
        manager.GetGameData().timeEventList.Add(progressEvent);

        rebootServerRoutine = RebootServer(progressEvent);
        StartCoroutine(rebootServerRoutine);
    }

    public void RestartRebootServer(TimeEvent progressEvent)
    {
        interactiveSprite.SetInteraction(false);

        rebootServerRoutine = RebootServer(progressEvent);
        StartCoroutine(rebootServerRoutine);
    }

    private IEnumerator RebootServer(TimeEvent progressEvent)
    {
        yield return new WaitWhile(() => manager.GetGameData().timeEventList.Contains(progressEvent));

        //Reset money loss due to dos attack
        manager.GetGameData().moneyLossList[StringDb.ThreatAttack.dos] = 0;

        ////reset base money earn
        //ClassDb.worldManager.totalMoneyEarnPerMinute = StringDb.baseEarn;

        //restart money generation
        manager.GetGameData().isMoneyLoss = true;
        //ClassDb.worldManager.isMoneyEarn = true;

        //restart all time event
        ClassDb.timeEventManager.StartTimeEventList(manager.GetGameData().timeEventList);

        interactiveSprite.SetInteraction(true);

        //reset flag to restart threat generation
        manager.GetGameData().hasDosDeployed = false;

        //MESSAGE FOR RECAP
        manager.GetGameData().hasThreatManaged = true;

    }

    public void StartCheckNetworkCfg(InteractiveSprite sprite)
    {
        sprite.SetInteraction(false);

        TimeEvent progressEvent = ClassDb.timeEventManager.NewTimeEvent(
            manager.GetGameData().serverCheckCfgTime, sprite.gameObject, true, true, StringDb.checkNetworkCfgRoutine);

        beforeCheckNetworkRoutine = BeforeCheckNetworkCfg(progressEvent, sprite);
        StartCoroutine(beforeCheckNetworkRoutine);
    }

    public void RestartCheckNetworkCfg(TimeEvent progressEvent, InteractiveSprite sprite)
    {
        sprite.SetInteraction(false);

        if (manager.GetGameData().hasReplayDeployed)
        {
            if (!manager.GetGameData().hasPlantChecked)
            {
                //TODO : CHECK
                beforeCheckNetworkRoutine = BeforeCheckNetworkCfg(progressEvent, sprite);
                StartCoroutine(beforeCheckNetworkRoutine);
            }
            else
            {
                checkNetworkCfgRoutine = CheckNetworkCfg(progressEvent, sprite);
                StartCoroutine(checkNetworkCfgRoutine);
            }
        }

        if (manager.GetGameData().hasDragonflyDeployed)
        {
            if (!manager.GetGameData().hasMalwareChecked)
            {
                //TODO : CHECK
                beforeCheckNetworkRoutine = BeforeCheckNetworkCfg(progressEvent, sprite);
                StartCoroutine(beforeCheckNetworkRoutine);
            }
            else
            {
                checkNetworkCfgRoutine = CheckNetworkCfg(progressEvent, sprite);
                StartCoroutine(checkNetworkCfgRoutine);
            }
        }

    }


    public IEnumerator BeforeCheckNetworkCfg(TimeEvent progressEvent, InteractiveSprite sprite)
    {
        if (manager.GetGameData().hasReplayDeployed)
        {
            if (!manager.GetGameData().hasPlantChecked)
            {
                //MESSAGE TO INFORM ABOUT CHECKING PLANT
                ClassDb.levelMessageManager.StartPlantCheck();

                //wait to close dialog to continue
                yield return new WaitUntil(() => manager.GetGameData().dialogEnabled);
            }

            yield return new WaitUntil(() => manager.GetGameData().hasPlantChecked);
        }

        if (manager.GetGameData().hasDragonflyDeployed)
        {
            if(!manager.GetGameData().hasMalwareChecked)
            {
                //MESSAGE TO INFORM ABOUT CHECKING MALWARE
                ClassDb.levelMessageManager.StartMalwareCheck();

                //wait to close dialog to continue
                yield return new WaitUntil(() => manager.GetGameData().dialogEnabled);
            }

            yield return new WaitUntil(() => manager.GetGameData().hasMalwareChecked);
        }

        manager.GetGameData().timeEventList.Add(progressEvent);

        checkNetworkCfgRoutine = CheckNetworkCfg(progressEvent, sprite);
        StartCoroutine(checkNetworkCfgRoutine);
    }

    private IEnumerator CheckNetworkCfg(TimeEvent progressEvent, InteractiveSprite sprite)
    {
        yield return new WaitWhile(() => manager.GetGameData().timeEventList.Contains(progressEvent));

        //Reset money loss due to replay attack
        manager.GetGameData().moneyLossList[StringDb.ThreatAttack.replay] = 0;
        manager.GetGameData().moneyLossList[StringDb.ThreatAttack.mitm] = 0;
        manager.GetGameData().moneyLossList[StringDb.ThreatAttack.dragonfly] = 0;

        ////reset base money earn
        //ClassDb.worldManager.totalMoneyEarnPerMinute = StringDb.baseEarn;

        //restart money generation
        manager.GetGameData().isMoneyLoss = true;
        //ClassDb.worldManager.isMoneyEarn = true;

        //restart all time event
        ClassDb.timeEventManager.StartTimeEventList(manager.GetGameData().timeEventList);

        sprite.SetInteraction(true);

        //reset flag to restart threat generation
        manager.GetGameData().hasReplayDeployed = false;
        manager.GetGameData().hasMitmDeployed = false;
        manager.GetGameData().hasDragonflyDeployed = false;

        //MESSAGE FOR RECAP
        manager.GetGameData().hasThreatManaged = true;


    }
}



