using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogBoxManager : MonoBehaviour
{
    private static TextMeshProUGUI dialogBoxTitle;
    private static TextMeshProUGUI dialogBoxMessage;
    public static Button dialogBoxBtnNext;
    public static Button dialogBoxBtnBack;
    private static TextMeshProUGUI dialogBoxBtnNextTxt;
    private static TextMeshProUGUI dialogBoxBtnBackTxt;

    private ILevelManager manager;

    public static int dialogIndex = 1;

    public static bool dialogEnabled;

    private Canvas dialogBox;

    private void Awake()
    {
        dialogEnabled = false;
    }

    private void OnEnable()
    {
        dialogBoxTitle = GameObject.Find(StringDb.dialogBoxTitle).GetComponent<TextMeshProUGUI>();
        dialogBoxMessage = GameObject.Find(StringDb.dialogBoxMessage).GetComponent<TextMeshProUGUI>();
        dialogBoxBtnNext = GameObject.Find(StringDb.dialogBoxBtnNext).GetComponent<Button>();
        dialogBoxBtnBack = GameObject.Find(StringDb.dialogBoxBtnBack).GetComponent<Button>();
        dialogBoxBtnNextTxt = GameObject.Find(StringDb.dialogBoxBtnNext).GetComponentInChildren<TextMeshProUGUI>();
        dialogBoxBtnBackTxt = GameObject.Find(StringDb.dialogBoxBtnBack).GetComponentInChildren<TextMeshProUGUI>();

        dialogEnabled = true;

        SetGameDataDialogBool(dialogEnabled);
    }

    private void OnDisable()
    {
        SetButtonActive();
        dialogEnabled = false;

        SetGameDataDialogBool(dialogEnabled);
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

    private void SetGameDataDialogBool(bool dialogBool)
    {
        if (SetLevelManager() == null) return;
        manager = SetLevelManager();

        manager.GetGameData().dialogEnabled = dialogBool;
    }

    public void SetDialog(string title, string message, string buttonBack, string buttonNext, string dialogString)
    {
        dialogBoxTitle.SetText(title);
        dialogBoxMessage.SetText(message);
        dialogBoxBtnBackTxt.SetText(buttonBack);
        dialogBoxBtnNextTxt.SetText(buttonNext);

        SetGameDataLastDialogString(dialogString);
    }

    private void SetGameDataLastDialogString(string dialogString)
    {
        if (SetLevelManager() == null) return;
        if (dialogString == StringDb.exitTxt) return;
        manager = SetLevelManager();

        manager.GetGameData().lastDialogShowed = dialogString;
    }

    public void ToggleDialogBox()
    {
        if (dialogEnabled)
        {
            ClassDb.prefabManager.ReturnPrefab(dialogBox.gameObject, PrefabManager.dialogIndex);
        }
        else
        {
            dialogBox = ClassDb.prefabManager.GetPrefab(ClassDb.prefabManager.prefabDialogBox.gameObject, PrefabManager.dialogIndex).GetComponent<Canvas>();

            dialogBox.name = "DialogBox" + dialogIndex++;
        }
        ClassDb.timeManager.ToggleTime();
    }

    private void SetButtonActive()
    {
        dialogBoxBtnNext.gameObject.SetActive(true);
        dialogBoxBtnBack.gameObject.SetActive(true);
    }
}