using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraManager : MonoBehaviour
{
    private int pixelHeight;
    private int pixelWidth;
    private Camera cameraMain;
    //private bool isCameraMainNull;
    private CinemachineVirtualCamera virtualCamera;

    [SerializeField] private float zoomScale;

    //private FloorManager floorManager;

    private ILevelManager manager;

    [SerializeField] private float minSize = 2.0f;
    [SerializeField] private float maxSize = 12.0f;

    private void Start()
    {
        manager = SetLevelManager();

        cameraMain = Camera.main;

        if (cameraMain == null) return;
        pixelHeight = cameraMain.pixelHeight;
        pixelWidth = cameraMain.pixelWidth;

        zoomScale = 0.1f;
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
            //|| isCameraMainNull
            ) return;

        if (Input.GetMouseButton(1))
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.x = Mathf.Clamp(mousePos.x, 0, pixelWidth);
            mousePos.y = Mathf.Clamp(mousePos.y, 0, pixelHeight);
            Vector3 mouseWorldPosition = cameraMain.ScreenToWorldPoint(mousePos);

            mouseWorldPosition.z = 0f;
            transform.position = mouseWorldPosition;
        }

        virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        virtualCamera.m_Lens.OrthographicSize += Input.mouseScrollDelta.y * zoomScale;
        //Debug.Log(virtualCamera.m_Lens.OrthographicSize);

        if (virtualCamera.m_Lens.OrthographicSize < minSize)
            virtualCamera.m_Lens.OrthographicSize = minSize;

        if (virtualCamera.m_Lens.OrthographicSize > maxSize)
            virtualCamera.m_Lens.OrthographicSize = maxSize;

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
