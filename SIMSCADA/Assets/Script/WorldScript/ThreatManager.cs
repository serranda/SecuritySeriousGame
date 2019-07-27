using System;
using System.Linq;
using UnityEngine;
using WeightedRandomization;
using Random = UnityEngine.Random;

public class ThreatManager : MonoBehaviour
{
    public Threat NewRandomLevel1Threat()
    {
        int id = ++GameData.lastThreatId;

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
                    threatAttack = (StringDb.ThreatAttack)GameData.threatRandomizer.GetNext();
                } while ((int)threatAttack < 3 ||
                         threatAttack == StringDb.ThreatAttack.stuxnet ||
                         threatAttack == StringDb.ThreatAttack.dragonfly ||
                         threatAttack == StringDb.ThreatAttack.createRemote);
                break;
            case StringDb.ThreatType.remote:
                do
                {
                    threatAttack = (StringDb.ThreatAttack)GameData.threatRandomizer.GetNext();

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
        int id = ++GameData.lastThreatId;

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
                    threatAttack = (StringDb.ThreatAttack)GameData.threatRandomizer.GetNext();

                } while ((int)threatAttack < 3);
                break;
            case StringDb.ThreatType.remote:
                do
                {
                    threatAttack = (StringDb.ThreatAttack)GameData.threatRandomizer.GetNext();

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
        TimeEvent timeEvent = ClassDb.level1Manager.timeEventList.FirstOrDefault(x => x.threat.aiController == ai);

        if (timeEvent == null) return null;

        //Debug.Log("NewThreatFromTimeEvent " + timeEvent.threat);

        Threat threat = timeEvent.threat;

        return threat;

    }

    //FOR TESTING GAME
    //---------------------------------------------------------------------------------------------------------------------
    public Threat NewLocalThreat()
    {
        int id = ++GameData.lastThreatId;
        StringDb.ThreatType threatType = StringDb.ThreatType.local;

        float deployTime = Random.Range(2f, 6f);

        StringDb.ThreatAttacker threatAttacker = (StringDb.ThreatAttacker)Random.Range(0, 2);

        StringDb.ThreatAttack threatAttack;

        do
        {
            threatAttack = (StringDb.ThreatAttack)GameData.threatRandomizer.GetNext();

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
        int id = ++GameData.lastThreatId;
        StringDb.ThreatType threatType = StringDb.ThreatType.remote;

        float deployTime = Random.Range(2f, 6f);

        StringDb.ThreatAttacker threatAttacker = (StringDb.ThreatAttacker)Random.Range(0, 2);

        StringDb.ThreatAttack threatAttack;

        do
        {
            threatAttack = (StringDb.ThreatAttack)GameData.threatRandomizer.GetNext();

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
        int id = ++GameData.lastThreatId;
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
