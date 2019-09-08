using UnityEngine;
using UnityEngine.SceneManagement;

public class ClassDb : MonoBehaviour
{
    public static PrefabManager prefabManager;
    public static MenuManager menuManager;
    public static SceneLoader sceneLoader;
    public static CharactersCommon charactersCommon;
    public static SpawnCharacter spawnCharacter;
    public static PauseManager pauseManager;
    public static LevelMessageManager levelMessageManager;
    public static TutorialMessageManager tutorialMessageManager;
    public static MenuMessageManager menuMessageManager;
    public static ThreatManager threatManager;
    public static TimeEventManager timeEventManager;
    public static TimeManager timeManager;
    public static CameraManager cameraManager;
    public static NotebookManager notebookManager;
    public static DialogBoxManager dialogBoxManager;
    public static TutorialDialogBoxManager tutorialDialogBoxManager;
    public static IdCardManager idCardManager;
    public static GameDataManager gameDataManager;
    public static ThreatChartManager threatChartManager;
    public static LogManager logManager;
    public static UserActionManager userActionManager;

    public static Pathfinding regularPathfinder;
    public static Pathfinding strictedPathfinder;

    private void Awake()
    {
        prefabManager = GetComponent<PrefabManager>();
        menuManager = GetComponent<MenuManager>();
        sceneLoader = GetComponent<SceneLoader>();
        charactersCommon = GetComponent<CharactersCommon>();
        spawnCharacter = GetComponent<SpawnCharacter>();
        pauseManager = GetComponent<PauseManager>();
        levelMessageManager = GetComponent<LevelMessageManager>();
        tutorialMessageManager = GetComponent<TutorialMessageManager>();
        menuMessageManager = GetComponent<MenuMessageManager>();
        threatManager = GetComponent<ThreatManager>();
        timeEventManager = GetComponent<TimeEventManager>();
        timeManager = GetComponent<TimeManager>();
        cameraManager = GetComponent<CameraManager>();
        notebookManager = GetComponent<NotebookManager>();
        dialogBoxManager = GetComponent<DialogBoxManager>();
        tutorialDialogBoxManager = GetComponent<TutorialDialogBoxManager>();
        idCardManager = GetComponent<IdCardManager>();
        gameDataManager = GetComponent<GameDataManager>();
        threatChartManager = GetComponent<ThreatChartManager>();
        logManager = GetComponent<LogManager>();
        userActionManager = GetComponent<UserActionManager>();

        if (SceneManager.GetActiveScene().buildIndex == StaticDb.menuSceneIndex ||
            SceneManager.GetActiveScene().buildIndex == StaticDb.loginSceneIndex) return;
        regularPathfinder = GameObject.Find("PathFinderRegular").GetComponent<Pathfinding>();
        strictedPathfinder = GameObject.Find("PathFinderRestricted").GetComponent<Pathfinding>();
    }
}
