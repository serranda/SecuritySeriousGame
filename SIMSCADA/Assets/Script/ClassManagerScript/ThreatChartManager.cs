using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AwesomeCharts;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ThreatChartManager : MonoBehaviour
{
    [SerializeField] private LineChart lineChart;

    [SerializeField] private Button backBtn;

    private static LineDataSet dosSet;
    private static LineDataSet replaySet;
    private static LineDataSet mitmSet;
    private static LineDataSet stuxnetSet;
    private static LineDataSet dragonflySet;
    private static LineDataSet malwareSet;

    private static List<LineDataSet> sets = new List<LineDataSet>();

    private List<Color> colors = new List<Color>()
    {
        Color.blue,
        Color.cyan,
        Color.green,
        Color.yellow,
        Color.red,
        Color.magenta
    };

    private float setLineThickness = 5.0f;

    private static Canvas chart;

    private ILevelManager manager;

    private void Start()
    {
        backBtn.onClick.RemoveAllListeners();
        backBtn.onClick.AddListener(delegate
        {
            ToggleThreatPlot();
        });

        ////DEBUG
        ////ADDING RANDOM VALUES TO LINECAHRT TO CHECK IF WORKING PROPERLY
        ////---------------------------------------------------------------------------------------------------------------------
        //sets = lineChart.GetChartData().DataSets;
        //foreach (LineDataSet set in sets)
        //{
        //    for (int i = 0; i < Random.Range(15, 21); i++)
        //    {
        //        set.AddEntry(new LineEntry(i, (float)Math.Round(Random.Range(5f,35f), 1)));
        //    }
        //}
        ////---------------------------------------------------------------------------------------------------------------------
    }

    private void OnEnable()
    {
        manager = SetLevelManager();

        manager.GetGameData().chartEnabled = true;

        if (sets.Count == 0) SetupChartDataSets();

        if(lineChart.GetChartData().DataSets.Count == 0) AddSets();

        ConfigChart();

        lineChart.SetDirty();
    }

    private void OnDisable()
    {
        manager = SetLevelManager();

        manager.GetGameData().chartEnabled = false;
    }

    //CONFIG LINE CHART UI SETTINGS 
    private void ConfigChart()
    {

        //SET DIMENSION FOR VALUE INDICATOR
        lineChart.Config.ValueIndicatorSize = 20;

        //SET X AND Y MAX AXIS VALUE DEPENDING ON SET DIMENSION
        lineChart.XAxis.MaxAxisValue = GetXMaxAxisValue() + 1;

        lineChart.YAxis.MaxAxisValue = GetYMaxAxisValue() + 5;

        //SET LEGEND VIEW   
        LegendView legendView = lineChart.legendView;

        for (int i = 0; i < legendView.Entries.Count; i++)
        {
            legendView.Entries[i].Title = lineChart.GetChartData().DataSets[i].Title;
        }
    }

    //RETURN MAXPOSITION BETWEEN EVERY LINE DATA SET
    private float GetXMaxAxisValue()
    {
        sets = lineChart.GetChartData().DataSets;

        float maxValue = 0;

        foreach (LineDataSet set in sets)
        {
            if (maxValue < set.GetMaxPosition())
                maxValue = set.GetMaxPosition();
        }

        return maxValue;
    }

    //RETURN MAXVALUE BETWEEN EVERY LINE DATA SET
    private float GetYMaxAxisValue()
    {
       sets = lineChart.GetChartData().DataSets;

        float maxValue = 0;

        foreach (LineDataSet set in sets)
        {
            if (maxValue < set.GetMaxValue())
                maxValue = set.GetMaxValue();
        }

        return maxValue;
    }

    //CREATE NEW LINE DATA SET FOR EACH THREAT TYPE, SET COLOR AND LINE THICKNESS AND ADD TO LINE CHART DATA SETS
    private void SetupChartDataSets()
    {
        dosSet = new LineDataSet { Title = "DOS", LineColor = colors[0], LineThickness = setLineThickness };
        replaySet = new LineDataSet { Title = "REPLAY", LineColor = colors[1], LineThickness = setLineThickness };
        mitmSet = new LineDataSet { Title = "MAN IN THE MIDDLE", LineColor = colors[2], LineThickness = setLineThickness };
        stuxnetSet = new LineDataSet { Title = "STUXNET", LineColor = colors[3], LineThickness = setLineThickness };
        dragonflySet = new LineDataSet { Title = "DRAGONFLY", LineColor = colors[4], LineThickness = setLineThickness };
        malwareSet = new LineDataSet { Title = "MALWARE", LineColor = colors[5], LineThickness = setLineThickness };

        sets.Add(dosSet);
        sets.Add(replaySet);
        sets.Add(mitmSet);
        sets.Add(stuxnetSet);
        sets.Add(dragonflySet);
        sets.Add(malwareSet);
    }

    private void AddSets()
    {
        lineChart.GetChartData().DataSets = sets;
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

    public void ToggleThreatPlot()
    {
        manager = SetLevelManager();

        if (manager.GetGameData().chartEnabled)
        {
            ClassDb.prefabManager.ReturnPrefab(chart.gameObject, PrefabManager.chartIndex);
            ClassDb.pauseManager.TogglePauseMenu();
        }
        else
        {
            chart = ClassDb.prefabManager.GetPrefab(ClassDb.prefabManager.prefabChart.gameObject, PrefabManager.chartIndex).GetComponent<Canvas>();
        }

        //ACCORDING TO THE chartEnabled FLAG VALUE RESTART OR STOP THE TIME
        ClassDb.timeManager.ToggleTime();
    }


    //UPDATE THE APPROPRIATE SET ACCORDING WITH THREAT TYPE AND UPDATING LINE CHART
    public void GetThreatData(Threat threat, float manageTime)
    {
        Debug.Log(lineChart);

        if (sets.Count == 0 ) SetupChartDataSets();

        switch (threat.threatAttack)
        {
            case StaticDb.ThreatAttack.dos:
                dosSet.AddEntry(new LineEntry(threat.id, (float)Math.Round(manageTime, 1)));
                break;
            case StaticDb.ThreatAttack.phishing:
                break;
            case StaticDb.ThreatAttack.replay:
                replaySet.AddEntry(new LineEntry(threat.id, (float)Math.Round(manageTime, 1)));
                break;
            case StaticDb.ThreatAttack.mitm:
                mitmSet.AddEntry(new LineEntry(threat.id, (float)Math.Round(manageTime, 1)));
                break;
            case StaticDb.ThreatAttack.stuxnet:
                stuxnetSet.AddEntry(new LineEntry(threat.id, (float)Math.Round(manageTime, 1)));
                break;
            case StaticDb.ThreatAttack.dragonfly:
                dragonflySet.AddEntry(new LineEntry(threat.id, (float)Math.Round(manageTime, 1)));
                break;
            case StaticDb.ThreatAttack.malware:
                malwareSet.AddEntry(new LineEntry(threat.id, (float)Math.Round(manageTime, 1)));
                break;
            case StaticDb.ThreatAttack.createRemote:
                break;
            case StaticDb.ThreatAttack.fakeLocal:
                break;
            case StaticDb.ThreatAttack.timeEvent:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        ClassDb.logManager.StartWritePlayerLogRoutine(StaticDb.player, StaticDb.logEvent.UserEvent, "THREAT " + threat.threatAttack.ToString().ToUpper() + " MANAGED AFTER " + manageTime + " SECONDS");
    }
}
