using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FullScreenWebGL : MonoBehaviour, IPointerDownHandler
{
    private Button button;

    private void OnEnable()
    {
        button = GetComponent<Button>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        bool isOn = GetButtonState();
        Screen.fullScreen = isOn;
        SettingsManager.gameSettings.fullScreen = isOn;

        Debug.Log(isOn);
    }

    private bool GetButtonState()
    {
        //CHECK WHICH SPRITE IS CURRENTLY DISPLAYED; SET BOOL AND SWITCH SPRITE

        if (button.image.sprite.name.Contains(StringDb.fsButtonOff))
        {
            Sprite[] sprites = Resources.LoadAll<Sprite>(Path.Combine(StringDb.fsButtonFolder, StringDb.fsButtonOn));
            button.image.sprite = sprites[0];

            SpriteState buttonSpriteState = new SpriteState
            {
                highlightedSprite = sprites[1],
                pressedSprite = sprites[2],
                disabledSprite = sprites[3]
            };

            button.spriteState = buttonSpriteState;

            return true;
        }
        else
        {
            Sprite[] sprites = Resources.LoadAll<Sprite>(Path.Combine(StringDb.fsButtonFolder, StringDb.fsButtonOff));
            button.image.sprite = sprites[0];

            SpriteState buttonSpriteState = new SpriteState
            {
                highlightedSprite = sprites[1],
                pressedSprite = sprites[2],
                disabledSprite = sprites[3]
            };

            button.spriteState = buttonSpriteState;

            return false;
        }

    }
}
