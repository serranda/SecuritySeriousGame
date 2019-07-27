using System;
using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager: MonoBehaviour
{    
    [SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] private GameSettingManager gameSettingManager;

    public void Awake()
    {
        playerName.text =string.Concat("Benvenuto ", StringDb.player.username) ;

        gameSettingManager.StartCheckWebSettingsFileRoutine();
    }


    private void Update()
    {
        gameSettingManager.GetSpriteFromBool(Screen.fullScreen);
    }


}

