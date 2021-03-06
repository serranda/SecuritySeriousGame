﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class ProgressBarManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Button button;
    private Canvas progressBar;

    private CanvasGroup canvasGroup;

    private ILevelManager manager;


    private void Start()
    {
        manager = SetLevelManager();

        progressBar = GetComponent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log(progressBar.name + " ENTERED");

        TimeEvent first = null;
        foreach (TimeEvent x in manager.GetGameData().timeEventList)
        {
            if (x.progressBar != progressBar) continue;
            first = x;
            break;
        }

        if (first == null || !first.stoppable) return;
        button = ClassDb.prefabManager.GetPrefab(ClassDb.prefabManager.prefabProgressBarButton.gameObject, PrefabManager.pbBtnIndex).GetComponent<Button>();

        Transform buttonTransform = button.transform;
        buttonTransform.parent = progressBar.transform;
        buttonTransform.localScale = Vector3.one;
        buttonTransform.localPosition = Vector3.zero;

        button.name = "Btn" + progressBar.name;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(delegate
        {
            StopTimeEvent();
            ClassDb.prefabManager.ReturnPrefab(button.gameObject, PrefabManager.pbBtnIndex);
            progressBar.GetComponentInChildren<Slider>().value = 0;
            ClassDb.prefabManager.ReturnPrefab(progressBar.gameObject, PrefabManager.pbIndex);

        });
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log(progressBar.name + " EXITED");

        TimeEvent first = null;
        foreach (TimeEvent x in manager.GetGameData().timeEventList)
        {
            if (x.progressBar != progressBar) continue;
            first = x;
            break;
        }

        if (first == null || !first.stoppable) return;
        ClassDb.prefabManager.ReturnPrefab(button.gameObject, PrefabManager.pbBtnIndex);
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


    private void StopTimeEvent()
    {
        //check if progress bar is child of interactive sprite or ai
        //stop all coroutine and then remove event from list
        if (GetComponentInParent<InteractiveSprite>())
        {
            switch (gameObject.transform.parent.tag)
            {
                case StaticDb.telephoneTag:
                    GetComponentInParent<TelephoneListener>().StopAllCoroutines();
                    break;
                case StaticDb.securityCheckTag:
                    GetComponentInParent<SecurityListener>().StopAllCoroutines();
                    break;
                case StaticDb.roomPcTag:
                    GetComponentInParent<RoomPcListener>().StopAllCoroutines();
                    break;
                case StaticDb.serverPcTag:
                    GetComponentInParent<ServerPcListener>().StopAllCoroutines();
                    break;
                default:
                    break;
            }

            GetComponentInParent<InteractiveSprite>().SetInteraction(true);
        }

        if (GetComponentInParent<AiController>())
        {
            GetComponentInParent<AiController>().SetInteraction(true);
            GetComponentInParent<AiListener>().StopAllCoroutines();
        }

        manager.GetGameData().timeEventList.Remove(manager.GetGameData().timeEventList.FirstOrDefault(x => x.progressBar == progressBar));
    }
}
