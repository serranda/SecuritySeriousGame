using UnityEngine;
using UnityEngine.SceneManagement;

public class ClassDb : MonoBehaviour
{
    public static PrefabManager prefabManager;
    public static LevelManager levelManager;
    public static TutorialManager tutorialManager;
    public static SettingsManager settingsManager;
    public static SceneLoader sceneLoader;
    public static CharactersCommon charactersCommon;
    public static SpawnCharacter spawnCharacter;
    public static PauseManager pauseManager;
    public static LevelMessageManager levelMessageManager;
    public static TutorialMessageManager tutorialMessageManager;
    public static LoginMessageManager loginMessageManager;
    public static ThreatManager threatManager;
    public static TimeEventManager timeEventManager;
    public static TimeManager timeManager;
    public static FollowCursor followCursor;
    public static NotebookManager notebookManager;
    public static DialogBoxManager dialogBoxManager;
    public static TutorialDialogBoxManager tutorialDialogBoxManager;
    public static IdCardManager idCardManager;
    public static DataLoader dataLoader;
    public static DataCollector dataCollector;
    public static LogManager logManager;

    public static Pathfinding regularPathfinder;
    public static Pathfinding strictedPathfinder;

    private void Awake()
    {
        prefabManager = GetComponent<PrefabManager>();
        levelManager = GetComponent<LevelManager>();
        tutorialManager = GetComponent<TutorialManager>();
        settingsManager = GetComponent<SettingsManager>();
        sceneLoader = GetComponent<SceneLoader>();
        charactersCommon = GetComponent<CharactersCommon>();
        spawnCharacter = GetComponent<SpawnCharacter>();
        pauseManager = GetComponent<PauseManager>();
        levelMessageManager = GetComponent<LevelMessageManager>();
        tutorialMessageManager = GetComponent<TutorialMessageManager>();
        loginMessageManager = GetComponent<LoginMessageManager>();
        threatManager = GetComponent<ThreatManager>();
        timeEventManager = GetComponent<TimeEventManager>();
        timeManager = GetComponent<TimeManager>();
        followCursor = GetComponent<FollowCursor>();
        notebookManager = GetComponent<NotebookManager>();
        dialogBoxManager = GetComponent<DialogBoxManager>();
        tutorialDialogBoxManager = GetComponent<TutorialDialogBoxManager>();
        idCardManager = GetComponent<IdCardManager>();
        dataLoader = GetComponent<DataLoader>();
        dataCollector = GetComponent<DataCollector>();
        logManager = GetComponent<LogManager>();

        if (SceneManager.GetActiveScene().buildIndex == StringDb.menuSceneIndex ||
            SceneManager.GetActiveScene().buildIndex == StringDb.loginSceneIndex) return;
        regularPathfinder = GameObject.Find("PathFinderRegular").GetComponent<Pathfinding>();
        strictedPathfinder = GameObject.Find("PathFinderRestricted").GetComponent<Pathfinding>();
    }
}
