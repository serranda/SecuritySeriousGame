using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DataCollector : MonoBehaviour
{
    private Dictionary<ItemStore, float> upgradeData;

    private SimplestPlot SimplestPlotScript;

    private Color[] MyColors = new Color[6];

    [SerializeField] private List<float> malwareYValues;
    [SerializeField] private List<float> mitmYValues;
    [SerializeField] private List<float> dosYValues;
    [SerializeField] private List<float> replayYValues;
    [SerializeField] private List<float> stuxnetYValues;
    [SerializeField] private List<float> dragonflyYValues;

    private Button backBtn;

    private void OnEnable()
    {
        upgradeData = new Dictionary<ItemStore, float>();
        MyColors[0] = Color.blue;
        MyColors[1] = Color.cyan;
        MyColors[2] = Color.green;
        MyColors[3] = Color.magenta;
        MyColors[4] = Color.red;
        MyColors[5] = Color.yellow;

        SimplestPlotScript = GetComponent<SimplestPlot>();

        SimplestPlotScript.InitializeVariables();

        SimplestPlotScript.SetResolution(new Vector2(800, 800));
        SimplestPlotScript.BackGroundColor = new Color(0.2f, 0.2f, 0.2f);

        

        for (int x = 0; x < MyColors.Length; x++)
        {
            SimplestPlotScript.SeriesPlotY.Add(new SimplestPlot.SeriesClass());

            SimplestPlotScript.SeriesPlotY[x].MyColor = MyColors[x];
        }

        backBtn = GameObject.Find("BackBtn").GetComponent<Button>();
        backBtn.onClick.RemoveAllListeners();
        backBtn.onClick.AddListener(delegate
        {
            ClassDb.prefabManager.ReturnPrefab(gameObject.GetComponentInParent<Canvas>().gameObject, PrefabManager.graphIndex);
            ClassDb.levelMessageManager.StartEndGame();
        });


        ////DEBUG
        ////---------------------------------------------------------------------------------------------------------------------
        //List<List<float>> listYValues = new List<List<float>>
        //{
        //    malwareYValues,
        //    mitmYValues,
        //    dosYValues,
        //    replayYValues,
        //    stuxnetYValues,
        //    dragonflyYValues
        //};

        //foreach (List<float> listYValue in listYValues)
        //{
        //    int y = 0;
        //    for (int i = 0; i < Random.Range(15, 20); i++)
        //    {
        //        listYValue.Add(Random.Range(4f, 25f));
        //    }
        //}
        //SimplestPlotScript.SeriesPlotY[0].YValues = dosYValues.ToArray();
        //SimplestPlotScript.SeriesPlotY[1].YValues = replayYValues.ToArray();
        //SimplestPlotScript.SeriesPlotY[2].YValues = mitmYValues.ToArray();
        //SimplestPlotScript.SeriesPlotY[3].YValues = stuxnetYValues.ToArray();
        //SimplestPlotScript.SeriesPlotY[4].YValues = dragonflyYValues.ToArray();
        //SimplestPlotScript.SeriesPlotY[5].YValues = malwareYValues.ToArray();


        //SimplestPlotScript.UpdatePlot();

        ////---------------------------------------------------------------------------------------------------------------------


    }

    public void GetThreatData(Threat threat, float manageTime)
    {
        switch (threat.threatAttack)
        {
            case StringDb.ThreatAttack.dos:
                dosYValues.Add(manageTime);
                SimplestPlotScript.SeriesPlotY[0].YValues = dosYValues.ToArray();
                break;
            case StringDb.ThreatAttack.phishing:
                break;
            case StringDb.ThreatAttack.replay:
                replayYValues.Add(manageTime);
                SimplestPlotScript.SeriesPlotY[1].YValues = replayYValues.ToArray();
                break;
            case StringDb.ThreatAttack.mitm:
                mitmYValues.Add(manageTime);
                SimplestPlotScript.SeriesPlotY[2].YValues = mitmYValues.ToArray();
                break;
            case StringDb.ThreatAttack.stuxnet:
                stuxnetYValues.Add(manageTime);
                SimplestPlotScript.SeriesPlotY[3].YValues = stuxnetYValues.ToArray();
                break;
            case StringDb.ThreatAttack.dragonfly:
                dragonflyYValues.Add(manageTime);
                SimplestPlotScript.SeriesPlotY[4].YValues = dragonflyYValues.ToArray();
                break;
            case StringDb.ThreatAttack.malware:
                SimplestPlotScript.SeriesPlotY[5].YValues = malwareYValues.ToArray();
                malwareYValues.Add(manageTime);
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

        //evaluate data and giving point

        SimplestPlotScript.UpdatePlot();

    }

    public void GetUpgradeData(ItemStore item, float purchaseTime)
    {
        upgradeData.Add(item, purchaseTime);
    }

}
