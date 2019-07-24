using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class StoreManager : MonoBehaviour
{
    public List<TextAsset> itemAsset;
    public List<ItemStore> itemList;

    private ScrollRect scrollRect;

    public Button itemButtonPrefab;
    public Button purchaseButton;
    private Button backBtn;

    private TextMeshProUGUI itemDescription;

    public ItemStore itemStoreSelected;

    private RectTransform content;

    private ItemStore defaultItemStore;

    private RoomPcListener roomPcListener;
    private TutorialRoomPcListener tutorialRoomPcListener;

    private void OnEnable()
    {
        roomPcListener = FindObjectOfType<RoomPcListener>();
        tutorialRoomPcListener = FindObjectOfType<TutorialRoomPcListener>();

        scrollRect = gameObject.GetComponentInChildren<ScrollRect>();

        itemAsset = new List<TextAsset>();
        itemAsset = Resources.LoadAll<TextAsset>("ItemStore").ToList();
        content = scrollRect.content;

        content.sizeDelta = new Vector2(content.sizeDelta.x, 300f * (itemAsset.Count-1));  

        itemList = GameData.itemStoreList;

        itemDescription = GameObject.Find("InfoTxt").GetComponent<TextMeshProUGUI>();

        defaultItemStore = new ItemStore();
        itemStoreSelected = defaultItemStore;

        backBtn = GameObject.Find("BackBtn").GetComponent<Button>();
        backBtn.onClick.RemoveAllListeners();
        backBtn.onClick.AddListener(delegate
        {
            if (RoomPcListener.storeEnabled)
            {
                roomPcListener.ToggleStoreScreen();
            }

            if (TutorialRoomPcListener.storeEnabled)
            {
                tutorialRoomPcListener.ToggleStoreScreen();
            }
        });


        if (itemList.Count == 0)
        {
            foreach (TextAsset textAsset in itemAsset)
            {
                ItemStore itemStore = ItemStoreFromJson(textAsset);
                itemList.Add(itemStore);
            }
        }

        foreach (ItemStore item in itemList)
        {
            GameObject itemOnList = ClassDb.prefabManager.GetPrefab(ClassDb.prefabManager.prefabStoreItem.gameObject, PrefabManager.storeItemIndex);
            item.itemObject = itemOnList;
            CheckItemLevel(item);
            itemOnList.transform.SetParent(content);
            itemOnList.transform.localScale = Vector3.one;
            itemOnList.name = "Item" + itemList.IndexOf(item);
            itemOnList.GetComponentsInChildren<TextMeshProUGUI>()[0].text = item.name;
            itemOnList.GetComponentsInChildren<TextMeshProUGUI>()[2].text = item.currentLevel.ToString();
            itemOnList.GetComponentsInChildren<TextMeshProUGUI>()[3].text = item.price.ToString();
            itemOnList.GetComponent<Button>().onClick.RemoveAllListeners();
            ItemStore item1 = item;
            itemOnList.GetComponent<Button>().onClick.AddListener(delegate
            {
                itemStoreSelected = item1;
                SetDescription(itemStoreSelected);
            });
        }

        purchaseButton.onClick.RemoveAllListeners();
        purchaseButton.onClick.AddListener(delegate
            {
                ClassDb.levelMessageManager.StartConfirmPurchase(itemStoreSelected);
                itemList.Remove(itemStoreSelected);
            });

        //disable interact with button until tutorial is finished
        if (TutorialManager.tutorialIsFinished)
        {
            foreach (ItemStore item in itemList)
            {
                item.itemObject.GetComponent<Button>().interactable = true;
            }
        }
        else
        {
            foreach (ItemStore item in itemList)
            {
                item.itemObject.GetComponent<Button>().interactable = false;
            }
        }
    }

    private void OnDisable()
    {
        foreach (Button buttonToDestroy in scrollRect.content.GetComponentsInChildren<Button>())
        {
            ClassDb.prefabManager.ReturnPrefab(buttonToDestroy.gameObject, PrefabManager.storeItemIndex);
        }

        GameData.itemStoreList = itemList;
    }

    private void Update()
    {
        purchaseButton.interactable = itemStoreSelected != defaultItemStore;
    }

    private ItemStore ItemStoreFromJson(TextAsset jsonFile)
    {
        ItemStore itemStore = JsonUtility.FromJson<ItemStore>(jsonFile.ToString());
        return itemStore;
    }

    private void SetDescription(ItemStore itemStore)
    {
        itemDescription.text = itemStore.descriptionBody;
        itemDescription.pageToDisplay = 1;
    }

    public void PurchaseItem(ItemStore itemStore)
    {
        GameData.money-= itemStore.price;
        SetItemLevel(itemStore);
        CheckItemLevel(itemStore);
        ApplyItemEffect(itemStore);
        UpdateItemList(itemStore);
        UpdateGui();

        itemStoreSelected = defaultItemStore;
        SetDescription(itemStoreSelected);
    }

    private void SetItemLevel(ItemStore itemStore)
    {
        itemStore.currentLevel++;

        if (itemStore.currentLevel < itemStore.finalLevel)
        {
            itemStore.price += itemStore.price / 2;
        }
    }

    private void CheckItemLevel(ItemStore itemStore)
    {
        if (itemStore.finalLevel == -1) return;
        if (itemStore.currentLevel < itemStore.finalLevel) return;
        itemStore.itemObject.GetComponent<Button>().interactable = false;
        itemStore.currentLevel--;
    }

    private void ApplyItemEffect(ItemStore itemStore)
    {
        switch (itemStore.effect)
        {
            case 0:
                //PURCHASED FIREWALL UPGRADE; INCREASING FIREWALL SUCCESS RATE
                GameData.firewallSuccessRate += 10;
                break;
            case 1:
                //PURCHASED IDS UPGRADE; INCREASING REMOTE IDS SUCCESS RATE
                GameData.remoteIdsCheckRate -= 2.0f;
                GameData.remoteIdsSuccessRate += 15;
                GameData.defenseDos += 5;
                break;
            case 2:
                //PURCHASED SERVER UPGRADE; NEW SERVER AVAILABLE
                GameData.serverAmount += 1;
                List<GameObject> servers = GameObject.FindGameObjectsWithTag(StringDb.serverPcTag).ToList();
                foreach (GameObject server in servers)
                {
                    server.GetComponent<InteractiveSprite>().CheckOperativeItem();
                }
                break;
            case 3:
                //PURCHASED PC UPGRADE; NEW PC AVAILABLE
                GameData.pcAmount += 1;
                List<GameObject> pcs = GameObject.FindGameObjectsWithTag(StringDb.roomPcTag).ToList();
                foreach (GameObject pc in pcs)
                {
                    pc.GetComponent<InteractiveSprite>().CheckOperativeItem();
                }
                break;
            case 4:
                //PURCHASED TELEPHONE UPGRADE; NEW TELEPHONE AVAILABLE
                GameData.telephoneAmount += 1;
                List<GameObject> telephones = GameObject.FindGameObjectsWithTag(StringDb.telephoneTag).ToList();
                foreach (GameObject telephone in telephones)
                {
                    telephone.GetComponent<InteractiveSprite>().CheckOperativeItem();
                }
                break;
            case 5:
                //PURCHASED SERVER UPGRADE; DECREASING TIME FOR SERVER ACTIVITIES
                GameData.serverRebootTime -= 5.0f;
                GameData.serverScanTime -= 5.0f;
                GameData.serverCheckCfgTime -= 5.0f;
                GameData.serverIdsCleanTime -= 5.0f;
                GameData.serverAntiMalwareTime -= 5.0f;
                break;
            case 6:
                //PURCHASED PC UPGRADE; DECREASING TIME FOR PC ACTIVITIES
                GameData.pcRecapTime -= 5.0f;
                GameData.pcPointOutTime -= 5.0f;
                break;
            case 7:
                //PURCHASED TELEPHONE UPGRADE; DECREASING TIME FOR TELEPHONE ACTIVITIES
                GameData.telephoneCheckPlantTime -= 5.0f;
                GameData.telephoneMoneyTime -= 2.0f;
                GameData.telephoneMoneyCoolDown -= 0.3f;
                break;
            case 8:
                //PURCHASED ID CARD UPGRADE; DECREASING TIME FOR SHOW ID CARD
                GameData.idCardTime -= 3.0f;
                break;
            case 9:
                //PURCHASED EMPLOYEE FORMATION COURSE; INCREASING TRUSTED EMPLOYEE
                int newTrustedEmployees = (int)(0.1 * GameData.trustedEmployees);
                if (GameData.trustedEmployees + newTrustedEmployees > GameData.totalEmployees)
                {
                    //message to inform about too many trusted employees
                    ClassDb.levelMessageManager.StartNewTrustedEmployees();
                }
                else
                {
                    GameData.trustedEmployees += newTrustedEmployees;
                }
                break;
            case 10:
                //PURCHASED PLANT UPGRADE; INCREASING PLANT RESISTANCE TO DAMAGE
                GameData.defensePlantResistance += 20;
                GameData.defenseStuxnet += 2;
                break;
            case 11:
                //PURCHASED IPS; INCREASING SUCCESS AGAINST DOS ATTACK
                GameData.defenseDos += 15;
                break;
            case 12:
                //PURCHASED BLACKLIST SERVER; INCREASING SUCCESS AGAINST PHISHING MAIL DAN DRAGONFLY
                GameData.defensePhishing += 20;
                GameData.defenseDragonfly += 2;
                break;
            case 13:
                //PURCHASED ALERT SYSTEM FOR HMI; INCREASING SUCCESS AGAINST REPLAY ATTACK
                GameData.defenseReplay += 20;
                break;
            case 14:
                //PURCHASED HMI UPGRADE; INCREASING SUCCESS AGAINST MITM ATTACK
                GameData.defenseMitm += 20;
                break;
            case 15:
                //PURCHASED ANTIMALWARE UPGRADE; INCREASING SUCCESS AGAINST MALWARE ATTACK
                GameData.defenseMalware += 20;
                break;
            case 16:
                //PURCHASED STUXNET DEFENSE; INCREASING SUCCESS AGAINST STUXNET ATTACK
                GameData.defenseStuxnet += 20;
                break;
            case 17:
                //PURCHASED DRAGONFLY DEFENSE; INCREASING SUCCESS AGAINST DRAGONFLY ATTACK
                GameData.defenseDragonfly += 20;
                break;
            case 18:
                //PURCHASED LOCAL IDS UPGRADE; DECREASING TIME FOR LOCAL IDS AND VALUES FOR INTERNAL SECURITY MANAGEMENT
                GameData.localIdsCheckRate -= 2.0f;
                GameData.localIdsWrongCounter -= 2;
                GameData.defenseCreateRemote += 20;
                break;
            case 19:
                //PURCHASED UPGRADE TO ENABLE ABILITY TO POINT OUT LOCAL THREAT
                GameData.pointOutPurchased = true;
                break;
            case 20:
                //PURCHASED UPGRADE IDS LOCAL; POINTS OUT ONLY THE REAL LOCAL THREAT, NOT THE FAKE ONE
                GameData.localIdsUpgraded = true;
                break;
            case 21:
                //PURCHASED ID CARD UPGRADE; NOW IT SHOWS IF AN EMPLOYEE IS AN ATTACKER OR NOT
                GameData.idCardUpgraded = true;
                break;
            case 22:
                //PURCHASED HIRING CAMPAIGN: INCREASE EMPLOYEES NUMBER
                int employeesHired = (int) (Random.Range(0.1f, 0.25f) * GameData.totalEmployees);
                ClassDb.levelMessageManager.StartNewEmployeesHired(employeesHired);
                break;
            case 23:
                //PURCHASED RESEARCH UPGRADE: ENABLED MONTHLY REPORT AND THREAT TENDENCIES  
                GameData.researchUpgrade = true;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        //ClassDb.dataCollector.GetUpgradeData(itemStore, DateTime.Now);

    }

    private void UpdateItemList(ItemStore itemStore)
    {
        itemList.Add(itemStore);
    }
    private void UpdateGui()
    {
        foreach (ItemStore item in itemList)
        {
            item.itemObject.GetComponentsInChildren<TextMeshProUGUI>()[0].text = item.name;
            item.itemObject.GetComponentsInChildren<TextMeshProUGUI>()[2].text = item.currentLevel.ToString();
            item.itemObject.GetComponentsInChildren<TextMeshProUGUI>()[3].text = item.price.ToString();
        }
    }
}