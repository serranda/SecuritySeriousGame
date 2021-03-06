﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeManager : MonoBehaviour
{
    private ILevelManager manager;

    private void Awake()
    {
        manager = SetLevelManager();

        Time.timeScale = 1.0f;
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

    public void ToggleTime()
    {
        if (SetLevelManager() == null) return;
        if (!PauseManager.pauseEnabled && !manager.GetGameData().dialogEnabled && !manager.GetGameData().noteBookEnabled && !manager.GetGameData().chartEnabled)
        {
            Time.timeScale = 1.0f;
        }
        else
        {
            Time.timeScale = 0.0f;
        }
    }
}
