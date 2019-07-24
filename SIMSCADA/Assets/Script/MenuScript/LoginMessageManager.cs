using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginMessageManager : MonoBehaviour
{
    private IEnumerator completeAllFieldRoutine;
    private IEnumerator existingPlayerRoutine;
    private IEnumerator playerNotRegisteredRoutine;

    private float messageDelay = 0f;

    public LoginBoxMessage MessageFromJson(TextAsset jsonFile)
    {
        LoginBoxMessage boxMessage = JsonUtility.FromJson<LoginBoxMessage>(jsonFile.text);

        Debug.Log(boxMessage.ToString());

        return boxMessage;
    }

    public void StartCompleteAllField()
    {
        completeAllFieldRoutine = CompleteAllField();
        StartCoroutine(completeAllFieldRoutine);
    }

    private IEnumerator CompleteAllField()
    {
        yield return new WaitForSeconds(messageDelay);
        yield return new WaitWhile(() => DialogBoxManager.dialogEnabled);

        ClassDb.dialogBoxManager.ToggleDialogBox();

        LoginBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StringDb.completeField));

        ClassDb.dialogBoxManager.SetDialog(
            message.head,
            message.body,
            message.backBtn,
            message.nextBtn
        );

        DialogBoxManager.dialogBoxBtnNext.onClick.RemoveAllListeners();
        DialogBoxManager.dialogBoxBtnNext.gameObject.SetActive(true);
        DialogBoxManager.dialogBoxBtnNext.onClick.AddListener(delegate
        {
            ClassDb.dialogBoxManager.ToggleDialogBox();
        });

        DialogBoxManager.dialogBoxBtnBack.onClick.RemoveAllListeners();
        DialogBoxManager.dialogBoxBtnBack.gameObject.SetActive(false);
    }

    public void StartExistingPlayer()
    {
        existingPlayerRoutine = ExistingPlayer();
        StartCoroutine(existingPlayerRoutine);
    }

    private IEnumerator ExistingPlayer()
    {
        yield return new WaitForSeconds(messageDelay);
        yield return new WaitWhile(() => DialogBoxManager.dialogEnabled);

        ClassDb.dialogBoxManager.ToggleDialogBox();

        LoginBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StringDb.existingPlayer));

        ClassDb.dialogBoxManager.SetDialog(
            message.head,
            message.body,
            message.backBtn,
            message.nextBtn
        );

        DialogBoxManager.dialogBoxBtnNext.onClick.RemoveAllListeners();
        DialogBoxManager.dialogBoxBtnNext.gameObject.SetActive(true);
        DialogBoxManager.dialogBoxBtnNext.onClick.AddListener(delegate
        {
            ClassDb.dialogBoxManager.ToggleDialogBox();
        });

        DialogBoxManager.dialogBoxBtnBack.onClick.RemoveAllListeners();
        DialogBoxManager.dialogBoxBtnBack.gameObject.SetActive(false);
    }

    public void StartPlayerNotRegistered()
    {
        playerNotRegisteredRoutine = PlayerNotRegistered();
        StartCoroutine(playerNotRegisteredRoutine);
    }

    private IEnumerator PlayerNotRegistered()
    {
        yield return new WaitForSeconds(messageDelay);
        yield return new WaitWhile(() => DialogBoxManager.dialogEnabled);

        ClassDb.dialogBoxManager.ToggleDialogBox();

        LoginBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StringDb.playerNotRegistered));

        ClassDb.dialogBoxManager.SetDialog(
            message.head,
            message.body,
            message.backBtn,
            message.nextBtn
        );

        DialogBoxManager.dialogBoxBtnNext.onClick.RemoveAllListeners();
        DialogBoxManager.dialogBoxBtnNext.gameObject.SetActive(true);
        DialogBoxManager.dialogBoxBtnNext.onClick.AddListener(delegate
        {
            ClassDb.dialogBoxManager.ToggleDialogBox();
        });

        DialogBoxManager.dialogBoxBtnBack.onClick.RemoveAllListeners();
        DialogBoxManager.dialogBoxBtnBack.gameObject.SetActive(false);
    }
}
