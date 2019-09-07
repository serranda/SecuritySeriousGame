using System;
using System.Collections;
using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialMessageManager : MonoBehaviour
{
    private IEnumerator exitRoutine;

    private ILevelManager manager;

    private void Start()
    {
        manager = SetLevelManager();
    }

    private ILevelManager SetLevelManager()
    {
        ILevelManager iManager;
        if (SceneManager.GetActiveScene().buildIndex == StaticDb.level1SceneIndex)
            iManager = FindObjectOfType<Level1Manager>();
        else
            iManager = FindObjectOfType<Level2Manager>();

        return iManager;
    }


    public TutorialDialogBoxMessage MessageFromJson(TextAsset jsonFile)
    {
        TutorialDialogBoxMessage box = JsonUtility.FromJson<TutorialDialogBoxMessage>(jsonFile.text);

        Debug.Log(box.ToString());

        return box;
    }

    public void Welcome1Message()
    {
        ClassDb.tutorialDialogBoxManager.ToggleTutorialDialogBox();

        TutorialDialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StaticDb.tutorialWelcome1));

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

        TutorialDialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StaticDb.tutorialWelcome2));

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

        TutorialDialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StaticDb.tutorialWelcome3));

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

        TutorialDialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StaticDb.tutorialWelcome4));

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

        TutorialDialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StaticDb.tutorialWelcome5));

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

        TutorialDialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StaticDb.tutorialWelcome6));

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

        TutorialDialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StaticDb.tutorialWelcome7));

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

        TutorialDialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StaticDb.tutorialSecurityCheck));

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

        TutorialDialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StaticDb.tutorialServer));

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

        TutorialDialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StaticDb.tutorialTelephone1));

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

        TutorialDialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StaticDb.tutorialTelephone2));

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

        TutorialDialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StaticDb.tutorialRemoteAttack));

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

        TutorialDialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StaticDb.tutorialPostWelcome));

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

        TutorialDialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StaticDb.tutorialPostAttack));

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

        TutorialDialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StaticDb.tutorialMarketPanel1));

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

        TutorialDialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StaticDb.tutorialMarketPanel2));

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

        TutorialDialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StaticDb.tutorialLocalAttack));

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

        TutorialDialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StaticDb.tutorialInteractiveObject));

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

        TutorialDialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StaticDb.tutorialHMIPC));

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

        TutorialDialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StaticDb.tutorialHMIPanel));

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

        TutorialDialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StaticDb.tutorialFinalMessage));

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

        TutorialDialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StaticDb.notebookMessage));

        ClassDb.tutorialDialogBoxManager.SetTutorialDialog(message.body);

        TutorialDialogBoxManager.btnTutorialDialog.onClick.RemoveAllListeners();
        TutorialDialogBoxManager.btnTutorialDialog.onClick.AddListener(delegate
        {
            ClassDb.tutorialDialogBoxManager.ToggleTutorialDialogBox();
        });

    }

    public void IdsFirewallIpsMessage()
    {
        ClassDb.tutorialDialogBoxManager.ToggleTutorialDialogBox();

        TutorialDialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StaticDb.tutorialIdsFirewallIps));

        ClassDb.tutorialDialogBoxManager.SetTutorialDialog(message.body);

        TutorialDialogBoxManager.btnTutorialDialog.onClick.RemoveAllListeners();
        TutorialDialogBoxManager.btnTutorialDialog.onClick.AddListener(delegate
        {
            ClassDb.tutorialDialogBoxManager.ToggleTutorialDialogBox();
        });
    }

    public void Exit()
    {
        //yield return new WaitWhile(() => DialogBoxManager.dialogEnabled);

        Canvas dialog = ClassDb.dialogBoxManager.OpenDialog();

        //set flag to true: this will prevent esc key to hide pause menu
        StaticDb.isShowingExit = true;

        //disable interaction with pause menu
        ClassDb.pauseManager.pauseRaycaster.enabled = false;
        //open dialog box

        //set the text on dialog box
        DialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StaticDb.tutorialExit)).ToDialogBoxMessage();

        dialog.GetComponent<DialogBoxManager>().SetDialog(
            message.head,
            message.body,
            message.backBtn,
            message.nextBtn,
            dialog
        );

        //set listeners for the buttons on the dialog box
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnNext.onClick.RemoveAllListeners();
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnNext.gameObject.SetActive(true);
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnNext.onClick.AddListener(delegate
        {
            ClassDb.pauseManager.TogglePauseMenu();
            ClassDb.dialogBoxManager.CloseDialog(dialog);
            ClassDb.sceneLoader.StartLoadByIndex(StaticDb.menuSceneIndex);
            StaticDb.isShowingExit = false;
        });

        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnBack.onClick.RemoveAllListeners();
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnBack.gameObject.SetActive(true);
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnBack.onClick.AddListener(delegate
        {
            ClassDb.dialogBoxManager.CloseDialog(dialog);
            ClassDb.pauseManager.pauseRaycaster.enabled = true;
            StaticDb.isShowingExit = false;
        });
    }


}
