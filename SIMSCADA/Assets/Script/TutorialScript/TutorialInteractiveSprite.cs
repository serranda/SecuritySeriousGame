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

public class TutorialInteractiveSprite : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public static bool onSprite;

    private SpriteRenderer interactiveSprite;

    private string realName;

    public Canvas actionMenu;

    public ActionButtonManager actionButtonManager;

    private BoxCollider2D boxCollider2D;

    private int spriteMaxIndex;

    public bool hasMenu;

    [SerializeField] private TutorialManager tutorialManager;

    // Start is called before the first frame update
    private void Start()
    {
        interactiveSprite = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();

        //get the real name fo the sprite without the index. necessary in order to swap the sprite
        int pos = interactiveSprite.sprite.name.IndexOf("_", StringComparison.Ordinal);
        realName = interactiveSprite.sprite.name.Substring(0, pos);

        spriteMaxIndex = Resources.LoadAll<Sprite>(Path.Combine(StaticDb.rscIntSpriteFolder, realName)).Length - 1;

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
        tutorialManager.tutorialGameData.pressedSprite = gameObject.name;
    }

    private void SetSprite(int index)
    {
        //set the sprite stored in the related folder located within the "Resources" folder
        interactiveSprite.sprite = Resources.LoadAll<Sprite>(Path.Combine(StaticDb.rscIntSpriteFolder, realName))[index];
    }

    public void ToggleMenu()
    {
        if (tutorialManager.tutorialGameData.buttonEnabled && gameObject.name == tutorialManager.tutorialGameData.pressedSprite)
        {
            ClassDb.prefabManager.ReturnPrefab(actionMenu.gameObject, PrefabManager.actionIndex);

            hasMenu = false;
        }
        else
        {
            //get the canvas for the displaying the option menu. only one action menu at time will be displayed
            if (GameObject.Find(StaticDb.actionMenuName))
            {
                actionMenu = GameObject.Find(StaticDb.actionMenuName).GetComponent<Canvas>();
                ClassDb.prefabManager.ReturnPrefab(actionMenu.gameObject, PrefabManager.actionIndex);
            }

            actionMenu = ClassDb.prefabManager.GetPrefab(ClassDb.prefabManager.prefabActionMenu.gameObject, PrefabManager.actionIndex)
                .GetComponent<Canvas>();

            Transform actionMenuTransform;
            (actionMenuTransform = actionMenu.transform).SetParent(interactiveSprite.transform);
            actionMenuTransform.localPosition = Vector3.zero;

            //set the button listener
            actionButtonManager = GameObject.Find(StaticDb.actionMenuName).GetComponent<ActionButtonManager>();
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
            case StaticDb.telephoneTag:
                TutorialTelephoneListener telephoneListener = GetComponent<TutorialTelephoneListener>();
                telephoneListener.SetTelephoneListeners();
                break;
            case StaticDb.securityCheckTag:
                TutorialSecurityListener securityListener = GetComponent<TutorialSecurityListener>();
                securityListener.SetSecurityListeners();
                break;
            case StaticDb.roomPcTag:
                TutorialRoomPcListener roomPcListener = GetComponent<TutorialRoomPcListener>();
                roomPcListener.SetComputerListeners();
                break;
            case StaticDb.serverPcTag:
                TutorialServerPcListener serverPcListener = GetComponent<TutorialServerPcListener>();
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
        Debug.Log(tutorialManager + " manager");
        Debug.Log(tutorialManager.tutorialGameData + " data");

        int limit;
        switch (gameObject.tag)
        {
            case StaticDb.telephoneTag:
                limit = tutorialManager.tutorialGameData.telephoneAmount;
                break;
            case StaticDb.securityCheckTag:
                limit = tutorialManager.tutorialGameData.securityCheckAmount;
                break;
            case StaticDb.roomPcTag:
                limit = tutorialManager.tutorialGameData.pcAmount;
                break;
            case StaticDb.serverPcTag:
                limit = tutorialManager.tutorialGameData.serverAmount;
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
