using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FullScreenWebGL : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private GUISettingManager guiSettingManager;

    public void OnPointerDown(PointerEventData eventData)
    {
        guiSettingManager.OnFullScreenChange();
    }
}
