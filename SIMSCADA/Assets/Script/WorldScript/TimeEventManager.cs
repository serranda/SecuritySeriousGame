using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimeEventManager : MonoBehaviour
{
    private IEnumerator deployRoutine;

    private ILevelManager manager;

    private void Start()
    {
        manager = SetLevelManager();
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

    public TimeEvent NewTimeEvent(float duration, GameObject parent, bool visible, bool stoppable, string routine)
    {
        //Debug.Log(parent.name);

        int id = ++manager.GetGameData().lastTimeEventId;

        float percentagePerMin = 100 / duration;

        Canvas progressBar = ClassDb.prefabManager.GetPrefab(ClassDb.prefabManager.prefabProgressBar.gameObject, PrefabManager.pbIndex).GetComponent<Canvas>();

        progressBar.name = "ProgressBar" + id;

        Transform progressBarTransform = progressBar.transform;
        progressBarTransform.SetParent(parent.transform);

        progressBarTransform.localPosition = new Vector3(0, 1.7f, 0);

        progressBar.GetComponent<CanvasGroup>().alpha = Convert.ToSingle(visible);

        TimeEvent timeEvent = new TimeEvent(id, duration, percentagePerMin, 0, progressBar, visible, stoppable, routine);

        return timeEvent;
    }

    public TimeEvent NewTimeEventFromThreat(Threat threat, GameObject parent, bool visible, bool stoppable)
    {
        int id = ++manager.GetGameData().lastTimeEventId;

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
        //NEED TO ITERATE ON LIST.TOARRAY IN ORDER TO EDIT VALUES INSIDE LIST
        foreach (TimeEvent timeEvent in timeEvents.ToArray())
        {
            timeEvent.progressBar.GetComponentInChildren<Slider>().value = timeEvent.currentPercentage += timeEvent.percentagePerMin;

            if (!(timeEvent.currentPercentage >= 100)) continue;
            if (timeEvent.threat != StaticDb.timeEventThreat)
            {
                //ClassDb.worldManager.DeployThreat(timeEvent.threat);
                deployRoutine = manager.DeployThreat(timeEvent.threat);
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
        TimeEvent timeEvent = manager.GetGameData().timeEventList.Find(x => x.threat == threat);

        return timeEvent;
    }

    public void RestoreTimeEvent(TimeEvent timeEvent)
    {
        //WITH THE INFORMATION STORED IN TIME EVENT, RESTORE ALL THE GUI REFERENCE

        //spawn a progressbar prefab
        Canvas progressBar = ClassDb.prefabManager.GetPrefab(ClassDb.prefabManager.prefabProgressBar.gameObject, PrefabManager.pbIndex).GetComponent<Canvas>();

        //rename progressbar
        progressBar.name = "ProgressBar" + timeEvent.id;

        //attach the progressbar to parent object (retrieved by finding in the game)
        GameObject parent = GameObject.Find(timeEvent.progressBarParentName);

        Transform progressBarTransform = progressBar.transform;

        progressBarTransform.SetParent(parent.transform);

        progressBarTransform.localPosition = new Vector3(0, 1.7f, 0);

        //set progressbar value according to the currentPercentage value
        progressBar.GetComponentInChildren<Slider>().value = timeEvent.currentPercentage;

        //set progressbar visibility
        progressBar.GetComponent<CanvasGroup>().alpha = Convert.ToSingle(timeEvent.visible);

        //set the progressbar reference to the time event
        timeEvent.progressBar = progressBar;

        //set threat reference
        timeEvent.threat = StaticDb.timeEventThreat;

            //check which routine need to be started after time event complete
        switch (timeEvent.routine)
        {
            case StaticDb.showAiIdRoutine:
                parent.GetComponent<AiListener>().RestartShowAiId(timeEvent);
                break;
            case StaticDb.pointOutLocalThreatRoutine:
                parent.GetComponent<RoomPcListener>().RestartPointOutLocalThreat(timeEvent);
                break;
            case StaticDb.rebootServerRoutine:
                parent.GetComponent<ServerPcListener>().RestartRebootServer(timeEvent);
                break;
            case StaticDb.idsCleanRoutine:
                parent.GetComponent<ServerPcListener>().RestartIdsClean(timeEvent);
                break;
            case StaticDb.antiMalwareScanRoutine:
                if (parent.name.Contains(StaticDb.roomPcTag))
                {
                    parent.GetComponent<RoomPcListener>().RestartAntiMalwareScan(timeEvent);
                }
                else
                {
                    parent.GetComponent<ServerPcListener>().RestartAntiMalwareScan(timeEvent, parent.GetComponent<InteractiveSprite>());
                }
                break;
            case StaticDb.checkNetworkCfgRoutine:
                if (parent.name.Contains(StaticDb.roomPcTag))
                {
                    parent.GetComponent<RoomPcListener>().RestartCheckNetworkCfg(timeEvent);
                }
                else
                {
                    parent.GetComponent<ServerPcListener>().RestartCheckNetworkCfg(timeEvent, parent.GetComponent<InteractiveSprite>());
                }
                break;
            case StaticDb.getMoneyRoutine:
                parent.GetComponent<TelephoneListener>().RestartGetMoney(timeEvent);
                break;
            case StaticDb.checkPlantRoutine:
                parent.GetComponent<TelephoneListener>().RestartCheckPlant(timeEvent);
                break;
            case StaticDb.coolDownRoutine:
                parent.GetComponent<TelephoneListener>().RestartCoolDown(timeEvent);
                break;
            case "":
                Debug.Log("EMPTY");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

    }

    public void RestoreThreatTimeEvent(TimeEvent timeEvent)
    {
        //deserialize serializableAiController and create aiController instance
        timeEvent.threat.aiController = ClassDb.spawnCharacter.RespawnAi(timeEvent.threat.serializableAiController);

        //set time event to aicontroller
        timeEvent.threat.aiController.timeEvent = timeEvent;

        //spawn a progressbar prefab
        Canvas progressBar = ClassDb.prefabManager.GetPrefab(ClassDb.prefabManager.prefabProgressBar.gameObject, PrefabManager.pbIndex).GetComponent<Canvas>();

        //rename progressbar
        progressBar.name = "ProgressBar" + timeEvent.id;

        //attach the progressbar to parent object (retrieved by finding in the game)
        GameObject parent = GameObject.Find(timeEvent.progressBarParentName);

        if (parent != null)
        {
            Transform progressBarTransform = progressBar.transform;
            progressBarTransform.SetParent(parent.transform);

            progressBarTransform.localPosition = new Vector3(0, 1.7f, 0);
        }

        //set progressbar value according to the currentPercentage value
        progressBar.GetComponentInChildren<Slider>().value = timeEvent.currentPercentage;

        //set progressbar visibility
        progressBar.GetComponent<CanvasGroup>().alpha = Convert.ToSingle(timeEvent.visible);

        //set the progressbar reference to the time event
        timeEvent.progressBar = progressBar;

        Debug.Log(timeEvent.threat.aiController.spriteToAnimate);

    }

}
