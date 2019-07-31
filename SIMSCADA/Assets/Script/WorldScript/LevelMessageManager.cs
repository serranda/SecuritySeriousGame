using System;
using System.Collections;
using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    private IEnumerator moneyEarnRoutine;
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

    private ILevelManager manager;

    private float messageDelay = 0f;

    private void Start()
    {
        manager = SetLevelManager();
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

        yield return new WaitWhile(() => DialogBoxManager.dialogEnabled);

        ClassDb.dialogBoxManager.ToggleDialogBox();

        DialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StringDb.welcomeTxt));

        ClassDb.dialogBoxManager.SetDialog(
            message.head,
            message.body,
            message.backBtn,
            message.nextBtn
        );

        DialogBoxManager.dialogBoxBtnNext.onClick.RemoveAllListeners();
        DialogBoxManager.dialogBoxBtnNext.gameObject.SetActive(true);
        DialogBoxManager.dialogBoxBtnNext.onClick.AddListener(delegate
        {
            ClassDb.dialogBoxManager.ToggleDialogBox();
            //TODO CHECK IF METHOD IS WORKING CORRECTLY
            manager.StopAllCoroutines();
            ClassDb.sceneLoader.StartLoadByIndex(StringDb.tutorialSceneIndex);

        }); ;

        DialogBoxManager.dialogBoxBtnBack.onClick.RemoveAllListeners();
        DialogBoxManager.dialogBoxBtnBack.gameObject.SetActive(true);
        DialogBoxManager.dialogBoxBtnBack.onClick.AddListener(delegate
        {
            TutorialManager.tutorialIsFinished = true;
            ClassDb.dialogBoxManager.ToggleDialogBox();
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
        yield return new WaitWhile(() => DialogBoxManager.dialogEnabled);

        ClassDb.dialogBoxManager.ToggleDialogBox();

        DialogBoxMessage message;
        switch (threat.threatType)
        {
            case StringDb.ThreatType.local:
                message = MessageFromJson(Resources.Load<TextAsset>(StringDb.localDeployed));
                break;
            case StringDb.ThreatType.remote:
                message = MessageFromJson(Resources.Load<TextAsset>(StringDb.remoteDeployed));
                break;
            case StringDb.ThreatType.fakeLocal:
                ClassDb.dialogBoxManager.ToggleDialogBox();
                yield break;
            case StringDb.ThreatType.timeEvent:
                ClassDb.dialogBoxManager.ToggleDialogBox();
                yield break;

            default:
                throw new ArgumentOutOfRangeException();
        }

        DialogBoxMessage messageIntern = threat.threatAttacker == StringDb.ThreatAttacker.intern
            ? MessageFromJson(Resources.Load<TextAsset>(StringDb.internalMessage))
            : StringDb.emptyMessage;

        ClassDb.dialogBoxManager.SetDialog(
            message.head,
            string.Format(message.body + "\n" + messageIntern.body, threat.threatAttack),
            message.backBtn,
            message.nextBtn
        );

        DialogBoxManager.dialogBoxBtnNext.onClick.RemoveAllListeners();
        DialogBoxManager.dialogBoxBtnNext.gameObject.SetActive(true);
        DialogBoxManager.dialogBoxBtnNext.onClick.AddListener(ClassDb.dialogBoxManager.ToggleDialogBox);

        DialogBoxManager.dialogBoxBtnBack.onClick.RemoveAllListeners();
        DialogBoxManager.dialogBoxBtnBack.gameObject.SetActive(false);
    }

    public void StartThreatStopped(Threat threat)
    {
        stopRoutine = ThreatStopped(threat);
        StartCoroutine(stopRoutine);
    }

    private IEnumerator ThreatStopped(Threat threat)
    {
        yield return new WaitForSeconds(messageDelay);
        yield return new WaitWhile(() => DialogBoxManager.dialogEnabled);

        ClassDb.dialogBoxManager.ToggleDialogBox();

        DialogBoxMessage message = new DialogBoxMessage();

        switch (threat.threatType)
        {
            case StringDb.ThreatType.local:
                message =  MessageFromJson(Resources.Load<TextAsset>(StringDb.localStopped));
                break;
            case StringDb.ThreatType.remote:
                message = MessageFromJson(Resources.Load<TextAsset>(StringDb.remoteStopped));
                break;
            case StringDb.ThreatType.fakeLocal:
                message = MessageFromJson(Resources.Load<TextAsset>(StringDb.fakeLocalStopped));
                break;
            case StringDb.ThreatType.timeEvent:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        DialogBoxMessage messageIntern;
        if (threat.threatType != StringDb.ThreatType.fakeLocal)
        {
            messageIntern = threat.threatAttacker == StringDb.ThreatAttacker.intern
                ? MessageFromJson(Resources.Load<TextAsset>(StringDb.internalMessage))
                : StringDb.emptyMessage;
        }
        else
        {
            messageIntern = StringDb.emptyMessage;
        }

        ClassDb.dialogBoxManager.SetDialog(
            message.head,
            message.body + "\n" + messageIntern.body,
            message.backBtn,
            message.nextBtn
        );
        
        DialogBoxManager.dialogBoxBtnNext.onClick.RemoveAllListeners();
        DialogBoxManager.dialogBoxBtnNext.gameObject.SetActive(true);
        DialogBoxManager.dialogBoxBtnNext.onClick.AddListener(ClassDb.dialogBoxManager.ToggleDialogBox);

        DialogBoxManager.dialogBoxBtnBack.onClick.RemoveAllListeners();
        DialogBoxManager.dialogBoxBtnBack.gameObject.SetActive(false);
    }

    public void StartIdsInterception()
    {
        idsInterceptRoutine = IdsInterception();
        StartCoroutine(idsInterceptRoutine);
    }

    private IEnumerator IdsInterception()
    {
        yield return new WaitForSeconds(messageDelay);
        yield return new WaitWhile(() => DialogBoxManager.dialogEnabled);

        ClassDb.dialogBoxManager.ToggleDialogBox();

        DialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StringDb.idsInterception));

        ClassDb.dialogBoxManager.SetDialog(
            message.head,
            message.body,
            message.backBtn,
            message.nextBtn
        );

        DialogBoxManager.dialogBoxBtnNext.onClick.RemoveAllListeners();
        DialogBoxManager.dialogBoxBtnNext.gameObject.SetActive(true);
        DialogBoxManager.dialogBoxBtnNext.onClick.AddListener(ClassDb.dialogBoxManager.ToggleDialogBox);

        DialogBoxManager.dialogBoxBtnBack.onClick.RemoveAllListeners();
        DialogBoxManager.dialogBoxBtnBack.gameObject.SetActive(false);
    }

    public void StartIdsClean()
    {
        idsCleanRoutine = IdsClean();
        StartCoroutine(idsCleanRoutine);
    }

    private IEnumerator IdsClean()
    {
        yield return new WaitForSeconds(messageDelay);
        yield return new WaitWhile(() => DialogBoxManager.dialogEnabled);

        ClassDb.dialogBoxManager.ToggleDialogBox();

        DialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StringDb.idsCleaned));

        ClassDb.dialogBoxManager.SetDialog(
            message.head,
            message.body,
            message.backBtn,
            message.nextBtn
        );

        DialogBoxManager.dialogBoxBtnNext.onClick.RemoveAllListeners();
        DialogBoxManager.dialogBoxBtnNext.gameObject.SetActive(true);
        DialogBoxManager.dialogBoxBtnNext.onClick.AddListener(ClassDb.dialogBoxManager.ToggleDialogBox);

        DialogBoxManager.dialogBoxBtnBack.onClick.RemoveAllListeners();
        DialogBoxManager.dialogBoxBtnBack.gameObject.SetActive(false);
    }

    public void StartMoneyLoss(StringDb.ThreatType type, float moneyLoss)
    {
        moneyLossRoutine = MoneyLoss(type, moneyLoss);
        StartCoroutine(moneyLossRoutine);
    }

    private IEnumerator MoneyLoss(StringDb.ThreatType type, float moneyLoss)
    {
        yield return new WaitForSeconds(messageDelay);
        yield return new WaitWhile(() => DialogBoxManager.dialogEnabled);

        ClassDb.dialogBoxManager.ToggleDialogBox();

        DialogBoxMessage message = new DialogBoxMessage();

        switch (type)
        {
            case StringDb.ThreatType.local:
                message = MessageFromJson(Resources.Load<TextAsset>(StringDb.localMoneyLoss));
                break;
            case StringDb.ThreatType.remote:
                message = MessageFromJson(Resources.Load<TextAsset>(StringDb.remoteMoneyLoss));
                break;
            case StringDb.ThreatType.fakeLocal:
                message = MessageFromJson(Resources.Load<TextAsset>(StringDb.fakeLocalMoneyLoss));
                break;
            case StringDb.ThreatType.timeEvent:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }


        ClassDb.dialogBoxManager.SetDialog(
            message.head,
            string.Format(message.body, Math.Round(moneyLoss, 0).ToString(CultureInfo.CurrentCulture)),
            message.backBtn,
            message.nextBtn
        );

        DialogBoxManager.dialogBoxBtnNext.onClick.RemoveAllListeners();
        DialogBoxManager.dialogBoxBtnNext.gameObject.SetActive(true);
        DialogBoxManager.dialogBoxBtnNext.onClick.AddListener(ClassDb.dialogBoxManager.ToggleDialogBox);

        DialogBoxManager.dialogBoxBtnBack.onClick.RemoveAllListeners();
        DialogBoxManager.dialogBoxBtnBack.gameObject.SetActive(false);

    }

    public void StartExit(byte[] imageBytes)
    {
        exitRoutine = Exit(imageBytes);
        StartCoroutine(exitRoutine);
    }

    private IEnumerator Exit(byte[] imageBytes)
    {
        yield return new WaitForSeconds(messageDelay);
        yield return new WaitWhile(() => DialogBoxManager.dialogEnabled);

        ClassDb.dialogBoxManager.ToggleDialogBox();

        //disable interaction with pause menu
        ClassDb.pauseManager.pauseRaycaster.enabled = false;
        //open dialog box

        //set the text on dialog box
        DialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StringDb.exitTxt));

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
            ClassDb.gameDataManager.StartSaveLevelGameData(imageBytes);
            ClassDb.sceneLoader.StartLoadByIndex(StringDb.menuSceneIndex);
        });

        DialogBoxManager.dialogBoxBtnBack.onClick.RemoveAllListeners();
        DialogBoxManager.dialogBoxBtnBack.gameObject.SetActive(true);
        DialogBoxManager.dialogBoxBtnBack.onClick.AddListener(delegate 
        {
            ClassDb.dialogBoxManager.ToggleDialogBox();
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
        yield return new WaitWhile(() => DialogBoxManager.dialogEnabled);

        ClassDb.dialogBoxManager.ToggleDialogBox();

        StoreManager storeManager = GameObject.Find(StringDb.storeScreenName).GetComponent<StoreManager>();

        DialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StringDb.purchase));

        ClassDb.dialogBoxManager.SetDialog(
            message.head,
            string.Format(message.body, item.price),
            message.backBtn,
            message.nextBtn
        );

        //open dialog box
        //set listeners for the buttons on the dialog box
        DialogBoxManager.dialogBoxBtnNext.onClick.RemoveAllListeners();
        DialogBoxManager.dialogBoxBtnNext.gameObject.SetActive(true);
        DialogBoxManager.dialogBoxBtnNext.onClick.AddListener(delegate
        {
            ClassDb.dialogBoxManager.ToggleDialogBox();
            storeManager.PurchaseItem(item);
        });

        DialogBoxManager.dialogBoxBtnBack.onClick.RemoveAllListeners();
        DialogBoxManager.dialogBoxBtnBack.gameObject.SetActive(true);
        DialogBoxManager.dialogBoxBtnBack.onClick.AddListener(delegate
        {
            ClassDb.dialogBoxManager.ToggleDialogBox();
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
        yield return new WaitWhile(() => DialogBoxManager.dialogEnabled);

        ClassDb.dialogBoxManager.ToggleDialogBox();

        DialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StringDb.failedCorruption));

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
            ClassDb.dialogBoxManager.ToggleDialogBox();
        });

        DialogBoxManager.dialogBoxBtnBack.onClick.RemoveAllListeners();
        DialogBoxManager.dialogBoxBtnBack.gameObject.SetActive(false);

    }

    public void StartMoneyEarn(float moneyEarn)
    {
        moneyEarnRoutine = MoneyEarn(moneyEarn);
        StartCoroutine(moneyEarnRoutine);
    }

    private IEnumerator MoneyEarn(float moneyEarn)
    {
        yield return new WaitForSeconds(messageDelay);
        yield return new WaitWhile(() => DialogBoxManager.dialogEnabled);

        ClassDb.dialogBoxManager.ToggleDialogBox();

        DialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StringDb.moneyEarn));

        ClassDb.dialogBoxManager.SetDialog(
            message.head,
            string.Format(message.body, Math.Round(moneyEarn, 0).ToString(CultureInfo.CurrentCulture)),
            message.backBtn,
            message.nextBtn
        );

        DialogBoxManager.dialogBoxBtnNext.onClick.RemoveAllListeners();
        DialogBoxManager.dialogBoxBtnNext.gameObject.SetActive(true);
        DialogBoxManager.dialogBoxBtnNext.onClick.AddListener(ClassDb.dialogBoxManager.ToggleDialogBox);

        DialogBoxManager.dialogBoxBtnBack.onClick.RemoveAllListeners();
        DialogBoxManager.dialogBoxBtnBack.gameObject.SetActive(false);
    }

    public void StartSuspiciousAi()
    {
        suspiciousAiRoutine = SuspiciousAi();
        StartCoroutine(suspiciousAiRoutine);
    }

    private IEnumerator SuspiciousAi()
    {
        yield return new WaitForSeconds(messageDelay);
        yield return new WaitWhile(() => DialogBoxManager.dialogEnabled);

        ClassDb.dialogBoxManager.ToggleDialogBox();

        DialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StringDb.suspiciousAi));

        ClassDb.dialogBoxManager.SetDialog(
            message.head,
            message.body,
            message.backBtn,
            message.nextBtn
        );

        DialogBoxManager.dialogBoxBtnNext.onClick.RemoveAllListeners();
        DialogBoxManager.dialogBoxBtnNext.gameObject.SetActive(true);
        DialogBoxManager.dialogBoxBtnNext.onClick.AddListener(ClassDb.dialogBoxManager.ToggleDialogBox);

        DialogBoxManager.dialogBoxBtnBack.onClick.RemoveAllListeners();
        DialogBoxManager.dialogBoxBtnBack.gameObject.SetActive(false);
    }

    public void StartPlantCheck()
    {
        plantCheckRoutine = PlantCheck();
        StartCoroutine(plantCheckRoutine);
    }

    private IEnumerator PlantCheck()
    {
        yield return new WaitForSeconds(messageDelay);
        yield return new WaitWhile(() => DialogBoxManager.dialogEnabled);

        ClassDb.dialogBoxManager.ToggleDialogBox();

        DialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StringDb.plantCheck));

        ClassDb.dialogBoxManager.SetDialog(
            message.head,
            message.body,
            message.backBtn,
            message.nextBtn
        );

        DialogBoxManager.dialogBoxBtnNext.onClick.RemoveAllListeners();
        DialogBoxManager.dialogBoxBtnNext.gameObject.SetActive(true);
        DialogBoxManager.dialogBoxBtnNext.onClick.AddListener(ClassDb.dialogBoxManager.ToggleDialogBox);

        DialogBoxManager.dialogBoxBtnBack.onClick.RemoveAllListeners();
        DialogBoxManager.dialogBoxBtnBack.gameObject.SetActive(false);
    }

    public void StartMalwareCheck()
    {
        malwareCheckRoutine = MalwareCheck();
        StartCoroutine(malwareCheckRoutine);
    }

    private IEnumerator MalwareCheck()
    {
        yield return new WaitForSeconds(messageDelay);
        yield return new WaitWhile(() => DialogBoxManager.dialogEnabled);

        ClassDb.dialogBoxManager.ToggleDialogBox();

        DialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StringDb.malwareCheck));

        ClassDb.dialogBoxManager.SetDialog(
            message.head,
            message.body,
            message.backBtn,
            message.nextBtn
        );

        DialogBoxManager.dialogBoxBtnNext.onClick.RemoveAllListeners();
        DialogBoxManager.dialogBoxBtnNext.gameObject.SetActive(true);
        DialogBoxManager.dialogBoxBtnNext.onClick.AddListener(ClassDb.dialogBoxManager.ToggleDialogBox);

        DialogBoxManager.dialogBoxBtnBack.onClick.RemoveAllListeners();
        DialogBoxManager.dialogBoxBtnBack.gameObject.SetActive(false);
    }

    public void StartThreatManagementResult(TimeSpan elapsedTime, float moneyLoss)
    {
        threatManagementRoutine = ThreatManagementResult(elapsedTime, moneyLoss);
        StartCoroutine(threatManagementRoutine);
    }

    private IEnumerator ThreatManagementResult(TimeSpan elapsedTime, float moneyLoss)
    {
        yield return new WaitForSeconds(messageDelay);
        yield return new WaitWhile(() => DialogBoxManager.dialogEnabled);

        ClassDb.dialogBoxManager.ToggleDialogBox();

        DialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StringDb.threatManagementResult));

        ClassDb.dialogBoxManager.SetDialog(
            message.head,
            string.Format(message.body, elapsedTime.TotalMinutes.ToString(CultureInfo.CurrentCulture), 
                Math.Round(moneyLoss, 0).ToString(CultureInfo.CurrentCulture)),
            message.backBtn,
            message.nextBtn
        );

        DialogBoxManager.dialogBoxBtnNext.onClick.RemoveAllListeners();
        DialogBoxManager.dialogBoxBtnNext.gameObject.SetActive(true);
        DialogBoxManager.dialogBoxBtnNext.onClick.AddListener(ClassDb.dialogBoxManager.ToggleDialogBox);

        DialogBoxManager.dialogBoxBtnBack.onClick.RemoveAllListeners();
        DialogBoxManager.dialogBoxBtnBack.gameObject.SetActive(false);
    }

    public void StartNewTrustedEmployees()
    {
        newTrustedEmployeesRoutine = NewTrustedEmployees();
        StartCoroutine(newTrustedEmployeesRoutine);
        //TODO ADD PROGRESS BAR FOR ENTIRE UPDATING COURSE

    }

    private IEnumerator NewTrustedEmployees()
    {
        yield return new WaitForSeconds(messageDelay);
        yield return new WaitWhile(() => DialogBoxManager.dialogEnabled);

        ClassDb.dialogBoxManager.ToggleDialogBox();

        DialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StringDb.newTrustedEmployees));

        ClassDb.dialogBoxManager.SetDialog(
            message.head,
            message.body,
            message.backBtn,
            message.nextBtn
        );

        DialogBoxManager.dialogBoxBtnNext.onClick.RemoveAllListeners();
        DialogBoxManager.dialogBoxBtnNext.gameObject.SetActive(true);
        DialogBoxManager.dialogBoxBtnNext.onClick.AddListener(ClassDb.dialogBoxManager.ToggleDialogBox);

        DialogBoxManager.dialogBoxBtnBack.onClick.RemoveAllListeners();
        DialogBoxManager.dialogBoxBtnBack.gameObject.SetActive(false);
    }

    public void StartNewEmployeesHired(int employeesHired)
    {
        newEmployeesHiredRoutine = NewEmployeesHired(employeesHired);
        StartCoroutine(newEmployeesHiredRoutine);
        //TODO ADD PROGRESS BAR FOR ENTIRE HIRING CAMPAIGN
    }

    private IEnumerator NewEmployeesHired(int employeesHired)
    {
        yield return new WaitForSeconds(messageDelay);
        yield return new WaitWhile(() => DialogBoxManager.dialogEnabled);
        yield return new WaitForSeconds(15);

        ClassDb.dialogBoxManager.ToggleDialogBox();

        DialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StringDb.newEmployeesHired));

        ClassDb.dialogBoxManager.SetDialog(
            message.head,
            string.Format(message.body, employeesHired),
            message.backBtn,
            message.nextBtn
        );

        DialogBoxManager.dialogBoxBtnNext.onClick.RemoveAllListeners();
        DialogBoxManager.dialogBoxBtnNext.gameObject.SetActive(true);
        DialogBoxManager.dialogBoxBtnNext.onClick.AddListener(delegate
        {
            ClassDb.dialogBoxManager.ToggleDialogBox();
            manager.GetGameData().totalEmployees += employeesHired;
        });

        DialogBoxManager.dialogBoxBtnBack.onClick.RemoveAllListeners();
        DialogBoxManager.dialogBoxBtnBack.gameObject.SetActive(false);
    }

    public void StartShowLessonFirstTime(Threat threat)
    {
        showLessonFirstTimeRoutine = ShowLessonFirstTime(threat);
        StartCoroutine(showLessonFirstTimeRoutine);
    }

    private IEnumerator ShowLessonFirstTime(Threat threat)
    {
        yield return new WaitForSeconds(messageDelay);
        yield return new WaitWhile(() => DialogBoxManager.dialogEnabled);

        ClassDb.dialogBoxManager.ToggleDialogBox();

        DialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StringDb.showLessonFirstTime));

        ClassDb.dialogBoxManager.SetDialog(
            message.head,
            string.Format(message.body, threat.threatAttack),
            message.backBtn,
            message.nextBtn
        );

        DialogBoxManager.dialogBoxBtnNext.onClick.RemoveAllListeners();
        DialogBoxManager.dialogBoxBtnNext.gameObject.SetActive(true);
        DialogBoxManager.dialogBoxBtnNext.onClick.AddListener(delegate
        {
            ClassDb.dialogBoxManager.ToggleDialogBox();

            NotebookManager.isFirstLesson = true;
            NotebookManager.firstLessonThreat = threat;

            ClassDb.notebookManager.ToggleNoteBook();

        });

        DialogBoxManager.dialogBoxBtnBack.onClick.RemoveAllListeners();
        DialogBoxManager.dialogBoxBtnBack.gameObject.SetActive(false);
    }

    public void StartShowReport(string threatAttack)
    {
        showReportRoutine = ShowReport(threatAttack);
        StartCoroutine(showReportRoutine);
    }

    private IEnumerator ShowReport(string threatAttack)
    {
        yield return new WaitForSeconds(messageDelay);
        yield return new WaitWhile(() => DialogBoxManager.dialogEnabled);

        ClassDb.dialogBoxManager.ToggleDialogBox();

        DialogBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StringDb.researchReport));

        ClassDb.dialogBoxManager.SetDialog(
            message.head,
            string.Format(message.body, threatAttack),
            message.backBtn,
            message.nextBtn
        );

        DialogBoxManager.dialogBoxBtnNext.onClick.RemoveAllListeners();
        DialogBoxManager.dialogBoxBtnNext.gameObject.SetActive(true);
        DialogBoxManager.dialogBoxBtnNext.onClick.AddListener(delegate
        {
            ClassDb.dialogBoxManager.ToggleDialogBox();
        });

        DialogBoxManager.dialogBoxBtnBack.onClick.RemoveAllListeners();
        DialogBoxManager.dialogBoxBtnBack.gameObject.SetActive(false);
    }

    public void StartEndGame()
    {
        endGameRoutine = EndGame();
        StartCoroutine(endGameRoutine);
    }

    private IEnumerator EndGame()
    {
        yield return new WaitForSeconds(messageDelay);
        yield return new WaitWhile(() => DialogBoxManager.dialogEnabled);

        ClassDb.dialogBoxManager.ToggleDialogBox();

        DialogBoxMessage message;
        message = MessageFromJson(manager.GetGameData().isGameWon ? 
            Resources.Load<TextAsset>(StringDb.endGameWin) : 
            Resources.Load<TextAsset>(StringDb.endGameLoss));


        ClassDb.dialogBoxManager.SetDialog(
            message.head,
            message.body,
            message.backBtn,
            message.nextBtn
        );

        DialogBoxManager.dialogBoxBtnNext.onClick.RemoveAllListeners();
        DialogBoxManager.dialogBoxBtnNext.gameObject.SetActive(true);
        DialogBoxManager.dialogBoxBtnNext.onClick.AddListener(delegate
        {
            ClassDb.dialogBoxManager.ToggleDialogBox();
            //LOAD LEVEL 2
            ClassDb.sceneLoader.StartLoadByIndex(StringDb.level2SceneIndex);
        });

        DialogBoxManager.dialogBoxBtnBack.onClick.RemoveAllListeners();
        DialogBoxManager.dialogBoxBtnBack.gameObject.SetActive(true);
        DialogBoxManager.dialogBoxBtnBack.onClick.AddListener(delegate
        {
            ClassDb.dialogBoxManager.ToggleDialogBox();

            ClassDb.prefabManager.GetPrefab(ClassDb.prefabManager.prefabGraph.gameObject, PrefabManager.graphIndex);

        });
    }


    //RIMUOVIBILE
    //--------------------------------------------------------------------------------------------------------
    //public void StartThreatRecap(StringDb.ThreatType type, int threatCount)
    //{
    //    localRecapRoutine = ThreatRecap(type, threatCount);
    //    StartCoroutine(localRecapRoutine);
    //}

    //private IEnumerator ThreatRecap(StringDb.ThreatType type, int threatCount)
    //{
    //    yield return new WaitForSeconds(messageDelay);
    //    yield return new WaitWhile(() => DialogBoxManager.dialogEnabled);

    //    ClassDb.dialogBoxManager.ToggleDialogBox();

    //    DialogBoxMessage message = new DialogBoxMessage();

    //    switch (type)
    //    {
    //        case StringDb.ThreatType.local:
    //            message = MessageFromJson(Resources.Load<TextAsset>(StringDb.localThreatRecap));
    //            break;
    //        case StringDb.ThreatType.remote:
    //            message = MessageFromJson(Resources.Load<TextAsset>(StringDb.remoteThreatRecap));
    //            break;
    //        case StringDb.ThreatType.fakeLocal:
    //            break;
    //        case StringDb.ThreatType.timeEvent:
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
