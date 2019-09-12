using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialTelephoneListener : MonoBehaviour
{
    private TutorialInteractiveSprite interactiveSprite;

    private IEnumerator moneyRoutine;
    private IEnumerator checkPlantRoutine;

    private static bool telephone1;
    private static bool telephone2;

    private IEnumerator telephone1Routine;
    private IEnumerator telephone2Routine;

    [SerializeField] private TutorialManager tutorialManager;


    private void Start()
    {
        interactiveSprite = GetComponent<TutorialInteractiveSprite>();
    }

    public void SetTelephoneListeners()
    {
        if (tutorialManager.telephoneFirstTime)
        {
            //show info message for security check
            telephone1Routine = Telephone1Routine();
            StartCoroutine(telephone1Routine);
        }
        List<Button> buttons;

        buttons = interactiveSprite.actionButtonManager.GetActiveCanvasGroup(3);

        buttons[0].GetComponentInChildren<TextMeshProUGUI>().text = "Richiedi fondi";
        buttons[0].onClick.RemoveAllListeners();

        buttons[1].GetComponentInChildren<TextMeshProUGUI>().text = "Vai alle lezioni";
        buttons[1].onClick.RemoveAllListeners();
        buttons[1].onClick.AddListener(delegate
        {
            ClassDb.notebookManager.ToggleNoteBook();
            interactiveSprite.ToggleMenu();
        });

        buttons[2].GetComponentInChildren<TextMeshProUGUI>().text = "Check dell'impianto";
        buttons[2].onClick.RemoveAllListeners();

        foreach (Button button in buttons)
        {
            button.interactable = true;
        }

        //disable interact with button until tutorial is finished
        if (tutorialManager.tutorialIsFinished) return;

        foreach (Button button in buttons)
        {
            //keep active button for scada screen and store screen
            if (buttons.IndexOf(button) == 1) continue;
            button.interactable = false;
        }
    }

    private IEnumerator Telephone1Routine()
    {
        //set flag to stop next coroutine
        telephone1 = false;

        //start next coroutine
        telephone2Routine = Telephone2Routine();
        StartCoroutine(telephone2Routine);

        //display message
        ClassDb.tutorialMessageManager.Telephone1Message();

        //wait until dialog has been disabled
        yield return new WaitWhile(() => tutorialManager.tutorialGameData.dialogEnabled);

        //set flag to start next coroutine
        telephone1 = true;


    }

    private IEnumerator Telephone2Routine()
    {
        //wait unitl previous corutine has finished
        yield return new WaitUntil(() => telephone1);

        //set flag to stop next coroutine
        telephone2 = false;

        //display message
        ClassDb.tutorialMessageManager.Telephone2Message();

        //wait until dialog has been disabled
        yield return new WaitWhile(() => tutorialManager.tutorialGameData.dialogEnabled);

        //set flag to start next coroutine
        telephone2 = true;

        tutorialManager.telephoneFirstTime = false;

    }
}
