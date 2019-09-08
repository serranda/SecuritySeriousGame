using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private Button resumeBtn;
    [SerializeField] private Button plotBtn;
    [SerializeField] private Button exitBtn;

    public static bool pauseEnabled;

    private bool listenerSet;

    public GraphicRaycaster pauseRaycaster;

    private IEnumerator screenShotRoutine;

    private ILevelManager manager;

    [SerializeField] private CanvasGroup canvasGroup;

    private ILevelManager SetLevelManager()
    {
        ILevelManager iManager;
        if (SceneManager.GetActiveScene().buildIndex == StaticDb.level1SceneIndex)
            iManager = FindObjectOfType<Level1Manager>();
        else
            iManager = FindObjectOfType<Level2Manager>();

        return iManager;
    }


    public void TogglePauseMenu()
    {
        if (pauseEnabled)
        {
            //TURN PAUSE MENU OFF
            CanvasOff();

            //SET FLAG TO RESUME TIME
            pauseEnabled = false;
            
        }
        else
        {
            //TURN PAUSE MENU ON
            CanvasOn();

            //CHECK IF LISTENER HAS BEEN ALREADY SET;
            //NEEDED BECAUSE THESE BUTTONS HAVE ALREADY LISTENER TO APPLY AND UPLOAD SETTINGS FILE,
            //SO IT'S NOT POSSIBLE TO DO THE RemoveAllListeners() CALL BECAUSE IT WILL REMOVE ALSO THE ONES SET IN THE GmeSettingManager SCRIPT  
            if (!listenerSet)
            {
                SetListener();
            }

            //SET FLAG TO STOP TIME
            pauseEnabled = true;
        }

        //ACCORDING TO THE pauseEnabled FLAG VALUE RESTART OR STOP THE TIME
        ClassDb.timeManager.ToggleTime();
    }

    public void SetListener()
    {
        manager = SetLevelManager();

        resumeBtn.onClick.AddListener(delegate
        {
            TogglePauseMenu();
        });

        plotBtn.onClick.AddListener(delegate
        {
            TogglePauseMenu();
            ClassDb.threatChartManager.ToggleThreatPlot();
        });

        //CHECK IF IS NORMAL LEVEL OR TUTORIAL LEVEL AND SET THE LISTENER FOR RIGHT EXIT MESSAGE
        if (SceneManager.GetActiveScene().buildIndex == StaticDb.tutorialSceneIndex)
        {
            exitBtn.onClick.AddListener(delegate
            {
                ClassDb.tutorialMessageManager.Exit();
            });
        }
        else
        {
            exitBtn.onClick.AddListener(delegate
            {
                ClassDb.levelMessageManager.StartExit(manager.GetGameData().levelIndex);
            });
        }

        //Debug.Log("SET LISTENER");

        //SET THE FLAG TO TRUE IN ORDER TO CALL THIS METHOD ONLY ONCE
        listenerSet = true;
    }

    private void CanvasOn()
    {
        //TURN ON THE CANVAS BY SETTING THE VALUES OF CANVAS GROUP
        canvasGroup.alpha = 1.0f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    private void CanvasOff()
    {
        //TURN OFF THE CANVAS BY SETTING THE VALUES OF CANVAS GROUP
        canvasGroup.alpha = 0.0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

}
