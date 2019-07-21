using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lesson
{
    public string id;
    public string textPath;
    public string textBody
    {
        get
        {
            return string.IsNullOrEmpty(textPath)
                ? string.Empty
                : Resources.Load<TextAsset>("LessonsBody/" + textPath + "_IT").text;
        }

    }

    public override string ToString()
    {
        return string.Format("Id: {0}, TextPath: {1}", id, textPath);
    }
}
