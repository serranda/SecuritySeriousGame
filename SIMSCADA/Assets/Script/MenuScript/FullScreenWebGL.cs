using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FullScreenWebGL : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private GameSettingManager gameSettingManager;

    public void OnPointerDown(PointerEventData eventData)
    {
        gameSettingManager.OnFullScreenChange();
    }
}
