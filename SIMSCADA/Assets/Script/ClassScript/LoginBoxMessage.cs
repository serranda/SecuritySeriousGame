using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginBoxMessage
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
                : Resources.Load<TextAsset>("LoginMessagesBody/" + bodyPath).text;
        }

    }


    public override string ToString()
    {
        return string.Format("Head: {0}, BodyPath: {1}, BackBtn: {2}, NextBtn: {3}, Body: {4}",
            head, bodyPath, backBtn, nextBtn, body);
    }
}
