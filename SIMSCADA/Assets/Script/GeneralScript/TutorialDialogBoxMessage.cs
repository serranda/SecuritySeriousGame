using UnityEngine;

public class TutorialDialogBoxMessage
{
    public string head;
    public string bodyPath;
    public string nextBtn;

    public string body
    {
        get
        {
            return string.IsNullOrEmpty(bodyPath) 
                ? string.Empty 
                : Resources.Load<TextAsset>("TutorialMessagesBody/" + bodyPath).text;
        }

    }


    public override string ToString()
    {
        return string.Format("Head: {0}, BodyPath: {1}, NextBtn: {2}, Body: {3}",
            head, bodyPath, nextBtn, body);
    }

}
