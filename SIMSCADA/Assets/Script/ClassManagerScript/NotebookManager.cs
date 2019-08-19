using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;
using UnityEngine.SceneManagement;

public class NotebookManager : MonoBehaviour
{
    private Button nextBtn;
    private CanvasGroup nbGroup;
    private Button previousBtn;
    private CanvasGroup pbGroup;
    private Button backBtn;

    private static Canvas noteBook;

    private ScrollRect btnList;
    private GameObject spawnedBtn;

    private TextMeshProUGUI pageL;
    private TextMeshProUGUI pageR;

    private TextMeshProUGUI titleL;
    private TextMeshProUGUI titleR;

    private int pageToDisplay;
    private int pageCount;
    private int colorIndex;

    private List<TextAsset> lessonTextAssetList;
    private List<Lesson> lessonObjList;

    private ToggleGroup toggleGroup;

    public static bool isFirstLesson;
    public static Threat firstLessonThreat;

    private IEnumerator notebookMessageRoutine;

    private ILevelManager manager;

    [SerializeField] private TutorialManager tutorialManager;

    private void Update()
    {
        pageCount = pageR.textInfo.pageCount;

        if (pageCount > 1)
        {
            pbGroup.interactable = true;
            pbGroup.alpha = 1;
            nbGroup.interactable = true;
            nbGroup.alpha = 1;

            if (pageL.pageToDisplay == 1)
            {
                //show only right button
                pbGroup.interactable = false;
                pbGroup.alpha = 0;
            }

            if (pageL.pageToDisplay != pageCount && pageR.pageToDisplay != pageCount) return;
            nbGroup.interactable = false;
            nbGroup.alpha = 0;
        }
        else
        {
            pbGroup.interactable = false;
            pbGroup.alpha = 0;
            nbGroup.interactable = false;
            nbGroup.alpha = 0;
        }
    }

    private void OnEnable()
    {

        manager = SetLevelManager();
        tutorialManager = FindObjectOfType<TutorialManager>();

        toggleGroup = GameObject.Find(StaticDb.noteBookToggleGroup).GetComponent<ToggleGroup>();

        if (tutorialManager != null && tutorialManager.notebookFirstTime)
        {
            //show info message for security check
            notebookMessageRoutine = NotebookMessageRoutine();
            StartCoroutine(notebookMessageRoutine);
        }


        if (manager != null)
            manager.GetGameData().noteBookEnabled = true;
        else
            tutorialManager.tutorialGameData.noteBookEnabled = true;

        colorIndex = 0;
        pageCount = 0;

        nextBtn = GameObject.Find(StaticDb.noteBookBtnNext).GetComponent<Button>();
        nbGroup = nextBtn.GetComponent<CanvasGroup>();

        previousBtn = GameObject.Find(StaticDb.noteBookBtnPrevious).GetComponent<Button>();
        pbGroup = previousBtn.GetComponent<CanvasGroup>();

        backBtn = GameObject.Find(StaticDb.noteBookBtnBack).GetComponent<Button>();

        nextBtn.onClick.RemoveAllListeners();
        nextBtn.onClick.AddListener(delegate
        {
            pageToDisplay++;
            ShowText();
        });

        previousBtn.onClick.RemoveAllListeners();
        previousBtn.onClick.AddListener(delegate
        {
            pageToDisplay -= 3;
            ShowText();
        });

        backBtn.onClick.RemoveAllListeners();
        backBtn.onClick.AddListener(delegate
        {
            if (manager != null)
            {
                ToggleNoteBook();
            }
            else
            {
                ToggleNoteBook(true);
            }
        });

        pageL = GameObject.Find(StaticDb.noteBookPageL).GetComponent<TextMeshProUGUI>();
        pageR = GameObject.Find(StaticDb.noteBookPageR).GetComponent<TextMeshProUGUI>();

        titleL = GameObject.Find(StaticDb.noteBookTitleL).GetComponent<TextMeshProUGUI>();
        titleR = GameObject.Find(StaticDb.noteBookTitleR).GetComponent<TextMeshProUGUI>();

        LoadLesson();
        toggleGroup.SetAllTogglesOff();
        ShowInitialPage();

        if (!isFirstLesson) return;
        ShowThreatLesson(firstLessonThreat);

    }

