using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private void Awake()
    {
        Time.timeScale = 1.0f;
    }

    public void ToggleTime()
    {
        if (!PauseManager.pauseEnabled && !DialogBoxManager.dialogEnabled && !NotebookManager.noteBookEnabled)
        {
            Time.timeScale = 1.0f;
        }
        else
        {
            Time.timeScale = 0.0f;
        }
    }
}
