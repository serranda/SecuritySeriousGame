using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBoxMessage
{
    public string head;
    public string bodyPath;
    public string backBtn;
    public string nextBtn;

    public string body =>
        string.IsNullOrEmpty(bodyPath)
            ? string.Empty
            : Resources.Load<TextAsset>("MenuMessagesBody/" + bodyPath).text;


    public override string ToString()
    {
        return $"Head: {head}, BodyPath: {bodyPath}, BackBtn: {backBtn}, NextBtn: {nextBtn}, Body: {body}";
    }
}
