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

        strictToggle = GameObject.Find(StringDb.strictToggle).GetComponent<Toggle>();
        neutralToggle = GameObject.Find(StringDb.mediumToggle).GetComponent<Toggle>();
        looseToggle = GameObject.Find(StringDb.looseToggle).GetComponent<Toggle>();

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
        if (SceneManager.GetActiveScene().buildIndex == StringDb.level1SceneIndex)
            iManager = FindObjectOfType<Level1Manager>();
        else
            iManager = FindObjectOfType<Level2Manager>();

        return iManager;
    }

    private void SetTogglePressed()
    {
        switch (manager.GetGameData().serverSecurity)
        {
            case StringDb.ServerSecurity.strict:
                activeToggle = strictToggle;
                break;
            case StringDb.ServerSecurity.medium:
                activeToggle = neutralToggle;
                break;
            case StringDb.ServerSecurity.loose:
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
            case StringDb.strictToggle:
                manager.GetGameData().serverSecurity = StringDb.ServerSecurity.strict;
                break;
            case StringDb.mediumToggle:
                manager.GetGameData().serverSecurity = StringDb.ServerSecurity.medium;
                break;
            case StringDb.looseToggle:
                manager.GetGameData().serverSecurity = StringDb.ServerSecurity.loose;
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
