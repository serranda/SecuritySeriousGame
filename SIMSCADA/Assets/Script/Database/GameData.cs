using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeightedRandomization;

public class GameData : MonoBehaviour
{
    //reference for the last sprite pressed; used for spawning action menu
    public static string pressedSprite;

    //check to see if first launch
    public static bool firstLaunch = true;

    //id for save slot
    public static int slotIndex = 0;

    //id for last scene reference
    public static int lastSceneIndex = 0;

    //id for last threat created
    public static int lastThreatId = 0;

    //id for last time event created
    public static int lastTimeEventId = 0;

    //id for last ai prefab instantiated
    public static int lastAiId = 0;

    //flag for point out ability purchased on the store
    public static bool pointOutPurchased = false;
    public static bool localIdsUpgraded = false;

    //flag to check if the deployed threat is the first one of his type
    public static bool isFirstDos = true;
    public static bool isFirstPhishing = true;
    public static bool isFirstMalware = true;
    public static bool isFirstStuxnet = true;
    public static bool isFirstDragonfly = true;
    public static bool isFirstMitm = true;
    public static bool isFirstReplay = true;

    //flag for id card upgrade
    public static bool idCardUpgraded = false;

    //flag for research upgrade
    public static bool researchUpgrade = false;

    //hud values
    public static DateTime date;
    public static float money;
    public static int successfulThreat;
    public static int totalThreat;
    public static int trustedEmployees;
    public static int totalEmployees;
    public static float reputation;

    //time value for day simulation
    public static float minutePercentage;
    public static float remoteIdsCheckTime;
    public static float localIdsCheckTime;
    public static float threatSpawnTime;
    public static float threatSpawnRate;

    //value for ids and firewall success rate && ids check rate
    public static int remoteIdsSuccessRate = 30;
    public static float remoteIdsCheckRate = 30.0f;
    public static float localIdsCheckRate = 30.0f;
    public static int localIdsWrongCounter = 10;
    public static int firewallSuccessRate = 30;

    //list storing ItemStore values with current levels
    public static List<ItemStore> itemStoreList = new List<ItemStore>();

    //values for defense against attack
    public static int defenseDos = 10;
    public static int defensePhishing = 10;
    public static int defenseReplay = 10;
    public static int defenseMitm = 10;
    public static int defenseMalware = 10;
    public static int defenseStuxnet = 10;
    public static int defenseDragonfly = 10;
    public static int defenseCreateRemote = 10;

    public static int defensePlantResistance = 10;

    //value for time needed to check id card
    public static float idCardTime = 20.0f;

    //value for pc && server && telephone item amount in the scene
    public static int serverAmount = 1;
    public static int pcAmount = 1;
    public static int telephoneAmount = 1;
    public static int securityCheckAmount = 1;

    //values for time need for pc && server && telephone operations
    public static float serverRebootTime = 40.0f;
    public static float serverScanTime = 40.0f;
    public static float serverCheckCfgTime = 40.0f;
    public static float serverIdsCleanTime = 40.0f;
    public static float serverAntiMalwareTime = 40.0f;

    public static float pcRecapTime = 40.0f;
    public static float pcPointOutTime = 40.0f;

    public static float telephoneCheckPlantTime = 40.0f;
    public static float telephoneMoneyTime = 30.0f;
    public static float telephoneMoneyCoolDown = 2.0f;

    //value for security level
    public static StringDb.ServerSecurity serverSecurity;

    //value for last ai sprite generated
    public static string lastMaleSpriteNumber;
    public static string lastFemaleSpriteNumber;

    //value for monthly threat tendencies 
    public static StringDb.ThreatAttack monthlyThreat;

    public static Dictionary<StringDb.ThreatAttack, float> weights = new Dictionary<StringDb.ThreatAttack, float>
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

    //valus for weighted randomizer for local and remote threats
    public static WeightedRandomizer<int> threatRandomizer = new WeightedRandomizer<int>();

    //values for endgame counter
    public static float winCounter;
    public static float lossCounter;

    //values to check if game has been won or lost
    public static bool isGameWon;

}
