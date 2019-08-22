using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemStore
{
    public string name;
    public string descriptionPath;

    public string descriptionBody =>
        string.IsNullOrEmpty(descriptionPath)
            ? string.Empty
            : Resources.Load<TextAsset>("ItemStoreBody/"+descriptionPath + "_IT").text;

    public string imageName;
    public int price;
    public int finalLevel;
    public int effect;
    public int currentLevel;
    public GameObject itemObject;
    public string threatAffinity;

    public override string ToString()
    {
        return
            $"Name: {name}, Description: {descriptionPath}, ImageName: {imageName}, Price: {price}, FinalLevel: {finalLevel}, Effect: {effect}, CurrentLevel: {currentLevel}, ItemObject: {itemObject}, ThreatAffinity: {threatAffinity}";
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

    public static IComparer<ItemStore> NameComparer { get; } = new nameRelationalComparer();
}
