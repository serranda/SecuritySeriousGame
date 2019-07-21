using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ServerRoomManager : MonoBehaviour
{
    private Toggle strictToggle;
    private Toggle neutralToggle;
    private Toggle looseToggle;
    
    public Toggle activeToggle;

    private Button backBtn;

    private SecurityListener securityListener;
    private TutorialSecurityListener tutorialSecurityListener;

    private void OnEnable()
    {
        securityListener = FindObjectOfType<SecurityListener>();
        tutorialSecurityListener = FindObjectOfType<TutorialSecurityListener>();

        strictToggle = GameObject.Find(StringDb.strictToggle).GetComponent<Toggle>();
        neutralToggle = GameObject.Find(StringDb.mediumToggle).GetComponent<Toggle>();
        looseToggle = GameObject.Find(StringDb.looseToggle).GetComponent<Toggle>();

        backBtn = GameObject.Find("BackBtn").GetComponent<Button>();
        backBtn.onClick.RemoveAllListeners();
        backBtn.onClick.AddListener(delegate
        {
            if (SecurityListener.serverRoomScreenEnabled)
            {
                securityListener.ToggleServerRoomScreen();
            }

            //if (TutorialSecurityListener.serverRoomScreenEnabled)
            //{
            //    tutorialSecurityListener.ToggleServerRoomScreen();
            //}
        });

        SetTogglePressed();
    }

    private void SetTogglePressed()
    {
        switch (GameData.serverSecurity)
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
                GameData.serverSecurity = StringDb.ServerSecurity.strict;
                break;
            case StringDb.mediumToggle:
                GameData.serverSecurity = StringDb.ServerSecurity.medium;
                break;
            case StringDb.looseToggle:
                GameData.serverSecurity = StringDb.ServerSecurity.loose;
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
