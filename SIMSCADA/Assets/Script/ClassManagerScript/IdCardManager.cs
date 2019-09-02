using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class IdCardManager : MonoBehaviour
{
    //private static Canvas idCard;

    [SerializeField] private Button backBtn;
    [SerializeField] private TextMeshProUGUI nameTmPro;
    [SerializeField] private TextMeshProUGUI surnameTmPro;
    [SerializeField] private TextMeshProUGUI jobTmPro;
    [SerializeField] private TextMeshProUGUI trustedTmPro;
    [SerializeField] private Image aiImage;

    [SerializeField] private TextMeshProUGUI attackerLbl;
    [SerializeField] private TextMeshProUGUI attackerNote;

    private ILevelManager manager;

    private void OnEnable()
    {
        manager = SetLevelManager();

        manager.GetGameData().idCardEnabled = true;

        backBtn.onClick.RemoveAllListeners();
        backBtn.onClick.AddListener(delegate
        {
            CloseIdCard(gameObject.GetComponent<Canvas>());
        });

        if (!manager.GetGameData().idCardUpgraded) return;

        attackerNote.GetComponent<CanvasGroup>().alpha = 1.0f;
        attackerLbl.GetComponent<CanvasGroup>().alpha = 1.0f;
    }

    private void OnDisable()
    {
        manager.GetGameData().idCardEnabled = false;
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

    public Canvas OpenIdCard()
    {
        Canvas idCard = ClassDb.prefabManager.GetPrefab(ClassDb.prefabManager.prefabIdCard.gameObject, PrefabManager.icIndex).GetComponent<Canvas>();
        //WRITE LOG
        ClassDb.logManager.StartWritePlayerLogRoutine(StaticDb.player, StaticDb.logEvent.UserEvent, "ID OPENED");

        return idCard;
    }

    public void CloseIdCard(Canvas idCard)
    {
        ClassDb.prefabManager.ReturnPrefab(idCard.gameObject, PrefabManager.icIndex);
        //WRITE LOG
        ClassDb.logManager.StartWritePlayerLogRoutine(StaticDb.player, StaticDb.logEvent.UserEvent, "ID CLOSED");
    }

    public void SetLabels(string aiName, string aiSurname, string aiJob, bool aiTrusted ,bool aiAttacker, string spriteName)
    {
        manager = SetLevelManager();

        string spriteFolder = Path.Combine(StaticDb.rscSpriteFolder, StaticDb.rscAiSpriteFolder);
        
        aiImage.sprite = Resources.LoadAll<Sprite>(Path.Combine(spriteFolder, spriteName))[0];

        nameTmPro.text = aiName;

        surnameTmPro.text = aiSurname;

        jobTmPro.text = aiJob;

        trustedTmPro.text = aiTrusted ? "Trusted" : "Normal";

        if (manager.GetGameData().idCardUpgraded)
        {
            attackerNote.text = string.Format(aiAttacker ?
                Resources.Load<TextAsset>(StaticDb.attacker + Random.Range(1, 4)).text :
                Resources.Load<TextAsset>(StaticDb.employee + Random.Range(1, 4)).text, Random.Range(3, 6));
        }
    }
}
