using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayPanelManager : MonoBehaviour
{
    private Button slot1Btn;
    private Button slot2Btn;
    private Button slot3Btn;

    private Image slot1Img;
    private Image slot2Img;
    private Image slot3Img;

    private TextMeshProUGUI slot1Info;
    private TextMeshProUGUI slot2Info;
    private TextMeshProUGUI slot3Info;

    private void OnEnable()
    {
        //get all the button and set the listener
        //if slotN is free=true --> play new game
        //else load slotN

        slot1Btn = GameObject.Find("Play0").GetComponent<Button>();
        slot2Btn = GameObject.Find("Play1").GetComponent<Button>();
        slot3Btn = GameObject.Find("Play2").GetComponent<Button>();

        slot1Img = GameObject.Find("Screen0").GetComponent<Image>();
        slot2Img = GameObject.Find("Screen1").GetComponent<Image>();
        slot3Img = GameObject.Find("Screen2").GetComponent<Image>();

        slot1Info = GameObject.Find("Info0").GetComponent<TextMeshProUGUI>();
        slot2Info = GameObject.Find("Info1").GetComponent<TextMeshProUGUI>();
        slot3Info = GameObject.Find("Info2").GetComponent<TextMeshProUGUI>();

        List<Button> slotBtnList = new List<Button>()
        {
            slot1Btn,
            slot2Btn,
            slot3Btn
        };

        List<Image> slotImageList = new List<Image>()
        {
            slot1Img,
            slot2Img,
            slot3Img
        };

        List<TextMeshProUGUI> slotInfoList = new List<TextMeshProUGUI>()
        {
            slot1Info,
            slot2Info,
            slot3Info
        };

        foreach(Button slotCurrentBtn in slotBtnList)
        {
            int index = slotBtnList.IndexOf(slotCurrentBtn);
            string slotFile = StringDb.slotName + index;
            string slotPath = Path.Combine(StringDb.slotFolderPath, slotFile);
            if (File.Exists(slotPath))
            {
                slotCurrentBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Carica";
                slotCurrentBtn.onClick.RemoveAllListeners();
                slotCurrentBtn.onClick.AddListener(delegate
                {
                    //Debug.Log(index);
                    //TODO FIX
                    //GameData.slotIndex = index;
                    ClassDb.sceneLoader.StartLoadByIndex(StringDb.level1SceneIndex);
                });
                byte[] data = File.ReadAllBytes(string.Concat(slotPath, StringDb.imageSlotExt));
                Texture2D texture = new Texture2D(300, 300, TextureFormat.ARGB32, false);
                texture.LoadImage(data);
                Sprite sprite = Sprite.Create(
                    texture, 
                    new Rect(0.0f, 0.0f, texture.width, texture.height), 
                    new Vector2(0.5f, 0.5f), 
                    63);
                slotImageList[index].sprite = sprite;
                slotImageList[index].preserveAspect = true;
                slotInfoList[index].text = File.GetLastWriteTime(slotPath).ToString("dd/MM/yyyy HH:mm");
            }
            else
            {
                slotCurrentBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Gioca";
                slotCurrentBtn.onClick.RemoveAllListeners();
                slotCurrentBtn.onClick.AddListener(delegate
                {
                    //Debug.Log(index);
                    //TODO FIX
                    //GameData.slotIndex = index;
                    ClassDb.sceneLoader.StartLoadByIndex(StringDb.level1SceneIndex);
                });
                slotImageList[index].sprite = Resources.Load<Sprite>("LoadPicture/EmptySlotImage");
                slotImageList[index].preserveAspect = true;
                slotInfoList[index].text = "DATI NON DISPONIBILI";
            }
        }
    }

}
