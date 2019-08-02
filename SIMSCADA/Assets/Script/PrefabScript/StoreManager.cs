using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    private Sprite[] sprites;

    private ILevelManager manager;

    private void OnEnable()
    {
        manager = SetLevelManager();

        sprites = Resources.LoadAll<Sprite>("ItemStoreSprite");

        roomPcListener = FindObjectOfType<RoomPcListener>();
        tutorialRoomPcListener = FindObjectOfType<TutorialRoomPcListener>();

        scrollRect = gameObject.GetComponentInChildren<ScrollRect>();

        itemAsset = new List<TextAsset>();
        itemAsset = Resources.LoadAll<TextAsset>("ItemStore").ToList();
        content = scrollRect.content;

        content.sizeDelta = new Vector2(content.sizeDelta.x, 300f * (itemAsset.Count-1));  

        itemList = manager.GetGameData().itemStoreList;

        itemDescription = GameObject.Find("InfoTxt").GetComponent<TextMeshProUGUI>();

        defaultItemStore = new ItemStore();
        itemStoreSelected = defaultItemStore;

        backBtn = GameObject.Find("BackBtn").GetComponent<Button>();
        backBtn.onClick.RemoveAllListeners();
        backBtn.onClick.AddListener(delegate
        {
            if (manager.GetGameData().storeEnabled)
            {
                roomPcListener.ToggleStoreScreen();
            }

            ////TODO FIX TUTORIAL
            //if (TutorialRoomPcListener.storeEnabled)
            //{
            //    tutorialRoomPcListener.ToggleStoreScreen();
            //}
        });

        //POPULATE ITEMLIST
        if (itemList.Count == 0)
        {
            foreach (TextAsset textAsset in itemAsset)
            {
                ItemStore itemStore = ItemStoreFromJson(textAsset);
                itemList.Add(itemStore);
            }
        }

        //SORT ITEMLIST
        itemList.Sort(ItemStore.NameComparer);

        foreach (ItemStore item in itemList)
        {
            GameObject itemOnList = ClassDb.prefabManager.GetPrefab(ClassDb.prefabManager.prefabStoreItem.gameObject, PrefabManager.storeItemIndex);
            item.itemObject = itemOnList;
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
                if (itemStoreSelected != defaultItemStore)
                {
                    itemStoreSelected.itemObject.GetComponent<Button>().image.sprite = sprites[0];
                }
                itemStoreSelected = item1;
                item1.itemObject.GetComponent<Button>().image.sprite = sprites[sprites.Length-1];
                SetDescription(itemStoreSelected);
            });
            CheckItemLevel(item);
        }

        purchaseButton.onClick.RemoveAllListeners();
        purchaseButton.onClick.AddListener(delegate
            {
                ClassDb.levelMessageManager.StartConfirmPurchase(itemStoreSelected);
                itemList.Remove(itemStoreSelected);
            });

        //disable interact with button until tutorial is finished
        if (SceneManager.GetActiveScene().buildIndex == StringDb.tutorialSceneIndex)
        {
            foreach (ItemStore item in itemList)
            {
                item.itemObject.GetComponent<Button>().interactable = false;
            }
        }
        else
        {
            foreach (ItemStore item in itemList)
            {
                CheckItemLevel(item);
            }
        }
    }

    private void OnDisable()
    {
        foreach (Button buttonToDestroy in scrollRect.content.GetComponentsInChildren<Button>())
        {
            ClassDb.prefabManager.ReturnPrefab(buttonToDestroy.gameObject, PrefabManager.storeItemIndex);
        }

        manager.GetGameData().itemStoreList = itemList;
    }

    private void Update()
    {
        purchaseButton.interactable = itemStoreSelected != defaultItemStore;
    }

    private ILevelManager SetLevelManager()
    {
        ILevelManager iManager;
        if (SceneManager.GetActiveScene().buildIndex == StringDb.level1SceneIndex)
            iManager = FindObjectOfType<Level1Manager>();
        else
            iManager = FindObjectOfType<Level2Manager>();

        return iManager;
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
        //reset sprite
        itemStore.itemObject.GetComponent<Button>().image.sprite = sprites[0];

        manager.GetGameData().money-= itemStore.price;
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
        Debug.Log("ITEM NOT INTERACTABLE");
        //itemStore.currentLevel--;
    }

    private void ApplyItemEffect(ItemStore itemStore)
    {
        switch (itemStore.effect)
        {
            case 0:
                //PURCHASED FIREWALL UPGRADE; INCREASING FIREWALL SUCCESS RATE
                manager.GetGameData().firewallSuccessRate += 10;
                break;
            case 1:
                //PURCHASED IDS UPGRADE; INCREASING REMOTE IDS SUCCESS RATE
                manager.GetGameData().remoteIdsCheckRate -= 2.0f;
                manager.GetGameData().remoteIdsSuccessRate += 15;
                manager.GetGameData().defenseDos += 5;
                break;
            case 2:
                //PURCHASED SERVER UPGRADE; NEW SERVER AVAILABLE
                manager.GetGameData().serverAmount += 1;
                List<GameObject> servers = GameObject.FindGameObjectsWithTag(StringDb.serverPcTag).ToList();
                foreach (GameObject server in servers)
                {
                    server.GetComponent<InteractiveSprite>().CheckOperativeItem();
                }
                break;
            case 3:
                //PURCHASED PC UPGRADE; NEW PC AVAILABLE
                manager.GetGameData().pcAmount += 1;
                List<GameObject> pcs = GameObject.FindGameObjectsWithTag(StringDb.roomPcTag).ToList();
                foreach (GameObject pc in pcs)
                {
                    pc.GetComponent<InteractiveSprite>().CheckOperativeItem();
                }
                break;
            case 4:
                //PURCHASED TELEPHONE UPGRADE; NEW TELEPHONE AVAILABLE
                manager.GetGameData().telephoneAmount += 1;
                List<GameObject> telephones = GameObject.FindGameObjectsWithTag(StringDb.telephoneTag).ToList();
                foreach (GameObject telephone in telephones)
                {
                    telephone.GetComponent<InteractiveSprite>().CheckOperativeItem();
                }
                break;
            case 5:
                //PURCHASED SERVER UPGRADE; DECREASING TIME FOR SERVER ACTIVITIES
                manager.GetGameData().serverRebootTime -= 5.0f;
                manager.GetGameData().serverScanTime -= 5.0f;
                manager.GetGameData().serverCheckCfgTime -= 5.0f;
                manager.GetGameData().serverIdsCleanTime -= 5.0f;
                manager.GetGameData().serverAntiMalwareTime -= 5.0f;
                break;
            case 6:
                //PURCHASED PC UPGRADE; DECREASING TIME FOR PC ACTIVITIES
                manager.GetGameData().pcRecapTime -= 5.0f;
                manager.GetGameData().pcPointOutTime -= 5.0f;
                break;
            case 7:
                //PURCHASED TELEPHONE UPGRADE; DECREASING TIME FOR TELEPHONE ACTIVITIES
                manager.GetGameData().telephoneCheckPlantTime -= 5.0f;
                manager.GetGameData().telephoneMoneyTime -= 2.0f;
                manager.GetGameData().telephoneMoneyCoolDown -= 0.3f;
                break;
            case 8:
                //PURCHASED ID CARD UPGRADE; DECREASING TIME FOR SHOW ID CARD
                manager.GetGameData().idCardTime -= 3.0f;
                break;
            case 9:
                //PURCHASED EMPLOYEE FORMATION COURSE; INCREASING TRUSTED EMPLOYEE
                int newTrustedEmployees = (int)(0.1 * manager.GetGameData().trustedEmployees);
                if (manager.GetGameData().trustedEmployees + newTrustedEmployees > manager.GetGameData().totalEmployees)
                {
                    //message to inform about too many trusted employees
                    ClassDb.levelMessageManager.StartNewTrustedEmployees();
                }
                else
                {
                    manager.GetGameData().trustedEmployees += newTrustedEmployees;
                }
                break;
            case 10:
                //PURCHASED PLANT UPGRADE; INCREASING PLANT RESISTANCE TO DAMAGE
                manager.GetGameData().defensePlantResistance += 20;
                manager.GetGameData().defenseStuxnet += 2;
                break;
            case 11:
                //PURCHASED IPS; INCREASING SUCCESS AGAINST DOS ATTACK
                manager.GetGameData().defenseDos += 15;
                break;
            case 12:
                //PURCHASED BLACKLIST SERVER; INCREASING SUCCESS AGAINST PHISHING MAIL DAN DRAGONFLY
                manager.GetGameData().defensePhishing += 20;
                manager.GetGameData().defenseDragonfly += 2;
                break;
            case 13:
                //PURCHASED ALERT SYSTEM FOR HMI; INCREASING SUCCESS AGAINST REPLAY ATTACK
                manager.GetGameData().defenseReplay += 20;
                break;
            case 14:
                //PURCHASED HMI UPGRADE; INCREASING SUCCESS AGAINST MITM ATTACK
                manager.GetGameData().defenseMitm += 20;
                break;
            case 15:
                //PURCHASED ANTIMALWARE UPGRADE; INCREASING SUCCESS AGAINST MALWARE ATTACK
                manager.GetGameData().defenseMalware += 20;
                break;
            case 16:
                //PURCHASED STUXNET DEFENSE; INCREASING SUCCESS AGAINST STUXNET ATTACK
                manager.GetGameData().defenseStuxnet += 20;
                break;
            case 17:
                //PURCHASED DRAGONFLY DEFENSE; INCREASING SUCCESS AGAINST DRAGONFLY ATTACK
                manager.GetGameData().defenseDragonfly += 20;
                break;
            case 18:
                //PURCHASED LOCAL IDS UPGRADE; DECREASING TIME FOR LOCAL IDS AND VALUES FOR INTERNAL SECURITY MANAGEMENT
                manager.GetGameData().localIdsCheckRate -= 2.0f;
                manager.GetGameData().localIdsWrongCounter -= 2;
                manager.GetGameData().defenseCreateRemote += 20;
                break;
            case 19:
                //PURCHASED UPGRADE TO ENABLE ABILITY TO POINT OUT LOCAL THREAT
                manager.GetGameData().pointOutPurchased = true;
                break;
            case 20:
                //PURCHASED UPGRADE IDS LOCAL; POINTS OUT ONLY THE REAL LOCAL THREAT, NOT THE FAKE ONE
                manager.GetGameData().localIdsUpgraded = true;
                break;
            case 21:
                //PURCHASED ID CARD UPGRADE; NOW IT SHOWS IF AN EMPLOYEE IS AN ATTACKER OR NOT
                manager.GetGameData().idCardUpgraded = true;
                break;
            case 22:
                //PURCHASED HIRING CAMPAIGN: INCREASE EMPLOYEES NUMBER
                int employeesHired = (int) (Random.Range(0.1f, 0.25f) * manager.GetGameData().totalEmployees);
                ClassDb.levelMessageManager.StartNewEmployeesHired(employeesHired);
                break;
            case 23:
                //PURCHASED RESEARCH UPGRADE: ENABLED MONTHLY REPORT AND THREAT TENDENCIES  
                manager.GetGameData().researchUpgrade = true;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        //TODO GET DATA ABOUT PURCHASE
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