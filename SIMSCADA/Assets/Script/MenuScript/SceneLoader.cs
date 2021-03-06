﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    private Canvas loadingScreen;
    private Slider progressBar;

    private IEnumerator loadRoutine;

    public void StartLoadByIndex(int sceneIndex)
    {
        loadingScreen = ClassDb.prefabManager.GetPrefab(ClassDb.prefabManager.prefabLoadingScreen.gameObject,
            PrefabManager.loadingScreenIndex).GetComponent<Canvas>();
        progressBar = loadingScreen.GetComponentInChildren<Slider>();


        loadRoutine = LoadByIndex(sceneIndex);
        StartCoroutine(loadRoutine);
    }
    public void StartNewGame()
    {
        StaticDb.isNewGame = true;

        StartLoadByIndex(StaticDb.level1SceneIndex);
    }

    public void StartLoadGame()
    {
        StaticDb.isNewGame = false;

        int levelIndex = ClassDb.menuManager.levelIndex;

        //assign scene index relative to level index
        int sceneIndex;

        switch (levelIndex)
        {
            case 1:
                sceneIndex = StaticDb.level1SceneIndex;
                break;
            case 2:
                sceneIndex = StaticDb.level2SceneIndex;
                break;
            default:
                sceneIndex = StaticDb.menuSceneIndex;
                break;
        }


        StartLoadByIndex(sceneIndex);
    }

    private IEnumerator LoadByIndex(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            //Debug.Log(progress);

            progressBar.value = progress;

            yield return null;

        }

        ClassDb.prefabManager.ReturnPrefab(loadingScreen.gameObject, PrefabManager.loadingScreenIndex);

    }

    public void Quit()
    {
        //UPDATE PLAYER LOG WITH LOGIN DATA
        ClassDb.logManager.StartWritePlayerLogRoutine(StaticDb.player, StaticDb.logEvent.SystemEvent, string.Concat(StaticDb.player.username, " has disconnected."));

        StartLoadByIndex(StaticDb.loginSceneIndex);
    }
}
