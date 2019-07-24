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

        if (LevelManager.hasDosDeployed ||
            LevelManager.hasPhishingDeployed ||
            LevelManager.hasReplayDeployed ||
            LevelManager.hasMitmDeployed ||
            LevelManager.hasMalwareDeployed ||
            LevelManager.hasStuxnetDeployed ||
            LevelManager.hasDragonflyDeployed ||
            isThreatDetected)
        {
            buttons = interactiveSprite.actionButtonManager.GetActiveCanvasGroup(7);

            if (LevelManager.isFirewallActive)
            {
                buttons[0].GetComponentInChildren<TextMeshProUGUI>().text = "Disattiva Firewall";
                buttons[0].onClick.RemoveAllListeners();
                buttons[0].onClick.AddListener(delegate
                {
                    ClassDb.levelManager.SetFirewallActive(false);
                    interactiveSprite.ToggleMenu();
                });
            }
            else
            {
                buttons[0].GetComponentInChildren<TextMeshProUGUI>().text = "Attiva Firewall";
                buttons[0].onClick.RemoveAllListeners();
                buttons[0].onClick.AddListener(delegate
                {
                    ClassDb.levelManager.SetFirewallActive(true);
                    interactiveSprite.ToggleMenu();
                });
            }


            if (LevelManager.isRemoteIdsActive)
            {
                buttons[1].GetComponentInChildren<TextMeshProUGUI>().text = "Disattiva IDS";
                buttons[1].onClick.RemoveAllListeners();
                buttons[1].onClick.AddListener(delegate
                {
                    ClassDb.levelManager.SetRemoteIdsActive(false);
                    interactiveSprite.ToggleMenu();
                });
            }
            else
            {
                buttons[1].GetComponentInChildren<TextMeshProUGUI>().text = "Attiva IDS";
                buttons[1].onClick.RemoveAllListeners();
                buttons[1].onClick.AddListener(delegate
                {
                    ClassDb.levelManager.SetRemoteIdsActive(true);
                    interactiveSprite.ToggleMenu();
                });
            }

            if (LevelManager.isLocalIdsActive)
            {
                buttons[2].GetComponentInChildren<TextMeshProUGUI>().text = "Disattiva Controlli Locali";
                buttons[2].onClick.RemoveAllListeners();
                buttons[2].onClick.AddListener(delegate
                {
                    ClassDb.levelManager.SetLocalIdsActive(false);
                    interactiveSprite.ToggleMenu();
                });
            }
            else
            {
                buttons[2].GetComponentInChildren<TextMeshProUGUI>().text = "Attiva Controlli Locali";
                buttons[2].onClick.RemoveAllListeners();
                buttons[2].onClick.AddListener(delegate
                {
                    ClassDb.levelManager.SetLocalIdsActive(true);
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

            if (LevelManager.isFirewallActive)
            {
                buttons[0].GetComponentInChildren<TextMeshProUGUI>().text = "Disattiva Firewall";
                buttons[0].onClick.RemoveAllListeners();
                buttons[0].onClick.AddListener(delegate
                {
                    ClassDb.levelManager.SetFirewallActive(false);
                    interactiveSprite.ToggleMenu();
                });
            }
            else
            {
                buttons[0].GetComponentInChildren<TextMeshProUGUI>().text = "Attiva Firewall";
                buttons[0].onClick.RemoveAllListeners();
                buttons[0].onClick.AddListener(delegate
                {
                    ClassDb.levelManager.SetFirewallActive(true);
                    interactiveSprite.ToggleMenu();
                });
            }


            if (LevelManager.isRemoteIdsActive)
            {
                buttons[1].GetComponentInChildren<TextMeshProUGUI>().text = "Disattiva IDS";
                buttons[1].onClick.RemoveAllListeners();
                buttons[1].onClick.AddListener(delegate
                {
                    ClassDb.levelManager.SetRemoteIdsActive(false);
                    interactiveSprite.ToggleMenu();
                });
            }
            else
            {
                buttons[1].GetComponentInChildren<TextMeshProUGUI>().text = "Attiva IDS";
                buttons[1].onClick.RemoveAllListeners();
                buttons[1].onClick.AddListener(delegate
                {
                    ClassDb.levelManager.SetRemoteIdsActive(true);
                    interactiveSprite.ToggleMenu();
                });
            }

            if (LevelManager.isLocalIdsActive)
            {
                buttons[2].GetComponentInChildren<TextMeshProUGUI>().text = "Disattiva Controlli Locali";
                buttons[2].onClick.RemoveAllListeners();
                buttons[2].onClick.AddListener(delegate
                {
                    ClassDb.levelManager.SetLocalIdsActive(false);
                    interactiveSprite.ToggleMenu();
                });
            }
            else
            {
                buttons[2].GetComponentInChildren<TextMeshProUGUI>().text = "Attiva Controlli Locali";
                buttons[2].onClick.RemoveAllListeners();
                buttons[2].onClick.AddListener(delegate
                {
                    ClassDb.levelManager.SetLocalIdsActive(true);
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

        if (LevelManager.hasStuxnetDeployed)
        {
            if (!LevelManager.hasPlantChecked)
            {
                //MESSAGE TO INFORM ABOUT CHECKING PLANT
                ClassDb.levelMessageManager.StartPlantCheck();

                //wait to close dialog to continue
                yield return new WaitUntil(() => DialogBoxManager.dialogEnabled);

            }

            yield return new WaitUntil(() => LevelManager.hasPlantChecked);
        }

        ClassDb.levelManager.timeEventList.Add(progressEvent);

        antiMalwareScanRoutine = AntiMalwareScan(progressEvent, sprite);
        StartCoroutine(antiMalwareScanRoutine);
    }

    private IEnumerator AntiMalwareScan(TimeEvent progressEvent, InteractiveSprite sprite)
    {
        yield return new WaitWhile(() => ClassDb.levelManager.timeEventList.Contains(progressEvent));

        //Reset money loss due to replay attack
        ClassDb.levelManager.moneyLossList[StringDb.ThreatAttack.malware] = 0;
        ClassDb.levelManager.moneyLossList[StringDb.ThreatAttack.stuxnet] = 0;

        ////reset base money earn
        //ClassDb.worldManager.totalMoneyEarnPerMinute = StringDb.baseEarn;

        //restart money generation
        ClassDb.levelManager.isMoneyLoss = true;
        //ClassDb.worldManager.isMoneyEarn = true;
        
        //restart all time event
        ClassDb.timeEventManager.StartTimeEventList(ClassDb.levelManager.timeEventList);

        sprite.SetInteraction(true);

        //reset flag to restart threat generation
        LevelManager.hasMalwareDeployed = false;
        LevelManager.hasStuxnetDeployed = false;

        if (LevelManager.hasDragonflyDeployed)
        {
            LevelManager.hasMalwareChecked = true;
            yield break;
        }

        //MESSAGE FOR RECAP
        LevelManager.hasThreatManaged = true;

    }

    private void StartIdsClean()
    {
        interactiveSprite.SetInteraction(false);

        //start duration: 90 min
        TimeEvent progressEvent = ClassDb.timeEventManager.NewTimeEvent(
            GameData.serverIdsCleanTime, interactiveSprite.gameObject, true, true);

        ClassDb.levelManager.timeEventList.Add(progressEvent);

        idsCleanRoutine = IdsClean(progressEvent);
        StartCoroutine(idsCleanRoutine);
    }

    private IEnumerator IdsClean(TimeEvent progressEvent)
    {
        threatDetectedList = ClassDb.levelManager.threatDetectedList;

        yield return new WaitWhile(() => ClassDb.levelManager.timeEventList.Contains(progressEvent));

        foreach (Threat threat in threatDetectedList)
        {
            ClassDb.levelManager.timeEventList.Remove(ClassDb.timeEventManager.GetThreatTimeEvent(threat));
            ClassDb.levelManager.remoteThreats.Remove(threat);
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
        ClassDb.levelManager.timeEventList.Add(progressEvent);

        rebootRoutine = RebootServer(progressEvent);
        StartCoroutine(rebootRoutine);
    }

    private IEnumerator RebootServer(TimeEvent progressEvent)
    {
        yield return new WaitWhile(() => ClassDb.levelManager.timeEventList.Contains(progressEvent));

        //Reset money loss due to dos attack
        ClassDb.levelManager.moneyLossList[StringDb.ThreatAttack.dos] = 0;
        
        ////reset base money earn
        //ClassDb.worldManager.totalMoneyEarnPerMinute = StringDb.baseEarn;

        //restart money generation
        ClassDb.levelManager.isMoneyLoss = true;
        //ClassDb.worldManager.isMoneyEarn = true;

        //restart all time event
        ClassDb.timeEventManager.StartTimeEventList(ClassDb.levelManager.timeEventList);

        interactiveSprite.SetInteraction(true);

        //reset flag to restart threat generation
        LevelManager.hasDosDeployed = false;

        //MESSAGE FOR RECAP
        LevelManager.hasThreatManaged = true;

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

        if (LevelManager.hasReplayDeployed)
        {
            if (!LevelManager.hasPlantChecked)
            {
                //MESSAGE TO INFORM ABOUT CHECKING PLANT
                ClassDb.levelMessageManager.StartPlantCheck();

                //wait to close dialog to continue
                yield return new WaitUntil(() => DialogBoxManager.dialogEnabled);
            }

            yield return new WaitUntil(() => LevelManager.hasPlantChecked);
        }

        if (LevelManager.hasDragonflyDeployed)
        {
            if(!LevelManager.hasMalwareChecked)
            {
                //MESSAGE TO INFORM ABOUT CHECKING MALWARE
                ClassDb.levelMessageManager.StartMalwareCheck();

                //wait to close dialog to continue
                yield return new WaitUntil(() => DialogBoxManager.dialogEnabled);
            }

            yield return new WaitUntil(() => LevelManager.hasMalwareChecked);
        }
        
        ClassDb.levelManager.timeEventList.Add(progressEvent);

        checkNetworkRoutine = CheckNetworkCfg(progressEvent, sprite);
        StartCoroutine(checkNetworkRoutine);
    }

    private IEnumerator CheckNetworkCfg(TimeEvent progressEvent, InteractiveSprite sprite)
    {
        yield return new WaitWhile(() => ClassDb.levelManager.timeEventList.Contains(progressEvent));

        //Reset money loss due to replay attack
        ClassDb.levelManager.moneyLossList[StringDb.ThreatAttack.replay] = 0;
        ClassDb.levelManager.moneyLossList[StringDb.ThreatAttack.mitm] = 0;
        ClassDb.levelManager.moneyLossList[StringDb.ThreatAttack.dragonfly] = 0;

        ////reset base money earn
        //ClassDb.worldManager.totalMoneyEarnPerMinute = StringDb.baseEarn;

        //restart money generation
        ClassDb.levelManager.isMoneyLoss = true;
        //ClassDb.worldManager.isMoneyEarn = true;

        //restart all time event
        ClassDb.timeEventManager.StartTimeEventList(ClassDb.levelManager.timeEventList);

        sprite.SetInteraction(true);

        //reset flag to restart threat generation
        LevelManager.hasReplayDeployed = false;
        LevelManager.hasMitmDeployed = false;
        LevelManager.hasDragonflyDeployed = false;

        //MESSAGE FOR RECAP
        LevelManager.hasThreatManaged = true;


    }
}



