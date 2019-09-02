using System;
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
        if (SceneManager.GetActiveScene().buildIndex == StaticDb.level1SceneIndex)
            iManager = FindObjectOfType<Level1Manager>();
        else
            iManager = FindObjectOfType<Level2Manager>();

        return iManager;
    }

    public Threat NewRandomLevel1Threat()
    {
        int id = ++manager.GetGameData().lastThreatId;

        StaticDb.ThreatType threatType = (StaticDb.ThreatType)Random.Range(0, 3);

        float deployTime = Random.Range(2f, 6f);

        StaticDb.ThreatAttacker threatAttacker = threatType == StaticDb.ThreatType.fakeLocal
            ? StaticDb.ThreatAttacker.intern
            : (StaticDb.ThreatAttacker)Random.Range(0, 2);

        StaticDb.ThreatAttack threatAttack;

        switch (threatType)
        {
            case StaticDb.ThreatType.local:
                do
                {
                    threatAttack = (StaticDb.ThreatAttack)manager.GetGameData().threatRandomizer.GetNext();
                } while ((int)threatAttack < 3 ||
                         threatAttack == StaticDb.ThreatAttack.stuxnet ||
                         threatAttack == StaticDb.ThreatAttack.dragonfly ||
                         threatAttack == StaticDb.ThreatAttack.createRemote);
                break;
            case StaticDb.ThreatType.remote:
                do
                {
                    threatAttack = (StaticDb.ThreatAttack)manager.GetGameData().threatRandomizer.GetNext();

                } while ((int)threatAttack > 5 ||
                         threatAttack == StaticDb.ThreatAttack.replay ||
                         threatAttack == StaticDb.ThreatAttack.stuxnet ||
                         threatAttack == StaticDb.ThreatAttack.dragonfly);
                break;
            case StaticDb.ThreatType.fakeLocal:
                threatAttack = StaticDb.ThreatAttack.fakeLocal;
                break;
            case StaticDb.ThreatType.timeEvent:
                threatAttack = StaticDb.ThreatAttack.timeEvent;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        StaticDb.ThreatDanger threatDanger = threatType == StaticDb.ThreatType.fakeLocal
            ? StaticDb.ThreatDanger.fakeLocal
            : (StaticDb.ThreatDanger)Random.Range(0, 3);

        float moneyLossPerMinute;

        switch (threatDanger)
        {
            case StaticDb.ThreatDanger.low:
                moneyLossPerMinute = Random.Range(2f, 2.5f);
                break;
            case StaticDb.ThreatDanger.medium:
                moneyLossPerMinute = Random.Range(3f, 3.5f);
                break;
            case StaticDb.ThreatDanger.high:
                moneyLossPerMinute = Random.Range(4f, 4.5f);
                break;
            case StaticDb.ThreatDanger.fakeLocal:
                moneyLossPerMinute = Random.Range(4f, 4.5f);
                break;
            case StaticDb.ThreatDanger.timeEvent:
                moneyLossPerMinute = 0;
                break;
            default:
                moneyLossPerMinute = 0;
                break;
        }

        WeightedRandomizer<int> rand = new WeightedRandomizer<int>();

        rand.AddOrUpdateWeight((int)StaticDb.ThreatAttack.dos, 0.1f);

        Threat threat = new Threat(id, threatType, deployTime, threatAttacker, threatDanger, threatAttack, moneyLossPerMinute, false);

        //Debug.Log(threat);

        return threat;
    }

    public Threat NewRandomLevel2Threat()
    {
        int id = ++manager.GetGameData().lastThreatId;

        StaticDb.ThreatType threatType = (StaticDb.ThreatType)Random.Range(0, 3);

        float deployTime = Random.Range(2f, 6f);

        StaticDb.ThreatAttacker threatAttacker = threatType == StaticDb.ThreatType.fakeLocal
            ? StaticDb.ThreatAttacker.intern
            : (StaticDb.ThreatAttacker)Random.Range(0, 2);

        StaticDb.ThreatAttack threatAttack;

        switch (threatType)
        {
            case StaticDb.ThreatType.local:
                do
                {
                    threatAttack = (StaticDb.ThreatAttack)manager.GetGameData().threatRandomizer.GetNext();

                } while ((int)threatAttack < 3);
                break;
            case StaticDb.ThreatType.remote:
                do
                {
                    threatAttack = (StaticDb.ThreatAttack)manager.GetGameData().threatRandomizer.GetNext();

                } while ((int)threatAttack > 5);
                break;
            case StaticDb.ThreatType.fakeLocal:
                threatAttack = StaticDb.ThreatAttack.fakeLocal;
                break;
            case StaticDb.ThreatType.timeEvent:
                threatAttack = StaticDb.ThreatAttack.timeEvent;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        StaticDb.ThreatDanger threatDanger = threatType == StaticDb.ThreatType.fakeLocal
            ? StaticDb.ThreatDanger.fakeLocal
            : (StaticDb.ThreatDanger)Random.Range(0, 3);

        float moneyLossPerMinute;

        switch (threatDanger)
        {
            case StaticDb.ThreatDanger.low:
                moneyLossPerMinute = Random.Range(2f, 2.5f);
                break;
            case StaticDb.ThreatDanger.medium:
                moneyLossPerMinute = Random.Range(3f, 3.5f);
                break;
            case StaticDb.ThreatDanger.high:
                moneyLossPerMinute = Random.Range(4f, 4.5f);
                break;
            case StaticDb.ThreatDanger.fakeLocal:
                moneyLossPerMinute = Random.Range(4f, 4.5f);
                break;
            case StaticDb.ThreatDanger.timeEvent:
                moneyLossPerMinute = 0;
                break;
            default:
                moneyLossPerMinute = 0;
                break;
        }

        WeightedRandomizer<int> rand = new WeightedRandomizer<int>();

        rand.AddOrUpdateWeight((int) StaticDb.ThreatAttack.dos, 0.1f);

        Threat threat = new Threat(id, threatType, deployTime, threatAttacker, threatDanger, threatAttack, moneyLossPerMinute, false);

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

    public Threat NewLocalThreat()
    {
        int id = ++manager.GetGameData().lastThreatId;
        StaticDb.ThreatType threatType = StaticDb.ThreatType.local;

        float deployTime = Random.Range(2f, 6f);

        StaticDb.ThreatAttacker threatAttacker = (StaticDb.ThreatAttacker)Random.Range(0, 2);

        StaticDb.ThreatAttack threatAttack;

        do
        {
            threatAttack = (StaticDb.ThreatAttack)manager.GetGameData().threatRandomizer.GetNext();

        } while ((int)threatAttack < 3);

        StaticDb.ThreatDanger threatDanger = (StaticDb.ThreatDanger)Random.Range(0, 3);

        float moneyLossPerMinute;

        switch (threatDanger)
        {
            case StaticDb.ThreatDanger.low:
                moneyLossPerMinute = Random.Range(1f, 1.5f);
                break;
            case StaticDb.ThreatDanger.medium:
                moneyLossPerMinute = Random.Range(2f, 2.5f);
                break;
            case StaticDb.ThreatDanger.high:
                moneyLossPerMinute = Random.Range(3f, 3.5f);
                break;
            case StaticDb.ThreatDanger.fakeLocal:
                moneyLossPerMinute = Random.Range(3f, 3.5f);
                break;
            case StaticDb.ThreatDanger.timeEvent:
                moneyLossPerMinute = 0;
                break;
            default:
                moneyLossPerMinute = 0;
                break;
        }

        Threat threat = new Threat(id, threatType, deployTime, threatAttacker, threatDanger, threatAttack, moneyLossPerMinute, false);

        //Debug.Log(threat);

        return threat;
    }

    public Threat NewRemoteThreat()
    {
        int id = ++manager.GetGameData().lastThreatId;
        StaticDb.ThreatType threatType = StaticDb.ThreatType.remote;

        float deployTime = Random.Range(2f, 6f);

        StaticDb.ThreatAttacker threatAttacker = (StaticDb.ThreatAttacker)Random.Range(0, 2);

        StaticDb.ThreatAttack threatAttack;

        do
        {
            threatAttack = (StaticDb.ThreatAttack)manager.GetGameData().threatRandomizer.GetNext();

        } while ((int)threatAttack > 5);

        StaticDb.ThreatDanger threatDanger = (StaticDb.ThreatDanger)Random.Range(0, 3);

        float moneyLossPerMinute;

        switch (threatDanger)
        {
            case StaticDb.ThreatDanger.low:
                moneyLossPerMinute = Random.Range(1f, 1.5f);
                break;
            case StaticDb.ThreatDanger.medium:
                moneyLossPerMinute = Random.Range(2f, 2.5f);
                break;
            case StaticDb.ThreatDanger.high:
                moneyLossPerMinute = Random.Range(3f, 3.5f);
                break;
            case StaticDb.ThreatDanger.fakeLocal:
                moneyLossPerMinute = Random.Range(3f, 3.5f);
                break;
            case StaticDb.ThreatDanger.timeEvent:
                moneyLossPerMinute = 0;
                break;
            default:
                moneyLossPerMinute = 0;
                break;
        }

        Threat threat = new Threat(id, threatType, deployTime, threatAttacker, threatDanger, threatAttack, moneyLossPerMinute, false);

        //Debug.Log(threat);

        return threat;
    }

    public Threat NewFromCreateRemoteThreat()
    {
        int id = ++manager.GetGameData().lastThreatId;
        StaticDb.ThreatType threatType = StaticDb.ThreatType.remote;

        float deployTime = Random.Range(2f, 6f);

        StaticDb.ThreatAttacker threatAttacker = (StaticDb.ThreatAttacker)Random.Range(0, 2);

        StaticDb.ThreatAttack threatAttack;

        do
        {
            threatAttack = (StaticDb.ThreatAttack)manager.GetGameData().threatRandomizer.GetNext();

        } while ((int)threatAttack > 5);

        StaticDb.ThreatDanger threatDanger = (StaticDb.ThreatDanger)Random.Range(0, 3);

        float moneyLossPerMinute;

        switch (threatDanger)
        {
            case StaticDb.ThreatDanger.low:
                moneyLossPerMinute = Random.Range(1f, 1.5f);
                break;
            case StaticDb.ThreatDanger.medium:
                moneyLossPerMinute = Random.Range(2f, 2.5f);
                break;
            case StaticDb.ThreatDanger.high:
                moneyLossPerMinute = Random.Range(3f, 3.5f);
                break;
            case StaticDb.ThreatDanger.fakeLocal:
                moneyLossPerMinute = Random.Range(3f, 3.5f);
                break;
            case StaticDb.ThreatDanger.timeEvent:
                moneyLossPerMinute = 0;
                break;
            default:
                moneyLossPerMinute = 0;
                break;
        }

        Threat threat = new Threat(id, threatType, deployTime, threatAttacker, threatDanger, threatAttack, moneyLossPerMinute, true);

        //Debug.Log(threat);

        return threat;
    }

    public Threat NewFakeLocalThreat()
    {
        int id = ++manager.GetGameData().lastThreatId;
        StaticDb.ThreatType threatType = StaticDb.ThreatType.fakeLocal;

        float deployTime = Random.Range(2f, 6f);

        StaticDb.ThreatAttacker threatAttacker = StaticDb.ThreatAttacker.intern;

        StaticDb.ThreatAttack threatAttack =  StaticDb.ThreatAttack.fakeLocal;

        StaticDb.ThreatDanger threatDanger = StaticDb.ThreatDanger.fakeLocal;

        float moneyLossPerMinute = Random.Range(3f, 3.5f);

        Threat threat = new Threat(id, threatType, deployTime, threatAttacker, threatDanger, threatAttack, moneyLossPerMinute, false);

        //Debug.Log(threat);

        return threat;
    }



}
