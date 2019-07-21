using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomPcListener : MonoBehaviour
{
    private static Canvas scadaScreen;
    public static bool scadaEnabled;

    private static Canvas storeScreen;
    public static bool storeEnabled;

    private IEnumerator localRecapRoutine;
    private IEnumerator localShowRoutine;

    private InteractiveSprite interactiveSprite;



    private void Start()
    {
        interactiveSprite = GetComponent<InteractiveSprite>();
    }

    public void SetComputerListeners()
    {
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
                    StartCheckNetworkCfg();
                    interactiveSprite.ToggleMenu();
                });

                buttons[6].GetComponentInChildren<TextMeshProUGUI>().text = "Esegui scansione malware";
                buttons[6].onClick.RemoveAllListeners();
                buttons[6].onClick.AddListener(delegate
                {
                    StartAntiMalwareScan();
                    interactiveSprite.ToggleMenu();
                });

                buttons[7].GetComponentInChildren<TextMeshProUGUI>().text = "Individua minacce";
                buttons[7].onClick.RemoveAllListeners();
                buttons[7].onClick.AddListener(delegate
                {
                    StartPointOutLocalThreat();
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
                    StartPointOutLocalThreat();
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
                    StartCheckNetworkCfg();
                    interactiveSprite.ToggleMenu();
                });

                buttons[6].GetComponentInChildren<TextMeshProUGUI>().text = "Esegui scansione malware";
                buttons[6].onClick.RemoveAllListeners();
                buttons[6].onClick.AddListener(delegate
                {
                    StartAntiMalwareScan();
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
    }

    public void ToggleScadaScreen()
    {
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

    //public void StartLocalThreatRecap()
    //{
    //    //start duration: 90 min
    //    TimeEvent progressEvent = ClassDb.timeEventManager.NewTimeEvent(
    //        StringDb.pcRecapTime, GameObject.Find(StringDb.roomPc), true);

    //    ClassDb.worldManager.timeEventList.Add(progressEvent);

    //    localRecapRoutine = LocalThreatRecap(progressEvent);
    //    StartCoroutine(localRecapRoutine);
    //}

    //IEnumerator LocalThreatRecap(TimeEvent progressEvent)
    //{
    //    interactiveSprite.SetInteraction(false);

    //    yield return new WaitWhile(() => ClassDb.worldManager.timeEventList.Contains(progressEvent));

    //    foreach (TimeEvent eventList in ClassDb.worldManager.timeEventList)
    //    {
    //        if(eventList.threat.threatType == StringDb.ThreatType.local)
    //            threatList.Add(eventList.threat);
    //    }

    //    ClassDb.messageManager.StartThreatRecap(StringDb.ThreatType.local, threatList.Count);

    //    foreach (Threat threat in threatList)
    //    {
    //        threat.aiController.gameObject.GetComponent<AiController>().idScanned = true;
    //    }

    //    localRecapShowed = threatList.Count != 0;

    //    interactiveSprite.SetInteraction(true);

    //}

    public void StartPointOutLocalThreat()
    {
        interactiveSprite.SetInteraction(false);

        TimeEvent progressEvent = ClassDb.timeEventManager.NewTimeEvent(
            GameData.pcPointOutTime, interactiveSprite.gameObject, true, true);

        ClassDb.levelManager.timeEventList.Add(progressEvent);

        localShowRoutine = PointOutLocalThreat(progressEvent);
        StartCoroutine(localShowRoutine);
    }

    IEnumerator PointOutLocalThreat(TimeEvent progressEvent)
    {
        yield return new WaitWhile(() => ClassDb.levelManager.timeEventList.Contains(progressEvent));

        foreach (Threat threat in ClassDb.levelManager.localThreats.Where(x => x.threatType == StringDb.ThreatType.local))
        {
            threat.aiController.PointOutThreat();
        }

        interactiveSprite.SetInteraction(true);
    }

    private void StartCheckNetworkCfg()
    {
        ServerPcListener serverPcListener = FindObjectsOfType<ServerPcListener>()[0];

        serverPcListener.StartCheckNetworkCfg(interactiveSprite);
    }

    private void StartAntiMalwareScan()
    {
        ServerPcListener serverPcListener = FindObjectsOfType<ServerPcListener>()[0];

        serverPcListener.StartAntiMalwareScan(interactiveSprite);
    }
}
