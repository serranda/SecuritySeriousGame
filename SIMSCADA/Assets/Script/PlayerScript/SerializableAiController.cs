using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]

public class SerializableAiController
{
    public Vector3 offsetDraw;
    public Vector3 scaleDraw;

    public bool isAttacker;
    public bool destroy;
    public bool idChecked;
    public bool idScanned;
    public bool isTrusted;
    public bool isWaiting;
    public bool isSuspected;
    public bool isAiPathValuesSet;

    public string spriteToAnimate;
    public string nSpriteName;
    public string hlSpriteName;
    public string prSpriteName;
    public string spriteNumber;
    public string idSpriteName;

    public StringDb.AiDangerResistance dangerResistance;

    public int aiId;

    public string aiName;
    public string aiSurname;
    public string aiJob;
    public string aiGender;

    public List<Node> path;

    public bool pathUpdated;
    public bool pathfinderChanged;

    public int wrongDestinationCounter;

    public int radiusBase;

    public bool onClickAi;

    public Vector3Int aiStartCellPos;
    public Vector3Int aiCellPos;
    public Vector3Int aiDestCellPos;

    public float aiSpeed;
    public Vector3Int aiObjective;

    public float timer;

    //public TimeEvent timeEvent;

    public SerializableAiController(AiController ai)
    {
        this.offsetDraw = ai.offsetDraw;
        this.scaleDraw = ai.scaleDraw;
        this.isAttacker = ai.isAttacker;
        this.destroy = ai.destroy;
        this.idChecked = ai.idChecked;
        this.idScanned = ai.idScanned;
        this.isTrusted = ai.isTrusted;
        this.isWaiting = ai.isWaiting;
        this.isSuspected = ai.isSuspected;
        this.isAiPathValuesSet = ai.isAiPathValuesSet;
        this.spriteToAnimate = ai.spriteToAnimate;
        this.nSpriteName = ai.nSpriteName;
        this.hlSpriteName = ai.hlSpriteName;
        this.prSpriteName = ai.prSpriteName;
        this.spriteNumber = ai.spriteNumber;
        this.idSpriteName = ai.idSpriteName;
        this.dangerResistance = ai.dangerResistance;
        this.aiId = ai.aiId;
        this.aiName = ai.aiName;
        this.aiSurname = ai.aiSurname;
        this.aiJob = ai.aiJob;
        this.aiGender = ai.aiGender;
        this.path = ai.path;
        this.pathUpdated = ai.pathUpdated;
        this.pathfinderChanged = ai.pathfinderChanged;
        this.wrongDestinationCounter = ai.wrongDestinationCounter;
        this.radiusBase = ai.radiusBase;
        this.onClickAi = ai.onClickAi;
        this.aiStartCellPos = ai.aiStartCellPos;
        this.aiCellPos = ai.aiCellPos;
        this.aiDestCellPos = ai.aiDestCellPos;
        this.aiSpeed = ai.aiSpeed;
        this.aiObjective = ai.aiObjective;
        this.timer = ai.timer;
        //this.timeEvent = ai.timeEvent;
    }

    public AiController DeserializeAiController(AiController ai)
    {
        ai.offsetDraw = this.offsetDraw;
        ai.scaleDraw = this.scaleDraw;
        ai.isAttacker = this.isAttacker;
        ai.destroy = this.destroy;
        ai.idChecked = this.idChecked;
        ai.idScanned = this.idScanned;
        ai.isTrusted = this.isTrusted;
        ai.isWaiting = this.isWaiting;
        ai.isSuspected = this.isSuspected;
        ai.isAiPathValuesSet = this.isAiPathValuesSet;
        ai.spriteToAnimate = this.spriteToAnimate;
        ai.nSpriteName = this.nSpriteName;
        ai.hlSpriteName = this.hlSpriteName;
        ai.prSpriteName = this.prSpriteName;
        ai.spriteNumber = this.spriteNumber;
        ai.idSpriteName = this.idSpriteName;
        ai.dangerResistance = this.dangerResistance;
        ai.aiId = this.aiId;
        ai.aiName = this.aiName;
        ai.aiSurname = this.aiSurname;
        ai.aiJob = this.aiJob;
        ai.aiGender = this.aiGender;
        ai.path = this.path;
        ai.pathUpdated = this.pathUpdated;
        ai.pathfinderChanged = this.pathfinderChanged;
        ai.wrongDestinationCounter = this.wrongDestinationCounter;
        ai.radiusBase = this.radiusBase;
        ai.onClickAi = this.onClickAi;
        ai.aiStartCellPos = this.aiStartCellPos;
        ai.aiCellPos = this.aiCellPos;
        ai.aiDestCellPos = this.aiDestCellPos;
        ai.aiSpeed = this.aiSpeed;
        ai.aiObjective = this.aiObjective;
        ai.timer = this.timer;
        //ai.timeEvent = this.timeEvent;

        return ai;
    }

    public override string ToString()
    {
        return $"OffsetDraw: {offsetDraw}, ScaleDraw: {scaleDraw}, IsAttacker: {isAttacker}, Destroy: {destroy}, IdChecked: {idChecked}, IdScanned: {idScanned}, " +
               $"IsTrusted: {isTrusted}, IsWaiting: {isWaiting}, IsSuspected: {isSuspected}, IsAiPathValuesSet: {isAiPathValuesSet}, SpriteToAnimate: {spriteToAnimate}, " +
               $"NSpriteName: {nSpriteName}, HlSpriteName: {hlSpriteName}, PrSpriteName: {prSpriteName}, SpriteNumber: {spriteNumber}, IdSpriteName: {idSpriteName}, " +
               $"DangerResistance: {dangerResistance}, AiId: {aiId}, AiName: {aiName}, AiSurname: {aiSurname}, AiJob: {aiJob}, AiGender: {aiGender}, Path: {path}, " +
               $"PathUpdated: {pathUpdated}, PathfinderChanged: {pathfinderChanged}, WrongDestinationCounter: {wrongDestinationCounter}, RadiusBase: {radiusBase}, " +
               $"OnClickAi: {onClickAi}, AiStartCellPos: {aiStartCellPos}, AiDestCellPos: {aiDestCellPos}, AiSpeed: {aiSpeed}, AiObjective: {aiObjective}, Timer: {timer}";
    }

    public AiController GetAiController()
    {
        return GameObject.Find(StringDb.aiPrefabName + this.aiId).GetComponent<AiController>(); ;
    }
}
