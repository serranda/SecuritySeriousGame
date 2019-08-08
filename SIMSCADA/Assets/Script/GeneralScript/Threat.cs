using UnityEngine;

[System.Serializable]
public class Threat
{
    public int id;
    public StringDb.ThreatType threatType;
    public float deployTime; //expressed in hour
    public StringDb.ThreatAttacker threatAttacker;
    public StringDb.ThreatDanger threatDanger;
    public StringDb.ThreatAttack threatAttack;
    public AiController aiController;
    public SerializableAiController serializableAiController;
    public float moneyLossPerMinute;

    public Threat(int id, StringDb.ThreatType threatType, float deployTime, StringDb.ThreatAttacker threatAttacker, StringDb.ThreatDanger threatDanger, StringDb.ThreatAttack threatAttack, AiController aiController, float moneyLossPerMinute)
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

    public Threat(int id, StringDb.ThreatType threatType, float deployTime, StringDb.ThreatAttacker threatAttacker, StringDb.ThreatDanger threatDanger, StringDb.ThreatAttack threatAttack, float moneyLossPerMinute)
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