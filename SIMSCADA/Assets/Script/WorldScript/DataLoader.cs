using System.Collections;
using System.Collections.Generic;
using System.IO;
using BayatGames.SaveGameFree;
using BayatGames.SaveGameFree.Serializers;
using UnityEngine;

public class DataLoader : MonoBehaviour
{
    private static string slotFile;
    private static string imageSlotFile;
    private static string slotPath;

    private PlayerController playerController;
    public void StartDataLoader()
    {
        //SaveGame.Serializer = new SaveGameXmlSerializer();
        ////Debug.Log(StringDb.slotIndex);
        //slotFile = string.Concat(StringDb.slotName, StringDb.slotIndex);
        //imageSlotFile = string.Concat(slotFile, StringDb.imageSlotExt);
        //slotPath = Path.Combine(StringDb.slotFolderPath, slotFile);
        //playerController = GameObject.Find(StringDb.playerPrefabName).GetComponent<PlayerController>();

        //CheckSave();
    }

    private void CheckSave()
    {
        //if (!File.Exists(slotPath)) return;
        //LoadOffice();
        //StringDb.firstLaunch = false;
    }

    public void SaveOffice()
    {
        //OfficeData officeData = new OfficeData
        //{
        //    playerWorldPos = playerController.rb2D.position,
        //    pathUpdated = playerController.pathUpdated,
        //    path = playerController.path,
        //    //destinationWorldPos = playerController.playerDestWorldPos,
        //    scadaEnabled = RoomPcListener.scadaEnabled,
        //    date = StringDb.date,
        //    reputation = StringDb.reputation,
        //    money = StringDb.money,
        //    successfulThreat = StringDb.successfulThreat,
        //    totalThreat = StringDb.totalThreat,
        //    trustedEmployees = StringDb.totalEmployees,
        //    totalEmployees = StringDb.totalEmployees,
        //    trustedEmpPerValues = StringDb.trustedEmpPerValues,
        //    totalEmpPerValues = StringDb.totalEmpPerValues
        //};


        //SaveGame.Save(slotPath, officeData, SaveGame.Serializer);
        //ScreenCapture.CaptureScreenshot(Path.Combine(StringDb.slotFolderPath, imageSlotFile));
    }

    private void LoadOffice()
    {
        ////Debug.Log(slotPath);
        //OfficeData officeData = SaveGame.Load(slotPath, new OfficeData(), SaveGame.Serializer);
        //playerController.rb2D.position = officeData.playerWorldPos;
        //playerController.pathUpdated = officeData.pathUpdated;
        //playerController.path = officeData.path;
        ////playerController.playerDestWorldPos = officeData.destinationWorldPos;
        //RoomPcListener.scadaEnabled = officeData.scadaEnabled;
        //StringDb.date = officeData.date;
        //StringDb.reputation = officeData.reputation;
        //StringDb.money = officeData.money;
        //StringDb.successfulThreat = officeData.successfulThreat;
        //StringDb.totalThreat = officeData.totalThreat;
        //StringDb.totalEmployees = officeData.trustedEmployees;
        //StringDb.totalEmployees = officeData.totalEmployees;
        //StringDb.trustedEmpPerValues = officeData.trustedEmpPerValues;
        //StringDb.totalEmpPerValues = officeData.totalEmpPerValues;

        //if (!RoomPcListener.scadaEnabled) return;

        //RoomPcListener roomPcListener = gameObject.AddComponent<RoomPcListener>();
        //roomPcListener.ToggleScadaScreen();


    }

}
