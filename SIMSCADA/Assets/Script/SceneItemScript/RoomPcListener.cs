using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoomPcListener : MonoBehaviour
{
    private static Canvas scadaScreen;

    private static Canvas storeScreen;

    private IEnumerator pointOutLocalThreatRoutine;

    private InteractiveSprite interactiveSprite;

    private ILevelManager manager;

    private void Start()
    {
        manager = SetLevelManager();

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

    public void SetComputerListeners()
    {
        List<Button> buttons;

        if (manager.GetGameData().pointOutPurchased)
        {
            if (manager.GetGameData().hasDosDeployed ||
                manager.GetGameData().hasPhishingDeployed ||
                manager.GetGameData().hasReplayDeployed ||
                manager.GetGameData().hasMitmDeployed ||
                manager.GetGameData().hasMalwareDeployed ||
                manager.GetGameData().hasStuxnetDeployed ||
                manager.GetGameData().hasDragonflyDeployed)
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

                if (manager.GetGameData().isFirewallActive)
                {
                    buttons[2].GetComponentInChildren<TextMeshProUGUI>().text = "Disattiva Firewall";
                    buttons[2].onClick.RemoveAllListeners();
                    buttons[2].onClick.AddListener(delegate
                    {
                        manager.SetFirewallActive(false);
                    });
                }
                else
                {
                    buttons[2].GetComponentInChildren<TextMeshProUGUI>().text = "Attiva Firewall";
                    buttons[2].onClick.RemoveAllListeners();
                    buttons[2].onClick.AddListener(delegate
                    {
                        manager.SetFirewallActive(true);
                    });
                }


                if (manager.GetGameData().isRemoteIdsActive)
                {
                    buttons[3].GetComponentInChildren<TextMeshProUGUI>().text = "Disattiva IDS";
                    buttons[3].onClick.RemoveAllListeners();
                    buttons[3].onClick.AddListener(delegate
                    {
                        manager.SetRemoteIdsActive(false);
                    });
                }
                else
                {
                    buttons[3].GetComponentInChildren<TextMeshProUGUI>().text = "Attiva IDS";
                    buttons[3].onClick.RemoveAllListeners();
                    buttons[3].onClick.AddListener(delegate
                    {
                        manager.SetRemoteIdsActive(true);
                    });
                }

                if (manager.GetGameData().isLocalIdsActive)
                {
                    buttons[4].GetComponentInChildren<TextMeshProUGUI>().text = "Disattiva Controlli Locali";
                    buttons[4].onClick.RemoveAllListeners();
                    buttons[4].onClick.AddListener(delegate
                    {
                        manager.SetLocalIdsActive(false);
                    });
                }
                else
                {
                    buttons[4].GetComponentInChildren<TextMeshProUGUI>().text = "Attiva Controlli Locali";
                    buttons[4].onClick.RemoveAllListeners();
                    buttons[4].onClick.AddListener(delegate
                    {
                        manager.SetLocalIdsActive(true);
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

                if (manager.GetGameData().isFirewallActive)
                {
                    buttons[2].GetComponentInChildren<TextMeshProUGUI>().text = "Disattiva Firewall";
                    buttons[2].onClick.RemoveAllListeners();
                    buttons[2].onClick.AddListener(delegate
                    {
                        manager.SetFirewallActive(false);
                        interactiveSprite.ToggleMenu();
                    });
                }
                else
                {
                    buttons[2].GetComponentInChildren<TextMeshProUGUI>().text = "Attiva Firewall";
                    buttons[2].onClick.RemoveAllListeners();
                    buttons[2].onClick.AddListener(delegate
                    {
                        manager.SetFirewallActive(true);
                        interactiveSprite.ToggleMenu();
                    });
                }


                if (manager.GetGameData().isRemoteIdsActive)
                {
                    buttons[3].GetComponentInChildren<TextMeshProUGUI>().text = "Disattiva IDS";
                    buttons[3].onClick.RemoveAllListeners();
                    buttons[3].onClick.AddListener(delegate
                    {
                        manager.SetRemoteIdsActive(false);
                        interactiveSprite.ToggleMenu();
                    });
                }
                else
                {
                    buttons[3].GetComponentInChildren<TextMeshProUGUI>().text = "Attiva IDS";
                    buttons[3].onClick.RemoveAllListeners();
                    buttons[3].onClick.AddListener(delegate
                    {
                        manager.SetRemoteIdsActive(true);
                        interactiveSprite.ToggleMenu();
                    });
                }

                if (manager.GetGameData().isLocalIdsActive)
                {
                    buttons[4].GetComponentInChildren<TextMeshProUGUI>().text = "Disattiva Controlli Locali";
                    buttons[4].onClick.RemoveAllListeners();
                    buttons[4].onClick.AddListener(delegate
                    {
                        manager.SetLocalIdsActive(false);
                        interactiveSprite.ToggleMenu();
                    });
                }
                else
                {
                    buttons[4].GetComponentInChildren<TextMeshProUGUI>().text = "Attiva Controlli Locali";
                    buttons[4].onClick.RemoveAllListeners();
                    buttons[4].onClick.AddListener(delegate
                    {
                        manager.SetLocalIdsActive(true);
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
            if (manager.GetGameData().hasDosDeployed ||
                manager.GetGameData().hasPhishingDeployed ||
                manager.GetGameData().hasReplayDeployed ||
                manager.GetGameData().hasMitmDeployed ||
                manager.GetGameData().hasMalwareDeployed ||
                manager.GetGameData().hasStuxnetDeployed ||
                manager.GetGameData().hasDragonflyDeployed)
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

                if (manager.GetGameData().isFirewallActive)
                {
                    buttons[2].GetComponentInChildren<TextMeshProUGUI>().text = "Disattiva Firewall";
                    buttons[2].onClick.RemoveAllListeners();
                    buttons[2].onClick.AddListener(delegate
                    {
                        manager.SetFirewallActive(false);
                        interactiveSprite.ToggleMenu();
                    });
                }
                else
                {
                    buttons[2].GetComponentInChildren<TextMeshProUGUI>().text = "Attiva Firewall";
                    buttons[2].onClick.RemoveAllListeners();
                    buttons[2].onClick.AddListener(delegate
                    {
                        manager.SetFirewallActive(true);
                        interactiveSprite.ToggleMenu();
                    });
                }

                if (manager.GetGameData().isRemoteIdsActive)
                {
                    buttons[3].GetComponentInChildren<TextMeshProUGUI>().text = "Disattiva IDS";
                    buttons[3].onClick.RemoveAllListeners();
                    buttons[3].onClick.AddListener(delegate
                    {
                        manager.SetRemoteIdsActive(false);
                        interactiveSprite.ToggleMenu();
                    });
                }
                else
                {
                    buttons[3].GetComponentInChildren<TextMeshProUGUI>().text = "Attiva IDS";
                    buttons[3].onClick.RemoveAllListeners();
                    buttons[3].onClick.AddListener(delegate
                    {
                        manager.SetRemoteIdsActive(true);
                        interactiveSprite.ToggleMenu();
                    });
                }

                if (manager.GetGameData().isLocalIdsActive)
                {
                    buttons[4].GetComponentInChildren<TextMeshProUGUI>().text = "Disattiva Controlli Locali";
                    buttons[4].onClick.RemoveAllListeners();
                    buttons[4].onClick.AddListener(delegate
                    {
                        manager.SetLocalIdsActive(false);
                        interactiveSprite.ToggleMenu();
                    });
                }
                else
                {
                    buttons[4].GetComponentInChildren<TextMeshProUGUI>().text = "Attiva Controlli Locali";
                    buttons[4].onClick.RemoveAllListeners();
                    buttons[4].onClick.AddListener(delegate
                    {
                        manager.SetLocalIdsActive(true);
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

                if (manager.GetGameData().isFirewallActive)
                {
                    buttons[2].GetComponentInChildren<TextMeshProUGUI>().text = "Disattiva Firewall";
                    buttons[2].onClick.RemoveAllListeners();
                    buttons[2].onClick.AddListener(delegate
                    {
                        manager.SetFirewallActive(false);
                        interactiveSprite.ToggleMenu();
                    });
                }
                else
                {
                    buttons[2].GetComponentInChildren<TextMeshProUGUI>().text = "Attiva Firewall";
                    buttons[2].onClick.RemoveAllListeners();
                    buttons[2].onClick.AddListener(delegate
                    {
                        manager.SetFirewallActive(true);
                        interactiveSprite.ToggleMenu();
                    });
                }


                if (manager.GetGameData().isRemoteIdsActive)
                {
                    buttons[3].GetComponentInChildren<TextMeshProUGUI>().text = "Disattiva IDS";
                    buttons[3].onClick.RemoveAllListeners();
                    buttons[3].onClick.AddListener(delegate
                    {
                        manager.SetRemoteIdsActive(false);
                        interactiveSprite.ToggleMenu();
                    });
                }
                else
                {
                    buttons[3].GetComponentInChildren<TextMeshProUGUI>().text = "Attiva IDS";
                    buttons[3].onClick.RemoveAllListeners();
                    buttons[3].onClick.AddListener(delegate
                    {
                        manager.SetRemoteIdsActive(true);
                        interactiveSprite.ToggleMenu();
                    });
                }

                if (manager.GetGameData().isLocalIdsActive)
                {
                    buttons[4].GetComponentInChildren<TextMeshProUGUI>().text = "Disattiva Controlli Locali";
                    buttons[4].onClick.RemoveAllListeners();
                    buttons[4].onClick.AddListener(delegate
                    {
                        manager.SetLocalIdsActive(false);
                        interactiveSprite.ToggleMenu();
                    });
                }
                else
                {
                    buttons[4].GetComponentInChildren<TextMeshProUGUI>().text = "Attiva Controlli Locali";
                    buttons[4].onClick.RemoveAllListeners();
                    buttons[4].onClick.AddListener(delegate
                    {
                        manager.SetLocalIdsActive(true);
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
        manager = SetLevelManager();

        if (manager.GetGameData().scadaEnabled)
        {
            ClassDb.prefabManager.ReturnPrefab(scadaScreen.gameObject, PrefabManager.scadaIndex);
            manager.GetGameData().scadaEnabled = false;
        }
        else
        {
            scadaScreen = ClassDb.prefabManager.GetPrefab(ClassDb.prefabManager.prefabScadaScreen.gameObject, PrefabManager.scadaIndex).GetComponent<Canvas>();
            manager.GetGameData().scadaEnabled = true;
        }
    }

    public void ToggleStoreScreen()
    {
        manager = SetLevelManager();

        if (manager.GetGameData().storeEnabled)
        {
            ClassDb.prefabManager.ReturnPrefab(storeScreen.gameObject, PrefabManager.storeIndex);
            manager.GetGameData().storeEnabled = false;
        }
        else
        {
            storeScreen = ClassDb.prefabManager.GetPrefab(ClassDb.prefabManager.prefabStoreScreen.gameObject, PrefabManager.storeIndex).GetComponent<Canvas>();
            manager.GetGameData().storeEnabled = true;
        }
    }

    public void StartPointOutLocalThreat()
    {
        interactiveSprite.SetInteraction(false);

        TimeEvent progressEvent = ClassDb.timeEventManager.NewTimeEvent(
            manager.GetGameData().pcPointOutTime, interactiveSprite.gameObject, true, true, StringDb.pointOutLocalThreatRoutine);

        manager.GetGameData().timeEventList.Add(progressEvent);

        pointOutLocalThreatRoutine = PointOutLocalThreat(progressEvent);
        StartCoroutine(pointOutLocalThreatRoutine);
    }

    public void RestartPointOutLocalThreat(TimeEvent progressEvent)
    {
        interactiveSprite.SetInteraction(false);

        pointOutLocalThreatRoutine = PointOutLocalThreat(progressEvent);
        StartCoroutine(pointOutLocalThreatRoutine);
    }

    private IEnumerator PointOutLocalThreat(TimeEvent progressEvent)
    {
        yield return new WaitWhile(() => manager.GetGameData().timeEventList.Contains(progressEvent));

        foreach (Threat threat in manager.GetGameData().localThreats.Where(x => x.threatType == StringDb.ThreatType.local))
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

    public void RestartCheckNetworkCfg(TimeEvent progressEvent)
    {
        ServerPcListener serverPcListener = FindObjectsOfType<ServerPcListener>()[0];

        serverPcListener.RestartCheckNetworkCfg(progressEvent, interactiveSprite);
    }

    private void StartAntiMalwareScan()
    {
        ServerPcListener serverPcListener = FindObjectsOfType<ServerPcListener>()[0];

        serverPcListener.StartAntiMalwareScan(interactiveSprite);
    }

    public void RestartAntiMalwareScan(TimeEvent progressEvent)
    {
        ServerPcListener serverPcListener = FindObjectsOfType<ServerPcListener>()[0];

        serverPcListener.RestartAntiMalwareScan(progressEvent, interactiveSprite);
    }


}
