using System.Collections;
using MuteColossus;
using UnityEngine;

public class AiListener : MonoBehaviour
{
    private AiController aiController;

    private IEnumerator checkRoutine;
    private IEnumerator showRoutine;

    private void OnEnable()
    {
        aiController = GetComponent<AiController>();
    }

    public void StartCheckAiId()
    {
        aiController.onClickAi = true;

        aiController.StartWait();

        if (!aiController.idChecked)
        {
            //checkRoutine = CheckAiId();
            //StartCoroutine(checkRoutine);
            CheckAiId();
        }
        else
        {
            TimeEvent showEvent = ClassDb.timeEventManager.NewTimeEvent(
                0, aiController.gameObject, false, true);

            showRoutine = ShowAiId(showEvent);
            StartCoroutine(showRoutine);
        }
    }

    private void CheckAiId()
    {
        aiController.SetInteraction(false);
        //yield return new WaitWhile(() => aiController.playerController.pathUpdated);
        TimeEvent progressEvent = ClassDb.timeEventManager.NewTimeEvent(
            GameData.idCardTime, aiController.gameObject, true, true);
        ClassDb.levelManager.timeEventList.Add(progressEvent);

        aiController.idChecked = true;

        showRoutine = ShowAiId(progressEvent);
        StartCoroutine(showRoutine);
    }

    private IEnumerator ShowAiId(TimeEvent progressEvent)
    {
        yield return new WaitWhile(() => ClassDb.levelManager.timeEventList.Contains(progressEvent));
        yield return new WaitWhile(() => IdCardManager.idCardEnabled);
        ClassDb.idCardManager.ToggleIdCard();

        string sprite;

        if (aiController.isAttacker)
        {
            string spriteNumber = Random.Range(1, 6).ToString("D2");
            string spriteName = StringDb.rscAiSpritePrefix + "_" + CNG.instance.gen.GenerateNameObject(Gender.Other).GetGenderString() + "_" + spriteNumber;

            sprite = spriteName;
        }
        else
        {
            sprite = aiController.nSpriteName;
        }

        ClassDb.idCardManager.SetLabels(aiController.aiName, aiController.aiSurname, aiController.aiJob, aiController.isTrusted,
            aiController.isAttacker, sprite);

        aiController.SetInteraction(true);

    }

    public void FireAi(GameObject aiGameObject)
    {
        aiController.onClickAi = false;
        ClassDb.spawnCharacter.RemoveAi(aiGameObject);

        ClassDb.levelManager.StopLocalThreat(aiController.timeEvent.threat);
        ClassDb.levelMessageManager.StartThreatStopped(aiController.timeEvent.threat);
    }
}
