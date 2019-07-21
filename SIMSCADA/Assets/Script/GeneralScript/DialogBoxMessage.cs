using UnityEngine;

public class DialogBoxMessage
{
    public string head;
    public string bodyPath;
    public string backBtn;
    public string nextBtn;

    public string body
    {
        get
        {
            return string.IsNullOrEmpty(bodyPath) 
                ? string.Empty 
                : Resources.Load<TextAsset>("MessagesBody/" + bodyPath + "_IT").text;
        }

    }


    public override string ToString()
    {
        return string.Format("Head: {0}, BodyPath: {1}, BackBtn: {2}, NextBtn: {3}, Body: {4}",
            head, bodyPath, backBtn, nextBtn, body);
    }

}
