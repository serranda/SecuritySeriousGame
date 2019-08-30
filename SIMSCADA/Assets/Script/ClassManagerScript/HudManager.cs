using UnityEngine;
using TMPro;
using System;
using System.Globalization;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HudManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI date;
    [SerializeField] private TextMeshProUGUI money;
    [SerializeField] private TextMeshProUGUI successfulThreat;
    [SerializeField] private TextMeshProUGUI totalThreat;
    [SerializeField] private TextMeshProUGUI trustedEmployees;
    [SerializeField] private TextMeshProUGUI totalEmployees;
    [SerializeField] private TextMeshProUGUI reputation;
    [SerializeField] private TextMeshProUGUI lastThreatAttack;

    [SerializeField] private Toggle toggleX1;
    [SerializeField] private Toggle toggleX2;
    [SerializeField] private Toggle toggleX5;
    [SerializeField] private Toggle toggleX10;

    public Toggle activeToggle;

    private ILevelManager manager;

    private TutorialManager tutorialManager;

    private void OnEnable()
    {
        manager = SetLevelManager();

        //date = GameObject.Find("Date").GetComponent<TextMeshProUGUI>();
        //money = GameObject.Find("MoneyInt").GetComponent<TextMeshProUGUI>();
        //successfulThreat = GameObject.Find("SuccessfulThreat").GetComponent<TextMeshProUGUI>();
        //totalThreat = GameObject.Find("TotalThreat").GetComponent<TextMeshProUGUI>();
        //trustedEmployees = GameObject.Find("TrustedEmployees").GetComponent<TextMeshProUGUI>();
        //totalEmployees = GameObject.Find("TotalEmployees").GetComponent<TextMeshProUGUI>();
        //reputation = GameObject.Find("Reputation").GetComponent<TextMeshProUGUI>();
        //lastThreatAttack = GameObject.Find("LastThreatType").GetComponent<TextMeshProUGUI>();

        //toggleX1 = GameObject.Find(StaticDb.toggleX1).GetComponent<Toggle>();
        //toggleX2 = GameObject.Find(StaticDb.toggleX2).GetComponent<Toggle>();
        //toggleX5 = GameObject.Find(StaticDb.toggleX5).GetComponent<Toggle>();
        //toggleX10 = GameObject.Find(StaticDb.toggleX10).GetComponent<Toggle>();

        SetTogglePressed();
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

    public void UpdateHud(DateTime dateHud, float moneyHud, int successfulThreatHud, int totalThreatHud, int trustedEmployeesHud,
        int totalEmployeesHud, float reputationHud)
    {
        date.text = dateHud.ToString("HH:mm dd/MM/yyyy");
        money.text = Math.Round(moneyHud, 0).ToString(CultureInfo.CurrentCulture) + " €";
        successfulThreat.text = successfulThreatHud.ToString();
        totalThreat.text = totalThreatHud.ToString();
        trustedEmployees.text = trustedEmployeesHud.ToString();
        totalEmployees.text = totalEmployeesHud.ToString();
        reputation.text = SetReputationString(reputationHud);
    }

    public void UpdateHud(float moneyHud, int successfulThreatHud, int totalThreatHud, int totalEmployeesHud, float reputationHud)
    {
        money.text = Math.Round(moneyHud, 0).ToString(CultureInfo.CurrentCulture) + " €";
        successfulThreat.text = successfulThreatHud.ToString();
        totalThreat.text = totalThreatHud.ToString();
        totalEmployees.text = totalEmployeesHud.ToString();
        reputation.text = reputation.text = SetReputationString(reputationHud);

    }

    public void UpdateLastThreat(Threat threat)
    {
        lastThreatAttack.text = threat.threatAttack.ToString().ToUpperInvariant();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //cursorOverHud = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //cursorOverHud = false;
    }

    private void SetTogglePressed()
    {
        if (manager != null)
        {
            switch (manager.GetGameData().simulationSpeedMultiplier)
            {
                case 1:
                    activeToggle = toggleX1;
                    break;
                case 2:
                    activeToggle = toggleX2;
                    break;
                case 5:
                    activeToggle = toggleX5;
                    break;
                case 10:
                    activeToggle = toggleX10;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        else
        {
            tutorialManager = FindObjectOfType<TutorialManager>();

            switch (tutorialManager.tutorialGameData.simulationSpeedMultiplier)
            {
                case 1:
                    activeToggle = toggleX1;
                    break;
                case 2:
                    activeToggle = toggleX2;
                    break;
                case 5:
                    activeToggle = toggleX5;
                    break;
                case 10:
                    activeToggle = toggleX10;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }



        activeToggle.isOn = true;
    }

    public void SetActiveToggle(Toggle toggle)
    {
        activeToggle = toggle;
        SetTimeSpeedMultiplier();
    }

    private string SetReputationString(float reputationPercentage)
    {
        string reputationString = string.Empty;

        if (reputationPercentage < 35)
            reputationString = "Molto Scarsa";
        if (reputationPercentage >= 35 && reputationPercentage < 50)
            reputationString = "Scarsa";
        if (reputationPercentage >= 50 && reputationPercentage < 65)
            reputationString = "Discreta";
        if (reputationPercentage >= 65 && reputationPercentage < 85)
            reputationString = "Buona";
        if (reputationPercentage >= 85)
            reputationString = "Ottima";

        return reputationString;
    }

    private void SetTimeSpeedMultiplier()
    {
        if (manager != null)
        {
            switch (activeToggle.name)
            {

                case StaticDb.toggleX1:
                    manager.GetGameData().simulationSpeedMultiplier = 1;
                    break;
                case StaticDb.toggleX2:
                    manager.GetGameData().simulationSpeedMultiplier = 2;
                    break;
                case StaticDb.toggleX5:
                    manager.GetGameData().simulationSpeedMultiplier = 5;
                    break;
                case StaticDb.toggleX10:
                    manager.GetGameData().simulationSpeedMultiplier = 10;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            //WRITE LOG
            ClassDb.logManager.StartWritePlayerLogRoutine(StaticDb.player, StaticDb.logEvent.UserEvent, "TIME SPEED: " + manager.GetGameData().simulationSpeedMultiplier);
        }
        else
        {
            tutorialManager = FindObjectOfType<TutorialManager>();

            switch (activeToggle.name)
            {

                case StaticDb.toggleX1:
                    tutorialManager.tutorialGameData.simulationSpeedMultiplier = 1;
                    break;
                case StaticDb.toggleX2:
                    tutorialManager.tutorialGameData.simulationSpeedMultiplier = 2;
                    break;
                case StaticDb.toggleX5:
                    tutorialManager.tutorialGameData.simulationSpeedMultiplier = 5;
                    break;
                case StaticDb.toggleX10:
                    tutorialManager.tutorialGameData.simulationSpeedMultiplier = 10;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }



    }
}
