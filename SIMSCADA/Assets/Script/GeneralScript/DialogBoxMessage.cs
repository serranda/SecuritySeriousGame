using UnityEngine;

public class DialogBoxMessage
{
    public string head;
    public string bodyPath;
    public string backBtn;
    public string nextBtn;

    public string body =>
        string.IsNullOrEmpty(bodyPath) 
            ? string.Empty 
            : Resources.Load<TextAsset>(bodyPath).text;


    public override string ToString() => $"Head: {head}, BodyPath: {bodyPath}, BackBtn: {backBtn}, NextBtn: {nextBtn}, Body: {body}";
}
