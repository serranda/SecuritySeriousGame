using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SecurityListener : MonoBehaviour
{
    private static Canvas serverRoomScreen;
    public static bool serverRoomScreenEnabled;
    private InteractiveSprite interactiveSprite;

    private void Start()
    {
        interactiveSprite = GetComponent<InteractiveSprite>();
    }

    public void SetSecurityListeners()
    {

        List<Button> buttons = interactiveSprite.actionButtonManager.GetActiveCanvasGroup(1);

        buttons[0].GetComponentInChildren<TextMeshProUGUI>().text = "Imposta accesso";
        buttons[0].onClick.RemoveAllListeners();
        buttons[0].onClick.AddListener(delegate
        {
            ToggleServerRoomScreen();
            interactiveSprite.ToggleMenu();
        });

        foreach (Button button in buttons)
        {
            button.interactable = true;
        }
    }

    public void ToggleServerRoomScreen()
    {
        if (serverRoomScreenEnabled)
        {
            ClassDb.prefabManager.ReturnPrefab(serverRoomScreen.gameObject, PrefabManager.serverIndex);
            serverRoomScreenEnabled = false;
        }
        else
        {
            serverRoomScreen = ClassDb.prefabManager.GetPrefab(ClassDb.prefabManager.prefabServerScreen.gameObject, PrefabManager.serverIndex).GetComponent<Canvas>();
            serverRoomScreenEnabled = true;
        }
    }
}
