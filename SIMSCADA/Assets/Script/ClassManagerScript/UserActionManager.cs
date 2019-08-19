using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserActionManager : MonoBehaviour
{
    public void RegisterThreatSolution(UserAction action, Threat threat)
    {
        List<StaticDb.ThreatSolution> threatSolutions = new List<StaticDb.ThreatSolution>();

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
                threatSolutions.Add(StaticDb.ThreatSolution.malwareScan);
                threatSolutions.Add(StaticDb.ThreatSolution.plantCheck);
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

        bool correct;

        foreach (StaticDb.ThreatSolution threatSolution in threatSolutions)
        {
            if (action.solution == threatSolution)
            {
                correct = true;
                Debug.Log("CORRECT ACTION");

            }
            else
            {
                correct = false;
                Debug.Log("WRONG ACTION");
            }


        }

        //TODO TEST IF IS WORKING WITH MULTIPLE THREAT SOLUTION


    }
}
