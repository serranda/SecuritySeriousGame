﻿using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using WeightedRandomization;
using Random = UnityEngine.Random;

public class ThreatManager : MonoBehaviour
{
    private ILevelManager manager;

    private void Start()
    {
        manager = SetLevelManager();
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

    public Threat NewRandomLevel1Threat()
    {
        int id = ++manager.GetGameData().lastThreatId;

        StringDb.ThreatType threatType = (StringDb.ThreatType)Random.Range(0, 3);

        float deployTime = Random.Range(2f, 6f);

        StringDb.ThreatAttacker threatAttacker = threatType == StringDb.ThreatType.fakeLocal
            ? StringDb.ThreatAttacker.intern
            : (StringDb.ThreatAttacker)Random.Range(0, 2);

        StringDb.ThreatAttack threatAttack;

        switch (threatType)
        {
            case StringDb.ThreatType.local:
                do
                {
                    threatAttack = (StringDb.ThreatAttack)manager.GetGameData().threatRandomizer.GetNext();
                } while ((int)threatAttack < 3 ||
                         threatAttack == StringDb.ThreatAttack.stuxnet ||
                         threatAttack == StringDb.ThreatAttack.dragonfly ||
                         threatAttack == StringDb.ThreatAttack.createRemote);
                break;
            case StringDb.ThreatType.remote:
                do
                {
                    threatAttack = (StringDb.ThreatAttack)manager.GetGameData().threatRandomizer.GetNext();

                } while ((int)threatAttack > 5 ||
                         threatAttack == StringDb.ThreatAttack.replay ||
                         threatAttack == StringDb.ThreatAttack.stuxnet ||
                         threatAttack == StringDb.ThreatAttack.dragonfly);
                break;
            case StringDb.ThreatType.fakeLocal:
                threatAttack = StringDb.ThreatAttack.fakeLocal;
                break;
            case StringDb.ThreatType.timeEvent:
                threatAttack = StringDb.ThreatAttack.timeEvent;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        StringDb.ThreatDanger threatDanger = threatType == StringDb.ThreatType.fakeLocal
            ? StringDb.ThreatDanger.fakeLocal
            : (StringDb.ThreatDanger)Random.Range(0, 3);

        float moneyLossPerMinute;

        switch (threatDanger)
        {
            case StringDb.ThreatDanger.low:
                moneyLossPerMinute = Random.Range(1f, 1.5f);
                break;
            case StringDb.ThreatDanger.medium:
                moneyLossPerMinute = Random.Range(2f, 2.5f);
                break;
            case StringDb.ThreatDanger.high:
                moneyLossPerMinute = Random.Range(3f, 3.5f);
                break;
            case StringDb.ThreatDanger.fakeLocal:
                moneyLossPerMinute = Random.Range(3f, 3.5f);
                break;
            case StringDb.ThreatDanger.timeEvent:
                moneyLossPerMinute = 0;
                break;
            default:
                moneyLossPerMinute = 0;
                break;
        }

        WeightedRandomizer<int> rand = new WeightedRandomizer<int>();

        rand.AddOrUpdateWeight((int)StringDb.ThreatAttack.dos, 0.1f);

        Threat threat = new Threat(id, threatType, deployTime, threatAttacker, threatDanger, threatAttack, moneyLossPerMinute);

        //Debug.Log(threat);

        return threat;
    }
    public Threat NewRandomLevel2Threat()
    {
        int id = ++manager.GetGameData().lastThreatId;

        StringDb.ThreatType threatType = (StringDb.ThreatType)Random.Range(0, 3);

        float deployTime = Random.Range(2f, 6f);

        StringDb.ThreatAttacker threatAttacker = threatType == StringDb.ThreatType.fakeLocal
            ? StringDb.ThreatAttacker.intern
            : (StringDb.ThreatAttacker)Random.Range(0, 2);

        StringDb.ThreatAttack threatAttack;

        switch (threatType)
        {
            case StringDb.ThreatType.local:
                do
                {
                    threatAttack = (StringDb.ThreatAttack)manager.GetGameData().threatRandomizer.GetNext();

                } while ((int)threatAttack < 3);
                break;
            case StringDb.ThreatType.remote:
                do
                {
                    threatAttack = (StringDb.ThreatAttack)manager.GetGameData().threatRandomizer.GetNext();

                } while ((int)threatAttack > 5);
                break;
            case StringDb.ThreatType.fakeLocal:
                threatAttack = StringDb.ThreatAttack.fakeLocal;
                break;
            case StringDb.ThreatType.timeEvent:
                threatAttack = StringDb.ThreatAttack.timeEvent;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        StringDb.ThreatDanger threatDanger = threatType == StringDb.ThreatType.fakeLocal
            ? StringDb.ThreatDanger.fakeLocal
            : (StringDb.ThreatDanger)Random.Range(0, 3);

        float moneyLossPerMinute;

        switch (threatDanger)
        {
            case StringDb.ThreatDanger.low:
                moneyLossPerMinute = Random.Range(1f, 1.5f);
                break;
            case StringDb.ThreatDanger.medium:
                moneyLossPerMinute = Random.Range(2f, 2.5f);
                break;
            case StringDb.ThreatDanger.high:
                moneyLossPerMinute = Random.Range(3f, 3.5f);
                break;
            case StringDb.ThreatDanger.fakeLocal:
                moneyLossPerMinute = Random.Range(3f, 3.5f);
                break;
            case StringDb.ThreatDanger.timeEvent:
                moneyLossPerMinute = 0;
                break;
            default:
                moneyLossPerMinute = 0;
                break;
        }

        WeightedRandomizer<int> rand = new WeightedRandomizer<int>();

        rand.AddOrUpdateWeight((int) StringDb.ThreatAttack.dos, 0.1f);

        Threat threat = new Threat(id, threatType, deployTime, threatAttacker, threatDanger, threatAttack, moneyLossPerMinute);

        //Debug.Log(threat);

        return threat;
    }

    public Threat NewThreatFromTimeEvent(AiController ai)
    {
        TimeEvent timeEvent = manager.GetGameData().timeEventList.FirstOrDefault(x => x.threat.aiController == ai);

        //Debug.Log("NewThreatFromTimeEvent " + timeEvent.threat);

        Threat threat = timeEvent?.threat;

        return threat;

    }

    //FOR TESTING GAME
    //---------------------------------------------------------------------------------------------------------------------
    public Threat NewLocalThreat()
    {
        int id = ++manager.GetGameData().lastThreatId;
        StringDb.ThreatType threatType = StringDb.ThreatType.local;

        float deployTime = Random.Range(2f, 6f);

        StringDb.ThreatAttacker threatAttacker = (StringDb.ThreatAttacker)Random.Range(0, 2);

        StringDb.ThreatAttack threatAttack;

        do
        {
            threatAttack = (StringDb.ThreatAttack)manager.GetGameData().threatRandomizer.GetNext();

        } while ((int)threatAttack < 3);

        StringDb.ThreatDanger threatDanger = (StringDb.ThreatDanger)Random.Range(0, 3);

        float moneyLossPerMinute;

        switch (threatDanger)
        {
            case StringDb.ThreatDanger.low:
                moneyLossPerMinute = Random.Range(1f, 1.5f);
                break;
            case StringDb.ThreatDanger.medium:
                moneyLossPerMinute = Random.Range(2f, 2.5f);
                break;
            case StringDb.ThreatDanger.high:
                moneyLossPerMinute = Random.Range(3f, 3.5f);
                break;
            case StringDb.ThreatDanger.fakeLocal:
                moneyLossPerMinute = Random.Range(3f, 3.5f);
                break;
            case StringDb.ThreatDanger.timeEvent:
                moneyLossPerMinute = 0;
                break;
            default:
                moneyLossPerMinute = 0;
                break;
        }

        Threat threat = new Threat(id, threatType, deployTime, threatAttacker, threatDanger, threatAttack, moneyLossPerMinute);

        //Debug.Log(threat);

        return threat;
    }

    public Threat NewRemoteThreat()
    {
        int id = ++manager.GetGameData().lastThreatId;
        StringDb.ThreatType threatType = StringDb.ThreatType.remote;

        float deployTime = Random.Range(2f, 6f);

        StringDb.ThreatAttacker threatAttacker = (StringDb.ThreatAttacker)Random.Range(0, 2);

        StringDb.ThreatAttack threatAttack;

        do
        {
            threatAttack = (StringDb.ThreatAttack)manager.GetGameData().threatRandomizer.GetNext();

        } while ((int)threatAttack > 5);

        StringDb.ThreatDanger threatDanger = (StringDb.ThreatDanger)Random.Range(0, 3);

        float moneyLossPerMinute;

        switch (threatDanger)
        {
            case StringDb.ThreatDanger.low:
                moneyLossPerMinute = Random.Range(1f, 1.5f);
                break;
            case StringDb.ThreatDanger.medium:
                moneyLossPerMinute = Random.Range(2f, 2.5f);
                break;
            case StringDb.ThreatDanger.high:
                moneyLossPerMinute = Random.Range(3f, 3.5f);
                break;
            case StringDb.ThreatDanger.fakeLocal:
                moneyLossPerMinute = Random.Range(3f, 3.5f);
                break;
            case StringDb.ThreatDanger.timeEvent:
                moneyLossPerMinute = 0;
                break;
            default:
                moneyLossPerMinute = 0;
                break;
        }

        Threat threat = new Threat(id, threatType, deployTime, threatAttacker, threatDanger, threatAttack, moneyLossPerMinute);

        //Debug.Log(threat);

        return threat;
    }

    public Threat NewFakeLocalThreat()
    {
        int id = ++manager.GetGameData().lastThreatId;
        StringDb.ThreatType threatType = StringDb.ThreatType.fakeLocal;

        float deployTime = Random.Range(2f, 6f);

        StringDb.ThreatAttacker threatAttacker = StringDb.ThreatAttacker.intern;

        StringDb.ThreatAttack threatAttack =  StringDb.ThreatAttack.fakeLocal;

        StringDb.ThreatDanger threatDanger = StringDb.ThreatDanger.fakeLocal;

        float moneyLossPerMinute = Random.Range(3f, 3.5f);

        Threat threat = new Threat(id, threatType, deployTime, threatAttacker, threatDanger, threatAttack, moneyLossPerMinute);

        //Debug.Log(threat);

        return threat;
    }
    //---------------------------------------------------------------------------------------------------------------------



}
