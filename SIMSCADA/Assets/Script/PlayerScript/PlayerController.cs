using System;
using System.Collections;
using System.Collections.Generic;
using MuteColossus;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb2D;

    private Vector3 offsetDraw;
    private Vector3 scaleDraw;

    public bool pathUpdated;
    public List<Node> path;
    private string cellLayout;
    public Pathfinding pathfinder;

    public Animator animator;

    public string spriteToAnimate;
    private string nSpriteName;
    private string hlSpriteName;
    private string prSpriteName;
    private string spriteNumber;

    public string plName;
    public string plSurname;
    public string plGender;

    [SerializeField] private float speed;

    [SerializeField] private Vector3Int playerStartCellPos;
    [SerializeField] private Vector3Int playerDestCellPos;

    private Camera cameraMain;
    private bool isCameraNotNull;

    private IEnumerator moveRoutine;
    private IEnumerator faceToAiRoutine;

    private static readonly int inputY = Animator.StringToHash("inputY");
    private static readonly int inputX = Animator.StringToHash("inputX");

    private float isoX;
    private float isoY;

    private FloorManager floorManager;

    private void OnEnable()
    {
        //initialize component
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        //set starting value in order to animate the right position
        ClassDb.charactersCommon.SetStartAnimation(animator);

        cameraMain = Camera.main;
        isCameraNotNull = cameraMain != null;

        pathUpdated = false;

        speed = 2f;

        floorManager = GameObject.Find(StringDb.floorTileMap).GetComponent<FloorManager>();

        //check the layout and set the respective parameter for offset and scale
        GetPlayerPathVariables();

        CreateIdInfo();

        SetPlayerSpriteNames();
        spriteToAnimate = nSpriteName;
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0) || !floorManager.pointerOnFloor) return;
        Vector3 playerDestWorldPos = isCameraNotNull ? cameraMain.ScreenToWorldPoint(Input.mousePosition) : Vector3.zero;
        SetPlayerDestination(playerDestWorldPos);
    }

    private void GetPlayerPathVariables()
    {
        SetPlayerPathfinder();
        //check the layout and set the respective parameter for offset and scale
        ClassDb.charactersCommon.CheckCellLayout(out cellLayout, pathfinder);
        ClassDb.charactersCommon.SetOffsetScaleDraw(cellLayout, pathfinder, out offsetDraw, out scaleDraw);
        
    }

    private void SetPlayerPathfinder()
    {
        pathfinder = ClassDb.regularPathfinder;
    }

    private void CreateIdInfo()
    {
        CharacterName characterName = CNG.instance.gen.GenerateNameObject(Gender.Male);
        plName = characterName.GetFirstName();
        plSurname = characterName.GetSurname();
        plGender = characterName.GetGenderString();
        //spriteNumber = Random.Range(1, 6).ToString("D2");
        spriteNumber = 1.ToString("D2");
    }

    private void SetPlayerSpriteNames()
    {
        nSpriteName = StringDb.rscPlSpritePrefix + "_" + plGender + "_" + spriteNumber;
        hlSpriteName = StringDb.rscPlSpritePrefix + "_" + plGender + "_" + spriteNumber + StringDb.rscHlSpriteSuffix;
        prSpriteName = StringDb.rscPlSpritePrefix + "_" + plGender + "_" + spriteNumber + StringDb.rscPrSpriteSuffix;
    }

    public void SetPlayerDestination(Vector3 playerDestWorldPos)
    {
        if (pathUpdated)
        {
            StopCoroutine(moveRoutine);
            ClassDb.charactersCommon.StopWalking(animator, out pathUpdated);
        }

        moveRoutine = Move();

        //get start and destination from mouse and actual position of character
        Vector2 position = rb2D.position;
        Vector3 playerStartWorldPos = new Vector3(position.x, position.y, 0);

        Vector3 playerNodeDest = new Vector3(playerDestWorldPos.x, playerDestWorldPos.y, 0);

        //set the start and the destination cell
        playerStartCellPos = pathfinder.listTileMap[0].layoutGrid.WorldToCell(playerStartWorldPos);
        playerDestCellPos = pathfinder.listTileMap[0].layoutGrid.WorldToCell(playerNodeDest);

        if (!pathfinder.EligibleClick(playerDestCellPos)) return;
        path = new List<Node>();
        try
        {
            pathfinder.FindPath(playerStartCellPos, playerDestCellPos, out path);
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

    private IEnumerator Move()
    {
        while (path.Count > 0)
        {
            DefineDirection(path[0].position, (Vector2Int)pathfinder.listTileMap[0].layoutGrid.WorldToCell(rb2D.position));

            //converting logical movement in world movement
            Vector3 pathWorldDestination = pathfinder.listTileMap[0].layoutGrid.CellToWorld(new Vector3Int(path[0].position.x, path[0].position.y, 0));

            //adjusting world movement with offset and scale parameter
            Vector2 realWorldDestination = new Vector2((offsetDraw.x * scaleDraw.x) + pathWorldDestination.x, (offsetDraw.y * scaleDraw.y) + pathWorldDestination.y);

            rb2D.position = Vector3.MoveTowards(rb2D.position, realWorldDestination, Time.fixedDeltaTime * speed);

            if (rb2D.position == realWorldDestination)
            {
                path.RemoveAt(0);
            }

            yield return new WaitForFixedUpdate();
        }

        ClassDb.charactersCommon.StopWalking(animator, out pathUpdated);
    }

    public void StartFacePlayerToAi(Vector2 aiPosition)
    {
        faceToAiRoutine = FacePlayerToAi(aiPosition);
        StartCoroutine(faceToAiRoutine);
    }

    private IEnumerator FacePlayerToAi(Vector2 aiPosition)
    {
        yield return new WaitWhile(() => pathUpdated);
        DefineDirection((Vector2Int)pathfinder.listTileMap[0].layoutGrid.WorldToCell(aiPosition), (Vector2Int)pathfinder.listTileMap[0].layoutGrid.WorldToCell(rb2D.position));
    }

    public void DefineDirection(Vector2Int destination, Vector2Int start)
    {
        Vector2 vector2 = destination - start;

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
}
