using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using MuteColossus;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;
// ReSharper disable IteratorNeverReturns

public class AiController : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    private BoxCollider2D boxCollider2D;

    private Rigidbody2D rb2D;

    private Vector3 offsetDraw;
    private Vector3 scaleDraw;

    public bool isAttacker;
    public bool toDestroy;
    public bool idChecked;
    public bool idScanned;
    public bool isTrusted;
    public bool isWaiting;
    public bool isSuspected;
    private bool isAiPathValuesSet;

    public Animator animator;
    public string spriteToAnimate;
    public string nSpriteName;
    private string hlSpriteName;
    private string prSpriteName;
    private string spriteNumber;

    public StringDb.AiDangerResistance dangerResistance;

    //public PlayerController playerController;

    public int aiId;

    public string aiName;
    public string aiSurname;
    public string aiJob;
    public string aiGender;

    private List<Node> path;

    public bool pathUpdated;
    private string cellLayout;
    public Pathfinding pathfinder;
    public bool pathfinderChanged;

    public int wrongDestinationCounter;

    private int radiusBase;
    [SerializeField] private float radius;

    public static bool cursorOverAi;
    public bool onClickAi;
    private Canvas actionMenu;
    private ActionButtonManager actionButtonManager;

    [SerializeField] private Vector3Int aiStartCellPos;
    [SerializeField] private Vector3Int aiDestCellPos;

    [SerializeField] private float aiSpeed;
    [SerializeField] private Vector3Int aiObjective;

    [SerializeField] private float timer;

    [SerializeField] public TimeEvent timeEvent;

    private float isoX;
    private float isoY;

    private IEnumerator waitRoutine;
    private IEnumerator aiRoutine;
    private IEnumerator moveRoutine;
    private IEnumerator changeRoutine;
    private IEnumerator faceToPlayerRoutine;

    private static readonly int inputY = Animator.StringToHash("inputY");
    private static readonly int inputX = Animator.StringToHash("inputX");

    private void OnEnable()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        
        //set starting idle animation
        ClassDb.charactersCommon.SetStartAnimation(animator);

        pathUpdated = false;
        toDestroy = false;
        idChecked = false;
        idScanned = false;
        pathfinderChanged = false;
        isWaiting = false;
        isAiPathValuesSet = false;
        isSuspected = false; 

        timer = 2f;

        wrongDestinationCounter = 0;

        aiId = GameData.lastAiId;
        radiusBase = 15;
        aiSpeed = 1.5f * StringDb.speedMultiplier;
        onClickAi = false;

        //playerController = GameObject.Find(StringDb.playerPrefabName).GetComponent<PlayerController>();

        GetAiPathVariables();

        CreateIdInfo();

        SetAiSpriteNames();
        spriteToAnimate = nSpriteName;

        aiRoutine = AiRoutine();
        StartCoroutine(aiRoutine);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (toDestroy) return;
        spriteToAnimate = prSpriteName;
        ToggleMenu();
        GameData.pressedSprite = gameObject.name;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (toDestroy) return;
        spriteToAnimate = hlSpriteName;
        cursorOverAi = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (toDestroy) return;
        spriteToAnimate = nSpriteName;
        cursorOverAi = false;
    }

    private void Update()
    {
        radius = radiusBase  * (1 - timeEvent.currentPercentage / 100);

        aiSpeed = 1.5f * StringDb.speedMultiplier;

    }

    public void AfterEnable()
    {
        SetAiThreat();

        if (timeEvent.threat.threatAttacker == StringDb.ThreatAttacker.intern)
        {
            SetCorruptionResistance();
            SetTrustedAi();
        }
        else
        {
            dangerResistance = StringDb.AiDangerResistance.external;
            isTrusted = false;
        }

        if (timeEvent.threat.threatType == StringDb.ThreatType.remote)
        {
            StopAllCoroutines();
        }

        aiJob = CreateJob();

        if(isAttacker)
            EditAttackerString();

    }

    private void SetAiThreat()
    {
        timeEvent.threat = ClassDb.threatManager.NewThreatFromTimeEvent(this);

        aiObjective = CreateObjective(timeEvent.threat.threatAttack);
    }

    private void SetCorruptionResistance()
    {
        dangerResistance = (StringDb.AiDangerResistance) Random.Range(0, 3);
    }

    private void SetTrustedAi()
    {
        isTrusted = Random.Range(0, 100) <= GameData.trustedEmployees * 100 / GameData.totalEmployees ;
        if (isTrusted)
        {
            dangerResistance = StringDb.AiDangerResistance.veryHigh;
        }
    }

    private void GetAiPathVariables()
    {
        isAiPathValuesSet = false;
        SetAiPathfinder();
        //check the layout and set the respective parameter for offset and scale
        ClassDb.charactersCommon.CheckCellLayout(out cellLayout, pathfinder);
        ClassDb.charactersCommon.SetOffsetScaleDraw(cellLayout, pathfinder, out offsetDraw, out scaleDraw);
        isAiPathValuesSet = true;
    }

    private void SetAiPathfinder()
    {
        //trusted employer, can go everywhere
        if (isTrusted)
        {
            pathfinder = ClassDb.regularPathfinder;
        }
        //normal employer, need to know which security level is set
        else
        {
            switch (GameData.serverSecurity)
            {
                case StringDb.ServerSecurity.strict:
                    pathfinder = ClassDb.strictedPathfinder;
                    break;
                case StringDb.ServerSecurity.medium:
                    pathfinder = timeEvent.threat.threatAttacker == StringDb.ThreatAttacker.intern ? ClassDb.regularPathfinder : ClassDb.strictedPathfinder;
                    break;
                case StringDb.ServerSecurity.loose:
                    pathfinder = ClassDb.regularPathfinder;
                    break;
                default:
                    Debug.Log("ErrorAssigningPathfinder");
                    break;
            }
        }

        pathfinderChanged = false;

    }

    private Vector3Int CreateObjective(StringDb.ThreatAttack threatAttack)
    {
        
        Vector3Int destination;

        switch (threatAttack)
        {
            case StringDb.ThreatAttack.dos:
                destination = StringDb.serverInDest;
                break;
            case StringDb.ThreatAttack.phishing:
                destination = StringDb.pcDest;
                break;
            case StringDb.ThreatAttack.replay:
                destination = StringDb.pcDest;
                break;
            case StringDb.ThreatAttack.mitm:
                destination = StringDb.pcDest;
                break;
            case StringDb.ThreatAttack.stuxnet:
                destination = StringDb.serverInDest;
                break;
            case StringDb.ThreatAttack.dragonfly:
                destination = StringDb.serverInDest;
                break;
            case StringDb.ThreatAttack.malware:
                destination = Random.Range(0, 2) == 0 ? StringDb.serverInDest : StringDb.pcDest;
                break;
            case StringDb.ThreatAttack.createRemote:
                destination = Random.Range(0, 2) == 0 ? StringDb.serverInDest : StringDb.pcDest;
                break;
            case StringDb.ThreatAttack.timeEvent:
                destination = Random.Range(0, 2) == 0 ? StringDb.serverInDest : StringDb.pcDest;
                break;
            case StringDb.ThreatAttack.fakeLocal:
                destination = Random.Range(0, 2) == 0 ? StringDb.serverInDest : StringDb.pcDest;
                break;
            default:
                throw new ArgumentOutOfRangeException("threatAttack", threatAttack, null);
        }

        return destination;
    }

    private void CreateIdInfo()
    {
        CharacterName characterName = CNG.instance.gen.GenerateNameObject(Gender.Other);
        aiName = characterName.GetFirstName();
        aiSurname = characterName.GetSurname();
        aiGender = characterName.GetGenderString();
        if (aiGender.Equals("Female"))
        {
            do
            {
                spriteNumber = Random.Range(1, 6).ToString("D2");

            } while (spriteNumber.Equals(GameData.lastFemaleSpriteNumber));

            GameData.lastFemaleSpriteNumber = spriteNumber;
        }
        else
        {
            do
            {
                spriteNumber = Random.Range(1, 6).ToString("D2");

            } while (spriteNumber.Equals(GameData.lastMaleSpriteNumber));

            GameData.lastMaleSpriteNumber = spriteNumber;
        }

    }

    private string CreateJob()
    {
        string job = isTrusted ? 
            StringDb.trustedJobs[Random.Range(0, StringDb.trustedJobs.Length - 1)] : 
            StringDb.normalJobs[Random.Range(0, StringDb.trustedJobs.Length - 1)];
        return job;
    }

    private void EditAttackerString()
    {
        //edit name, surname and job with random char
        int num = Random.Range(0, 26); // Zero to 25
        char let = (char)('a' + num);

        aiName = aiName.Insert(Random.Range(1, aiName.Length), @let.ToString().ToUpper());
        aiSurname = aiSurname.Insert(Random.Range(1, aiSurname.Length), @let.ToString().ToUpper());
        aiJob = aiJob.Insert(Random.Range(1, aiJob.Length), @let.ToString());
    }

    private void SetAiSpriteNames()
    {
        nSpriteName = StringDb.rscAiSpritePrefix + "_" + aiGender + "_" + spriteNumber;
        hlSpriteName = StringDb.rscAiSpritePrefix + "_" + aiGender + "_" + spriteNumber + StringDb.rscHlSpriteSuffix;
        prSpriteName = StringDb.rscAiSpritePrefix + "_" + aiGender + "_" + spriteNumber + StringDb.rscPrSpriteSuffix;
    }

    private IEnumerator AiRoutine()
    {
        yield return new WaitWhile(() => !isAiPathValuesSet);
        for (;;)
        {
            SetAiDestination();
            yield return new WaitWhile(() => pathUpdated);
            yield return new WaitForSeconds(timer);
        }
    }

    private void SetAiDestination()
    {
        moveRoutine = Move();

        Vector2 position = rb2D.position;
        aiStartCellPos = pathfinder.listTileMap[0].layoutGrid.WorldToCell(new Vector3(position.x, position.y, 0));


        if (toDestroy)
        {
            //set destination for destroying ai
            aiDestCellPos = StringDb.aiSpawn;
        }
        else if (pathfinderChanged)
        {
            //set destination outside server room
            aiDestCellPos = StringDb.serverOutDest;
        }
        else
        {
            //create destination around objective
            aiDestCellPos = CreateNeighborhood(aiObjective);
        }

        //ai has valid destination cell; pathfinder find a route from startCell to destinationCell
        if (!pathfinder.EligibleClick(aiDestCellPos))
        {
            wrongDestinationCounter++;
            return;
        }
        path = new List<Node>();
        try
        {
            pathfinder.FindPath(aiStartCellPos, aiDestCellPos, out path);
            ClassDb.charactersCommon.StartWalking(animator, out pathUpdated);
            StartCoroutine(moveRoutine);
        }
        catch (Exception e)
        {
            Debug.Log(e);
            ClassDb.charactersCommon.StopWalking(animator, out pathUpdated);
            StopCoroutine(moveRoutine);
        }
    }

    private Vector3Int CreateNeighborhood(Vector3Int objective)
    {
        Vector3Int destCell = new Vector3Int(
            Random.Range(objective.x - (int)radius, objective.x + (int)radius),
            Random.Range(objective.y - (int)radius, objective.y + (int)radius),
            0);
        return destCell;
    }

    private IEnumerator Move()
    {
        while (path.Count > 0)
        {
            DefineDirection(path[0].position, (Vector2Int) pathfinder.listTileMap[0].layoutGrid.WorldToCell(rb2D.position));

            //converting logical movement in world movement
            Vector3 pathWorldDestination = pathfinder.listTileMap[0].layoutGrid.CellToWorld(new Vector3Int(path[0].position.x, path[0].position.y, 0));

            //adjusting world movement with offset and scale parameter
            Vector2 realWorldDestination = new Vector2((offsetDraw.x * scaleDraw.x) + pathWorldDestination.x, (offsetDraw.y * scaleDraw.y) + pathWorldDestination.y);

            rb2D.position = Vector3.MoveTowards(rb2D.position, realWorldDestination, Time.fixedDeltaTime * aiSpeed);

            if (rb2D.position == realWorldDestination)
            {
                path.RemoveAt(0);
            }

            yield return new WaitForFixedUpdate();
        }

        if (toDestroy)
        {
            ClassDb.prefabManager.ReturnPrefab(gameObject, PrefabManager.aiIndex);
        }

        ClassDb.charactersCommon.StopWalking(animator, out pathUpdated);
    }

    public void DefineDirection(Vector2Int destination, Vector2Int start)
    {

        Vector2 vector2 = StandardizeVector(destination - start);


        if (cellLayout.Equals("Isometric") || cellLayout.Equals("IsometricZAsY"))
        {

            if (Math.Abs(vector2.x - (-1)) < float.Epsilon)
            {
                if (Math.Abs(vector2.y - (-1)) < float.Epsilon)
                {
                    isoX = 0;
                    isoY = -1;
                }
                else if (Math.Abs(vector2.y) < float.Epsilon)
                {
                    isoX = -1;
                    isoY = -1;
                }
                else if (Math.Abs(vector2.y - 1) < float.Epsilon)
                {
                    isoX = -1;
                    isoY = 0;
                }
            }
            else if (Math.Abs(vector2.x - 1) < float.Epsilon)
            {
                if (Math.Abs(vector2.y - (-1)) < float.Epsilon)
                {
                    isoX = 1;
                    isoY = 0;
                }
                else if (Math.Abs(vector2.y) < float.Epsilon)
                {
                    isoX = 1;
                    isoY = 1;
                }
                else if (Math.Abs(vector2.y - 1) < float.Epsilon)
                {
                    isoX = 0;
                    isoY = 1;
                }
            }
            else if (Math.Abs(vector2.x) < float.Epsilon)
            {
                if (Math.Abs(vector2.y - (-1)) < float.Epsilon)
                {
                    isoX = 1;
                    isoY = -1;
                }
                else if (Math.Abs(vector2.y - 1) < float.Epsilon)
                {
                    isoX = -1;
                    isoY = 1;
                }
            }

            animator.SetFloat(inputX, isoX);
            animator.SetFloat(inputY, isoY);
        }
        else if (cellLayout.Equals("Rectangular"))
        {
            animator.SetFloat(inputX, vector2.x);
            animator.SetFloat(inputY, vector2.y);
        }
       
    }

    private Vector2 StandardizeVector(Vector2 vector)
    {
        Vector2 returnVector = vector;
        if (returnVector.x > 1)
        {
            returnVector.x = 1;
        }
        if (returnVector.x < -1)
        {
            returnVector.x = - 1;
        }
        if (returnVector.y > 1)
        {
            returnVector.y = 1;
        }
        if (returnVector.y < -1)
        {
            returnVector.y = -1;
        }

        return returnVector;
    }

    public void StartChangePathfinder()
    {
        changeRoutine = ChangePathfinder();

        Pathfinding finder = ClassDb.strictedPathfinder;

        StopCoroutine(aiRoutine);

        if (!finder.EligibleClick(aiDestCellPos))
        {
            if (pathUpdated)
            {
                StopCoroutine(moveRoutine);
                ClassDb.charactersCommon.StopWalking(animator, out pathUpdated);
            }

            if (!CheckActualPositionEligibility())
            {
                SetAiDestination();
            }
        }
        StartCoroutine(changeRoutine);
    }

    private bool CheckActualPositionEligibility()
    {
        Vector2 position = rb2D.position;
        Pathfinding finder = ClassDb.strictedPathfinder;
        Vector3Int aiPos = finder.listTileMap[0].layoutGrid.WorldToCell(new Vector3(position.x, position.y, 0));
        return finder.EligibleClick(aiPos);
    }

    private IEnumerator ChangePathfinder()
    {
        yield return new WaitWhile(() => pathUpdated);
        SetAiPathfinder();
        StartCoroutine(aiRoutine);
    }

    public void StartWait()
    {
        isWaiting = true;

        StopCoroutine(aiRoutine);

        if (pathUpdated)
        {
            StopCoroutine(moveRoutine);
            ClassDb.charactersCommon.StopWalking(animator, out pathUpdated);
        }

        waitRoutine = Wait();

        StartCoroutine(waitRoutine);
    }

    private IEnumerator Wait()
    {
        yield return new WaitWhile(() => onClickAi);
        StartCoroutine(aiRoutine);
        isWaiting = false;
    }

    public void DestroyAi()
    {
        if (timeEvent.threat.threatType == StringDb.ThreatType.remote)
        {
            ClassDb.prefabManager.ReturnPrefab(gameObject, PrefabManager.aiIndex);
        }
        else
        {
            toDestroy = true;
            StopAllCoroutines();
            SetAiDestination();
        }
    }

    public void ToggleMenu()
    {
        if (ActionButtonManager.buttonEnabled && gameObject.name == GameData.pressedSprite)
        {
            ClassDb.prefabManager.ReturnPrefab(actionMenu.gameObject, PrefabManager.actionIndex);
        }
        else
        {
            //get the canvas to display the option menu. only one action menu at time will be displayed
            if (GameObject.Find(StringDb.actionMenuName))
            {
               actionMenu = GameObject.Find(StringDb.actionMenuName).GetComponent<Canvas>();

               ClassDb.prefabManager.ReturnPrefab(actionMenu.gameObject, PrefabManager.actionIndex);
            }

            actionMenu = ClassDb.prefabManager.GetPrefab(ClassDb.prefabManager.prefabActionMenu.gameObject, PrefabManager.actionIndex)
                .GetComponent<Canvas>();

            Transform actionMenuTransform;
            (actionMenuTransform = actionMenu.transform).SetParent(gameObject.transform);
            actionMenuTransform.localPosition = new Vector3(0, 0.7f, 0);

            actionButtonManager = GameObject.Find(StringDb.actionMenuName).GetComponent<ActionButtonManager>();
            actionButtonManager.GetButtons();
            SetListener();
        }

    }

    private void SetListener()
    {
        AiListener aiListener = GetComponent<AiListener>();

        List<Button> buttons = actionButtonManager.GetActiveCanvasGroup(3);

        buttons[0].GetComponentInChildren<TextMeshProUGUI>().text = "Chiedi credenziali";
        buttons[0].onClick.RemoveAllListeners();
        buttons[0].onClick.AddListener(delegate
        {
            //if (!idChecked)
            //{
            //    MovePlayerToAi();
            //}
            aiListener.StartCheckAiId();
            ClassDb.prefabManager.ReturnPrefab(actionMenu.gameObject, PrefabManager.actionIndex);
        });

        buttons[1].GetComponentInChildren<TextMeshProUGUI>().text = "Licenzia";
        buttons[1].onClick.RemoveAllListeners();
        buttons[1].onClick.AddListener(delegate
        {
            aiListener.FireAi(gameObject);
            ClassDb.prefabManager.ReturnPrefab(actionMenu.gameObject, PrefabManager.actionIndex);

        });

        buttons[2].GetComponentInChildren<TextMeshProUGUI>().text = "Congeda";
        buttons[2].onClick.RemoveAllListeners();
        buttons[2].onClick.AddListener(delegate
        {
            onClickAi = false;
            ClassDb.prefabManager.ReturnPrefab(actionMenu.gameObject, PrefabManager.actionIndex);
        });


    }

    //private void MovePlayerToAi()
    //{
    //    Vector3 playerDestination = FindPlayerDestination(rb2D.position);

    //    playerController.SetPlayerDestination(playerDestination);
    //    playerController.StartFacePlayerToAi(rb2D.position);
    //    StartFaceAiToPlayer();
    //}

    //private void StartFaceAiToPlayer()
    //{
    //    faceToPlayerRoutine = FaceAiToPlayer();
    //    StartCoroutine(faceToPlayerRoutine);
    //}

    //private IEnumerator FaceAiToPlayer()
    //{
    //    while (playerController.pathUpdated)
    //    {
    //        DefineDirection((Vector2Int)pathfinder.listTileMap[0].layoutGrid.WorldToCell(playerController.rb2D.position),
    //            (Vector2Int)pathfinder.listTileMap[0].layoutGrid.WorldToCell(rb2D.position));
    //        yield return new WaitForFixedUpdate();
    //    }
    //}

    //private Vector3 FindPlayerDestination(Vector2 position)
    //{
    //    Vector3 playerDestination = Vector3Int.zero;

    //    Vector2Int aiCellPosition = (Vector2Int) pathfinder.listTileMap[0].layoutGrid.WorldToCell(position);

    //    for (int x = -1; x <= 1; x++)
    //    {
    //        for (int y = -1; y <= 1; y++)
    //        {
    //            Vector3Int vector = new Vector3Int(aiCellPosition.x + x, aiCellPosition.y + y,0);
    //            if (!playerController.pathfinder.EligibleClick(vector)) continue;
    //            playerDestination = pathfinder.listTileMap[0].layoutGrid.CellToWorld(vector);
    //            break;
    //        }
    //    }

    //    return playerDestination;
    //}

    public void BeforeDeploy()
    {
        SetAiDestination();
    }

    public bool ThreatDeployEligibility()
    {
        if (timeEvent.threat.threatType == StringDb.ThreatType.remote) return true;

        Vector2 position = rb2D.position;
        Vector3Int aiPosCell = pathfinder.listTileMap[0].layoutGrid.WorldToCell(new Vector3(position.x, position.y, 0));

        return aiPosCell == aiObjective;
    }

    public void PointOutThreat()
    {
        //if (!isAttacker) return;
        isSuspected = true;

        //CHANGE SPRITE TO THE SUSPECTED ONES
        hlSpriteName = hlSpriteName.Insert(12, "Suspected");
        prSpriteName = prSpriteName.Insert(12, "Suspected");

        Debug.Log(hlSpriteName + " " + prSpriteName);

        ClassDb.levelMessageManager.StartSuspiciousAi();
    }

    public void SetInteraction(bool colliderEnabled)
    {
        boxCollider2D.enabled = colliderEnabled;

        if (timeEvent.progressBar)
        {
            timeEvent.progressBar.GetComponent<CanvasGroup>().blocksRaycasts = colliderEnabled;
        }

        GetComponent<SpriteRenderer>().color = colliderEnabled ? Color.white : Color.grey;
    }
}
