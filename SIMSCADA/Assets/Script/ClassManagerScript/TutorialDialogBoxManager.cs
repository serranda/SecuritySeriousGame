using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialDialogBoxManager : MonoBehaviour
{
    public static TextMeshProUGUI bodyTutorialDialog;
    public static Button btnTutorialDialog;

    public static RectTransform dialogTransform;

    public static int i = 1;

    public static bool dialogEnabled;

    private Canvas tutorialDialog;

    private void Awake()
    {
        dialogEnabled = false;
    }

    private void OnEnable()
    {
        bodyTutorialDialog = GameObject.Find(StaticDb.tutorialDialogBoxMessage).GetComponent<TextMeshProUGUI>();
        btnTutorialDialog = GameObject.Find(StaticDb.tutorialDialogBoxBtn).GetComponent<Button>();

        dialogTransform = GameObject.Find(StaticDb.tutorialPanel).GetComponent<RectTransform>();

        dialogEnabled = true;
    }

    private void OnDisable()
    {
        dialogEnabled = false;
    }

    public void SetTutorialDialog(string message)
    {
        bodyTutorialDialog.SetText(message);
    }

    public void ToggleTutorialDialogBox()
    {
        if (dialogEnabled)
        {
            ClassDb.prefabManager.ReturnPrefab(tutorialDialog.gameObject, PrefabManager.tutorialDialogIndex);
        }
        else
        {
            tutorialDialog = ClassDb.prefabManager.GetPrefab(ClassDb.prefabManager.prefabTutorialDialogBox.gameObject, PrefabManager.tutorialDialogIndex).GetComponent<Canvas>();

            tutorialDialog.name = "TutorialDialogBox" + i++;
        }
        ClassDb.timeManager.ToggleTime();
    }

    public void MoveUpDialog()
    {
        dialogTransform.anchorMin = new Vector2(0.5f, 1f);
        dialogTransform.anchorMax = new Vector2(0.5f, 1f);
        dialogTransform.pivot = new Vector2(0.5f, 1f);
    }

    public void MoveDownDialog()
    {
        dialogTransform.anchorMin = new Vector2(0.5f, 0f);
        dialogTransform.anchorMax = new Vector2(0.5f, 0f);
        dialogTransform.pivot = new Vector2(0.5f, 0f);
    }
}