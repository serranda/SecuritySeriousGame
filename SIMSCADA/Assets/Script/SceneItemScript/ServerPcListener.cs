using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ServerPcListener : MonoBehaviour
{
    [SerializeField] private InteractiveSprite interactiveSprite;

    private IEnumerator beforeAntiMalwareScanRoutine;
    private IEnumerator antiMalwareScanRoutine;
    private IEnumerator idsCleanRoutine;
    private IEnumerator rebootRoutine;
    private IEnumerator beforeCheckNetworkRoutine;
    private IEnumerator checkNetworkRoutine;

    public static bool isThreatDetected;
    public List<Threat> threatDetectedList;


    private void Start()
    {
        isThreatDetected = false;

        interactiveSprite = GetComponent<InteractiveSprite>();
    }

    public void SetSeverPcListeners()
    {
        List<Button> buttons;

        if (Level1Manager.hasDosDeployed ||
            Level1Manager.hasPhishingDeployed ||
            Level1Manager.hasReplayDeployed ||
            Level1Manager.hasMitmDeployed ||
            Level1Manager.hasMalwareDeployed ||
            Level1Manager.hasStuxnetDeployed ||
            Level1Manager.hasDragonflyDeployed ||
            isThreatDetected)
        {
            buttons = interactiveSprite.actionButtonManager.GetActiveCanvasGroup(7);

            if (Level1Manager.isFirewallActive)
            {
                buttons[0].GetComponentInChildren<TextMeshProUGUI>().text = "Disattiva Firewall";
                buttons[0].onClick.RemoveAllListeners();
                buttons[0].onClick.AddListener(delegate
                {
                    ClassDb.level1Manager.SetFirewallActive(false);
                    interactiveSprite.ToggleMenu();
                });
            }
            else
            {
                buttons[0].GetComponentInChildren<TextMeshProUGUI>().text = "Attiva Firewall";
                buttons[0].onClick.RemoveAllListeners();
                buttons[0].onClick.AddListener(delegate
                {
                    ClassDb.level1Manager.SetFirewallActive(true);
                    interactiveSprite.ToggleMenu();
                });
            }


            if (Level1Manager.isRemoteIdsActive)
            {
                buttons[1].GetComponentInChildren<TextMeshProUGUI>().text = "Disattiva IDS";
                buttons[1].onClick.RemoveAllListeners();
                buttons[1].onClick.AddListener(delegate
                {
                    ClassDb.level1Manager.SetRemoteIdsActive(false);
                    interactiveSprite.ToggleMenu();
                });
            }
            else
            {
                buttons[1].GetComponentInChildren<TextMeshProUGUI>().text = "Attiva IDS";
                buttons[1].onClick.RemoveAllListeners();
                buttons[1].onClick.AddListener(delegate
                {
                    ClassDb.level1Manager.SetRemoteIdsActive(true);
                    interactiveSprite.ToggleMenu();
                });
            }

            if (Level1Manager.isLocalIdsActive)
            {
                buttons[2].GetComponentInChildren<TextMeshProUGUI>().text = "Disattiva Controlli Locali";
                buttons[2].onClick.RemoveAllListeners();
                buttons[2].onClick.AddListener(delegate
                {
                    ClassDb.level1Manager.SetLocalIdsActive(false);
                    interactiveSprite.ToggleMenu();
                });
            }
            else
            {
                buttons[2].GetComponentInChildren<TextMeshProUGUI>().text = "Attiva Controlli Locali";
                buttons[2].onClick.RemoveAllListeners();
                buttons[2].onClick.AddListener(delegate
                {
                    ClassDb.level1Manager.SetLocalIdsActive(true);
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

            if (Level1Manager.isFirewallActive)
            {
                buttons[0].GetComponentInChildren<TextMeshProUGUI>().text = "Disattiva Firewall";
                buttons[0].onClick.RemoveAllListeners();
                buttons[0].onClick.AddListener(delegate
                {
                    ClassDb.level1Manager.SetFirewallActive(false);
                    interactiveSprite.ToggleMenu();
                });
            }
            else
            {
                buttons[0].GetComponentInChildren<TextMeshProUGUI>().text = "Attiva Firewall";
                buttons[0].onClick.RemoveAllListeners();
                buttons[0].onClick.AddListener(delegate
                {
                    ClassDb.level1Manager.SetFirewallActive(true);
                    interactiveSprite.ToggleMenu();
                });
            }


            if (Level1Manager.isRemoteIdsActive)
            {
                buttons[1].GetComponentInChildren<TextMeshProUGUI>().text = "Disattiva IDS";
                buttons[1].onClick.RemoveAllListeners();
                buttons[1].onClick.AddListener(delegate
                {
                    ClassDb.level1Manager.SetRemoteIdsActive(false);
                    interactiveSprite.ToggleMenu();
                });
            }
            else
            {
                buttons[1].GetComponentInChildren<TextMeshProUGUI>().text = "Attiva IDS";
                buttons[1].onClick.RemoveAllListeners();
                buttons[1].onClick.AddListener(delegate
                {
                    ClassDb.level1Manager.SetRemoteIdsActive(true);
                    interactiveSprite.ToggleMenu();
                });
            }

            if (Level1Manager.isLocalIdsActive)
            {
                buttons[2].GetComponentInChildren<TextMeshProUGUI>().text = "Disattiva Controlli Locali";
                buttons[2].onClick.RemoveAllListeners();
                buttons[2].onClick.AddListener(delegate
                {
                    ClassDb.level1Manager.SetLocalIdsActive(false);
                    interactiveSprite.ToggleMenu();
                });
            }
            else
            {
                buttons[2].GetComponentInChildren<TextMeshProUGUI>().text = "Attiva Controlli Locali";
                buttons[2].onClick.RemoveAllListeners();
                buttons[2].onClick.AddListener(delegate
                {
                    ClassDb.level1Manager.SetLocalIdsActive(true);
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

        beforeAntiMalwareScanRoutine = BeforeAntiMalwareScan(sprite);
        StartCoroutine(beforeAntiMalwareScanRoutine);
    }

    private IEnumerator BeforeAntiMalwareScan(InteractiveSprite sprite)
    {

        TimeEvent progressEvent = ClassDb.timeEventManager.NewTimeEvent(
            GameData.serverAntiMalwareTime, sprite.gameObject, true, true);

        if (Level1Manager.hasStuxnetDeployed)
        {
            if (!Level1Manager.hasPlantChecked)
            {
                //MESSAGE TO INFORM ABOUT CHECKING PLANT
                ClassDb.levelMessageManager.StartPlantCheck();

                //wait to close dialog to continue
                yield return new WaitUntil(() => DialogBoxManager.dialogEnabled);

            }

            yield return new WaitUntil(() => Level1Manager.hasPlantChecked);
        }

        ClassDb.level1Manager.timeEventList.Add(progressEvent);

        antiMalwareScanRoutine = AntiMalwareScan(progressEvent, sprite);
        StartCoroutine(antiMalwareScanRoutine);
    }

    private IEnumerator AntiMalwareScan(TimeEvent progressEvent, InteractiveSprite sprite)
    {
        yield return new WaitWhile(() => ClassDb.level1Manager.timeEventList.Contains(progressEvent));

        //Reset money loss due to replay attack
        ClassDb.level1Manager.moneyLossList[StringDb.ThreatAttack.malware] = 0;
        ClassDb.level1Manager.moneyLossList[StringDb.ThreatAttack.stuxnet] = 0;

        ////reset base money earn
        //ClassDb.worldManager.totalMoneyEarnPerMinute = StringDb.baseEarn;

        //restart money generation
        ClassDb.level1Manager.isMoneyLoss = true;
        //ClassDb.worldManager.isMoneyEarn = true;
        
        //restart all time event
        ClassDb.timeEventManager.StartTimeEventList(ClassDb.level1Manager.timeEventList);

        sprite.SetInteraction(true);

        //reset flag to restart threat generation
        Level1Manager.hasMalwareDeployed = false;
        Level1Manager.hasStuxnetDeployed = false;

        if (Level1Manager.hasDragonflyDeployed)
        {
            Level1Manager.hasMalwareChecked = true;
            yield break;
        }

        //MESSAGE FOR RECAP
        Level1Manager.hasThreatManaged = true;

    }

    private void StartIdsClean()
    {
        interactiveSprite.SetInteraction(false);

        //start duration: 90 min
        TimeEvent progressEvent = ClassDb.timeEventManager.NewTimeEvent(
            GameData.serverIdsCleanTime, interactiveSprite.gameObject, true, true);

        ClassDb.level1Manager.timeEventList.Add(progressEvent);

        idsCleanRoutine = IdsClean(progressEvent);
        StartCoroutine(idsCleanRoutine);
    }

    private IEnumerator IdsClean(TimeEvent progressEvent)
    {
        threatDetectedList = ClassDb.level1Manager.threatDetectedList;

        yield return new WaitWhile(() => ClassDb.level1Manager.timeEventList.Contains(progressEvent));

        foreach (Threat threat in threatDetectedList)
        {
            ClassDb.level1Manager.timeEventList.Remove(ClassDb.timeEventManager.GetThreatTimeEvent(threat));
            ClassDb.level1Manager.remoteThreats.Remove(threat);
        }

        isThreatDetected = false;
        ClassDb.levelMessageManager.StartIdsClean();
        interactiveSprite.SetInteraction(true);

    }

    private void StartRebootServer()
    {
        interactiveSprite.SetInteraction(false);

        TimeEvent progressEvent = ClassDb.timeEventManager.NewTimeEvent(
            GameData.serverRebootTime, interactiveSprite.gameObject, true, true);
        ClassDb.level1Manager.timeEventList.Add(progressEvent);

        rebootRoutine = RebootServer(progressEvent);
        StartCoroutine(rebootRoutine);
    }

    private IEnumerator RebootServer(TimeEvent progressEvent)
    {
        yield return new WaitWhile(() => ClassDb.level1Manager.timeEventList.Contains(progressEvent));

        //Reset money loss due to dos attack
        ClassDb.level1Manager.moneyLossList[StringDb.ThreatAttack.dos] = 0;
        
        ////reset base money earn
        //ClassDb.worldManager.totalMoneyEarnPerMinute = StringDb.baseEarn;

        //restart money generation
        ClassDb.level1Manager.isMoneyLoss = true;
        //ClassDb.worldManager.isMoneyEarn = true;

        //restart all time event
        ClassDb.timeEventManager.StartTimeEventList(ClassDb.level1Manager.timeEventList);

        interactiveSprite.SetInteraction(true);

        //reset flag to restart threat generation
        Level1Manager.hasDosDeployed = false;

        //MESSAGE FOR RECAP
        Level1Manager.hasThreatManaged = true;

    }

    public void StartCheckNetworkCfg(InteractiveSprite sprite)
    {
        sprite.SetInteraction(false);

        beforeCheckNetworkRoutine = BeforeCheckNetworkCfg(sprite);
        StartCoroutine(beforeCheckNetworkRoutine);
    }

    public IEnumerator BeforeCheckNetworkCfg(InteractiveSprite sprite)
    {
        TimeEvent progressEvent = ClassDb.timeEventManager.NewTimeEvent(
            GameData.serverCheckCfgTime, sprite.gameObject, true, true);

        if (Level1Manager.hasReplayDeployed)
        {
            if (!Level1Manager.hasPlantChecked)
            {
                //MESSAGE TO INFORM ABOUT CHECKING PLANT
                ClassDb.levelMessageManager.StartPlantCheck();

                //wait to close dialog to continue
                yield return new WaitUntil(() => DialogBoxManager.dialogEnabled);
            }

            yield return new WaitUntil(() => Level1Manager.hasPlantChecked);
        }

        if (Level1Manager.hasDragonflyDeployed)
        {
            if(!Level1Manager.hasMalwareChecked)
            {
                //MESSAGE TO INFORM ABOUT CHECKING MALWARE
                ClassDb.levelMessageManager.StartMalwareCheck();

                //wait to close dialog to continue
                yield return new WaitUntil(() => DialogBoxManager.dialogEnabled);
            }

            yield return new WaitUntil(() => Level1Manager.hasMalwareChecked);
        }
        
        ClassDb.level1Manager.timeEventList.Add(progressEvent);

        checkNetworkRoutine = CheckNetworkCfg(progressEvent, sprite);
        StartCoroutine(checkNetworkRoutine);
    }

    private IEnumerator CheckNetworkCfg(TimeEvent progressEvent, InteractiveSprite sprite)
    {
        yield return new WaitWhile(() => ClassDb.level1Manager.timeEventList.Contains(progressEvent));

        //Reset money loss due to replay attack
        ClassDb.level1Manager.moneyLossList[StringDb.ThreatAttack.replay] = 0;
        ClassDb.level1Manager.moneyLossList[StringDb.ThreatAttack.mitm] = 0;
        ClassDb.level1Manager.moneyLossList[StringDb.ThreatAttack.dragonfly] = 0;

        ////reset base money earn
        //ClassDb.worldManager.totalMoneyEarnPerMinute = StringDb.baseEarn;

        //restart money generation
        ClassDb.level1Manager.isMoneyLoss = true;
        //ClassDb.worldManager.isMoneyEarn = true;

        //restart all time event
        ClassDb.timeEventManager.StartTimeEventList(ClassDb.level1Manager.timeEventList);

        sprite.SetInteraction(true);

        //reset flag to restart threat generation
        Level1Manager.hasReplayDeployed = false;
        Level1Manager.hasMitmDeployed = false;
        Level1Manager.hasDragonflyDeployed = false;

        //MESSAGE FOR RECAP
        Level1Manager.hasThreatManaged = true;


    }
}



