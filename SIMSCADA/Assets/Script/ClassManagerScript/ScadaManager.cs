using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScadaManager : MonoBehaviour
{
    private Button backBtn;

    private RoomPcListener roomPcListener;
    private TutorialRoomPcListener tutorialRoomPcListener;

    private ILevelManager manager;

    private TutorialManager tutorialManager;

    private void OnEnable()
    {
        manager = SetLevelManager();
        tutorialManager = FindObjectOfType<TutorialManager>();

        roomPcListener = FindObjectOfType<RoomPcListener>();
        tutorialRoomPcListener = FindObjectOfType<TutorialRoomPcListener>();

        backBtn = GameObject.Find("BackBtn").GetComponent<Button>();
        backBtn.onClick.RemoveAllListeners();
        backBtn.onClick.AddListener(delegate
        {
            if (manager != null)
            {
                if (manager.GetGameData().scadaEnabled)
                {
                    roomPcListener.ToggleScadaScreen();
                }
            }
            else
            {
                if (tutorialManager.tutorialGameData.scadaEnabled)
                {
                    tutorialRoomPcListener.ToggleScadaScreen();
                }
            }
        });
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

}
