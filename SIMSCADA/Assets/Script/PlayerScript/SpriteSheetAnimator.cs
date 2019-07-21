using System;
using System.IO;
using UnityEngine;

public class SpriteSheetAnimator : MonoBehaviour
{
    private Sprite[] sprites;
    private SpriteRenderer spriteRenderer;
    private AiController aiController;
    private PlayerController playerController;
    private string spriteName;
    private string spriteFolder;
    private bool isAiControllerNotNull;

    private void Start()
    {
        isAiControllerNotNull = gameObject.GetComponent<AiController>() != null;
        if (gameObject.GetComponent<AiController>() != null)
        {
            aiController = gameObject.GetComponent<AiController>();
            spriteRenderer = aiController.GetComponent<SpriteRenderer>();
            spriteName = aiController.spriteToAnimate;
        }
        else
        {
            playerController = gameObject.GetComponent<PlayerController>();
            spriteRenderer = playerController.GetComponent<SpriteRenderer>();
            spriteName = playerController.spriteToAnimate;
        }

        sprites = Resources.LoadAll<Sprite>(Path.Combine(StringDb.rscSpriteFolder, spriteName));
    }
    private void Update()
    {
        spriteName = isAiControllerNotNull ? aiController.spriteToAnimate : playerController.spriteToAnimate;
        spriteFolder = isAiControllerNotNull
            ? Path.Combine(StringDb.rscSpriteFolder, StringDb.rscAiSpriteFolder)
            : Path.Combine(StringDb.rscSpriteFolder, StringDb.rscPlSpriteFolder);
        sprites = Resources.LoadAll<Sprite>(Path.Combine(spriteFolder, spriteName));
    }
    public void SetSpriteWithAnimator(int index)
    {
        if (aiController != null)
        {
            if (aiController.timeEvent.threat.threatType != StringDb.ThreatType.local &&
                aiController.timeEvent.threat.threatType != StringDb.ThreatType.fakeLocal) return;
            try
            {
                if (index < 0 || index >= sprites.Length)
                    return;
                spriteRenderer.sprite = sprites[index];
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
        else
        {
            try
            {
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
}
