using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InteractiveSprite : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public static bool onSprite;

    private SpriteRenderer interactiveSprite;

    private string realName;

    public Canvas actionMenu;

    public ActionButtonManager actionButtonManager;

    private BoxCollider2D boxCollider2D;

    private int spriteMaxIndex;

    public bool hasMenu;

    private ILevelManager manager;


    // Start is called before the first frame update
    private void Start()
    {
        manager = SetLevelManager();

        interactiveSprite = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();

        //get the real name fo the sprite without the index. necessary in order to swap the sprite
        int pos = interactiveSprite.sprite.name.IndexOf("_", StringComparison.Ordinal);
        realName = interactiveSprite.sprite.name.Substring(0, pos);

        spriteMaxIndex = Resources.LoadAll<Sprite>(Path.Combine(StringDb.rscIntSpriteFolder, realName)).Length - 1;

        //SET OPERATIVE
        CheckOperativeItem();

        ////set the actual sprite with the normal one
        //SetSprite(0);

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //swap the actual sprite with the highlighted one
        SetSprite(1);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //swap the actual sprite with the pressed one
        SetSprite(2);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
       //prevent accidental trigger 
        if (eventData.pointerCurrentRaycast.gameObject != gameObject) return;

        //swap the actual sprite with the highlighted one
        onSprite = true;
        SetSprite(1);
        //Debug.Log("ENTER " + gameObject.name);

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (GetComponentInChildren<ProgressBarManager>()) return;

        //swap the actual sprite with the normal one
        onSprite = false;
        SetSprite(0);

        //Debug.Log("EXIT " + gameObject.name);


    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ToggleMenu();
        manager.GetGameData().pressedSprite = gameObject.name;
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


    private void SetSprite(int index)
    {
        //set the sprite stored in the related folder located within the "Resources" folder
        interactiveSprite.sprite = Resources.LoadAll<Sprite>(Path.Combine(StringDb.rscIntSpriteFolder, realName))[index];
    }

    public void ToggleMenu()
    {
        if (ActionButtonManager.buttonEnabled && gameObject.name == manager.GetGameData().pressedSprite)
        {
            ClassDb.prefabManager.ReturnPrefab(actionMenu.gameObject, PrefabManager.actionIndex);

            hasMenu = false;

        }
        else
        {
            //get the canvas for the displaying the option menu. only one action menu at time will be displayed
            if (GameObject.Find(StringDb.actionMenuName))
            {
                actionMenu = GameObject.Find(StringDb.actionMenuName).GetComponent<Canvas>();
                ClassDb.prefabManager.ReturnPrefab(actionMenu.gameObject, PrefabManager.actionIndex);
            }

            actionMenu = ClassDb.prefabManager.GetPrefab(ClassDb.prefabManager.prefabActionMenu.gameObject, PrefabManager.actionIndex)
                .GetComponent<Canvas>();

            Transform actionMenuTransform;
            (actionMenuTransform = actionMenu.transform).SetParent(interactiveSprite.transform);
            actionMenuTransform.localPosition = Vector3.zero;

            //set the button listener
            actionButtonManager = GameObject.Find(StringDb.actionMenuName).GetComponent<ActionButtonManager>();
            actionButtonManager.GetButtons();

            SetListener();

            hasMenu = true;

        }

    }

    private void SetListener()
    {
        //set all the listener for the function related to the button onClick
        switch (gameObject.tag)
        {
            case StringDb.telephoneTag:
                TelephoneListener telephoneListener = GetComponent<TelephoneListener>();
                telephoneListener.SetTelephoneListeners();
                break;
            case StringDb.securityCheckTag:
                SecurityListener securityListener = GetComponent<SecurityListener>();
                securityListener.SetSecurityListeners();
                break;
            case StringDb.roomPcTag:
                RoomPcListener roomPcListener = GetComponent<RoomPcListener>();
                roomPcListener.SetComputerListeners();
                break;
            case StringDb.serverPcTag:
                ServerPcListener serverPcListener = GetComponent<ServerPcListener>();
                serverPcListener.SetSeverPcListeners();
                break;
            default:
                actionButtonManager.ResetButtonOnClick();
                break;
        }
    }

    public void SetInteraction(bool colliderEnabled)
    {
        boxCollider2D.enabled = colliderEnabled;
        SetSprite((spriteMaxIndex - 1)  * (int)Convert.ToSingle(!colliderEnabled));
    }

    public void CheckOperativeItem()
    {
        int limit;
        switch (gameObject.tag)
        {
            case StringDb.telephoneTag:
                limit = manager.GetGameData().telephoneAmount;
                break;
            case StringDb.securityCheckTag:
                limit = manager.GetGameData().securityCheckAmount;
                break;
            case StringDb.roomPcTag:
                limit = manager.GetGameData().pcAmount;
                break;
            case StringDb.serverPcTag:
                limit = manager.GetGameData().serverAmount;
                break;
            default:
                limit = 1;
                break;
        }

        SetOperative(Convert.ToInt32(gameObject.name.Trim(gameObject.tag.ToCharArray())) <= limit);
    }

    private void SetOperative(bool colliderEnabled)
    {
        boxCollider2D.enabled = colliderEnabled;
        SetSprite(spriteMaxIndex * (int)Convert.ToSingle(!colliderEnabled));
    }

}
