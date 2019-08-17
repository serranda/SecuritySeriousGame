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

public class ThreatChartController : MonoBehaviour
{
    [SerializeField] private LineChart lineChart;

    [SerializeField] private Button backBtn;

    [SerializeField] private LineDataSet dosSet;
    [SerializeField] private LineDataSet replaySet;
    [SerializeField] private LineDataSet mitmSet;
    [SerializeField] private LineDataSet stuxnetSet;
    [SerializeField] private LineDataSet dragonflySet;
    [SerializeField] private LineDataSet malwareSet;

    private List<LineDataSet> sets = new List<LineDataSet>();

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


        SetupChartData();

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

        //lineChart.SetDirty();
        //ConfigChart();

        ////---------------------------------------------------------------------------------------------------------------------
    }

    private void OnEnable()
    {
        manager = SetLevelManager();

        manager.GetGameData().chartEnabled = true;

        ConfigChart();
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
    private void SetupChartData()
    {
        dosSet = new LineDataSet { Title = "DOS", LineColor = colors[0], LineThickness = setLineThickness };
        replaySet = new LineDataSet { Title = "REPLAY", LineColor = colors[1], LineThickness = setLineThickness };
        mitmSet = new LineDataSet { Title = "MAN IN THE MIDDLE", LineColor = colors[2], LineThickness = setLineThickness };
        stuxnetSet = new LineDataSet { Title = "STUXNET", LineColor = colors[3], LineThickness = setLineThickness };
        dragonflySet = new LineDataSet { Title = "DRAGONFLY", LineColor = colors[4], LineThickness = setLineThickness };
        malwareSet = new LineDataSet { Title = "MALWARE", LineColor = colors[5], LineThickness = setLineThickness };

        lineChart.GetChartData().DataSets.Add(dosSet);
        lineChart.GetChartData().DataSets.Add(replaySet);
        lineChart.GetChartData().DataSets.Add(mitmSet);
        lineChart.GetChartData().DataSets.Add(stuxnetSet);
        lineChart.GetChartData().DataSets.Add(dragonflySet);
        lineChart.GetChartData().DataSets.Add(malwareSet);
    }

    private ILevelManager SetLevelManager()
    {
        ILevelManager iManager;
        if (SceneManager.GetActiveScene().buildIndex == StringDb.level1SceneIndex)
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
        switch (threat.threatAttack)
        {
            case StringDb.ThreatAttack.dos:
                dosSet.AddEntry(new LineEntry(threat.id, (float)Math.Round(manageTime)));
                break;
            case StringDb.ThreatAttack.phishing:
                break;
            case StringDb.ThreatAttack.replay:
                replaySet.AddEntry(new LineEntry(threat.id, (float)Math.Round(manageTime)));
                break;
            case StringDb.ThreatAttack.mitm:
                mitmSet.AddEntry(new LineEntry(threat.id, (float)Math.Round(manageTime)));
                break;
            case StringDb.ThreatAttack.stuxnet:
                stuxnetSet.AddEntry(new LineEntry(threat.id, (float)Math.Round(manageTime)));
                break;
            case StringDb.ThreatAttack.dragonfly:
                dragonflySet.AddEntry(new LineEntry(threat.id, (float)Math.Round(manageTime)));
                break;
            case StringDb.ThreatAttack.malware:
                malwareSet.AddEntry(new LineEntry(threat.id, (float)Math.Round(manageTime)));
                break;
            case StringDb.ThreatAttack.createRemote:
                break;
            case StringDb.ThreatAttack.fakeLocal:
                break;
            case StringDb.ThreatAttack.timeEvent:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        lineChart.SetDirty();
        ConfigChart();

    }
}
