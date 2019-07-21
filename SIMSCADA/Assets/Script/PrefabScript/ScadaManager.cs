using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScadaManager : MonoBehaviour
{
    private Button backBtn;

    private RoomPcListener roomPcListener;
    private TutorialRoomPcListener tutorialRoomPcListener;

    private void OnEnable()
    {
        roomPcListener = FindObjectOfType<RoomPcListener>();
        tutorialRoomPcListener = FindObjectOfType<TutorialRoomPcListener>();

        backBtn = GameObject.Find("BackBtn").GetComponent<Button>();
        backBtn.onClick.RemoveAllListeners();
        backBtn.onClick.AddListener(delegate
        {
            if (RoomPcListener.scadaEnabled)
            {
                roomPcListener.ToggleScadaScreen();
            }

            if (TutorialRoomPcListener.scadaEnabled)
            {
                tutorialRoomPcListener.ToggleScadaScreen();
            }
        });
    }
}
