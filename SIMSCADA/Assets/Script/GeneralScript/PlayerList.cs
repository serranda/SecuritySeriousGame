using System.Collections.Generic;

[System.Serializable]

public class PlayerList
{
    public List<Player> list;

    public PlayerList()
    {
        list = new List<Player>();
    }

    public override string ToString()
    {
        return string.Format("List: {0}", list);
    }
}
