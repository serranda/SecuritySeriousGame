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
    public static MessageManager messageManager;
    public static TutorialMessageManager tutorialMessageManager;
    public static ThreatManager threatManager;
    public static TimeEventManager timeEventManager;
    public static TimeManager timeManager;
    public static FollowCursor followCursor;
    public static Pathfinding regularPathfinder;
    public static Pathfinding strictedPathfinder;
    public static NotebookManager notebookManager;
    public static DialogBoxManager dialogBoxManager;
    public static TutorialDialogBoxManager tutorialDialogBoxManager;
    public static IdCardManager idCardManager;
    public static DataLoader dataLoader;
    public static DataCollector dataCollector;

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
        messageManager = GetComponent<MessageManager>();
        tutorialMessageManager = GetComponent<TutorialMessageManager>();
        threatManager = GetComponent<ThreatManager>();
        timeEventManager = GetComponent<TimeEventManager>();
        timeManager = GetComponent<TimeManager>();
        followCursor = GetComponent<FollowCursor>();
        if (SceneManager.GetActiveScene().buildIndex != StringDb.menuSceneIndex)
        {
            regularPathfinder = GameObject.Find("PathFinderRegular").GetComponent<Pathfinding>();
            strictedPathfinder = GameObject.Find("PathFinderRestricted").GetComponent<Pathfinding>();
        }

        notebookManager = GetComponent<NotebookManager>();
        dialogBoxManager = GetComponent<DialogBoxManager>();
        tutorialDialogBoxManager = GetComponent<TutorialDialogBoxManager>();
        idCardManager = GetComponent<IdCardManager>();
        dataLoader = GetComponent<DataLoader>();
        dataCollector = GetComponent<DataCollector>();
    }
}
