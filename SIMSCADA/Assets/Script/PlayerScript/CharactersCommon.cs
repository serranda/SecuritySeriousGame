using System;
using UnityEngine;


public class CharactersCommon : MonoBehaviour
{

    private static readonly int isWalking = Animator.StringToHash("isWalking");
    private static readonly int inputY = Animator.StringToHash("inputY");
    private static readonly int inputX = Animator.StringToHash("inputX");


    public void CheckCellLayout(out string cellLayout, Pathfinding pathfinding)
    {
        cellLayout = pathfinding.listTileMap[0].layoutGrid.cellLayout.ToString();
    }

    public void SetOffsetScaleDraw(string cellLayout, Pathfinding pathfinding, out Vector3 offsetDraw, out Vector3 scaleDraw)
    {
        //check if isometric layout
        if (cellLayout.Equals("Isometric") || cellLayout.Equals("IsometricZAsY"))
        {
            offsetDraw.x = 0;
            offsetDraw.y = pathfinding.listTileMap[0].layoutGrid.cellSize.y / 2;
            offsetDraw.z = 0;
            scaleDraw = pathfinding.listTileMap[0].transform.lossyScale;
        }
        else if (cellLayout.Equals("Rectangle"))
        {
            offsetDraw = pathfinding.listTileMap[0].layoutGrid.cellSize / 2;
            scaleDraw = pathfinding.listTileMap[0].transform.lossyScale;
        }
        else
        {
            offsetDraw = Vector3.zero;
            scaleDraw = Vector3.zero;
        }
    }

    public void CenterPlayer(Rigidbody2D rb2D, Pathfinding pathfinding, Vector3 offsetDraw, Vector3 scaleDraw)
    {
        Vector2 position = rb2D.position;
        Vector3 initPos = new Vector3(position.x, position.y, 0);
        Vector3Int initCellPos = pathfinding.listTileMap[0].layoutGrid.WorldToCell(initPos);        
        Vector3 initWorldPos = pathfinding.listTileMap[0].layoutGrid.CellToWorld(initCellPos);
        Vector3 finalWorldPos = new Vector3((offsetDraw.x * scaleDraw.x) + initWorldPos.x, (offsetDraw.y * scaleDraw.y) + initWorldPos.y, 0);

        rb2D.MovePosition(finalWorldPos);
    }

    public void StartWalking(Animator animator, out bool pathUpdated)
    {
        pathUpdated = true;
        animator.SetBool(isWalking, true);
    }

    public void StopWalking(Animator animator, out bool pathUpdated)
    {
        pathUpdated = false;
        animator.SetBool(isWalking, false);
    }

    public void SetStartAnimation(Animator animator)
    {
        animator.SetFloat(inputX, 0);
        animator.SetFloat(inputY, -1);
    }

}
