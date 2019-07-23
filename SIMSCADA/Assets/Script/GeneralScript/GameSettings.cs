[System.Serializable]

public class GameSettings
{ 
    public bool fullScreen;
    public float volume;
    public int resolutionIndex;

    public GameSettings(bool fullScreen, float volume, int resolutionIndex)
    {
        this.fullScreen = fullScreen;
        this.volume = volume;
        this.resolutionIndex = resolutionIndex;
    }

    public GameSettings(bool fullScreen, float volume)
    {
        this.fullScreen = fullScreen;
        this.volume = volume;
        this.resolutionIndex = 0;
    }

    public override string ToString()
    {
        return string.Format("FullScreen: {0}, Volume: {1}, ResolutionIndex: {2}", fullScreen, volume, resolutionIndex);
    }
}
