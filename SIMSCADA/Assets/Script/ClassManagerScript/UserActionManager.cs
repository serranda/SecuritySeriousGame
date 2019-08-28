using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserActionManager : MonoBehaviour
{
    public void RegisterThreatSolution(UserAction action, Threat threat, bool isThreatDetected)
    {
        List<StaticDb.ThreatSolution> threatSolutions = new List<StaticDb.ThreatSolution>();

        if (isThreatDetected)
        {
            //THE RIGHT ACTION IS TO CHECK IDS
            threatSolutions.Add(StaticDb.ThreatSolution.idsClean);
        }
        else
        {
            //RIGHT ACTION DEPENDS TO THREAT ATTACK TYPE
            switch (threat.threatAttack)
            {
                case StaticDb.ThreatAttack.dos:
                    threatSolutions.Add(StaticDb.ThreatSolution.reboot);
                    break;
                case StaticDb.ThreatAttack.phishing:
                    break;
                case StaticDb.ThreatAttack.replay:
                    threatSolutions.Add(StaticDb.ThreatSolution.plantCheck);
                    threatSolutions.Add(StaticDb.ThreatSolution.networkCheck);
                    break;
                case StaticDb.ThreatAttack.mitm:
                    threatSolutions.Add(StaticDb.ThreatSolution.networkCheck);
                    break;
                case StaticDb.ThreatAttack.stuxnet:
                    threatSolutions.Add(StaticDb.ThreatSolution.plantCheck);
                    threatSolutions.Add(StaticDb.ThreatSolution.malwareScan);
                    break;
                case StaticDb.ThreatAttack.dragonfly:
                    threatSolutions.Add(StaticDb.ThreatSolution.malwareScan);
                    threatSolutions.Add(StaticDb.ThreatSolution.networkCheck);
                    break;
                case StaticDb.ThreatAttack.malware:
                    threatSolutions.Add(StaticDb.ThreatSolution.malwareScan);
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
        }

        foreach (StaticDb.ThreatSolution threatSolution in threatSolutions)
        {
            if (action.solution == threatSolution)
            {
                ClassDb.logManager.StartWritePlayerLogRoutine(StaticDb.player, StaticDb.logEvent.GameEvent, "CORRECT ACTION");
            }
            else
            {
                ClassDb.logManager.StartWritePlayerLogRoutine(StaticDb.player, StaticDb.logEvent.GameEvent, "WRONG ACTION");
            }
        }
    }

    public void RegisterItemPurchase(ItemStore itemStore, Threat monthlyThreat)
    {
        int affinityPoint = 0;

        foreach (string affinity in itemStore.threatAffinity)
        {
            if (affinity == StaticDb.ThreatAffinity.all.ToString())
            {
                affinityPoint++;
                continue;
            }

            if (monthlyThreat.threatType == StaticDb.ThreatType.remote ||
                 monthlyThreat.threatType == StaticDb.ThreatType.hybrid)
            {
                if (affinity == StaticDb.ThreatAffinity.remote.ToString())
                {
                    affinityPoint++;
                    continue;
                }
            }

            if (monthlyThreat.threatType == StaticDb.ThreatType.local ||
                 monthlyThreat.threatType == StaticDb.ThreatType.hybrid)
            {
                if (affinity == StaticDb.ThreatAffinity.local.ToString())
                {
                    affinityPoint++;
                    continue;
                }
            }

            if (affinity == monthlyThreat.threatAttack.ToString())
                affinityPoint++;
        }

        Debug.Log(affinityPoint);

        if (affinityPoint == itemStore.threatAffinity.Count)
        {
            ClassDb.logManager.StartWritePlayerLogRoutine(StaticDb.player, StaticDb.logEvent.GameEvent, itemStore.name.ToUpper() + " PURCHASED; PERFECT PURCHASE");
        }
        else if (affinityPoint == 0)
        {
            ClassDb.logManager.StartWritePlayerLogRoutine(StaticDb.player, StaticDb.logEvent.GameEvent, itemStore.name.ToUpper() + " PURCHASED; USELESS PURCHASE");
        }
        else
        {
            ClassDb.logManager.StartWritePlayerLogRoutine(StaticDb.player, StaticDb.logEvent.GameEvent, itemStore.name.ToUpper() + " PURCHASED; GOOD PURCHASE");
        }

    }
}