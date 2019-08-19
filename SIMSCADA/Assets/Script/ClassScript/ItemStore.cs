using System;
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
    private static readonly IComparer<ItemStore> NameComparerInstance = new nameRelationalComparer();

    public override string ToString()
    {
        return string.Format("Name: {0}, Description: {1}, ImageName: {2}, Price: {3}, FinalLevel: {4}, Effect: {5}, CurrentLevel: {6}, ItemObject: {7}", 
            name, descriptionPath, imageName, price, finalLevel, effect, currentLevel, itemObject);
    }

    private sealed class nameRelationalComparer : IComparer<ItemStore>
    {
        public int Compare(ItemStore x, ItemStore y)
        {
            if (ReferenceEquals(x, y)) return 0;
            if (ReferenceEquals(null, y)) return 1;
            if (ReferenceEquals(null, x)) return -1;
            return string.Compare(x.name, y.name, StringComparison.Ordinal);
        }
    }

    public static IComparer<ItemStore> NameComparer
    {
        get { return NameComparerInstance; }
    }
}
