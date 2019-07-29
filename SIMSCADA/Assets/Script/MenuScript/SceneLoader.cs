using System;
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
        //TODO FIX
        //GameData.lastSceneIndex = SceneManager.GetActiveScene().buildIndex;

        loadingScreen = ClassDb.prefabManager.GetPrefab(ClassDb.prefabManager.prefabLoadingScreen.gameObject,
            PrefabManager.loadingScreenIndex).GetComponent<Canvas>();
        progressBar = loadingScreen.GetComponentInChildren<Slider>();


        loadRoutine = LoadByIndex(sceneIndex);
        StartCoroutine(loadRoutine);
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
        ClassDb.logManager.StartWritePlayerLogRoutine(StringDb.player, StringDb.logEvent.SystemEvent, string.Concat(StringDb.player.username, " has disconnected."));

        StartLoadByIndex(StringDb.loginSceneIndex);
    }
}
