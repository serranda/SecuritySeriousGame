using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogBoxManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogBoxTitle;
    [SerializeField] private TextMeshProUGUI dialogBoxMessage;
    public Button dialogBoxBtnNext;
    public Button dialogBoxBtnBack;
    [SerializeField] private TextMeshProUGUI dialogBoxBtnNextTxt;
    [SerializeField] private TextMeshProUGUI dialogBoxBtnBackTxt;

    private ILevelManager manager;

    public static int dialogIndex = 1;

    private void OnEnable()
    {
        SetGameDataDialogBool();
    }

    private void OnDisable()
    {
        SetButtonActive();

        SetGameDataDialogBool();
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

    private void SetSortOrder(Canvas dialogBox)
    {
        switch (dialogBoxTitle.text)
        {
            case "ESCI":
                dialogBox.sortingOrder = 4;
                break;
            case "CHIUDI TUTTE LE FINESTRE":
                dialogBox.sortingOrder = 3;
                break;
            default:
                dialogBox.sortingOrder = 2;
                break;
        }
    }

    private void SetGameDataDialogBool()
    {
        if (SetLevelManager() == null) return;
        manager = SetLevelManager();

        manager.GetGameData().dialogEnabled = FindObjectsOfType<DialogBoxManager>().Length > 1;
    }

    public void SetDialog(string title, string message, string buttonBack, string buttonNext, Canvas dialogBox)
    {
        dialogBoxTitle.SetText(title);
        dialogBoxMessage.SetText(message);
        dialogBoxBtnBackTxt.SetText(buttonBack);
        dialogBoxBtnNextTxt.SetText(buttonNext);

        SetSortOrder(dialogBox);

    }


    public Canvas OpenDialog()
    {
        Canvas dialogBox = ClassDb.prefabManager.GetPrefab(ClassDb.prefabManager.prefabDialogBox.gameObject, PrefabManager.dialogIndex).GetComponent<Canvas>();

        dialogBox.name = "DialogBox" + dialogIndex++;

        if (SetLevelManager() != null)
        {
            ClassDb.timeManager.ToggleTime();
        }

        return dialogBox;
    }

    public void CloseDialog(Canvas box)
    {
        ClassDb.prefabManager.ReturnPrefab(box.gameObject, PrefabManager.dialogIndex);

        if (SetLevelManager() == null) return;

        ClassDb.timeManager.ToggleTime();
    }

    private void SetButtonActive()
    {
        dialogBoxBtnNext.gameObject.SetActive(true);
        dialogBoxBtnBack.gameObject.SetActive(true);
    }
}