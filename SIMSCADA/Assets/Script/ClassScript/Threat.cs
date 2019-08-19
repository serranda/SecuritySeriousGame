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
    public AiController aiController;
    public SerializableAiController serializableAiController;
    public float moneyLossPerMinute;

    public Threat(int id, StaticDb.ThreatType threatType, float deployTime, StaticDb.ThreatAttacker threatAttacker, StaticDb.ThreatDanger threatDanger, StaticDb.ThreatAttack threatAttack, AiController aiController, float moneyLossPerMinute)
    {
        this.id = id;
        this.threatType = threatType;
        this.deployTime = deployTime;
        this.threatAttacker = threatAttacker;
        this.threatDanger = threatDanger;
        this.threatAttack = threatAttack;
        this.aiController = aiController;
        this.moneyLossPerMinute = moneyLossPerMinute;
    }

    public Threat(int id, StaticDb.ThreatType threatType, float deployTime, StaticDb.ThreatAttacker threatAttacker, StaticDb.ThreatDanger threatDanger, StaticDb.ThreatAttack threatAttack, float moneyLossPerMinute)
    {
        this.id = id;
        this.threatType = threatType;
        this.deployTime = deployTime;
        this.threatAttacker = threatAttacker;
        this.threatDanger = threatDanger;
        this.threatAttack = threatAttack;
        this.moneyLossPerMinute = moneyLossPerMinute;
    }

    public override string ToString()
    {
        return
            $"Id: {id}, ThreatType: {threatType}, DeployTime: {deployTime}, ThreatAttacker: {threatAttacker}, ThreatDanger: {threatDanger}, " +
            $"ThreatAttack: {threatAttack}, SerializableAiController: {aiController}, MoneyLossPerHour: {moneyLossPerMinute}";
    }
}