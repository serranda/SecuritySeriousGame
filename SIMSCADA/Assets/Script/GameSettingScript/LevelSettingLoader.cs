using UnityEngine;

public class LevelSettingLoader : MonoBehaviour
{
    [SerializeField] private GameSettingManager gameSettingManager;

    public void Awake()
    {

        gameSettingManager.StartCheckWebSettingsFileRoutine();
    }


    private void Update()
    {
        gameSettingManager.GetSpriteFromBool(Screen.fullScreen);
    }
}
