using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LongTextManager : MonoBehaviour
{
    private int pageCount;
    [SerializeField] private TextMeshProUGUI text;

    [SerializeField] private Button leftButton;
    [SerializeField] private CanvasGroup lbGroup;

    [SerializeField] private Button rightButton;
    [SerializeField] private CanvasGroup rbGroup;

    private HorizontalLayoutGroup horizontalLayout;

    private void OnEnable()
    {
        //text = GetComponent<TextMeshProUGUI>();
        text.pageToDisplay = 1;

        pageCount = text.textInfo.pageCount;

        SetLeftButton();
        SetRightButton();
    }

    private void Update()
    {
        text = GetComponent<TextMeshProUGUI>();

        pageCount = text.textInfo.pageCount;

        if (pageCount > 1)
        {
            lbGroup.interactable = true;
            lbGroup.blocksRaycasts = true;
            lbGroup.alpha = 1;

            rbGroup.interactable = true;
            rbGroup.blocksRaycasts = true;
            rbGroup.alpha = 1;

            if (text.pageToDisplay == 1)
            {
                //show only right button
                lbGroup.interactable = false;
                lbGroup.blocksRaycasts = false;
                lbGroup.alpha = 0;
            }

            if (text.pageToDisplay != pageCount) return;
            //show only left button
            rbGroup.interactable = false;
            rbGroup.blocksRaycasts = false;
            rbGroup.alpha = 0;
        }
        else
        {
            lbGroup.interactable = false;
            lbGroup.blocksRaycasts = false;
            lbGroup.alpha = 0;

            rbGroup.interactable = false;
            rbGroup.blocksRaycasts = false;
            rbGroup.alpha = 0;
        }
    }

    private void SetLeftButton()
    {
        //leftButton = GameObject.Find(StaticDb.dialogBoxBtnLeft).GetComponent<Button>();
        leftButton.onClick.RemoveAllListeners();
        leftButton.onClick.AddListener(delegate
        {
            text.pageToDisplay--;
            //Debug.Log(leftButton.transform.parent.parent.parent.parent.name);
            //Debug.Log(text);
            //Debug.Log(text.pageToDisplay);
        });

        //lbGroup = leftButton.GetComponent<CanvasGroup>();
    }

    private void SetRightButton()
    {
        //rightButton = GameObject.Find(StaticDb.dialogBoxBtnRight).GetComponent<Button>();
        rightButton.onClick.RemoveAllListeners();
        rightButton.onClick.AddListener(delegate
        {
            text.pageToDisplay++;
            //Debug.Log(rightButton.transform.parent.parent.parent.parent.name);
            //Debug.Log(text);
            //Debug.Log(text.pageToDisplay);
        });
        //rbGroup = rightButton.GetComponent<CanvasGroup>();
    }

    

}
