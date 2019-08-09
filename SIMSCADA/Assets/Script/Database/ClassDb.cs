using UnityEngine;
using UnityEngine.SceneManagement;

public class ClassDb : MonoBehaviour
{
    public static PrefabManager prefabManager;
    //public static Level1Manager level1Manager;
    //public static TutorialManager tutorialManager;
    public static MenuManager menuManager;
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
    public static CameraManager cameraManager;
    public static NotebookManager notebookManager;
    public static DialogBoxManager dialogBoxManager;
    public static TutorialDialogBoxManager tutorialDialogBoxManager;
    public static IdCardManager idCardManager;
    public static GameDataManager gameDataManager;
    public static DataCollector dataCollector;
    public static LogManager logManager;

    public static Pathfinding regularPathfinder;
    public static Pathfinding strictedPathfinder;

    private void Awake()
    {
        prefabManager = GetComponent<PrefabManager>();
        //level1Manager = GetComponent<Level1Manager>();
        //tutorialManager = GetComponent<TutorialManager>();
        menuManager = GetComponent<MenuManager>();
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
        cameraManager = GetComponent<CameraManager>();
        notebookManager = GetComponent<NotebookManager>();
        dialogBoxManager = GetComponent<DialogBoxManager>();
        tutorialDialogBoxManager = GetComponent<TutorialDialogBoxManager>();
        idCardManager = GetComponent<IdCardManager>();
        gameDataManager = GetComponent<GameDataManager>();
        dataCollector = GetComponent<DataCollector>();
        logManager = GetComponent<LogManager>();

        if (SceneManager.GetActiveScene().buildIndex == StringDb.menuSceneIndex ||
            SceneManager.GetActiveScene().buildIndex == StringDb.loginSceneIndex) return;
        regularPathfinder = GameObject.Find("PathFinderRegular").GetComponent<Pathfinding>();
        strictedPathfinder = GameObject.Find("PathFinderRestricted").GetComponent<Pathfinding>();
    }
}
