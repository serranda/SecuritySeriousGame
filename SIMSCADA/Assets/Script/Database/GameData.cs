using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeightedRandomization;

[System.Serializable]

public class GameData
{
    //reference for the last sprite pressed; used for spawning action menu
    public string pressedSprite;

    //check to see if first launch
    public bool firstLaunch = true;

    //id for save slot
    public int indexSlot;

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

    //values for first of his type deployed threat
    public Threat firstThreat;

    //flag for id card upgrade
    public bool idCardUpgraded;

    //flag for research upgrade
    public bool researchUpgrade;

    //hud values
    public DateTime date = StaticDb.starterDate;
    public long longDate = StaticDb.starterDate.ToFileTimeUtc();
    public float money = 1000;
    public int successfulThreat;
    public int totalThreat;
    public int trustedEmployees = 10;
    public int totalEmployees = 50;
    public float reputation = 35;

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
    public StaticDb.ServerSecurity serverSecurity = StaticDb.ServerSecurity.medium;

    //value for last ai sprite generated
    public string lastMaleSpriteNumber;
    public string lastFemaleSpriteNumber;

    //value for monthly threat tendencies 
    public StaticDb.ThreatAttack monthlyThreat;

    public ThreatAttackFloatDictionary weights = new ThreatAttackFloatDictionary
    {
        {StaticDb.ThreatAttack.dos, 0f},
        {StaticDb.ThreatAttack.phishing, 0f},
        {StaticDb.ThreatAttack.replay, 0f},
        {StaticDb.ThreatAttack.mitm, 0f},
        {StaticDb.ThreatAttack.stuxnet, 0f},
        {StaticDb.ThreatAttack.dragonfly, 0f},
        {StaticDb.ThreatAttack.malware, 0f},
        {StaticDb.ThreatAttack.createRemote, 0f}
    };

    //values for weighted randomizer for local and remote threats
    public WeightedRandomizer<int> threatRandomizer = new WeightedRandomizer<int>();

    //values for endgame counter
    public float winCounter;
    public float lossCounter;

    //values to check if game has been won or lost
    public bool isGameWon;

    //list for time event
    public List<TimeEvent> timeEventList = new List<TimeEvent>();

    //list for deployed threat
    public List<Threat> deployedThreatList = new List<Threat>();

    //list for remote threat, local threat and detected threat still deploying
    public List<Threat> remoteThreats = new List<Threat>();
    public List<Threat> localThreats = new List<Threat>();
    public List<Threat> threatDetectedList = new List<Threat>();

    //int for previous month check (in order to increase month)
    public int previousMonth;

    ////flag to check if there is a money loss
    //public bool isMoneyLoss;

    //dictionary in which are stored alle the money loss factor relative to all types of threat
    public ThreatAttackFloatDictionary moneyLossList = new ThreatAttackFloatDictionary
    {
        {StaticDb.ThreatAttack.dos, 0f},
        {StaticDb.ThreatAttack.phishing, 0f},
        {StaticDb.ThreatAttack.replay, 0f},
        {StaticDb.ThreatAttack.mitm, 0f},
        {StaticDb.ThreatAttack.stuxnet, 0f},
        {StaticDb.ThreatAttack.dragonfly, 0f},
        {StaticDb.ThreatAttack.malware, 0f}
    };

    //sums of all the money loss per minutes factor
    public float totalMoneyLossPerMinute;

    //sums of all the money earns per minutes
    public float totalMoneyEarnPerMinute;

    //sums of all the money cost per minutes need to keep active ids, firewall and local security
    public float totalCostPerMinute;

    //int for the speed factor mutiplier
    public int simulationSpeedMultiplier = 1;

    //int for the time simulation; how many milliseconds needed for a minute in game time
    public int millisecondsPerMinutes = 10000;

    //int for the spawn rate of threat
    public int threatSpawnBaseTime = 2000;

    //flags to check which type of threat has been deployed
    public bool hasThreatDeployed;

    //flags to check if, before going on with the defensive action, the plant has been checked or has been done a malware scan
    public bool hasPlantChecked = true;
    public bool hasMalwareChecked = true;

    //flag to check if all the correct actions has been done and the threat has been managed
    public bool hasThreatManaged;

    //flags to check if the firewall, the remote ids or the local ids has been activated
    public bool isFirewallActive;
    public bool isRemoteIdsActive;
    public bool isLocalIdsActive;

    //flags to check which prefab are active
    public bool scadaEnabled;
    public bool storeEnabled;
    public bool securityScreenEnabled;
    public bool noteBookEnabled;
    public bool idCardEnabled;
    public bool buttonEnabled;
    public bool dialogEnabled;
    public bool chartEnabled;

    //value for item store selected
    public ItemStore itemStoreSelected = new ItemStore();

    //vale for last threat deployed
    public Threat lastThreatDeployed;

    //value for last threat stopped
    public Threat lastThreatStopped;

    //values for money amount and threat type which caused last money loss
    public float lastAmountLoss;
    public StaticDb.ThreatType lastTypeLoss;

    //value for last money earn amount
    public float lastAmountEarn;

    //values for last threat management result
    public TimeSpan lastManagementTime;

    //value for new employees hired
    public int employeesHired;

    //bool to check if cursor is over an ai sprite
    public bool cursorOverAi;

    //camera zoom value
    public float cameraZoom;

    //value for level index;
    public int levelIndex;

}