    private void OnDisable()
    {
        if (manager != null)
            manager.GetGameData().noteBookEnabled = false;
        else
            tutorialManager.tutorialGameData.noteBookEnabled = false;
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

    public void ToggleNoteBook()
    {
        manager = SetLevelManager();
        tutorialManager = FindObjectOfType<TutorialManager>();

        //TODO CHECK FOR TUTORIAL
        if (manager != null)
        {
            if (manager.GetGameData().noteBookEnabled)
            {
                ClassDb.prefabManager.ReturnPrefab(noteBook.gameObject, PrefabManager.noteBookIndex);
                foreach (Toggle lessonButton in btnList.content.GetComponentsInChildren<Toggle>())
                {
                    ClassDb.prefabManager.ReturnPrefab(lessonButton.gameObject, PrefabManager.notebookButtonIndex);
                }
            }
            else
            {
                noteBook = ClassDb.prefabManager.GetPrefab(ClassDb.prefabManager.prefabNoteBook.gameObject, PrefabManager.noteBookIndex).GetComponent<Canvas>();
            }

            if (ClassDb.timeManager)
                ClassDb.timeManager.ToggleTime();
        }
        else
        {
            if (tutorialManager.tutorialGameData.noteBookEnabled)
            {
                ClassDb.prefabManager.ReturnPrefab(noteBook.gameObject, PrefabManager.noteBookIndex);
                foreach (Toggle lessonButton in btnList.content.GetComponentsInChildren<Toggle>())
                {
                    ClassDb.prefabManager.ReturnPrefab(lessonButton.gameObject, PrefabManager.notebookButtonIndex);
                }
            }
            else
            {
                noteBook = ClassDb.prefabManager.GetPrefab(ClassDb.prefabManager.prefabNoteBook.gameObject, PrefabManager.noteBookIndex).GetComponent<Canvas>();
            }
        }
    }

    public void ToggleNoteBook(bool toggle)
    {
        if (toggle)
        {
            ClassDb.prefabManager.ReturnPrefab(noteBook.gameObject, PrefabManager.noteBookIndex);
            foreach (Toggle lessonButton in btnList.content.GetComponentsInChildren<Toggle>())
            {
                ClassDb.prefabManager.ReturnPrefab(lessonButton.gameObject, PrefabManager.notebookButtonIndex);
            }
        }
        else
        {
            noteBook = ClassDb.prefabManager.GetPrefab(ClassDb.prefabManager.prefabNoteBook.gameObject, PrefabManager.noteBookIndex).GetComponent<Canvas>();
        }
    }

    private void SetPageTextBody(Lesson lesson)
    {

        titleL.text = lesson.id.ToUpperInvariant();
        titleR.text = lesson.id.ToUpperInvariant();

        pageL.text = lesson.textBody;
        pageR.text = lesson.textBody;

        if (lesson.textBody == string.Empty)
        {
            pageL.text = "<EMPTY FIELD>";
            pageR.text = "";
        }
        ShowText();
    }

    private void ShowText()
    {
        pageL.pageToDisplay = pageToDisplay;
        pageToDisplay++;
        pageR.pageToDisplay = pageToDisplay;
    }

    private void ShowInitialPage()
    {
        pageToDisplay = 1;

        titleL.text = string.Empty;
        titleR.text = string.Empty;

        pageL.text = Resources.Load<TextAsset>("NotebookInitialPage/NotebookInitialPage").text;
        pageR.text = Resources.Load<TextAsset>("NotebookInitialPage/NotebookInitialPage").text;

        ShowText();
    }

    private Color SetColor(int i)
    {
        Color color;

        switch (i)
        {
            case 0:
                color = Color.blue;
                break;
            case 1:
                color = Color.cyan;
                break;
            case 2:
                color = Color.green;
                break;
            case 3:
                color = Color.magenta;
                break;
            case 4:
                color = Color.red;
                break;
            case 5:
                color = Color.gray;
                break;
            case 6:
                color = Color.yellow;
                break;
            default:
                color = Color.white;
                break;
        }

        return color;
    }

    private void LoadLesson()
    {
        btnList = GameObject.Find(StaticDb.noteBookBtnList).GetComponent<ScrollRect>();

        lessonTextAssetList = new List<TextAsset>();
        lessonObjList = new List<Lesson>();

        lessonTextAssetList = Resources.LoadAll<TextAsset>("Lessons").ToList();

        foreach (TextAsset lessonTextAsset in lessonTextAssetList)
        {
            Lesson lessonObject = JsonUtility.FromJson<Lesson>(lessonTextAsset.ToString());
            lessonObjList.Add(lessonObject);
        }

        foreach (Lesson lessonObject in lessonObjList)
        {
            spawnedBtn = ClassDb.prefabManager.GetPrefab(ClassDb.prefabManager.prefabNotebookButton.gameObject,
                PrefabManager.notebookButtonIndex);
            spawnedBtn.transform.SetParent(btnList.content);
            spawnedBtn.transform.localScale = Vector3.one;
            spawnedBtn.name = lessonObject.id + "Lesson";
            spawnedBtn.GetComponent<Toggle>().image.color = SetColor(colorIndex);
            spawnedBtn.GetComponent<Toggle>().graphic.color = SetColor(colorIndex);

            spawnedBtn.GetComponentInChildren<TextMeshProUGUI>().text = lessonObject.id;

            spawnedBtn.GetComponent<Toggle>().group = toggleGroup;
            spawnedBtn.GetComponent<Toggle>().onValueChanged.RemoveAllListeners();
            Lesson lesson = lessonObject;
            spawnedBtn.GetComponent<Toggle>().onValueChanged.AddListener(delegate
            {
                pageToDisplay = 1;
                SetPageTextBody(lesson);
            });

            colorIndex++;
            if (colorIndex == 7)
            {
                colorIndex = 0;
            }
        }


    }

    private void ShowThreatLesson(Threat threat)
    {

        Lesson find = new Lesson();

        switch (threat.threatAttack)
        {
            case StaticDb.ThreatAttack.dos:
                find = lessonObjList.Find(lesson => lesson.id.ToLowerInvariant() == StaticDb.ThreatAttack.dos.ToString());
                break;
            case StaticDb.ThreatAttack.phishing:
                find = lessonObjList.Find(lesson => lesson.id.ToLowerInvariant() == StaticDb.ThreatAttack.phishing.ToString());
                break;
            case StaticDb.ThreatAttack.replay:
                find = lessonObjList.Find(lesson => lesson.id.ToLowerInvariant() == StaticDb.ThreatAttack.replay.ToString());
                break;
            case StaticDb.ThreatAttack.mitm:
                find = lessonObjList.Find(lesson => lesson.id.ToLowerInvariant() == StaticDb.ThreatAttack.mitm.ToString());
                break;
            case StaticDb.ThreatAttack.stuxnet:
                find = lessonObjList.Find(lesson => lesson.id.ToLowerInvariant() == StaticDb.ThreatAttack.stuxnet.ToString());
                break;
            case StaticDb.ThreatAttack.dragonfly:
                find = lessonObjList.Find(lesson => lesson.id.ToLowerInvariant() == StaticDb.ThreatAttack.dragonfly.ToString());
                break;
            case StaticDb.ThreatAttack.malware:
                find = lessonObjList.Find(lesson => lesson.id.ToLowerInvariant() == StaticDb.ThreatAttack.malware.ToString());
                break;
            case StaticDb.ThreatAttack.createRemote:
                break;
            case StaticDb.ThreatAttack.fakeLocal:
                break;
            case StaticDb.ThreatAttack.timeEvent:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        GameObject.Find(find.id + "Lesson").GetComponent<Toggle>().isOn = true;

        isFirstLesson = false;

    }

    private IEnumerator NotebookMessageRoutine()
    {
        //display message
        ClassDb.tutorialMessageManager.NoteBookMessage();

        //wait until dialog has been disabled
        yield return new WaitWhile(() => TutorialDialogBoxManager.dialogEnabled);

        tutorialManager.notebookFirstTime = false;

    }
}
