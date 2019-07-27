using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialServerPcListener : MonoBehaviour
{
    [SerializeField] private TutorialInteractiveSprite interactiveSprite;

    private IEnumerator beforeAntiMalwareScanRoutine;
    private IEnumerator antiMalwareScanRoutine;
    private IEnumerator idsCleanRoutine;
    private IEnumerator rebootRoutine;
    private IEnumerator beforeCheckNetworkRoutine;
    private IEnumerator checkNetworkRoutine;

    public static bool isThreatDetected;
    public List<Threat> threatDetectedList;


    private IEnumerator serverMessageRoutine;

    private void Start()
    {
        isThreatDetected = false;

        interactiveSprite = GetComponent<TutorialInteractiveSprite>();
    }

    public void SetSeverPcListeners()
    {
        if (TutorialManager.serverPcFirstTime)
        {
            //show info message for security check
            serverMessageRoutine = ServerMessageRoutine();
            StartCoroutine(serverMessageRoutine);
        }

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
                //StartIdsClean();
                interactiveSprite.ToggleMenu();
            });

            buttons[4].GetComponentInChildren<TextMeshProUGUI>().text = "Check configurazione di rete";
            buttons[4].onClick.RemoveAllListeners();
            buttons[4].onClick.AddListener(delegate
            {
                //StartCheckNetworkCfg(interactiveSprite);
                interactiveSprite.ToggleMenu();
            });

            buttons[5].GetComponentInChildren<TextMeshProUGUI>().text = "Riavvia server";
            buttons[5].onClick.RemoveAllListeners();
            buttons[5].onClick.AddListener(delegate
            {
                //StartRebootServer();
                interactiveSprite.ToggleMenu();
            });

            buttons[6].GetComponentInChildren<TextMeshProUGUI>().text = "Esegui scansione malware";
            buttons[6].onClick.RemoveAllListeners();
            buttons[6].onClick.AddListener(delegate
            {
                //StartAntiMalwareScan(interactiveSprite);
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

        //disable interact with button until tutorial is finished
        if (TutorialManager.tutorialIsFinished) return;

        foreach (Button button in buttons)
        {
            button.interactable = false;
        }
    }

    private IEnumerator ServerMessageRoutine()
    {
        //display message
        ClassDb.tutorialMessageManager.ServerMessage();

        //wait until dialog has been disabled
        yield return new WaitWhile(() => TutorialDialogBoxManager.dialogEnabled);

        TutorialManager.serverPcFirstTime = false;

        interactiveSprite.ToggleMenu();


    }

}



