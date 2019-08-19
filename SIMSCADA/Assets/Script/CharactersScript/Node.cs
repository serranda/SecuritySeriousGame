using UnityEngine;

/// <summary>
/// Stores all information relevant to the A* algorithm
/// </summary>

[System.Serializable]
public class Node
{

    public bool passable;
    public int gCost;
    public int hCost;
    public Vector2Int position;
    public Node parent;
    public int fCost
    {
        get { return gCost + hCost; }
    }
    public Node()
    {
        passable = false;
        gCost = 0;
        hCost = 0;
        position = default(Vector2Int);
        parent = null;
    }

    public Node(int x, int y, bool passable)
    {
        position = new Vector2Int(x, y);
        this.passable = passable;
    }
    public override string ToString()
    {
        return "x: " + position.x + " y: " + position.y + " passable: " + passable;
    }
    public override bool Equals(object obj)
    {
        if (obj == null)
            return false;

        Node node = obj as Node;
        return node != null && Equals(node);
    }
    public override int GetHashCode()
    {
        return position.x ^ position.y;
    }
    public bool Equals(Node obj)
    {
        if (obj == null)
        {
            return false;
        }

        return (position.x == obj.position.x) && (position.y == obj.position.y);
    }
}

