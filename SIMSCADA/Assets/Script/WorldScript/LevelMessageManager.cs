using System;
using System.Collections;
using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMessageManager : MonoBehaviour
{
    private IEnumerator deployRoutine;
    private IEnumerator stopRoutine;
    private IEnumerator moneyLossRoutine;
    private IEnumerator idsInterceptRoutine;
    private IEnumerator welcomeRoutine;
    private IEnumerator exitRoutine;
    private IEnumerator purchaseRoutine;
    private IEnumerator localRecapRoutine;
    private IEnumerator failedCorruptionRoutine;
    private IEnumerator idsCleanRoutine;
    private IEnumerator moneyEarnTrueRoutine;
    private IEnumerator moneyEarnFalseRoutine;
    private IEnumerator remoteRecapRoutine;
    private IEnumerator suspiciousAiRoutine;
    private IEnumerator plantCheckRoutine;
    private IEnumerator malwareCheckRoutine;
    private IEnumerator threatManagementRoutine;
    private IEnumerator newTrustedEmployeesRoutine;
    private IEnumerator newEmployeesHiredRoutine;
    private IEnumerator showLessonFirstTimeRoutine;
    private IEnumerator showReportRoutine;
    private IEnumerator endGameRoutine;
    private IEnumerator closeDialogRoutine;

    private ILevelManager manager;

    private float messageDelay = 0f;

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


    public DialogBoxMessage MessageFromJson(TextAsset jsonFile)
    {
        DialogBoxMessage dialogBoxMessage = JsonUtility.FromJson<DialogBoxMessage>(jsonFile.text);

        Debug.Log(dialogBoxMessage.ToString());

        return dialogBoxMessage;
    }

    // Start is called before the first frame update
    public void StartWelcome()
    {
        welcomeRoutine = Welcome();
        StartCoroutine(welcomeRoutine);
    }

    private IEnumerator Welcome()
    {
        yield return new WaitForSeconds(messageDelay);

        Canvas dialog = ClassDb.dialogBoxManager.OpenDialog();

        DialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StaticDb.welcomeTxt));

        dialog.GetComponent<DialogBoxManager>().SetDialog(
            message.head,
            message.body,
            message.backBtn,
            message.nextBtn,
            dialog
        );

        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnNext.onClick.RemoveAllListeners();
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnNext.gameObject.SetActive(true);
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnNext.onClick.AddListener(delegate
        {
            ClassDb.dialogBoxManager.CloseDialog(dialog);
            //TODO CHECK IF StopAllCoroutines METHOD IS WORKING CORRECTLY
            manager.StopAllCoroutines();
            ClassDb.sceneLoader.StartLoadByIndex(StaticDb.tutorialSceneIndex);

        });

        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnBack.onClick.RemoveAllListeners();
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnBack.gameObject.SetActive(true);
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnBack.onClick.AddListener(delegate
        {
            ClassDb.dialogBoxManager.CloseDialog(dialog);
        });

    }

    public void StartThreatDeployed(Threat threat)
    {
        deployRoutine = ThreatDeployed(threat);
        StartCoroutine(deployRoutine);
    }

    private IEnumerator ThreatDeployed(Threat threat)
    {
        yield return new WaitForSeconds(messageDelay);

        manager.GetGameData().lastThreatDeployed = threat;

        Canvas dialog = ClassDb.dialogBoxManager.OpenDialog();

        DialogBoxMessage message;

        switch (threat.threatType)
        {
            case StaticDb.ThreatType.local:
                message = MessageFromJson(Resources.Load<TextAsset>(StaticDb.localDeployed));
                break;
            case StaticDb.ThreatType.remote:
                message = MessageFromJson(Resources.Load<TextAsset>(StaticDb.remoteDeployed));
                break;
            case StaticDb.ThreatType.fakeLocal:
                ClassDb.dialogBoxManager.CloseDialog(dialog);
                yield break;
            case StaticDb.ThreatType.timeEvent:
                ClassDb.dialogBoxManager.CloseDialog(dialog);
                yield break;

            default:
                throw new ArgumentOutOfRangeException();
        }


        DialogBoxMessage messageIntern = threat.threatAttacker == StaticDb.ThreatAttacker.intern
            ? MessageFromJson(Resources.Load<TextAsset>(StaticDb.internalMessage))
            : StaticDb.emptyMessage;

        dialog.GetComponent<DialogBoxManager>().SetDialog(
            message.head,
            string.Format(message.body + "\n" + messageIntern.body, threat.threatAttack),
            message.backBtn,
            message.nextBtn,
            dialog
        );

        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnNext.onClick.RemoveAllListeners();
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnNext.gameObject.SetActive(true);
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnNext.onClick.AddListener(delegate
        {
            ClassDb.dialogBoxManager.CloseDialog(dialog);
        });

        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnBack.onClick.RemoveAllListeners();
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnBack.gameObject.SetActive(false);
    }

    public void StartThreatStopped(Threat threat)
    {
        stopRoutine = ThreatStopped(threat);
        StartCoroutine(stopRoutine);
    }

    private IEnumerator ThreatStopped(Threat threat)
    {
        yield return new WaitForSeconds(messageDelay);

        manager.GetGameData().lastThreatStopped = threat;

        Canvas dialog = ClassDb.dialogBoxManager.OpenDialog();

        DialogBoxMessage message;

        switch (threat.threatType)
        {
            case StaticDb.ThreatType.local:
                message =  MessageFromJson(Resources.Load<TextAsset>(StaticDb.localStopped));
                break;
            case StaticDb.ThreatType.remote:
                message = MessageFromJson(Resources.Load<TextAsset>(StaticDb.remoteStopped));
                break;
            case StaticDb.ThreatType.fakeLocal:
                message = MessageFromJson(Resources.Load<TextAsset>(StaticDb.fakeLocalStopped));
                break;
            case StaticDb.ThreatType.timeEvent:
                ClassDb.dialogBoxManager.CloseDialog(dialog);
                yield break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        DialogBoxMessage messageIntern;
        if (threat.threatType != StaticDb.ThreatType.fakeLocal)
        {
            messageIntern = threat.threatAttacker == StaticDb.ThreatAttacker.intern
                ? MessageFromJson(Resources.Load<TextAsset>(StaticDb.internalMessage))
                : StaticDb.emptyMessage;
        }
        else
        {
            messageIntern = StaticDb.emptyMessage;
        }

        dialog.GetComponent<DialogBoxManager>().SetDialog(
            message.head,
            message.body + "\n" + messageIntern.body,
            message.backBtn,
            message.nextBtn,
            dialog
        );

        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnNext.onClick.RemoveAllListeners();
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnNext.gameObject.SetActive(true);
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnNext.onClick.AddListener(delegate
        {
            ClassDb.dialogBoxManager.CloseDialog(dialog);
        });

        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnBack.onClick.RemoveAllListeners();
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnBack.gameObject.SetActive(false);
    }

    public void StartIdsInterception()
    {
        idsInterceptRoutine = IdsInterception();
        StartCoroutine(idsInterceptRoutine);
    }

    private IEnumerator IdsInterception()
    {
        yield return new WaitForSeconds(messageDelay);

        Canvas dialog = ClassDb.dialogBoxManager.OpenDialog();

        DialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StaticDb.idsInterception));

        dialog.GetComponent<DialogBoxManager>().SetDialog(
            message.head,
            message.body,
            message.backBtn,
            message.nextBtn,
            dialog
        );

        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnNext.onClick.RemoveAllListeners();
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnNext.gameObject.SetActive(true);
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnNext.onClick.AddListener(delegate
        {
            ClassDb.dialogBoxManager.CloseDialog(dialog);
        });

        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnBack.onClick.RemoveAllListeners();
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnBack.gameObject.SetActive(false);
    }

    public void StartIdsClean()
    {
        idsCleanRoutine = IdsClean();
        StartCoroutine(idsCleanRoutine);
    }

    private IEnumerator IdsClean()
    {
        yield return new WaitForSeconds(messageDelay);

        Canvas dialog = ClassDb.dialogBoxManager.OpenDialog();

        DialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StaticDb.idsCleaned));

        dialog.GetComponent<DialogBoxManager>().SetDialog(
            message.head,
            message.body,
            message.backBtn,
            message.nextBtn,
            dialog
        );

        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnNext.onClick.RemoveAllListeners();
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnNext.gameObject.SetActive(true);
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnNext.onClick.AddListener(delegate
        {
            ClassDb.dialogBoxManager.CloseDialog(dialog);
        });

        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnBack.onClick.RemoveAllListeners();
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnBack.gameObject.SetActive(false);
    }

    public void StartMoneyLoss(StaticDb.ThreatType type, float moneyLoss)
    {
        moneyLossRoutine = MoneyLoss(type, moneyLoss);
        StartCoroutine(moneyLossRoutine);
    }

    private IEnumerator MoneyLoss(StaticDb.ThreatType type, float moneyLoss)
    {
        yield return new WaitForSeconds(messageDelay);

        manager.GetGameData().lastTypeLoss = type;
        manager.GetGameData().lastAmountLoss = moneyLoss;

        Canvas dialog = ClassDb.dialogBoxManager.OpenDialog();

        DialogBoxMessage message;


        switch (type)
        {
            case StaticDb.ThreatType.local:
                message = MessageFromJson(Resources.Load<TextAsset>(StaticDb.localMoneyLoss));
                break;
            case StaticDb.ThreatType.remote:
                message = MessageFromJson(Resources.Load<TextAsset>(StaticDb.remoteMoneyLoss));
                break;
            case StaticDb.ThreatType.fakeLocal:
                message = MessageFromJson(Resources.Load<TextAsset>(StaticDb.fakeLocalMoneyLoss));
                break;
            case StaticDb.ThreatType.timeEvent:
                ClassDb.dialogBoxManager.CloseDialog(dialog);
                yield break;
            default:
                throw new ArgumentOutOfRangeException();
        }


        dialog.GetComponent<DialogBoxManager>().SetDialog(
            message.head,
            string.Format(message.body, Math.Round(moneyLoss, 0).ToString(CultureInfo.CurrentCulture)),
            message.backBtn,
            message.nextBtn,
            dialog
        );

        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnNext.onClick.RemoveAllListeners();
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnNext.gameObject.SetActive(true);
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnNext.onClick.AddListener(delegate
        {
            ClassDb.dialogBoxManager.CloseDialog(dialog);
        });

        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnBack.onClick.RemoveAllListeners();
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnBack.gameObject.SetActive(false);

    }

    public void StartExit(byte[] imageBytes)
    {
        exitRoutine = Exit(imageBytes);
        StartCoroutine(exitRoutine);
    }

    private IEnumerator Exit(byte[] imageBytes)
    {
        yield return new WaitForSeconds(messageDelay);

        Canvas dialog = ClassDb.dialogBoxManager.OpenDialog();

        //disable interaction with pause menu
        ClassDb.pauseManager.pauseRaycaster.enabled = false;
        //open dialog box

        //set the text on dialog box
        DialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StaticDb.exitTxt));

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
            ClassDb.gameDataManager.StartSaveLevelGameData(imageBytes);
            ClassDb.sceneLoader.StartLoadByIndex(StaticDb.menuSceneIndex);
        });

        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnBack.onClick.RemoveAllListeners();
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnBack.gameObject.SetActive(true);
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnBack.onClick.AddListener(delegate 
        {
            ClassDb.dialogBoxManager.CloseDialog(dialog);
            ClassDb.pauseManager.pauseRaycaster.enabled = true;
        });
    }

    public void StartConfirmPurchase(ItemStore item)
    {
        purchaseRoutine = ConfirmPurchase(item);
        StartCoroutine(purchaseRoutine);
    }

    private IEnumerator ConfirmPurchase(ItemStore item)
    {
        yield return new WaitForSeconds(messageDelay);
        Canvas dialog = ClassDb.dialogBoxManager.OpenDialog();

        StoreManager storeManager = GameObject.Find(StaticDb.storeScreenName).GetComponent<StoreManager>();

        DialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StaticDb.purchase));

        dialog.GetComponent<DialogBoxManager>().SetDialog(
            message.head,
            string.Format(message.body, item.price),
            message.backBtn,
            message.nextBtn,
            dialog
        );

        //open dialog box
        //set listeners for the buttons on the dialog box
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnNext.onClick.RemoveAllListeners();
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnNext.gameObject.SetActive(true);
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnNext.onClick.AddListener(delegate
        {
            ClassDb.dialogBoxManager.CloseDialog(dialog);
            storeManager.PurchaseItem(item);
        });

        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnBack.onClick.RemoveAllListeners();
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnBack.gameObject.SetActive(true);
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnBack.onClick.AddListener(delegate
        {
            ClassDb.dialogBoxManager.CloseDialog(dialog);
        });
    }

    public void StartFailedCorruption()
    {
        failedCorruptionRoutine = FailedCorruption();
        StartCoroutine(failedCorruptionRoutine);
    }

    private IEnumerator FailedCorruption()
    {
        yield return new WaitForSeconds(messageDelay);

        Canvas dialog = ClassDb.dialogBoxManager.OpenDialog();

        DialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StaticDb.failedCorruption));

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
            ClassDb.dialogBoxManager.CloseDialog(dialog);
        });

        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnBack.onClick.RemoveAllListeners();
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnBack.gameObject.SetActive(false);

    }

    public void StartMoneyEarnTrue(float moneyEarn)
    {
        moneyEarnTrueRoutine = MoneyEarnTrue(moneyEarn);
        StartCoroutine(moneyEarnTrueRoutine);
    }

    private IEnumerator MoneyEarnTrue(float moneyEarn)
    {
        yield return new WaitForSeconds(messageDelay);

        manager.GetGameData().lastAmountEarn = moneyEarn;

        Canvas dialog = ClassDb.dialogBoxManager.OpenDialog();

        DialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StaticDb.moneyEarnTrue));

        dialog.GetComponent<DialogBoxManager>().SetDialog(
            message.head,
            string.Format(message.body, Math.Round(moneyEarn, 0).ToString(CultureInfo.CurrentCulture)),
            message.backBtn,
            message.nextBtn,
            dialog
        );

        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnNext.onClick.RemoveAllListeners();
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnNext.gameObject.SetActive(true);
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnNext.onClick.AddListener(delegate
        {
            ClassDb.dialogBoxManager.CloseDialog(dialog);
            FindObjectOfType<RoomPcListener>().ToggleStoreScreen();
        });

        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnBack.onClick.RemoveAllListeners();
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnBack.gameObject.SetActive(true);
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnBack.onClick.AddListener(delegate
        {
            ClassDb.dialogBoxManager.CloseDialog(dialog);
        }); 
    }

    public void StartMoneyEarnFalse()
    {
        moneyEarnFalseRoutine = MoneyEarnFalse();
        StartCoroutine(moneyEarnFalseRoutine);
    }

    private IEnumerator MoneyEarnFalse()
    {
        yield return new WaitForSeconds(messageDelay);

        Canvas dialog = ClassDb.dialogBoxManager.OpenDialog();

        DialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StaticDb.moneyEarnFalse));

        dialog.GetComponent<DialogBoxManager>().SetDialog(
            message.head,
            message.body,
            message.backBtn,
            message.nextBtn,
            dialog
        );

        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnNext.onClick.RemoveAllListeners();
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnNext.gameObject.SetActive(true);
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnNext.onClick.AddListener(delegate
        {
            ClassDb.dialogBoxManager.CloseDialog(dialog);
        });

        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnBack.onClick.RemoveAllListeners();
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnBack.gameObject.SetActive(false);
    }

    public void StartSuspiciousAi()
    {
        suspiciousAiRoutine = SuspiciousAi();
        StartCoroutine(suspiciousAiRoutine);
    }

    private IEnumerator SuspiciousAi()
    {
        yield return new WaitForSeconds(messageDelay);

        Canvas dialog = ClassDb.dialogBoxManager.OpenDialog();

        DialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StaticDb.suspiciousAi));

        dialog.GetComponent<DialogBoxManager>().SetDialog(
            message.head,
            message.body,
            message.backBtn,
            message.nextBtn,
            dialog
        );

        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnNext.onClick.RemoveAllListeners();
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnNext.gameObject.SetActive(true);
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnNext.onClick.AddListener(delegate
        {
            ClassDb.dialogBoxManager.CloseDialog(dialog);
        });

        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnBack.onClick.RemoveAllListeners();
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnBack.gameObject.SetActive(false);
    }

    public void StartPlantCheck()
    {
        plantCheckRoutine = PlantCheck();
        StartCoroutine(plantCheckRoutine);
    }

    private IEnumerator PlantCheck()
    {
        yield return new WaitForSeconds(messageDelay);

        Canvas dialog = ClassDb.dialogBoxManager.OpenDialog();

        DialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StaticDb.plantCheck));

        dialog.GetComponent<DialogBoxManager>().SetDialog(
            message.head,
            message.body,
            message.backBtn,
            message.nextBtn,
            dialog
        );

        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnNext.onClick.RemoveAllListeners();
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnNext.gameObject.SetActive(true);
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnNext.onClick.AddListener(delegate
        {
            ClassDb.dialogBoxManager.CloseDialog(dialog);
        });

        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnBack.onClick.RemoveAllListeners();
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnBack.gameObject.SetActive(false);
    }

    public void StartMalwareCheck()
    {
        malwareCheckRoutine = MalwareCheck();
        StartCoroutine(malwareCheckRoutine);
    }

    private IEnumerator MalwareCheck()
    {
        yield return new WaitForSeconds(messageDelay);

        Canvas dialog = ClassDb.dialogBoxManager.OpenDialog();

        DialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StaticDb.malwareCheck));

        dialog.GetComponent<DialogBoxManager>().SetDialog(
            message.head,
            message.body,
            message.backBtn,
            message.nextBtn,
            dialog
        );

        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnNext.onClick.RemoveAllListeners();
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnNext.gameObject.SetActive(true);
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnNext.onClick.AddListener(delegate
        {
            ClassDb.dialogBoxManager.CloseDialog(dialog);
        });

        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnBack.onClick.RemoveAllListeners();
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnBack.gameObject.SetActive(false);
    }

    public void StartThreatManagementResult(TimeSpan elapsedTime, float moneyLoss)
    {
        threatManagementRoutine = ThreatManagementResult(elapsedTime, moneyLoss);
        StartCoroutine(threatManagementRoutine);
    }

    private IEnumerator ThreatManagementResult(TimeSpan elapsedTime, float moneyLoss)
    {
        yield return new WaitForSeconds(messageDelay);

        manager.GetGameData().lastManagementTime = elapsedTime;
        manager.GetGameData().lastAmountLoss = moneyLoss;

        Canvas dialog = ClassDb.dialogBoxManager.OpenDialog();

        DialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StaticDb.threatManagementResult));

        dialog.GetComponent<DialogBoxManager>().SetDialog(
            message.head,
            string.Format(message.body, elapsedTime.TotalMinutes.ToString(CultureInfo.CurrentCulture), 
                Math.Round(moneyLoss, 0).ToString(CultureInfo.CurrentCulture)),
            message.backBtn,
            message.nextBtn,
            dialog
        );

        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnNext.onClick.RemoveAllListeners();
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnNext.gameObject.SetActive(true);
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnNext.onClick.AddListener(delegate
        {
            ClassDb.dialogBoxManager.CloseDialog(dialog);
        });

        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnBack.onClick.RemoveAllListeners();
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnBack.gameObject.SetActive(false);
    }

    public void StartNewTrustedEmployees()
    {
        newTrustedEmployeesRoutine = NewTrustedEmployees();
        StartCoroutine(newTrustedEmployeesRoutine);
    }

    private IEnumerator NewTrustedEmployees()
    {
        yield return new WaitForSeconds(messageDelay);

        Canvas dialog = ClassDb.dialogBoxManager.OpenDialog();

        DialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StaticDb.newTrustedEmployees));

        dialog.GetComponent<DialogBoxManager>().SetDialog(
            message.head,
            message.body,
            message.backBtn,
            message.nextBtn,
            dialog
        );

        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnNext.onClick.RemoveAllListeners();
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnNext.gameObject.SetActive(true);
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnNext.onClick.AddListener(delegate
        {
            ClassDb.dialogBoxManager.CloseDialog(dialog);
        });

        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnBack.onClick.RemoveAllListeners();
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnBack.gameObject.SetActive(false);
    }

    public void StartNewEmployeesHired(int employeesHired)
    {
        newEmployeesHiredRoutine = NewEmployeesHired(employeesHired);
        StartCoroutine(newEmployeesHiredRoutine);
    }

    private IEnumerator NewEmployeesHired(int employeesHired)
    {
        yield return new WaitForSeconds(messageDelay);

        manager.GetGameData().employeesHired = employeesHired;

        Canvas dialog = ClassDb.dialogBoxManager.OpenDialog();

        DialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StaticDb.newEmployeesHired));

        dialog.GetComponent<DialogBoxManager>().SetDialog(
            message.head,
            string.Format(message.body, employeesHired),
            message.backBtn,
            message.nextBtn,
            dialog
        );

        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnNext.onClick.RemoveAllListeners();
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnNext.gameObject.SetActive(true);
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnNext.onClick.AddListener(delegate
        {
            ClassDb.dialogBoxManager.CloseDialog(dialog);
            manager.GetGameData().totalEmployees += employeesHired;
        });

        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnBack.onClick.RemoveAllListeners();
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnBack.gameObject.SetActive(false);
    }

    public void StartShowLessonFirstTime(Threat threat)
    {
        showLessonFirstTimeRoutine = ShowLessonFirstTime(threat);
        StartCoroutine(showLessonFirstTimeRoutine);
    }

    private IEnumerator ShowLessonFirstTime(Threat threat)
    {
        yield return new WaitForSeconds(messageDelay);

        manager.GetGameData().firstThreat = threat;

        Canvas dialog = ClassDb.dialogBoxManager.OpenDialog();

        DialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StaticDb.showLessonFirstTime));

        dialog.GetComponent<DialogBoxManager>().SetDialog(
            message.head,
            string.Format(message.body, threat.threatAttack),
            message.backBtn,
            message.nextBtn,
            dialog
        );

        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnNext.onClick.RemoveAllListeners();
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnNext.gameObject.SetActive(true);
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnNext.onClick.AddListener(delegate
        {
            ClassDb.dialogBoxManager.CloseDialog(dialog);

            NotebookManager.isFirstLesson = true;
            NotebookManager.firstLessonThreat = threat;

            ClassDb.notebookManager.ToggleNoteBook();

        });

        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnBack.onClick.RemoveAllListeners();
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnBack.gameObject.SetActive(false);
    }

    public void StartShowReport(string threatAttack)
    {
        showReportRoutine = ShowReport(threatAttack);
        StartCoroutine(showReportRoutine);
    }

    private IEnumerator ShowReport(string threatAttack)
    {
        yield return new WaitForSeconds(messageDelay);

        Canvas dialog = ClassDb.dialogBoxManager.OpenDialog();

        DialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StaticDb.researchReport));

        dialog.GetComponent<DialogBoxManager>().SetDialog(
            message.head,
            string.Format(message.body, threatAttack),
            message.backBtn,
            message.nextBtn,
            dialog
        );

        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnNext.onClick.RemoveAllListeners();
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnNext.gameObject.SetActive(true);
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnNext.onClick.AddListener(delegate
        {
            ClassDb.dialogBoxManager.CloseDialog(dialog);
        });

        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnBack.onClick.RemoveAllListeners();
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnBack.gameObject.SetActive(false);
    }

    public void StartEndGame()
    {
        endGameRoutine = EndGame();
        StartCoroutine(endGameRoutine);
    }

    private IEnumerator EndGame()
    {
        yield return new WaitForSeconds(messageDelay);

        Canvas dialog = ClassDb.dialogBoxManager.OpenDialog();

        DialogBoxMessage message = MessageFromJson(manager.GetGameData().isGameWon 
            ? Resources.Load<TextAsset>(StaticDb.endGameWin) 
            : Resources.Load<TextAsset>(StaticDb.endGameLoss));

        dialog.GetComponent<DialogBoxManager>().SetDialog(
            message.head,
            message.body,
            message.backBtn,
            message.nextBtn,
            dialog
        );

        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnNext.onClick.RemoveAllListeners();
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnNext.gameObject.SetActive(true);
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnNext.onClick.AddListener(delegate
        {
            ClassDb.dialogBoxManager.CloseDialog(dialog);
            //LOAD LEVEL 2
            ClassDb.sceneLoader.StartLoadByIndex(StaticDb.level2SceneIndex);
        });

        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnBack.onClick.RemoveAllListeners();
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnBack.gameObject.SetActive(true);
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnBack.onClick.AddListener(delegate
        {
            ClassDb.dialogBoxManager.CloseDialog(dialog);
        });
    }

    public void StartCloseDialog()
    {
        closeDialogRoutine = CloseDialog();
        StartCoroutine(closeDialogRoutine);
    }

    private IEnumerator CloseDialog()
    {
        yield return new WaitForSeconds(messageDelay);

        Canvas dialog = ClassDb.dialogBoxManager.OpenDialog();

        DialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StaticDb.closeDialog));

        dialog.GetComponent<DialogBoxManager>().SetDialog(
            message.head,
            message.body,
            message.backBtn,
            message.nextBtn,
            dialog
        );

        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnNext.onClick.RemoveAllListeners();
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnNext.gameObject.SetActive(true);
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnNext.onClick.AddListener(delegate
        {
            ClassDb.dialogBoxManager.CloseDialog(dialog);
        });

        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnBack.onClick.RemoveAllListeners();
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnBack.gameObject.SetActive(false);
    }


    //RIMUOVIBILE
    //--------------------------------------------------------------------------------------------------------
    //public void StartThreatRecap(StaticDb.ThreatType type, int threatCount)
    //{
    //    localRecapRoutine = ThreatRecap(type, threatCount);
    //    StartCoroutine(localRecapRoutine);
    //}

    //private IEnumerator ThreatRecap(StaticDb.ThreatType type, int threatCount)
    //{
    //    yield return new WaitForSeconds(messageDelay);
    //    yield return new WaitWhile(() => DialogBoxManager.dialogEnabled);

    //    ClassDb.dialogBoxManager.ToggleDialogBox();

    //    DialogBoxMessage message = new DialogBoxMessage();

    //    switch (type)
    //    {
    //        case StaticDb.ThreatType.local:
    //            message = MessageFromJson(Resources.Load<TextAsset>(StaticDb.localThreatRecap));
    //            break;
    //        case StaticDb.ThreatType.remote:
    //            message = MessageFromJson(Resources.Load<TextAsset>(StaticDb.remoteThreatRecap));
    //            break;
    //        case StaticDb.ThreatType.fakeLocal:
    //            break;
    //        case StaticDb.ThreatType.timeEvent:
    //            break;
    //        default:
    //            throw new ArgumentOutOfRangeException();
    //    }

    //    ClassDb.dialogBoxManager.SetDialog(
    //        message.head,
    //        string.Format(message.body, threatCount),
    //        message.backBtn,
    //        message.nextBtn
    //    );

    //    DialogBoxManager.dialogBoxBtnNext.onClick.RemoveAllListeners();
    //    DialogBoxManager.dialogBoxBtnNext.gameObject.SetActive(true);
    //    DialogBoxManager.dialogBoxBtnNext.onClick.AddListener(ClassDb.dialogBoxManager.ToggleDialogBox);

    //    DialogBoxManager.dialogBoxBtnBack.onClick.RemoveAllListeners();
    //    DialogBoxManager.dialogBoxBtnBack.gameObject.SetActive(false);
    //}
    //--------------------------------------------------------------------------------------------------------
}
