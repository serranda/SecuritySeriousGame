using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogBoxManager : MonoBehaviour
{
    private static TextMeshProUGUI dialogBoxTitle;
    private static TextMeshProUGUI dialogBoxMessage;
    public static Button dialogBoxBtnNext;
    public static Button dialogBoxBtnBack;
    private static TextMeshProUGUI dialogBoxBtnNextTxt;
    private static TextMeshProUGUI dialogBoxBtnBackTxt;

    public static int i = 1;

    //TODO move this flag on gamedata and try to set a string with the name of the dialog file txt
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
    }

    private void OnDisable()
    {
        SetButtonActive();
        dialogEnabled = false;
    }

    public void SetDialog(string title, string message, string buttonBack, string buttonNext)
    {
        dialogBoxTitle.SetText(title);
        dialogBoxMessage.SetText(message);
        dialogBoxBtnBackTxt.SetText(buttonBack);
        dialogBoxBtnNextTxt.SetText(buttonNext);
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

            dialogBox.name = "DialogBox" + i++;
        }
        ClassDb.timeManager.ToggleTime();
    }

    private void SetButtonActive()
    {
        dialogBoxBtnNext.gameObject.SetActive(true);
        dialogBoxBtnBack.gameObject.SetActive(true);
    }
}