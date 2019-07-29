using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeightedRandomization;

public class GameData : MonoBehaviour
{
    //reference for the last sprite pressed; used for spawning action menu
    public string pressedSprite;

    //check to see if first launch
    public bool firstLaunch = true;

    //id for save slot
    public int slotIndex;

    //id for last scene reference
    public int lastSceneIndex;

    //id for last threat created
    public int lastThreatId;

    //id for last time event created
    public int lastTimeEventId;

    //id for last ai prefab instantiated
    public int lastAiId;

    //flag for point out ability purchased on the store
    public bool pointOutPurchased;
    public bool localIdsUpgraded;

    //flag to check if the deployed threat is the first one of his type
    public bool isFirstDos = true;
    public bool isFirstPhishing = true;
    public bool isFirstMalware = true;
    public bool isFirstStuxnet = true;
    public bool isFirstDragonfly = true;
    public bool isFirstMitm = true;
    public bool isFirstReplay = true;

    //flag for id card upgrade
    public bool idCardUpgraded;

    //flag for research upgrade
    public bool researchUpgrade;

    //hud values
    public DateTime date;
    public float money;
    public int successfulThreat;
    public int totalThreat;
    public int trustedEmployees;
    public int totalEmployees;
    public float reputation;

    //time value for day simulation
    public float minutePercentage;
    public float remoteIdsCheckTime;
    public float localIdsCheckTime;
    public float threatSpawnTime;
    public float threatSpawnRate;

    //value for ids and firewall success rate && ids check rate
    public int remoteIdsSuccessRate = 30;
    public float remoteIdsCheckRate = 30.0f;
    public float localIdsCheckRate = 30.0f;
    public int localIdsWrongCounter = 10;
    public int firewallSuccessRate = 30;

    //list storing ItemStore values with current levels
    public List<ItemStore> itemStoreList = new List<ItemStore>();

    //values for defense against attack
    public int defenseDos = 10;
    public int defensePhishing = 10;
    public int defenseReplay = 10;
    public int defenseMitm = 10;
    public int defenseMalware = 10;
    public int defenseStuxnet = 10;
    public int defenseDragonfly = 10;
    public int defenseCreateRemote = 10;

    public int defensePlantResistance = 10;

    //value for time needed to check id card
    public float idCardTime = 20.0f;

    //value for pc && server && telephone item amount in the scene
    public int serverAmount = 1;
    public int pcAmount = 1;
    public int telephoneAmount = 1;
    public int securityCheckAmount = 1;

    //values for time need for pc && server && telephone operations
    public float serverRebootTime = 40.0f;
    public float serverScanTime = 40.0f;
    public float serverCheckCfgTime = 40.0f;
    public float serverIdsCleanTime = 40.0f;
    public float serverAntiMalwareTime = 40.0f;

    public float pcRecapTime = 40.0f;
    public float pcPointOutTime = 40.0f;

    public float telephoneCheckPlantTime = 40.0f;
    public float telephoneMoneyTime = 30.0f;
    public float telephoneMoneyCoolDown = 2.0f;

    //value for security level
    public StringDb.ServerSecurity serverSecurity;

    //value for last ai sprite generated
    public string lastMaleSpriteNumber;
    public string lastFemaleSpriteNumber;

    //value for monthly threat tendencies 
    public StringDb.ThreatAttack monthlyThreat;

    public Dictionary<StringDb.ThreatAttack, float> weights = new Dictionary<StringDb.ThreatAttack, float>
    {
        {StringDb.ThreatAttack.dos, 0f},
        {StringDb.ThreatAttack.phishing, 0f},
        {StringDb.ThreatAttack.replay, 0f},
        {StringDb.ThreatAttack.mitm, 0f},
        {StringDb.ThreatAttack.stuxnet, 0f},
        {StringDb.ThreatAttack.dragonfly, 0f},
        {StringDb.ThreatAttack.malware, 0f},
        {StringDb.ThreatAttack.createRemote, 0f}
    };

    //values for weighted randomizer for local and remote threats
    public WeightedRandomizer<int> threatRandomizer = new WeightedRandomizer<int>();

    //values for endgame counter
    public float winCounter;
    public float lossCounter;

    //values to check if game has been won or lost
    public bool isGameWon;
}
