using System;
using System.IO;
using UnityEngine;

public class SpriteSheetAnimator : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    private AiController aiController;
    //private PlayerController playerController;

    private string spriteName;
    private string spriteFolder;

    private Sprite[] sprites;

    //private bool isAiControllerNotNull;

    private void Start()
    {
        ////these lines of codes should be used when there is also the player prefab

        //isAiControllerNotNull = gameObject.GetComponent<AiController>() != null;
        //if (gameObject.GetComponent<AiController>() != null)
        //{
        //    aiController = gameObject.GetComponent<AiController>();
        //}
        //else
        //{
        //    playerController = gameObject.GetComponent<PlayerController>();
        //}

        aiController = gameObject.GetComponent<AiController>();
    }

    public void SetSpriteWithAnimator(int index)
    {
        ////these lines of codes should be used when there is also the player prefab

        //try
        //{
        //    if (aiController != null)
        //    {
        //        if (aiController.timeEvent.threat.threatType != StaticDb.ThreatType.local &&
        //            aiController.timeEvent.threat.threatType != StaticDb.ThreatType.fakeLocal) return;

        //        if (index < 0 || index >= sprites.Length)
        //            return;
        //        spriteRenderer.sprite = sprites[index];

        //    }
        //    else
        //    {

        //        if (index < 0 || index >= sprites.Length)
        //            return;
        //        spriteRenderer.sprite = sprites[index];

        //    }
        //}
        //catch (Exception e)
        //{
        //    Debug.Log(e);
        //}

        try
        {
            spriteName = aiController.spriteToAnimate;

            spriteFolder = Path.Combine(StaticDb.rscSpriteFolder, StaticDb.rscAiSpriteFolder);

            sprites = Resources.LoadAll<Sprite>(Path.Combine(spriteFolder, spriteName));

            if (aiController.timeEvent.threat.threatType != StaticDb.ThreatType.local &&
                aiController.timeEvent.threat.threatType != StaticDb.ThreatType.fakeLocal) return;

            if (index < 0 || index >= sprites.Length)
                return;
            spriteRenderer.sprite = sprites[index];

        }
        catch (Exception e)
        {
            Debug.Log(e);
        }


    }
}
