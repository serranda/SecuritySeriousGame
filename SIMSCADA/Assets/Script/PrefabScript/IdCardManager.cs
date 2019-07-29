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
    public static bool idCardEnabled;
    private static Canvas idCard;

    private static Button backBtn;
    private static TextMeshProUGUI nameTmPro;
    private static TextMeshProUGUI surnameTmPro;
    private static TextMeshProUGUI jobTmPro;
    private static TextMeshProUGUI trustedTmPro;
    private static Image aiImage;

    private static TextMeshProUGUI attackerNote;
    private static TextMeshProUGUI attackerLbl;

    //private static Sprite[] sprites;

    private ILevelManager manager;


    private void OnEnable()
    {
        manager = SetLevelManager();

        idCardEnabled = true;

        //sprites = Resources.LoadAll<Sprite>(Path.Combine(StringDb.marksFolder, StringDb.marksPrefix));

        backBtn = GameObject.Find(StringDb.idCardBtnBack).GetComponent<Button>();

        backBtn.onClick.RemoveAllListeners();
        backBtn.onClick.AddListener(ToggleIdCard);

        nameTmPro = GameObject.Find(StringDb.idCardName).GetComponent<TextMeshProUGUI>();
        surnameTmPro = GameObject.Find(StringDb.idCardSurname).GetComponent<TextMeshProUGUI>();
        jobTmPro = GameObject.Find(StringDb.idCardJob).GetComponent<TextMeshProUGUI>();
        trustedTmPro = GameObject.Find(StringDb.idCardTrusted).GetComponent<TextMeshProUGUI>();
        aiImage = GameObject.Find(StringDb.idCardAiImage).GetComponent<Image>();

        attackerNote = GameObject.Find(StringDb.idCardAttackerNote).GetComponent<TextMeshProUGUI>();
        attackerLbl = GameObject.Find(StringDb.idCardAttackerLbl).GetComponent<TextMeshProUGUI>();

        if (!manager.GetGameData().idCardUpgraded) return;

        attackerNote.GetComponent<CanvasGroup>().alpha = 1.0f;
        attackerLbl.GetComponent<CanvasGroup>().alpha = 1.0f;
    }

    private void OnDisable()
    {
        idCardEnabled = false;
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


    public void ToggleIdCard()
    {
        if (idCardEnabled)
        {
            ClassDb.prefabManager.ReturnPrefab(idCard.gameObject, PrefabManager.icIndex);
        }
        else
        {
            idCard = ClassDb.prefabManager.GetPrefab(ClassDb.prefabManager.prefabIdCard.gameObject, PrefabManager.icIndex).GetComponent<Canvas>();
        }

    }

    public void SetLabels(string aiName, string aiSurname, string aiJob, bool aiTrusted ,bool aiAttacker, string spriteName)
    {
        string spriteFolder = Path.Combine(StringDb.rscSpriteFolder, StringDb.rscAiSpriteFolder);
        
        aiImage.sprite = Resources.LoadAll<Sprite>(Path.Combine(spriteFolder, spriteName))[0] ;

        nameTmPro.text = aiName;
        surnameTmPro.text = aiSurname;
        jobTmPro.text = aiJob;
        trustedTmPro.text = aiTrusted ? "Trusted" : "Normal";
        if (manager.GetGameData().idCardUpgraded)
        {
            attackerNote.text = string.Format(aiAttacker ?
                Resources.Load<TextAsset>(StringDb.attacker + Random.Range(1, 4)).text :
                Resources.Load<TextAsset>(StringDb.employee + Random.Range(1, 4)).text, Random.Range(3, 6));
        }
    }
}
