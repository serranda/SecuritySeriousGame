using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SecurityListener : MonoBehaviour, IObjectListener
{
    private static Canvas securityScreen;
    private InteractiveSprite interactiveSprite;

    private ILevelManager manager;


    private void Start()
    {
        manager = SetLevelManager();

        interactiveSprite = GetComponent<InteractiveSprite>();
    }

    private ILevelManager SetLevelManager()
    {
        ILevelManager iManager;
        if (SceneManager.GetActiveScene().buildIndex == StaticDb.level1SceneIndex)
            iManager = FindObjectOfType<Level1Manager>();
        else
            iManager = FindObjectOfType<Level2Manager>();

        return iManager;
    }

    public void SetListeners()
    {
        List<Button> buttons = interactiveSprite.actionButtonManager.GetActiveCanvasGroup(1);

        buttons[0].GetComponentInChildren<TextMeshProUGUI>().text = "Imposta accesso";
        buttons[0].onClick.RemoveAllListeners();
        buttons[0].onClick.AddListener(delegate
        {
            ToggleSecurityScreen();
            interactiveSprite.ToggleMenu();
        });

        foreach (Button button in buttons)
        {
            button.interactable = true;
        }
    }

    public void ToggleSecurityScreen()
    {
        if (manager.GetGameData().securityScreenEnabled)
        {
            ClassDb.prefabManager.ReturnPrefab(securityScreen.gameObject, PrefabManager.securityIndex);
            manager.GetGameData().securityScreenEnabled = false;

            //WRITE LOG
            ClassDb.logManager.StartWritePlayerLogRoutine(StaticDb.player, StaticDb.logEvent.UserEvent, "SECURITY SCREEN CLOSED");
        }
        else
        {
            securityScreen = ClassDb.prefabManager.GetPrefab(ClassDb.prefabManager.prefabSecurityScreen.gameObject, PrefabManager.securityIndex).GetComponent<Canvas>();
            manager.GetGameData().securityScreenEnabled = true;

            //WRITE LOG
            ClassDb.logManager.StartWritePlayerLogRoutine(StaticDb.player, StaticDb.logEvent.UserEvent, "SECURITY SCREEN OPENED");
        }
    }
}
