using UnityEngine;

public class Lesson
{
    public string id;
    public string textPath;

    public string textBody =>
        string.IsNullOrEmpty(textPath)
            ? string.Empty
            : Resources.Load<TextAsset>("LessonsBody/" + textPath + "_IT").text;

    public override string ToString()
    {
        return $"Id: {id}, TextPath: {textPath}";
    }
}
