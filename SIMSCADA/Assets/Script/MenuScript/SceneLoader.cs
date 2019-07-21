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
        GameData.lastSceneIndex = SceneManager.GetActiveScene().buildIndex;

        loadingScreen = ClassDb.prefabManager.GetPrefab(ClassDb.prefabManager.prefabLoadingScreen.gameObject,
            PrefabManager.loadingScreenIndex).GetComponent<Canvas>();
        progressBar = loadingScreen.GetComponentInChildren<Slider>();


        loadRoutine = LoadByIndex(sceneIndex);
        StartCoroutine(loadRoutine);
    }

    //public void LoadLastSceneIndex()
    //{
    //    SceneManager.LoadSceneAsync(StringDb.lastSceneIndex);
    //}
    //public void SetLastSceneIndex()
    //{
    //    StringDb.lastSceneIndex = SceneManager.GetActiveScene().buildIndex;
    //}

    private IEnumerator LoadByIndex(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);


        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            Debug.Log(progress);

            progressBar.value = progress;

            yield return null;

        }

        ClassDb.prefabManager.ReturnPrefab(loadingScreen.gameObject, PrefabManager.loadingScreenIndex);

    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
