using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject exitPanel;
    [SerializeField] private Button exitButton;

    // Start is called before the first frame update
    void Start()
    {
#if UNITY_WEBGL

        exitPanel.SetActive(false);
        exitButton.gameObject.SetActive(false);

#else

        exitPanel.SetActive(true);
        exitButton.gameObject.SetActive(true);

#endif
    }
}
