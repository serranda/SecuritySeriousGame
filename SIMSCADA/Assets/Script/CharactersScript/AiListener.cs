using System.Collections;
using MuteColossus;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AiListener : MonoBehaviour
{
    private AiController aiController;

    private IEnumerator showAiIdRoutine;

    private ILevelManager manager;

    private void OnEnable()
    {
        aiController = GetComponent<AiController>();
        manager = SetLevelManager();
    }

    private ILevelManager SetLevelManager()
    {
        ILevelManager iManager;
        if (SceneManager.GetActiveScene().buildIndex == StaticDb.level1SceneIndex)
            iManager = FindObjectOfType<Level1Manager>();
        else
            iManager = FindObjectOfType<Level2Manager>();

        return iManager;
    }

    public void StartShowAiId()
    {
        aiController.onClickAi = true;

        aiController.SetInteraction(false);

        aiController.StartWait();

        float progressDuration;

        if (!aiController.idChecked)
        {
            progressDuration = manager.GetGameData().idCardTime;
        }
        else
        {
            progressDuration = 0;
        }

        TimeEvent progressEvent = ClassDb.timeEventManager.NewTimeEvent(
            progressDuration, aiController.gameObject, true, true, StaticDb.showAiIdRoutine);

        manager.GetGameData().timeEventList.Add(progressEvent);

        showAiIdRoutine = ShowAiId(progressEvent);
        StartCoroutine(showAiIdRoutine);

    }

    public void RestartShowAiId(TimeEvent progressEvent)
    {
        aiController.onClickAi = true;

        aiController.SetInteraction(false);

        aiController.StartWait();

        showAiIdRoutine = ShowAiId(progressEvent);
        StartCoroutine(showAiIdRoutine);
    }

    private IEnumerator ShowAiId(TimeEvent progressEvent)
    {
        yield return new WaitWhile(() => manager.GetGameData().timeEventList.Contains(progressEvent));
        yield return new WaitWhile(() => manager.GetGameData().idCardEnabled);

        ClassDb.idCardManager.ToggleIdCard();

        aiController.idChecked = true;

        if (aiController.idSpriteName == null)
        {
            if (aiController.isAttacker)
            {
                string spriteNumber = Random.Range(1, 6).ToString("D2");
                string spriteName = StaticDb.rscAiSpritePrefix + "_" + CNG.instance.gen.GenerateNameObject(Gender.Other).GetGenderString() + "_" + spriteNumber;

                aiController.idSpriteName = spriteName;
            }
            else
            {
                aiController.idSpriteName = aiController.nSpriteName;
            }
        }


        ClassDb.idCardManager.SetLabels(aiController.aiName, aiController.aiSurname, aiController.aiJob, aiController.isTrusted,
            aiController.isAttacker, aiController.idSpriteName);

        aiController.SetInteraction(true);
    }

    public void FireAi(GameObject aiGameObject)
    {
        aiController.onClickAi = false;
        ClassDb.spawnCharacter.RemoveAi(aiGameObject);

        manager.StopLocalThreat(aiController.timeEvent.threat);
        ClassDb.levelMessageManager.StartThreatStopped(aiController.timeEvent.threat);
    }
}
