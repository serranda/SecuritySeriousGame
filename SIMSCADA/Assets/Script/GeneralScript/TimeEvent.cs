using System.Collections;
using UnityEngine;

[System.Serializable]
public class TimeEvent
{
    //TODO: ADD A VALUE IN ORDER TO STORE THE SUCCESSIVE METHOD WHICH WILL BE CALLED AT THE END OF TIME EVENT
    public int id;
    public float duration; //expressed in minute
    public float percentagePerMin;
    public float currentPercentage;
    public Canvas progressBar;
    public bool visible;
    public Threat threat;
    public bool stoppable;
    public string progressBarParentName;
    public string routine;

    //TIME EVENT CONSTRUCTOR FOR THREAT EVENT
    public TimeEvent(int id, float duration, float percentagePerMin, float currentPercentage, Canvas progressBar,
        bool visible, Threat threat, bool stoppable)
    {
        this.id = id;
        this.duration = duration;
        this.percentagePerMin = percentagePerMin;
        this.currentPercentage = currentPercentage;
        this.progressBar = progressBar;
        this.visible = visible;
        this.threat = threat;
        this.stoppable = stoppable;
        this.progressBarParentName = progressBar.transform.parent.name;

    }

    //TIME EVENT CONSTRUCTOR FOR GENERAL EVENT
    public TimeEvent(int id, float duration, float percentagePerMin, float currentPercentage, Canvas progressBar, bool visible, bool stoppable, string routine)
    {
        this.id = id;
        this.duration = duration;
        this.percentagePerMin = percentagePerMin;
        this.currentPercentage = currentPercentage;
        this.progressBar = progressBar;
        this.visible = visible;
        this.threat = StringDb.timeEventThreat;
        this.stoppable = stoppable;
        this.progressBarParentName = progressBar.transform.parent.name;
        this.routine = routine;
    }

    public override string ToString()
    {
        return string.Format("Id: {0}, Duration: {1}, PercentagePerMin: {2}, CurrentPercentage: {3}, ProgressBar: {4}, Visible: {5}, Threat: {6}, Stoppable: {7}, ProgressBarParentName: {8}, Routine, {9}", 
            id, duration, percentagePerMin, currentPercentage, progressBar, visible, threat, stoppable, progressBarParentName, routine);
    }
}
