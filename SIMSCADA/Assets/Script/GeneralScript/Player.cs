[System.Serializable]

public class Player
{
    public string username;
    public string password;
    public string name;
    public string surname;
    public string folderName
    {
        get
        {
            return string.Concat(username, "_", name, surname);
        }

    }

    public Player(string username, string password)
    {
        this.username = username;
        this.password = password;
        this.name = string.Empty;
        this.surname = string.Empty;
    }

    public Player(string username, string password, string name, string surname)
    {
        this.username = username;
        this.password = password;
        this.name = name;
        this.surname = surname;
    }

    public override string ToString()
    {
        return string.Format("Username: {0}, Password: {1}, Name: {2}, Surname: {3}", username, password, name, surname);
    }
}

