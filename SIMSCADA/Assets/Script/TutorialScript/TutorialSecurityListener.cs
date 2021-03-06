﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialSecurityListener : MonoBehaviour
{
    private static Canvas serverRoomScreen;
    private TutorialInteractiveSprite interactiveSprite;

    private IEnumerator securityMessageRoutine;

    [SerializeField] private TutorialManager tutorialManager;


    private void Start()
    {
        interactiveSprite = GetComponent<TutorialInteractiveSprite>();
    }

    public void SetSecurityListeners()
    {

        List<Button> buttons = interactiveSprite.actionButtonManager.GetActiveCanvasGroup(1);

        if (tutorialManager.securityCheckFirstTime)
        {
            //show info message for security check
            securityMessageRoutine = SecurityMessage();
            StartCoroutine(securityMessageRoutine);

        }

        buttons[0].GetComponentInChildren<TextMeshProUGUI>().text = "Imposta accesso";
        buttons[0].onClick.RemoveAllListeners();
        buttons[0].onClick.AddListener(delegate
        {
            //ToggleSecurityScreen();
            interactiveSprite.ToggleMenu();
        });

        foreach (Button button in buttons)
        {
            button.interactable = true;
        }

        //disable interact with button until tutorial is finished
        if (tutorialManager.tutorialIsFinished) return;

        foreach (Button button in buttons)
        {
            button.interactable = false;
        }

    }

    private IEnumerator SecurityMessage()
    {
        //display message
        ClassDb.tutorialMessageManager.SecurityCheckMessage();

        //wait until dialog has been disabled
        yield return new WaitWhile(() => tutorialManager.tutorialGameData.dialogEnabled);

        tutorialManager.securityCheckFirstTime = false;
    }
}
