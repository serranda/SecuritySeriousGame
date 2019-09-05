[System.Serializable]

public class Player
{
    public string username;
    public string hash;
    public string name;
    public string surname;
    public string salt;

    public static int staticInt = 72;
    public string folderName => string.Concat(username, "_", name, surname);

    public Player(string username, string hash, string salt)
    {
        this.username = username;
        this.hash = hash;
        this.name = string.Empty;
        this.surname = string.Empty;
        this.salt = salt;
    }

    public Player(string username, string hash, string name, string surname, string salt)
    {
        this.username = username;
        this.hash = hash;
        this.name = name;
        this.surname = surname;
        this.salt = salt;
    }

    public override string ToString()
    {
        return $"Username: {username}, Password: {hash}, Name: {name}, Surname: {surname}, Salt: {salt}";
    }
}

