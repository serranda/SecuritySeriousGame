using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialServerPcListener : MonoBehaviour
{
    private TutorialInteractiveSprite interactiveSprite;
    private IEnumerator serverMessageRoutine;

    [SerializeField] private TutorialManager tutorialManager;

    private void Start()
    {
        interactiveSprite = GetComponent<TutorialInteractiveSprite>();
    }

    public void SetSeverPcListeners()
    {
        if (tutorialManager.serverPcFirstTime)
        {
            //show info message for security check
            serverMessageRoutine = ServerMessageRoutine();
            StartCoroutine(serverMessageRoutine);
        }

        List<Button> buttons = interactiveSprite.actionButtonManager.GetActiveCanvasGroup(7);

        if (tutorialManager.tutorialGameData.isFirewallActive)
        {
            buttons[0].GetComponentInChildren<TextMeshProUGUI>().text = "Disattiva Firewall";
            buttons[0].onClick.RemoveAllListeners();
            buttons[0].onClick.AddListener(delegate
            {
                tutorialManager.SetFirewallActive(false);
                interactiveSprite.ToggleMenu();
                interactiveSprite.ToggleMenu();
            });
        }
        else
        {
            buttons[0].GetComponentInChildren<TextMeshProUGUI>().text = "Attiva Firewall";
            buttons[0].onClick.RemoveAllListeners();
            buttons[0].onClick.AddListener(delegate
            {
                tutorialManager.SetFirewallActive(true);
                interactiveSprite.ToggleMenu();
                interactiveSprite.ToggleMenu();
            });
        }


        if (tutorialManager.tutorialGameData.isRemoteIdsActive)
        {
            buttons[1].GetComponentInChildren<TextMeshProUGUI>().text = "Disattiva IDS";
            buttons[1].onClick.RemoveAllListeners();
            buttons[1].onClick.AddListener(delegate
            {
                tutorialManager.SetRemoteIdsActive(false);
                interactiveSprite.ToggleMenu();
                interactiveSprite.ToggleMenu();
            });
        }
        else
        {
            buttons[1].GetComponentInChildren<TextMeshProUGUI>().text = "Attiva IDS";
            buttons[1].onClick.RemoveAllListeners();
            buttons[1].onClick.AddListener(delegate
            {
                tutorialManager.SetRemoteIdsActive(true);
                interactiveSprite.ToggleMenu();
                interactiveSprite.ToggleMenu();
            });
        }

        if (tutorialManager.tutorialGameData.isLocalIdsActive)
        {
            buttons[2].GetComponentInChildren<TextMeshProUGUI>().text = "Disattiva Controlli Locali";
            buttons[2].onClick.RemoveAllListeners();
            buttons[2].onClick.AddListener(delegate
            {
                tutorialManager.SetLocalIdsActive(false);
                interactiveSprite.ToggleMenu();
                interactiveSprite.ToggleMenu();
            });
        }
        else
        {
            buttons[2].GetComponentInChildren<TextMeshProUGUI>().text = "Attiva Controlli Locali";
            buttons[2].onClick.RemoveAllListeners();
            buttons[2].onClick.AddListener(delegate
            {
                tutorialManager.SetLocalIdsActive(true);
                interactiveSprite.ToggleMenu();
                interactiveSprite.ToggleMenu();
            });
        }

        buttons[3].GetComponentInChildren<TextMeshProUGUI>().text = "Controlla eventi IDS";
        buttons[3].onClick.RemoveAllListeners();

        buttons[4].GetComponentInChildren<TextMeshProUGUI>().text = "Check configurazione di rete";
        buttons[4].onClick.RemoveAllListeners();

        buttons[5].GetComponentInChildren<TextMeshProUGUI>().text = "Riavvia server";
        buttons[5].onClick.RemoveAllListeners();

        buttons[6].GetComponentInChildren<TextMeshProUGUI>().text = "Esegui scansione malware";
        buttons[6].onClick.RemoveAllListeners();


        foreach (Button button in buttons)
        {
            button.interactable = true;
        }

        //disable interact with button until tutorial is finished
        if (tutorialManager.tutorialIsFinished) return;

        foreach (Button button in buttons)
        {
            if (buttons.IndexOf(button) >= 0 && buttons.IndexOf(button) <= 2) continue;
            button.interactable = false;
        }
    }

    private IEnumerator ServerMessageRoutine()
    {
        //display message
        ClassDb.tutorialMessageManager.ServerMessage();

        //wait until dialog has been disabled
        yield return new WaitWhile(() => TutorialDialogBoxManager.dialogEnabled);

        tutorialManager.serverPcFirstTime = false;
    }

}



