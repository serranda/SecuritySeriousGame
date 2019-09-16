using System.Collections;

interface ILevelManager
{
    IEnumerator StarterCoroutine();

    void SpawnHud();

    void StartTimeRoutine();

    void StartAllCoroutines();

    void UpdateMinutes();

    IEnumerator OnNewMinute();

    void CheckEndgame();

    void SetTrendThreatAttack();

    void SetRandomizer();

    float UpdateMoney();

    IEnumerator CreateNewThreat();

    void NewThreat();

    void InstantiateNewThreat(Threat threat);

    void StartLocalThreat(Threat threat);

    void StartRemoteThreat(Threat threat);

    void StartFakeLocalThreat(Threat threat);

    IEnumerator DeployThreat(Threat threat);

    bool BeforeDeployThreat(Threat threat);

    void AfterDeployThreat(Threat threat);

    void StopLocalThreat(Threat threat);

    void UpdateReputation(Threat threat, StaticDb.ThreatStatus threatStatus);

    IEnumerator RemoteIdsCheckRoutine();

    void RemoteIdsCheck();

    IEnumerator LocalIdsCheckRoutine();

    void LocalIdsCheck();

    void StartThreatManagementResultData(Threat threat);

    IEnumerator ThreatManagementResultData(Threat threat);

    void SetFirewallActive(bool active);

    void SetRemoteIdsActive(bool active);

    void SetLocalIdsActive(bool active);

    GameData GetGameData();

    void SetGameData(GameData data);

    void StopAllCoroutines();

    void RestorePrefabs(GameData data);

}
