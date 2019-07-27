using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeEventManager : MonoBehaviour
{
    private IEnumerator deployRoutine;

    public TimeEvent NewTimeEvent(float duration, GameObject parent, bool visible, bool stoppable)
    {
        //Debug.Log(parent.name);

        int id = ++GameData.lastTimeEventId;

        float percentagePerMin = 100 / duration;

        Canvas progressBar = ClassDb.prefabManager.GetPrefab(ClassDb.prefabManager.prefabProgressBar.gameObject, PrefabManager.pbIndex).GetComponent<Canvas>();

        progressBar.name = "ProgressBar" + id;

        Transform progressBarTransform = progressBar.transform;
        progressBarTransform.SetParent(parent.transform);

        progressBarTransform.localPosition = new Vector3(0, 1.7f, 0);

        progressBar.GetComponent<CanvasGroup>().alpha = Convert.ToSingle(visible);

        TimeEvent timeEvent = new TimeEvent(id, duration, percentagePerMin, 0, progressBar, visible, stoppable);

        return timeEvent;
    }

    public TimeEvent NewTimeEventFromThreat(Threat threat, GameObject parent, bool visible, bool stoppable)
    {
        int id = ++GameData.lastTimeEventId;

        //threat deploy time is in hour while percentage is calculated on minute scale
        float duration = threat.deployTime * 60;

        float percentagePerMin = 100 / duration;

        Canvas progressBar = ClassDb.prefabManager.GetPrefab(ClassDb.prefabManager.prefabProgressBar.gameObject, PrefabManager.pbIndex).GetComponent<Canvas>();

        progressBar.name = "ProgressBar" + id;

        if (parent!=null)
        {
            Transform progressBarTransform = progressBar.transform;
            progressBarTransform.SetParent(parent.transform);

            progressBarTransform.localPosition = new Vector3(0, 1.7f, 0);
        }

        progressBar.GetComponent<CanvasGroup>().alpha = Convert.ToSingle(visible);

        TimeEvent timeEvent = new TimeEvent(id, duration, percentagePerMin, 0, progressBar, visible, threat, stoppable);

        //Debug.Log(timeEvent);

        return timeEvent;

    }

    public List<TimeEvent> UpdateTimeEventList(List<TimeEvent> timeEvents)
    {
        foreach (TimeEvent timeEvent in timeEvents.ToArray())
        {
            timeEvent.progressBar.GetComponentInChildren<Slider>().value = timeEvent.currentPercentage += timeEvent.percentagePerMin;

            if (!(timeEvent.currentPercentage >= 100)) continue;
            if (timeEvent.threat != StringDb.timeEventThreat)
            {
                //ClassDb.worldManager.DeployThreat(timeEvent.threat);
                deployRoutine = ClassDb.level1Manager.DeployThreat(timeEvent.threat);
                StartCoroutine(deployRoutine);
            }
            timeEvents.Remove(timeEvent);
            timeEvent.progressBar.GetComponentInChildren<Slider>().value = 0f;
            ClassDb.prefabManager.ReturnPrefab(timeEvent.progressBar.gameObject, PrefabManager.pbIndex);
            
        }

        return timeEvents;
    }

    public void StopTimeEventList(List<TimeEvent> timeEvents)
    {
        foreach (TimeEvent timeEvent in timeEvents)
        {
            timeEvent.percentagePerMin = 0;
        }
    }

    public void StartTimeEventList(List<TimeEvent> timeEvents)
    {
        foreach (TimeEvent timeEvent in timeEvents)
        {
            timeEvent.percentagePerMin = 100 / timeEvent.duration;
        }
    }

    public void UpdateVisibility(TimeEvent timeEvent, bool visible)
    {
        timeEvent.progressBar.GetComponent<CanvasGroup>().alpha = Convert.ToSingle(visible);
        timeEvent.visible = visible;
    }

    public TimeEvent GetThreatTimeEvent(Threat threat)
    {
        TimeEvent timeEvent = ClassDb.level1Manager.timeEventList.Find(x => x.threat == threat);

        return timeEvent;
    }
}
