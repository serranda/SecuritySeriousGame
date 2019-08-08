using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VersionManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI versionText;

    // Start is called before the first frame update
    void Start()
    {
        versionText.text = string.Concat("v",Application.version);
    }
}
