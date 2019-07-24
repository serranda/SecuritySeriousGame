using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    private Button resumeBtn;
    private Button exitBtn;

    public static bool pauseEnabled;

    public static Canvas pauseMenu;

    private void Awake()
    {
        pauseEnabled = false;
    }
    public void TogglePauseMenu()
    {
        if (pauseEnabled)
        {
            ClassDb.prefabManager.ReturnPrefab(pauseMenu.gameObject, PrefabManager.pauseIndex);
#if UNITY_WEBGL

            StartCoroutine(ClassDb.settingsManager.SaveSettingsWebFile(SettingsManager.gameSettings));


#else
            ClassDb.settingsManager.SaveSettingsLocalFile(SettingsManager.gameSettings);

#endif

        }
        else
        {
            pauseMenu = ClassDb.prefabManager.GetPrefab(ClassDb.prefabManager.prefabPauseMenu.gameObject, PrefabManager.pauseIndex).GetComponent<Canvas>();
            GetButtons();
            SetListener();
        }

        ClassDb.timeManager.ToggleTime();
    }
    public void GetButtons()
    {
        resumeBtn = GameObject.Find(StringDb.pauseBtnResume).GetComponent<Button>();
        exitBtn = GameObject.Find(StringDb.pauseBtnExit).GetComponent<Button>();
    }
    public void SetListener()
    {
        resumeBtn.onClick.RemoveAllListeners();
        resumeBtn.onClick.AddListener(TogglePauseMenu);

        //lessonBtn.onClick.RemoveAllListeners();
        //lessonBtn.onClick.AddListener(ClassDb.notebookManager.ToggleNoteBook);

        if (SceneManager.GetActiveScene().buildIndex == StringDb.tutorialSceneIndex)
        {
            exitBtn.onClick.RemoveAllListeners();
            exitBtn.onClick.AddListener(ClassDb.tutorialMessageManager.StartExit);
        }
        else
        {
            exitBtn.onClick.RemoveAllListeners();
            exitBtn.onClick.AddListener(ClassDb.levelMessageManager.StartExit);
        }


    }
}
