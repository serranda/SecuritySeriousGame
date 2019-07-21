using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemStore
{
    public string name;
    public string descriptionPath;
    public string descriptionBody
    {
        get
        {
            return string.IsNullOrEmpty(descriptionPath)
                ? string.Empty
                : Resources.Load<TextAsset>("ItemStoreBody/"+descriptionPath + "_IT").text;
        }

    }
    public string imageName;
    public int price;
    public int finalLevel;
    public int effect;
    public int currentLevel;
    public GameObject itemObject;

    public override string ToString()
    {
        return string.Format("Name: {0}, Description: {1}, ImageName: {2}, Price: {3}, FinalLevel: {4}, Effect: {5}, CurrentLevel: {6}, ItemObject: {7}", 
            name, descriptionPath, imageName, price, finalLevel, effect, currentLevel, itemObject);
    }
}
