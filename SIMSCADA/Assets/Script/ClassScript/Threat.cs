using System;
using UnityEngine;

[System.Serializable]
public class Threat
{
    public int id;
    public StaticDb.ThreatType threatType;
    public float deployTime; //expressed in hour
    public StaticDb.ThreatAttacker threatAttacker;
    public StaticDb.ThreatDanger threatDanger;
    public StaticDb.ThreatAttack threatAttack;
    public float moneyLossPerMinute;
    public bool fromCreateRemote;

    //values for ai controller prefab; the serializable one is created when game is saved
    public AiController aiController;
    public SerializableAiController serializableAiController;


    //public Threat(int id, StaticDb.ThreatType threatType, float deployTime, StaticDb.ThreatAttacker threatAttacker, StaticDb.ThreatDanger threatDanger, StaticDb.ThreatAttack threatAttack, AiController aiController, float moneyLossPerMinute)
    //{
    //    this.id = id;
    //    this.threatType = threatType;
    //    this.deployTime = deployTime;
    //    this.threatAttacker = threatAttacker;
    //    this.threatDanger = threatDanger;
    //    this.threatAttack = threatAttack;
    //    this.aiController = aiController;
    //    this.moneyLossPerMinute = moneyLossPerMinute;
    //}

    public Threat(int id, StaticDb.ThreatType threatType, float deployTime, StaticDb.ThreatAttacker threatAttacker, StaticDb.ThreatDanger threatDanger, StaticDb.ThreatAttack threatAttack, float moneyLossPerMinute, bool fromCreateRemote)
    {
        this.id = id;
        this.threatType = threatType;
        this.deployTime = deployTime;
        this.threatAttacker = threatAttacker;
        this.threatDanger = threatDanger;
        this.threatAttack = threatAttack;
        this.moneyLossPerMinute = moneyLossPerMinute;
        this.fromCreateRemote = fromCreateRemote;
    }

    public override string ToString()
    {
        return
            $"Id: {id}, ThreatType: {threatType}, DeployTime: {deployTime}, ThreatAttacker: {threatAttacker}, ThreatDanger: {threatDanger}, " +
            $"ThreatAttack: {threatAttack}, AiController: {aiController}, MoneyLossPerMinute: {moneyLossPerMinute}";
    }

    public static Threat GetThreatFromThreatAttack(StaticDb.ThreatAttack attack)
    {
        StaticDb.ThreatType type;
        switch (attack)
        {
            case StaticDb.ThreatAttack.dos:
            case StaticDb.ThreatAttack.phishing:
            case StaticDb.ThreatAttack.replay:
                type = StaticDb.ThreatType.remote;
                break;
            case StaticDb.ThreatAttack.malware:
            case StaticDb.ThreatAttack.createRemote:
                type = StaticDb.ThreatType.local;
                break;
            case StaticDb.ThreatAttack.mitm:
            case StaticDb.ThreatAttack.stuxnet:
            case StaticDb.ThreatAttack.dragonfly:
                type = StaticDb.ThreatType.hybrid;
                break;
            case StaticDb.ThreatAttack.fakeLocal:
                type = StaticDb.ThreatType.fakeLocal;
                break;
            case StaticDb.ThreatAttack.timeEvent:
                type = StaticDb.ThreatType.timeEvent;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(attack), attack, null);
        }

        return new Threat(-1, type, -1, StaticDb.ThreatAttacker.timeEvent, StaticDb.ThreatDanger.timeEvent, attack, 0, false);
    }
}