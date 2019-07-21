using UnityEngine;

public class GameSettings
{ 
    public bool fullScreen;
    public float volume;
    public int resolutionIndex;

    public override string ToString()
    {
        return string.Format("FullScreen: {0}, Volume: {1}, ResolutionIndex: {2}", fullScreen, volume, resolutionIndex);
    }
}
