using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <inheritdoc />
/// <summary>
/// An implementation of A* for the new TileMap system released by Unity
/// </summary>
public class Pathfinding : MonoBehaviour
{
    private Dictionary<Vector3Int, bool> labyrinthMap;
    private int counter;
    
    [SerializeField]
    public List<Tilemap> listTileMap;

    private void Start()
    {

        labyrinthMap = new Dictionary<Vector3Int, bool>();
        counter = 0;

        SetTileMapsSize(listTileMap);

        foreach (Tilemap tileMap in listTileMap)
        {
            Vector3Int size = tileMap.size;
            int xBound = size.x / 2;
            int yBound = size.y / 2;

            for (int x = -xBound; x < xBound; x++)
            {
                for (int y = -yBound; y < yBound; y++)
                {

                    Vector3Int tileCoordinates = new Vector3Int(x, y, 0);
                    TileBase tile = tileMap.GetTile(tileCoordinates);
                    if (counter == 0)
                    {
                        labyrinthMap.Add(tileCoordinates, tile == null);
                    }
                    else
                    {
                        if (tile == null && labyrinthMap[tileCoordinates])
                        {
                            labyrinthMap[tileCoordinates] = true;
                        }
                        else
                        {
                            labyrinthMap[tileCoordinates] = false;
                        }
                    }
                }
            }
            counter++;
        }        
    }

    /// <summary>
    /// Simple check to see if the user clicked on a passable wall as our destination
    /// </summary>
    /// <param name="target">The logical position of the tile</param>
    /// <returns></returns>
    public bool EligibleClick(Vector3Int target)
    {
        return labyrinthMap[target];
    }

    /// <summary>
    /// Looks for a path
    /// </summary>
    /// <param name="start">The starting position</param>
    /// <param name="target">The destination the path should lead to</param>
    /// <param name="path">The path created</param>
    public void FindPath(Vector3Int start, Vector3Int target, out List<Node> path)
    {
        path = new List<Node>();
        Node startNode = new Node(start.x, start.y, true);
        Node targetNode = new Node(target.x, target.y, true);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();

        if (!labyrinthMap[target])
                return;

            openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode.Equals(targetNode))
            {
                path = RetracePath(startNode, currentNode);
                return;
            }

            foreach (Node neighbor in GetNeighbors(currentNode))
            {
                if (!neighbor.passable || closedSet.Contains(neighbor))
                    continue;

                int newMovementCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);

                if (newMovementCostToNeighbor >= neighbor.gCost && openSet.Contains(neighbor)) continue;
                neighbor.gCost = newMovementCostToNeighbor;
                neighbor.hCost = GetDistance(neighbor, targetNode);
                neighbor.parent = currentNode;

                if (!openSet.Contains(neighbor)) openSet.Add(neighbor);
                openSet = openSet.OrderBy(a => a.fCost).ToList();
            }
            openSet = openSet.OrderBy(a => a.fCost).ToList();
        }
    }
    private List<Node> RetracePath(Node startNode, Node endNote)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNote;

        while (!currentNode.Equals(startNode))
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse();
        return path;
    }
    private int GetDistance(Node a, Node b)
    {
        int dstX = Mathf.Abs(a.position.x - b.position.x);
        int dstY = Mathf.Abs(a.position.y - b.position.y);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);

        return 14 * dstX + 10 * (dstY - dstX);
    }
    private List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int nx = node.position.x + x;
                int ny = node.position.y + y;

                bool current = labyrinthMap[new Vector3Int(nx, ny, 0)];

                if (current)
                    neighbors.Add(new Node(nx, ny, true));
            }
        }

        return neighbors;
    }
    private void SetTileMapsSize(List<Tilemap> tileMaps)
    {
        int x = 100;
        int y = 100;

        foreach (Tilemap tileMap in tileMaps)
        {
            if (tileMap.size.x > x)
            {
                x = tileMap.size.x;
            }
            if (tileMap.size.y > y)
            {
                y = tileMap.size.y;
            }

            Vector3Int size = new Vector3Int(x, y, 0);

            tileMap.size = size;
        }
    }
}

