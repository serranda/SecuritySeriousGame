[System.Serializable]

public class GameSettings
{ 
    public bool fullScreen;
    public float volume;

    public GameSettings(bool fullScreen, float volume)
    {
        this.fullScreen = fullScreen;
        this.volume = volume;
    }

    public override string ToString()
    {
        return string.Format("FullScreen: {0}, Volume: {1}, ResolutionIndex: {2}", fullScreen, volume);
    }
}
