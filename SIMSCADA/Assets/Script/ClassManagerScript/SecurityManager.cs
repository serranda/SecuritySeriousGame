using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SecurityManager : MonoBehaviour
{
    private Toggle strictToggle;
    private Toggle neutralToggle;
    private Toggle looseToggle;
    
    public Toggle activeToggle;

    private Button backBtn;

    private SecurityListener securityListener;
    private TutorialSecurityListener tutorialSecurityListener;

    private ILevelManager manager;

    private void OnEnable()
    {
        manager = SetLevelManager();

        securityListener = FindObjectOfType<SecurityListener>();
        tutorialSecurityListener = FindObjectOfType<TutorialSecurityListener>();

        strictToggle = GameObject.Find(StaticDb.strictToggle).GetComponent<Toggle>();
        neutralToggle = GameObject.Find(StaticDb.mediumToggle).GetComponent<Toggle>();
        looseToggle = GameObject.Find(StaticDb.looseToggle).GetComponent<Toggle>();

        backBtn = GameObject.Find("BackBtn").GetComponent<Button>();
        backBtn.onClick.RemoveAllListeners();
        backBtn.onClick.AddListener(delegate
        {
            if (manager.GetGameData().securityScreenEnabled)
            {
                securityListener.ToggleSecurityScreen();
            }

            //if (TutorialSecurityListener.securityScreenEnabled)
            //{
            //    tutorialSecurityListener.ToggleSecurityScreen();
            //}
        });

        SetTogglePressed();
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

    private void SetTogglePressed()
    {
        switch (manager.GetGameData().serverSecurity)
        {
            case StaticDb.ServerSecurity.strict:
                activeToggle = strictToggle;
                break;
            case StaticDb.ServerSecurity.medium:
                activeToggle = neutralToggle;
                break;
            case StaticDb.ServerSecurity.loose:
                activeToggle = looseToggle;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        activeToggle.isOn = true;
    }

    public void SetActiveToggle(Toggle toggle)
    {
        activeToggle = toggle;
        SetSecurityLevel();
    }

    private void SetSecurityLevel()
    {
        switch (activeToggle.name)
        {
            case StaticDb.strictToggle:
                manager.GetGameData().serverSecurity = StaticDb.ServerSecurity.strict;
                break;
            case StaticDb.mediumToggle:
                manager.GetGameData().serverSecurity = StaticDb.ServerSecurity.medium;
                break;
            case StaticDb.looseToggle:
                manager.GetGameData().serverSecurity = StaticDb.ServerSecurity.loose;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
       
    }

    public void ChangeAiPathfinder()
    {
        List<AiController> aiOnScene = FindObjectsOfType<AiController>().ToList();

        foreach (AiController controller in aiOnScene)
        {
            controller.pathfinderChanged = true;
            controller.StartChangePathfinder();
        }
    }
}
