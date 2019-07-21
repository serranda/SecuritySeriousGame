using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScrollManager : MonoBehaviour
{
    public float scaleRate;
    public Vector2 anchorPosition;

    private ScrollRect scroll;
    private TMP_Dropdown dropdown;
    
    // Start is called before the first frame update
    private void Start()
    {
        //get the scroll rect and the dropdown
        scroll = GetComponent<ScrollRect>();
        dropdown = GetComponentInParent<TMP_Dropdown>();

        //adjusting scrollbar position related to actual dropdown value
        NormalizePosition();

    }
    private void Update()
    {

        //adjusting the scale for best fit in the dropdwon sprite
        RectTransform viewport = scroll.viewport;
        viewport.transform.localScale = new Vector2(scaleRate, scaleRate);
        viewport.anchoredPosition = anchorPosition;

    }
    private void NormalizePosition()
    {
        float totalOption = dropdown.options.Count;
        float correctPosition = dropdown.value / totalOption;

        scroll.verticalScrollbar.value = 1 - correctPosition;

    }
}
