using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FollowCursor : MonoBehaviour
{
    private int pixelHeight;
    private int pixelWidth;
    private Camera cameraMain;
    private bool isCameraMainNull;

    //private FloorManager floorManager;

    private ILevelManager manager;


    private void Start()
    {
        manager = SetLevelManager();

        cameraMain = Camera.main;
        isCameraMainNull = cameraMain == null;

        if (cameraMain == null) return;
        pixelHeight = cameraMain.pixelHeight;
        pixelWidth = cameraMain.pixelWidth;

        //floorManager = GameObject.Find(StringDb.floorTileMap).GetComponent<FloorManager>();

    }
    // Update is called once per frame
    private void Update()
    {

        if ( PauseManager.pauseEnabled
            || manager.GetGameData().dialogEnabled
            || manager.GetGameData().scadaEnabled
            || manager.GetGameData().cursorOverAi
            || manager.GetGameData().storeEnabled
            || InteractiveSprite.onSprite
            || isCameraMainNull
            || !Input.GetMouseButton(1)
            ) return;

        Vector3 mousePos = Input.mousePosition;
        mousePos.x = Mathf.Clamp(mousePos.x, 0, pixelWidth);
        mousePos.y = Mathf.Clamp(mousePos.y, 0, pixelHeight);
        Vector3 mouseWorldPosition = cameraMain.ScreenToWorldPoint(mousePos);

        mouseWorldPosition.z = 0f;
        transform.position = mouseWorldPosition;

    }

    private ILevelManager SetLevelManager()
    {
        ILevelManager iManager;
        if (SceneManager.GetActiveScene().buildIndex == StringDb.level1SceneIndex)
            iManager = FindObjectOfType<Level1Manager>();
        else
            iManager = FindObjectOfType<Level2Manager>();

        return iManager;
    }
}
