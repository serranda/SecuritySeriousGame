using UnityEngine;

public class TutorialDialogBoxMessage
{
    public string head;
    public string bodyPath;
    public string backBtn;
    public string nextBtn;

    public string body =>
        string.IsNullOrEmpty(bodyPath) 
            ? string.Empty 
            : Resources.Load<TextAsset>(bodyPath).text;

    public override string ToString() => $"{nameof(head)}: {head}, {nameof(bodyPath)}: {bodyPath}, {nameof(backBtn)}: {backBtn}, {nameof(nextBtn)}: {nextBtn}, {nameof(body)}: {body}";

    public DialogBoxMessage ToDialogBoxMessage()
    {
        DialogBoxMessage dialogBoxMessage = new DialogBoxMessage
        {
            head = this.head,
            bodyPath = this.bodyPath,
            backBtn = this.backBtn,
            nextBtn = this.nextBtn
        };


        return dialogBoxMessage;

    }

}
