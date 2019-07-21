using UnityEngine;

[System.Serializable]
public class TimeEvent
{
    public int id;
    public float duration; //expressed in minute
    public float percentagePerMin;
    public float currentPercentage;
    public Canvas progressBar;
    public bool visible;
    public Threat threat;
    public bool stoppable;

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
    }

    public TimeEvent(int id, float duration, float percentagePerMin, float currentPercentage, Canvas progressBar, bool visible, bool stoppable)
    {
        this.id = id;
        this.duration = duration;
        this.percentagePerMin = percentagePerMin;
        this.currentPercentage = currentPercentage;
        this.progressBar = progressBar;
        this.visible = visible;
        this.threat = StringDb.timeEventThreat;
        this.stoppable = stoppable;

    }

    public override string ToString()
    {
        return string.Format("Id: {0}, Duration: {1}, PercentagePerMin: {2}, CurrentPercentage: {3}, Threat: {4}", 
            id, duration, percentagePerMin, currentPercentage, threat);
    }
}
