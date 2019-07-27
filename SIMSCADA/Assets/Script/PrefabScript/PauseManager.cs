using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private Button resumeBtn;
    [SerializeField] private Button exitBtn;

    public static bool pauseEnabled;

    private bool listenerSet;

    public GraphicRaycaster pauseRaycaster;

    [SerializeField] private CanvasGroup canvasGroup;

    public void TogglePauseMenu()
    {
        if (pauseEnabled)
        {
            CanvasOff();

            pauseEnabled = false;
        }
        else
        {
            if (!listenerSet)
            {
                SetListener();
            }

            CanvasOn();

            pauseEnabled = true;
        }

        ClassDb.timeManager.ToggleTime();
    }

    public void SetListener()
    {
        //resumeBtn.onClick.RemoveAllListeners();
        resumeBtn.onClick.AddListener(TogglePauseMenu);

        if (SceneManager.GetActiveScene().buildIndex == StringDb.tutorialSceneIndex)
        {
            //exitBtn.onClick.RemoveAllListeners();
            exitBtn.onClick.AddListener(ClassDb.tutorialMessageManager.StartExit);
        }
        else
        {
            //exitBtn.onClick.RemoveAllListeners();
            exitBtn.onClick.AddListener(ClassDb.levelMessageManager.StartExit);
        }

        Debug.Log("SET LISTENER");

        listenerSet = true;
    }

    private void CanvasOn()
    {
        canvasGroup.alpha = 1.0f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    private void CanvasOff()
    {
        canvasGroup.alpha = 0.0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

}
