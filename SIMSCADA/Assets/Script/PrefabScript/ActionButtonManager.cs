using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonManager : MonoBehaviour
{
    public static bool buttonEnabled;


    public List<Button> buttons;

    private void Awake()
    {
        buttonEnabled = false;
        GetButtons();
    }

    private void OnEnable()
    {
        buttonEnabled = true;

    }

    private void OnDisable()
    {
        ResetActiveButton();
        ResetButtonOnClick();

        buttonEnabled = false;

    }

    public void GetButtons()
    {
        buttons = GetComponentsInChildren<Button>().ToList();
    }

    private void ResetActiveButton()
    {
        foreach (Button button in buttons)
        {
            button.GetComponentInChildren<CanvasGroup>().alpha = 1;
            button.GetComponentInChildren<CanvasGroup>().interactable = true;
            button.GetComponentInChildren<CanvasGroup>().blocksRaycasts = true;
        }
    }

    public void ResetButtonOnClick()
    {
        foreach (Button button in buttons)
        {
            button.onClick.RemoveAllListeners();
        }
    }

    public List<Button> GetActiveCanvasGroup(int buttonOnScene)
    {
        //according to the number of button needed on scene
        //it will be changed the corresponding values of canvas group to hide or display the buttons
        //the button requested will be added to the return list in order to set listener, text and so on

        List<Button> activeButtons = new List<Button>();

        switch (buttonOnScene)
        {
            case 0:
                break;
            case 1:
                activeButtons.Add(buttons[1]);
                break;
            case 2:
                activeButtons.Add(buttons[1]);
                activeButtons.Add(buttons[6]);
                break;
            case 3:
                activeButtons.Add(buttons[1]);
                activeButtons.Add(buttons[5]);
                activeButtons.Add(buttons[7]);
                break;
            case 4:
                activeButtons.Add(buttons[0]);
                activeButtons.Add(buttons[2]);
                activeButtons.Add(buttons[5]);
                activeButtons.Add(buttons[7]);
                break;
            case 5:
                activeButtons.Add(buttons[1]);
                activeButtons.Add(buttons[3]);
                activeButtons.Add(buttons[4]);
                activeButtons.Add(buttons[5]);
                activeButtons.Add(buttons[7]);
                break;
            case 6:
                activeButtons.Add(buttons[0]);
                activeButtons.Add(buttons[2]);
                activeButtons.Add(buttons[3]);
                activeButtons.Add(buttons[4]);
                activeButtons.Add(buttons[5]);
                activeButtons.Add(buttons[7]);
                break;
            case 7:
                activeButtons.Add(buttons[0]);
                activeButtons.Add(buttons[1]);
                activeButtons.Add(buttons[2]);
                activeButtons.Add(buttons[3]);
                activeButtons.Add(buttons[4]);
                activeButtons.Add(buttons[5]);
                activeButtons.Add(buttons[7]);
                break;
            case 8:
                activeButtons.Add(buttons[0]);
                activeButtons.Add(buttons[1]);
                activeButtons.Add(buttons[2]);
                activeButtons.Add(buttons[3]);
                activeButtons.Add(buttons[4]);
                activeButtons.Add(buttons[5]);
                activeButtons.Add(buttons[6]);
                activeButtons.Add(buttons[7]);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        foreach (Button button in buttons)
        {
            if (activeButtons.Contains(button)) continue;
            button.GetComponentInChildren<CanvasGroup>().alpha = 0;
            button.GetComponentInChildren<CanvasGroup>().interactable = false;
            button.GetComponentInChildren<CanvasGroup>().blocksRaycasts = false;
        }

        return activeButtons;
    }
}
