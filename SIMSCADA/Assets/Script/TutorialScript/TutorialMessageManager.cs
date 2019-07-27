using System;
using System.Collections;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class TutorialMessageManager : MonoBehaviour
{

    private IEnumerator exitRoutine;

    public TutorialDialogBoxMessage MessageFromJson(TextAsset jsonFile)
    {
        TutorialDialogBoxMessage box = JsonUtility.FromJson<TutorialDialogBoxMessage>(jsonFile.text);

        Debug.Log(box.ToString());

        return box;
    }

    public void Welcome1Message()
    {
        ClassDb.tutorialDialogBoxManager.ToggleTutorialDialogBox();

        TutorialDialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StringDb.tutorialWelcome1));

        ClassDb.tutorialDialogBoxManager.SetTutorialDialog(message.body);

        TutorialDialogBoxManager.btnTutorialDialog.onClick.RemoveAllListeners();
        TutorialDialogBoxManager.btnTutorialDialog.onClick.AddListener(delegate
        {
            ClassDb.tutorialDialogBoxManager.ToggleTutorialDialogBox();
        });

    }

    public void Welcome2Message()
    {
        ClassDb.tutorialDialogBoxManager.ToggleTutorialDialogBox();

        TutorialDialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StringDb.tutorialWelcome2));

        ClassDb.tutorialDialogBoxManager.SetTutorialDialog(message.body);

        TutorialDialogBoxManager.btnTutorialDialog.onClick.RemoveAllListeners();
        TutorialDialogBoxManager.btnTutorialDialog.onClick.AddListener(delegate
        {
            ClassDb.tutorialDialogBoxManager.ToggleTutorialDialogBox();
        });

    }

    public void Welcome3Message()
    {
        ClassDb.tutorialDialogBoxManager.ToggleTutorialDialogBox();

        TutorialDialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StringDb.tutorialWelcome3));

        ClassDb.tutorialDialogBoxManager.SetTutorialDialog(message.body);

        TutorialDialogBoxManager.btnTutorialDialog.onClick.RemoveAllListeners();
        TutorialDialogBoxManager.btnTutorialDialog.onClick.AddListener(delegate
        {
            ClassDb.tutorialDialogBoxManager.ToggleTutorialDialogBox();
        });

    }

    public void Welcome4Message()
    {
        ClassDb.tutorialDialogBoxManager.ToggleTutorialDialogBox();

        TutorialDialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StringDb.tutorialWelcome4));

        ClassDb.tutorialDialogBoxManager.SetTutorialDialog(message.body);

        TutorialDialogBoxManager.btnTutorialDialog.onClick.RemoveAllListeners();
        TutorialDialogBoxManager.btnTutorialDialog.onClick.AddListener(delegate
        {
            ClassDb.tutorialDialogBoxManager.ToggleTutorialDialogBox();
        });

    }

    public void Welcome5Message()
    {
        ClassDb.tutorialDialogBoxManager.ToggleTutorialDialogBox();

        TutorialDialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StringDb.tutorialWelcome5));

        ClassDb.tutorialDialogBoxManager.SetTutorialDialog(message.body);

        TutorialDialogBoxManager.btnTutorialDialog.onClick.RemoveAllListeners();
        TutorialDialogBoxManager.btnTutorialDialog.onClick.AddListener(delegate
        {
            ClassDb.tutorialDialogBoxManager.ToggleTutorialDialogBox();
        });

    }

    public void Welcome6Message()
    {
        ClassDb.tutorialDialogBoxManager.ToggleTutorialDialogBox();

        TutorialDialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StringDb.tutorialWelcome6));

        ClassDb.tutorialDialogBoxManager.SetTutorialDialog(message.body);

        TutorialDialogBoxManager.btnTutorialDialog.onClick.RemoveAllListeners();
        TutorialDialogBoxManager.btnTutorialDialog.onClick.AddListener(delegate
        {
            ClassDb.tutorialDialogBoxManager.ToggleTutorialDialogBox();
        });

    }

    public void Welcome7Message()
    {
        ClassDb.tutorialDialogBoxManager.ToggleTutorialDialogBox();

        TutorialDialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StringDb.tutorialWelcome7));

        ClassDb.tutorialDialogBoxManager.SetTutorialDialog(message.body);

        TutorialDialogBoxManager.btnTutorialDialog.onClick.RemoveAllListeners();
        TutorialDialogBoxManager.btnTutorialDialog.onClick.AddListener(delegate
        {
            ClassDb.tutorialDialogBoxManager.ToggleTutorialDialogBox();
        });

    }

    public void SecurityCheckMessage()
    {
        ClassDb.tutorialDialogBoxManager.ToggleTutorialDialogBox();

        TutorialDialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StringDb.tutorialSecurityCheck));

        ClassDb.tutorialDialogBoxManager.SetTutorialDialog(message.body);

        TutorialDialogBoxManager.btnTutorialDialog.onClick.RemoveAllListeners();
        TutorialDialogBoxManager.btnTutorialDialog.onClick.AddListener(delegate
        {
            ClassDb.tutorialDialogBoxManager.ToggleTutorialDialogBox();
        });

    }

    public void ServerMessage()
    {
        ClassDb.tutorialDialogBoxManager.ToggleTutorialDialogBox();

        TutorialDialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StringDb.tutorialServer));

        ClassDb.tutorialDialogBoxManager.SetTutorialDialog(message.body);

        TutorialDialogBoxManager.btnTutorialDialog.onClick.RemoveAllListeners();
        TutorialDialogBoxManager.btnTutorialDialog.onClick.AddListener(delegate
        {
            ClassDb.tutorialDialogBoxManager.ToggleTutorialDialogBox();
        });

    }

    public void Telephone1Message()
    {
        ClassDb.tutorialDialogBoxManager.ToggleTutorialDialogBox();

        TutorialDialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StringDb.tutorialTelephone1));

        ClassDb.tutorialDialogBoxManager.SetTutorialDialog(message.body);

        TutorialDialogBoxManager.btnTutorialDialog.onClick.RemoveAllListeners();
        TutorialDialogBoxManager.btnTutorialDialog.onClick.AddListener(delegate
        {
            ClassDb.tutorialDialogBoxManager.ToggleTutorialDialogBox();
        });

    }

    public void Telephone2Message()
    {
        ClassDb.tutorialDialogBoxManager.ToggleTutorialDialogBox();

        TutorialDialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StringDb.tutorialTelephone2));

        ClassDb.tutorialDialogBoxManager.SetTutorialDialog(message.body);

        TutorialDialogBoxManager.btnTutorialDialog.onClick.RemoveAllListeners();
        TutorialDialogBoxManager.btnTutorialDialog.onClick.AddListener(delegate
        {
            ClassDb.tutorialDialogBoxManager.ToggleTutorialDialogBox();
        });

    }

    public void RemoteAttackMessage()
    {
        ClassDb.tutorialDialogBoxManager.ToggleTutorialDialogBox();

        TutorialDialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StringDb.tutorialRemoteAttack));

        ClassDb.tutorialDialogBoxManager.SetTutorialDialog(message.body);

        TutorialDialogBoxManager.btnTutorialDialog.onClick.RemoveAllListeners();
        TutorialDialogBoxManager.btnTutorialDialog.onClick.AddListener(delegate
        {
            ClassDb.tutorialDialogBoxManager.ToggleTutorialDialogBox();
        });

    }

    public void PostWelcomeMessage()
    {
        ClassDb.tutorialDialogBoxManager.ToggleTutorialDialogBox();

        TutorialDialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StringDb.tutorialPostWelcome));

        ClassDb.tutorialDialogBoxManager.SetTutorialDialog(message.body);

        TutorialDialogBoxManager.btnTutorialDialog.onClick.RemoveAllListeners();
        TutorialDialogBoxManager.btnTutorialDialog.onClick.AddListener(delegate
        {
            ClassDb.tutorialDialogBoxManager.ToggleTutorialDialogBox();
        });

    }
    
    public void PostAttackMessage()
    {
        ClassDb.tutorialDialogBoxManager.ToggleTutorialDialogBox();

        TutorialDialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StringDb.tutorialPostAttack));

        ClassDb.tutorialDialogBoxManager.SetTutorialDialog(message.body);

        TutorialDialogBoxManager.btnTutorialDialog.onClick.RemoveAllListeners();
        TutorialDialogBoxManager.btnTutorialDialog.onClick.AddListener(delegate
        {
            ClassDb.tutorialDialogBoxManager.ToggleTutorialDialogBox();
        });

    }

    public void MarketPanel1Message()
    {
        ClassDb.tutorialDialogBoxManager.ToggleTutorialDialogBox();

        TutorialDialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StringDb.tutorialMarketPanel1));

        ClassDb.tutorialDialogBoxManager.SetTutorialDialog(message.body);

        TutorialDialogBoxManager.btnTutorialDialog.onClick.RemoveAllListeners();
        TutorialDialogBoxManager.btnTutorialDialog.onClick.AddListener(delegate
        {
            ClassDb.tutorialDialogBoxManager.ToggleTutorialDialogBox();
        });

    }

    public void MarketPanel2Message()
    {
        ClassDb.tutorialDialogBoxManager.ToggleTutorialDialogBox();

        TutorialDialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StringDb.tutorialMarketPanel2));

        ClassDb.tutorialDialogBoxManager.SetTutorialDialog(message.body);

        TutorialDialogBoxManager.btnTutorialDialog.onClick.RemoveAllListeners();
        TutorialDialogBoxManager.btnTutorialDialog.onClick.AddListener(delegate
        {
            ClassDb.tutorialDialogBoxManager.ToggleTutorialDialogBox();
        });

    }

    public void LocalAttackMessage()
    {
        ClassDb.tutorialDialogBoxManager.ToggleTutorialDialogBox();

        TutorialDialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StringDb.tutorialLocalAttack));

        ClassDb.tutorialDialogBoxManager.SetTutorialDialog(message.body);

        TutorialDialogBoxManager.btnTutorialDialog.onClick.RemoveAllListeners();
        TutorialDialogBoxManager.btnTutorialDialog.onClick.AddListener(delegate
        {
            ClassDb.tutorialDialogBoxManager.ToggleTutorialDialogBox();
        });

    }

    public void InteractiveObjectMessage()
    {
        ClassDb.tutorialDialogBoxManager.ToggleTutorialDialogBox();

        TutorialDialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StringDb.tutorialInteractiveObject));

        ClassDb.tutorialDialogBoxManager.SetTutorialDialog(message.body);

        TutorialDialogBoxManager.btnTutorialDialog.onClick.RemoveAllListeners();
        TutorialDialogBoxManager.btnTutorialDialog.onClick.AddListener(delegate
        {
            ClassDb.tutorialDialogBoxManager.ToggleTutorialDialogBox();
        });

    }

    public void HMIPCMessage()
    {
        ClassDb.tutorialDialogBoxManager.ToggleTutorialDialogBox();

        TutorialDialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StringDb.tutorialHMIPC));

        ClassDb.tutorialDialogBoxManager.SetTutorialDialog(message.body);

        TutorialDialogBoxManager.btnTutorialDialog.onClick.RemoveAllListeners();
        TutorialDialogBoxManager.btnTutorialDialog.onClick.AddListener(delegate
        {
            ClassDb.tutorialDialogBoxManager.ToggleTutorialDialogBox();
        });

    }

    public void HMIPanelMessage()
    {
        ClassDb.tutorialDialogBoxManager.ToggleTutorialDialogBox();

        TutorialDialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StringDb.tutorialHMIPanel));

        ClassDb.tutorialDialogBoxManager.SetTutorialDialog(message.body);

        TutorialDialogBoxManager.btnTutorialDialog.onClick.RemoveAllListeners();
        TutorialDialogBoxManager.btnTutorialDialog.onClick.AddListener(delegate
        {
            ClassDb.tutorialDialogBoxManager.ToggleTutorialDialogBox();
        });

    }

    public void FinalMessage()
    {
        ClassDb.tutorialDialogBoxManager.ToggleTutorialDialogBox();

        TutorialDialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StringDb.tutorialFinalMessage));

        ClassDb.tutorialDialogBoxManager.SetTutorialDialog(message.body);

        TutorialDialogBoxManager.btnTutorialDialog.onClick.RemoveAllListeners();
        TutorialDialogBoxManager.btnTutorialDialog.onClick.AddListener(delegate
        {
            ClassDb.tutorialDialogBoxManager.ToggleTutorialDialogBox();
        });

    }

    public void NoteBookMessage()
    {
        ClassDb.tutorialDialogBoxManager.ToggleTutorialDialogBox();

        TutorialDialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StringDb.notebookMessage));

        ClassDb.tutorialDialogBoxManager.SetTutorialDialog(message.body);

        TutorialDialogBoxManager.btnTutorialDialog.onClick.RemoveAllListeners();
        TutorialDialogBoxManager.btnTutorialDialog.onClick.AddListener(delegate
        {
            ClassDb.tutorialDialogBoxManager.ToggleTutorialDialogBox();
        });

    }

    public void StartExit()
    {
        exitRoutine = Exit();
        StartCoroutine(exitRoutine);
    }

    private IEnumerator Exit()
    {
        yield return new WaitWhile(() => DialogBoxManager.dialogEnabled);

        ClassDb.dialogBoxManager.ToggleDialogBox();

        //disable interaction with pause menu
        ClassDb.pauseManager.pauseRaycaster.enabled = false;
        //open dialog box

        //set the text on dialog box
        DialogBoxMessage message = ClassDb.levelMessageManager.MessageFromJson(Resources.Load<TextAsset>(StringDb.tutorialExit));

        ClassDb.dialogBoxManager.SetDialog(
            message.head,
            message.body,
            message.backBtn,
            message.nextBtn
        );

        //set listeners for the buttons on the dialog box
        DialogBoxManager.dialogBoxBtnNext.onClick.RemoveAllListeners();
        DialogBoxManager.dialogBoxBtnNext.gameObject.SetActive(true);
        DialogBoxManager.dialogBoxBtnNext.onClick.AddListener(delegate
        {
            ClassDb.pauseManager.TogglePauseMenu();
            ClassDb.dialogBoxManager.ToggleDialogBox();
            ClassDb.sceneLoader.StartLoadByIndex(GameData.lastSceneIndex);
        });

        DialogBoxManager.dialogBoxBtnBack.onClick.RemoveAllListeners();
        DialogBoxManager.dialogBoxBtnBack.gameObject.SetActive(true);
        DialogBoxManager.dialogBoxBtnBack.onClick.AddListener(delegate
        {
            ClassDb.dialogBoxManager.ToggleDialogBox();
            ClassDb.pauseManager.pauseRaycaster.enabled = true;
        });
    }


}
