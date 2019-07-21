using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCursor : MonoBehaviour
{
    private int pixelHeight;
    private int pixelWidth;
    private Camera cameraMain;
    private bool isCameraMainNull;

    //private FloorManager floorManager;


    private void Start()
    {
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
            || DialogBoxManager.dialogEnabled
            || RoomPcListener.scadaEnabled
            || AiController.cursorOverAi
            || RoomPcListener.storeEnabled
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
}
