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
        //yield return new WaitWhile(() => DialogBoxManager.dialogEnabled);

        Canvas dialog = ClassDb.dialogBoxManager.OpenDialog();

        LoginBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StaticDb.completeField));

        dialog.GetComponent<DialogBoxManager>().SetDialog(
            message.head,
            message.body,
            message.backBtn,
            message.nextBtn,
            dialog
        );

        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnNext.onClick.RemoveAllListeners();
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnNext.gameObject.SetActive(true);
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnNext.onClick.AddListener(delegate
        {
            ClassDb.dialogBoxManager.CloseDialog(dialog);
        });

        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnBack.onClick.RemoveAllListeners();
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnBack.gameObject.SetActive(false);
    }

    public void StartExistingPlayer()
    {
        existingPlayerRoutine = ExistingPlayer();
        StartCoroutine(existingPlayerRoutine);
    }

    private IEnumerator ExistingPlayer()
    {
        yield return new WaitForSeconds(messageDelay);
        //yield return new WaitWhile(() => DialogBoxManager.dialogEnabled);

        Canvas dialog = ClassDb.dialogBoxManager.OpenDialog();

        LoginBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StaticDb.existingPlayer));

        dialog.GetComponent<DialogBoxManager>().SetDialog(
            message.head,
            message.body,
            message.backBtn,
            message.nextBtn,
            dialog
        );

        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnNext.onClick.RemoveAllListeners();
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnNext.gameObject.SetActive(true);
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnNext.onClick.AddListener(delegate
        {
            ClassDb.dialogBoxManager.CloseDialog(dialog);
        });

        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnBack.onClick.RemoveAllListeners();
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnBack.gameObject.SetActive(false);
    }

    public void StartPlayerNotRegistered()
    {
        playerNotRegisteredRoutine = PlayerNotRegistered();
        StartCoroutine(playerNotRegisteredRoutine);
    }

    private IEnumerator PlayerNotRegistered()
    {
        yield return new WaitForSeconds(messageDelay);
        //yield return new WaitWhile(() => DialogBoxManager.dialogEnabled);

        Canvas dialog = ClassDb.dialogBoxManager.OpenDialog();

        LoginBoxMessage message = MessageFromJson(Resources.Load<TextAsset>(StaticDb.playerNotRegistered));

        dialog.GetComponent<DialogBoxManager>().SetDialog(
            message.head,
            message.body,
            message.backBtn,
            message.nextBtn, 
            dialog
        );

        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnNext.onClick.RemoveAllListeners();
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnNext.gameObject.SetActive(true);
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnNext.onClick.AddListener(delegate
        {
            ClassDb.dialogBoxManager.CloseDialog(dialog);
        });

        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnBack.onClick.RemoveAllListeners();
        dialog.GetComponent<DialogBoxManager>().dialogBoxBtnBack.gameObject.SetActive(false);
    }
}
