using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrefabManager : MonoBehaviour
{
    public Canvas prefabActionMenu;
    public Canvas prefabDialogBox;
    public Canvas prefabTutorialDialogBox;
    public Canvas prefabScadaScreen;
    public Canvas prefabStoreScreen;
    public Canvas prefabNoteBook;
    public Canvas prefabHud;
    public Canvas prefabProgressBar;
    public Button prefabProgressBarButton;
    public Canvas prefabIdCard;
    public Canvas prefabServerScreen;
    public SpriteRenderer prefabPlayer;
    public SpriteRenderer prefabAi;
    public Button prefabStoreItem;
    public Toggle prefabNotebookButton;
    public Canvas prefabLoadingScreen;
    public Canvas prefabGraph;

    private Stack<GameObject> inactiveActionMenu;
    private Stack<GameObject> inactiveDialogBox;
    private Stack<GameObject> inactiveTutorialDialogBox;
    private Stack<GameObject> inactiveScadaScreen;
    private Stack<GameObject> inactiveStoreScreen;
    private Stack<GameObject> inactiveServerScreen;
    private Stack<GameObject> inactiveNoteBook;
    private Stack<GameObject> inactiveHud;
    private Stack<GameObject> inactiveProgressBar;
    private Stack<GameObject> inactiveProgressBarButton;
    private Stack<GameObject> inactiveIdCard;
    private Stack<GameObject> inactivePlayer;
    private Stack<GameObject> inactiveAi;
    private Stack<GameObject> inactiveStoreItem;
    private Stack<GameObject> inactiveNotebookButton;
    private Stack<GameObject> inactiveLoadingScreen;
    private Stack<GameObject> inactiveGraph;

    public static int actionIndex;
    public static int dialogIndex;
    public static int tutorialDialogIndex;
    public static int scadaIndex;
    public static int storeIndex;
    public static int serverIndex;
    public static int noteBookIndex;
    public static int hudIndex;
    public static int pbIndex;
    public static int pbBtnIndex;
    public static int icIndex;
    public static int playerIndex;
    public static int aiIndex;
    public static int storeItemIndex;
    public static int notebookButtonIndex;
    public static int loadingScreenIndex;
    public static int graphIndex;

    private static List<Stack<GameObject>> stackList;

    // Start is called before the first frame update
    private void Awake()
    {        
        inactiveActionMenu = new Stack<GameObject>();
        inactiveDialogBox = new Stack<GameObject>();
        inactiveTutorialDialogBox = new Stack<GameObject>();
        inactiveScadaScreen = new Stack<GameObject>();
        inactiveStoreScreen = new Stack<GameObject>();
        inactiveServerScreen = new Stack<GameObject>();
        inactiveNoteBook = new Stack<GameObject>();
        inactiveHud = new Stack<GameObject>();
        inactiveProgressBar = new Stack<GameObject>();
        inactiveProgressBarButton = new Stack<GameObject>();
        inactiveIdCard = new Stack<GameObject>();
        inactivePlayer = new Stack<GameObject>();
        inactiveAi = new Stack<GameObject>();
        inactiveStoreItem = new Stack<GameObject>();
        inactiveNotebookButton = new Stack<GameObject>();
        inactiveLoadingScreen = new Stack<GameObject>();
        inactiveGraph = new Stack<GameObject>();

        stackList = new List<Stack<GameObject>>()
        {
            inactiveActionMenu,
            inactiveDialogBox,
            inactiveTutorialDialogBox,
            inactiveScadaScreen,
            inactiveStoreScreen,
            inactiveServerScreen,
            inactiveNoteBook,
            inactiveHud,
            inactiveProgressBar,
            inactiveProgressBarButton,
            inactiveIdCard,
            inactivePlayer,
            inactiveAi,
            inactiveStoreItem,
            inactiveNotebookButton,
            inactiveLoadingScreen,
            inactiveGraph
        };

        actionIndex = stackList.IndexOf(inactiveActionMenu);
        dialogIndex = stackList.IndexOf(inactiveDialogBox);
        tutorialDialogIndex = stackList.IndexOf(inactiveTutorialDialogBox);
        scadaIndex = stackList.IndexOf(inactiveScadaScreen);
        storeIndex = stackList.IndexOf(inactiveStoreScreen);
        serverIndex = stackList.IndexOf(inactiveServerScreen);
        noteBookIndex = stackList.IndexOf(inactiveNoteBook);
        hudIndex = stackList.IndexOf(inactiveHud);
        pbIndex = stackList.IndexOf(inactiveProgressBar);
        pbBtnIndex = stackList.IndexOf(inactiveProgressBarButton);
        icIndex = stackList.IndexOf(inactiveIdCard);
        playerIndex = stackList.IndexOf(inactivePlayer);
        aiIndex = stackList.IndexOf(inactiveAi);
        storeItemIndex = stackList.IndexOf(inactiveStoreItem);
        notebookButtonIndex = stackList.IndexOf(inactiveNotebookButton);
        loadingScreenIndex = stackList.IndexOf(inactiveLoadingScreen);
        graphIndex = stackList.IndexOf(inactiveGraph);
    }

    public GameObject GetPrefab(GameObject prefabRef, int stackIndex)
    {
        GameObject prefabSpawned = stackList[stackIndex].Count>0 ? stackList[stackIndex].Pop() : Instantiate(prefabRef);

        prefabSpawned.transform.SetParent(null);
        prefabSpawned.SetActive(true);

        return prefabSpawned;
    }
    public void ReturnPrefab(GameObject prefabToReturn, int stackIndex)
    {
        prefabToReturn.transform.SetParent(null);
        prefabToReturn.SetActive(false);
        stackList[stackIndex].Push(prefabToReturn);
    }
}
