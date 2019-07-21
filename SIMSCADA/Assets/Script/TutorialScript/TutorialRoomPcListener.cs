using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialRoomPcListener : MonoBehaviour
{
    private static Canvas scadaScreen;
    public static bool scadaEnabled;

    private static Canvas storeScreen;
    public static bool storeEnabled;

    private IEnumerator localRecapRoutine;
    private IEnumerator localShowRoutine;

    private TutorialInteractiveSprite interactiveSprite;

    private static bool market1;
    private static bool market2;


    private IEnumerator hmipcRoutine;
    private IEnumerator hmipanelRoutine;
    private IEnumerator market1Routine;
    private IEnumerator market2Routine;


    private void Start()
    {
        interactiveSprite = GetComponent<TutorialInteractiveSprite>();
    }

    public void SetComputerListeners()
    {
        if (TutorialManager.roomPcFirstTime)
        {
            //show info message for security check
            hmipcRoutine = HmiPcRoutine();
            StartCoroutine(hmipcRoutine);
        }

        List<Button> buttons;

        if (GameData.pointOutPurchased)
        {
            if (LevelManager.hasDosDeployed ||
                LevelManager.hasPhishingDeployed ||
                LevelManager.hasReplayDeployed ||
                LevelManager.hasMitmDeployed ||
                LevelManager.hasMalwareDeployed ||
                LevelManager.hasStuxnetDeployed ||
                LevelManager.hasDragonflyDeployed)
            {
                buttons = interactiveSprite.actionButtonManager.GetActiveCanvasGroup(8);

                buttons[0].GetComponentInChildren<TextMeshProUGUI>().text = "Apri monitor SCADA";
                buttons[0].onClick.RemoveAllListeners();
                buttons[0].onClick.AddListener(delegate
                {
                    ToggleScadaScreen();
                    interactiveSprite.ToggleMenu();
                });

                buttons[1].GetComponentInChildren<TextMeshProUGUI>().text = "Vai al Negozio";
                buttons[1].onClick.RemoveAllListeners();
                buttons[1].onClick.AddListener(delegate
                {
                    ToggleStoreScreen();
                    interactiveSprite.ToggleMenu();

                });

                if (LevelManager.isFirewallActive)
                {
                    buttons[2].GetComponentInChildren<TextMeshProUGUI>().text = "Disattiva Firewall";
                    buttons[2].onClick.RemoveAllListeners();
                    buttons[2].onClick.AddListener(delegate
                    {
                        ClassDb.levelManager.SetFirewallActive(false);
                        interactiveSprite.ToggleMenu();
                    });
                }
                else
                {
                    buttons[2].GetComponentInChildren<TextMeshProUGUI>().text = "Attiva Firewall";
                    buttons[2].onClick.RemoveAllListeners();
                    buttons[2].onClick.AddListener(delegate
                    {
                        ClassDb.levelManager.SetFirewallActive(true);
                        interactiveSprite.ToggleMenu();
                    });
                }


                if (LevelManager.isRemoteIdsActive)
                {
                    buttons[3].GetComponentInChildren<TextMeshProUGUI>().text = "Disattiva IDS";
                    buttons[3].onClick.RemoveAllListeners();
                    buttons[3].onClick.AddListener(delegate
                    {
                        ClassDb.levelManager.SetRemoteIdsActive(false);
                        interactiveSprite.ToggleMenu();
                    });
                }
                else
                {
                    buttons[3].GetComponentInChildren<TextMeshProUGUI>().text = "Attiva IDS";
                    buttons[3].onClick.RemoveAllListeners();
                    buttons[3].onClick.AddListener(delegate
                    {
                        ClassDb.levelManager.SetRemoteIdsActive(true);
                        interactiveSprite.ToggleMenu();
                    });
                }

                if (LevelManager.isLocalIdsActive)
                {
                    buttons[4].GetComponentInChildren<TextMeshProUGUI>().text = "Disattiva Controlli Locali";
                    buttons[4].onClick.RemoveAllListeners();
                    buttons[4].onClick.AddListener(delegate
                    {
                        ClassDb.levelManager.SetLocalIdsActive(false);
                        interactiveSprite.ToggleMenu();
                    });
                }
                else
                {
                    buttons[4].GetComponentInChildren<TextMeshProUGUI>().text = "Attiva Controlli Locali";
                    buttons[4].onClick.RemoveAllListeners();
                    buttons[4].onClick.AddListener(delegate
                    {
                        ClassDb.levelManager.SetLocalIdsActive(true);
                        interactiveSprite.ToggleMenu();
                    });
                }

                buttons[5].GetComponentInChildren<TextMeshProUGUI>().text = "Check configurazione di rete";
                buttons[5].onClick.RemoveAllListeners();
                buttons[5].onClick.AddListener(delegate
                {
                    //StartCheckNetworkCfg();
                    interactiveSprite.ToggleMenu();
                });

                buttons[6].GetComponentInChildren<TextMeshProUGUI>().text = "Esegui scansione malware";
                buttons[6].onClick.RemoveAllListeners();
                buttons[6].onClick.AddListener(delegate
                {
                    //StartAntiMalwareScan();
                    interactiveSprite.ToggleMenu();
                });

                buttons[7].GetComponentInChildren<TextMeshProUGUI>().text = "Individua minacce";
                buttons[7].onClick.RemoveAllListeners();
                buttons[7].onClick.AddListener(delegate
                {
                    //StartPointOutLocalThreat();
                    interactiveSprite.ToggleMenu();
                });
            }
            else
            {
                buttons = interactiveSprite.actionButtonManager.GetActiveCanvasGroup(5);

                buttons[0].GetComponentInChildren<TextMeshProUGUI>().text = "Apri monitor SCADA";
                buttons[0].onClick.RemoveAllListeners();
                buttons[0].onClick.AddListener(delegate
                {
                    ToggleScadaScreen();
                    interactiveSprite.ToggleMenu();
                });

                buttons[1].GetComponentInChildren<TextMeshProUGUI>().text = "Vai al Negozio";
                buttons[1].onClick.RemoveAllListeners();
                buttons[1].onClick.AddListener(delegate
                {
                    ToggleStoreScreen();
                    interactiveSprite.ToggleMenu();

                });

                if (LevelManager.isFirewallActive)
                {
                    buttons[2].GetComponentInChildren<TextMeshProUGUI>().text = "Disattiva Firewall";
                    buttons[2].onClick.RemoveAllListeners();
                    buttons[2].onClick.AddListener(delegate
                    {
                        ClassDb.levelManager.SetFirewallActive(false);
                        interactiveSprite.ToggleMenu();
                    });
                }
                else
                {
                    buttons[2].GetComponentInChildren<TextMeshProUGUI>().text = "Attiva Firewall";
                    buttons[2].onClick.RemoveAllListeners();
                    buttons[2].onClick.AddListener(delegate
                    {
                        ClassDb.levelManager.SetFirewallActive(true);
                        interactiveSprite.ToggleMenu();
                    });
                }


                if (LevelManager.isRemoteIdsActive)
                {
                    buttons[3].GetComponentInChildren<TextMeshProUGUI>().text = "Disattiva IDS";
                    buttons[3].onClick.RemoveAllListeners();
                    buttons[3].onClick.AddListener(delegate
                    {
                        ClassDb.levelManager.SetRemoteIdsActive(false);
                        interactiveSprite.ToggleMenu();
                    });
                }
                else
                {
                    buttons[3].GetComponentInChildren<TextMeshProUGUI>().text = "Attiva IDS";
                    buttons[3].onClick.RemoveAllListeners();
                    buttons[3].onClick.AddListener(delegate
                    {
                        ClassDb.levelManager.SetRemoteIdsActive(true);
                        interactiveSprite.ToggleMenu();
                    });
                }

                if (LevelManager.isLocalIdsActive)
                {
                    buttons[4].GetComponentInChildren<TextMeshProUGUI>().text = "Disattiva Controlli Locali";
                    buttons[4].onClick.RemoveAllListeners();
                    buttons[4].onClick.AddListener(delegate
                    {
                        ClassDb.levelManager.SetLocalIdsActive(false);
                        interactiveSprite.ToggleMenu();
                    });
                }
                else
                {
                    buttons[4].GetComponentInChildren<TextMeshProUGUI>().text = "Attiva Controlli Locali";
                    buttons[4].onClick.RemoveAllListeners();
                    buttons[4].onClick.AddListener(delegate
                    {
                        ClassDb.levelManager.SetLocalIdsActive(true);
                        interactiveSprite.ToggleMenu();
                    });
                }

                buttons[5].GetComponentInChildren<TextMeshProUGUI>().text = "Individua minacce";
                buttons[5].onClick.RemoveAllListeners();
                buttons[5].onClick.AddListener(delegate
                {
                    //StartPointOutLocalThreat();
                    interactiveSprite.ToggleMenu();
                });
            }
        }
        else
        {
            if (LevelManager.hasDosDeployed ||
                LevelManager.hasPhishingDeployed ||
                LevelManager.hasReplayDeployed ||
                LevelManager.hasMitmDeployed ||
                LevelManager.hasMalwareDeployed ||
                LevelManager.hasStuxnetDeployed ||
                LevelManager.hasDragonflyDeployed)
            {
                buttons = interactiveSprite.actionButtonManager.GetActiveCanvasGroup(7);

                buttons[0].GetComponentInChildren<TextMeshProUGUI>().text = "Apri monitor SCADA";
                buttons[0].onClick.RemoveAllListeners();
                buttons[0].onClick.AddListener(delegate
                {
                    ToggleScadaScreen();
                    interactiveSprite.ToggleMenu();
                });

                buttons[1].GetComponentInChildren<TextMeshProUGUI>().text = "Vai al Negozio";
                buttons[1].onClick.RemoveAllListeners();
                buttons[1].onClick.AddListener(delegate
                {
                    ToggleStoreScreen();
                    interactiveSprite.ToggleMenu();

                });

                if (LevelManager.isFirewallActive)
                {
                    buttons[2].GetComponentInChildren<TextMeshProUGUI>().text = "Disattiva Firewall";
                    buttons[2].onClick.RemoveAllListeners();
                    buttons[2].onClick.AddListener(delegate
                    {
                        ClassDb.levelManager.SetFirewallActive(false);
                        interactiveSprite.ToggleMenu();
                    });
                }
                else
                {
                    buttons[2].GetComponentInChildren<TextMeshProUGUI>().text = "Attiva Firewall";
                    buttons[2].onClick.RemoveAllListeners();
                    buttons[2].onClick.AddListener(delegate
                    {
                        ClassDb.levelManager.SetFirewallActive(true);
                        interactiveSprite.ToggleMenu();
                    });
                }

                if (LevelManager.isRemoteIdsActive)
                {
                    buttons[3].GetComponentInChildren<TextMeshProUGUI>().text = "Disattiva IDS";
                    buttons[3].onClick.RemoveAllListeners();
                    buttons[3].onClick.AddListener(delegate
                    {
                        ClassDb.levelManager.SetRemoteIdsActive(false);
                        interactiveSprite.ToggleMenu();
                    });
                }
                else
                {
                    buttons[3].GetComponentInChildren<TextMeshProUGUI>().text = "Attiva IDS";
                    buttons[3].onClick.RemoveAllListeners();
                    buttons[3].onClick.AddListener(delegate
                    {
                        ClassDb.levelManager.SetRemoteIdsActive(true);
                        interactiveSprite.ToggleMenu();
                    });
                }

                if (LevelManager.isLocalIdsActive)
                {
                    buttons[4].GetComponentInChildren<TextMeshProUGUI>().text = "Disattiva Controlli Locali";
                    buttons[4].onClick.RemoveAllListeners();
                    buttons[4].onClick.AddListener(delegate
                    {
                        ClassDb.levelManager.SetLocalIdsActive(false);
                        interactiveSprite.ToggleMenu();
                    });
                }
                else
                {
                    buttons[4].GetComponentInChildren<TextMeshProUGUI>().text = "Attiva Controlli Locali";
                    buttons[4].onClick.RemoveAllListeners();
                    buttons[4].onClick.AddListener(delegate
                    {
                        ClassDb.levelManager.SetLocalIdsActive(true);
                        interactiveSprite.ToggleMenu();
                    });
                }

                buttons[5].GetComponentInChildren<TextMeshProUGUI>().text = "Check configurazione di rete";
                buttons[5].onClick.RemoveAllListeners();
                buttons[5].onClick.AddListener(delegate
                {
                    //StartCheckNetworkCfg();
                    interactiveSprite.ToggleMenu();
                });

                buttons[6].GetComponentInChildren<TextMeshProUGUI>().text = "Esegui scansione malware";
                buttons[6].onClick.RemoveAllListeners();
                buttons[6].onClick.AddListener(delegate
                {
                    //StartAntiMalwareScan();
                    interactiveSprite.ToggleMenu();
                });
            }
            else
            {
                buttons = interactiveSprite.actionButtonManager.GetActiveCanvasGroup(5);

                buttons[0].GetComponentInChildren<TextMeshProUGUI>().text = "Apri monitor SCADA";
                buttons[0].onClick.RemoveAllListeners();
                buttons[0].onClick.AddListener(delegate
                {
                    ToggleScadaScreen();
                    interactiveSprite.ToggleMenu();
                });

                buttons[1].GetComponentInChildren<TextMeshProUGUI>().text = "Vai al Negozio";
                buttons[1].onClick.RemoveAllListeners();
                buttons[1].onClick.AddListener(delegate
                {
                    ToggleStoreScreen();
                    interactiveSprite.ToggleMenu();

                });

                if (LevelManager.isFirewallActive)
                {
                    buttons[2].GetComponentInChildren<TextMeshProUGUI>().text = "Disattiva Firewall";
                    buttons[2].onClick.RemoveAllListeners();
                    buttons[2].onClick.AddListener(delegate
                    {
                        ClassDb.levelManager.SetFirewallActive(false);
                        interactiveSprite.ToggleMenu();
                    });
                }
                else
                {
                    buttons[2].GetComponentInChildren<TextMeshProUGUI>().text = "Attiva Firewall";
                    buttons[2].onClick.RemoveAllListeners();
                    buttons[2].onClick.AddListener(delegate
                    {
                        ClassDb.levelManager.SetFirewallActive(true);
                        interactiveSprite.ToggleMenu();
                    });
                }


                if (LevelManager.isRemoteIdsActive)
                {
                    buttons[3].GetComponentInChildren<TextMeshProUGUI>().text = "Disattiva IDS";
                    buttons[3].onClick.RemoveAllListeners();
                    buttons[3].onClick.AddListener(delegate
                    {
                        ClassDb.levelManager.SetRemoteIdsActive(false);
                        interactiveSprite.ToggleMenu();
                    });
                }
                else
                {
                    buttons[3].GetComponentInChildren<TextMeshProUGUI>().text = "Attiva IDS";
                    buttons[3].onClick.RemoveAllListeners();
                    buttons[3].onClick.AddListener(delegate
                    {
                        ClassDb.levelManager.SetRemoteIdsActive(true);
                        interactiveSprite.ToggleMenu();
                    });
                }

                if (LevelManager.isLocalIdsActive)
                {
                    buttons[4].GetComponentInChildren<TextMeshProUGUI>().text = "Disattiva Controlli Locali";
                    buttons[4].onClick.RemoveAllListeners();
                    buttons[4].onClick.AddListener(delegate
                    {
                        ClassDb.levelManager.SetLocalIdsActive(false);
                        interactiveSprite.ToggleMenu();
                    });
                }
                else
                {
                    buttons[4].GetComponentInChildren<TextMeshProUGUI>().text = "Attiva Controlli Locali";
                    buttons[4].onClick.RemoveAllListeners();
                    buttons[4].onClick.AddListener(delegate
                    {
                        ClassDb.levelManager.SetLocalIdsActive(true);
                        interactiveSprite.ToggleMenu();
                    });
                }
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
            //keep active button for scada screen and store screen
            if (buttons.IndexOf(button) == 0 || buttons.IndexOf(button) == 1) continue;
            button.interactable = false;
        }
    }

    public void ToggleScadaScreen()
    {
        if (TutorialManager.firstTimeHMIPanel)
        {
            TutorialManager.firstTimeHMIPanel = false;

            hmipanelRoutine = HmiPanelRoutine();
            StartCoroutine(hmipanelRoutine);
        }

        if (scadaEnabled)
        {
            ClassDb.prefabManager.ReturnPrefab(scadaScreen.gameObject, PrefabManager.scadaIndex);
            scadaEnabled = false;
        }
        else
        {
            scadaScreen = ClassDb.prefabManager.GetPrefab(ClassDb.prefabManager.prefabScadaScreen.gameObject, PrefabManager.scadaIndex).GetComponent<Canvas>();
            scadaEnabled = true;
        }
    }

    public void ToggleStoreScreen()
    {
        if (TutorialManager.firstTimeMarket)
        {
            TutorialManager.firstTimeMarket = false;

            market1Routine = Market1Routine();
            StartCoroutine(market1Routine);
        }

        if (storeEnabled)
        {
            ClassDb.prefabManager.ReturnPrefab(storeScreen.gameObject, PrefabManager.storeIndex);
            storeEnabled = false;
        }
        else
        {
            storeScreen = ClassDb.prefabManager.GetPrefab(ClassDb.prefabManager.prefabStoreScreen.gameObject, PrefabManager.storeIndex).GetComponent<Canvas>();
            storeEnabled = true;
        }
    }

    private IEnumerator Market1Routine()
    {
        //set flag to stop next coroutine
        market1 = false;

        //start next coroutine
        market2Routine = Market2Routine();
        StartCoroutine(market2Routine);

        //display message
        ClassDb.tutorialMessageManager.MarketPanel1Message();

        //wait until dialog has been disabled
        yield return new WaitWhile(() => TutorialDialogBoxManager.dialogEnabled);

        //set flag to start next coroutine
        market1 = true;


    }

    private IEnumerator Market2Routine()
    {
        //wait unitl previous corutine has finished
        yield return new WaitUntil(() => market1);

        //set flag to stop next coroutine
        market2 = false;

        //display message
        ClassDb.tutorialMessageManager.MarketPanel2Message();

        //wait until dialog has been disabled
        yield return new WaitWhile(() => TutorialDialogBoxManager.dialogEnabled);

        //set flag to start next coroutine
        market2 = true;

    }

    private IEnumerator HmiPcRoutine()
    {
        //display message
        ClassDb.tutorialMessageManager.HMIPCMessage();

        //wait until dialog has been disabled
        yield return new WaitWhile(() => TutorialDialogBoxManager.dialogEnabled);

        TutorialManager.roomPcFirstTime = false;

    }

    private IEnumerator HmiPanelRoutine()
    {
        //display message
        ClassDb.tutorialMessageManager.HMIPanelMessage();

        //wait until dialog has been disabled
        yield return new WaitWhile(() => TutorialDialogBoxManager.dialogEnabled);

    }

}
