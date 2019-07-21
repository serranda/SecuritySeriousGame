using UnityEngine;
using TMPro;
using System;
using System.Globalization;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HudManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TextMeshProUGUI date;
    private TextMeshProUGUI money;
    private TextMeshProUGUI successfulThreat;
    private TextMeshProUGUI totalThreat;
    private TextMeshProUGUI trustedEmployees;
    private TextMeshProUGUI totalEmployees;
    private TextMeshProUGUI reputation;
    private TextMeshProUGUI lastThreatAttack;

    public static bool cursorOverHud;

    private Toggle toggleX1;
    private Toggle toggleX2;
    private Toggle toggleX5;
    private Toggle toggleX10;

    public Toggle activeToggle;

    private void OnEnable()
    {
        date = GameObject.Find("Date").GetComponent<TextMeshProUGUI>();
        money = GameObject.Find("MoneyInt").GetComponent<TextMeshProUGUI>();
        successfulThreat = GameObject.Find("SuccessfulThreat").GetComponent<TextMeshProUGUI>();
        totalThreat = GameObject.Find("TotalThreat").GetComponent<TextMeshProUGUI>();
        trustedEmployees = GameObject.Find("TrustedEmployees").GetComponent<TextMeshProUGUI>();
        totalEmployees = GameObject.Find("TotalEmployees").GetComponent<TextMeshProUGUI>();
        reputation = GameObject.Find("Reputation").GetComponent<TextMeshProUGUI>();
        lastThreatAttack = GameObject.Find("LastThreatType").GetComponent<TextMeshProUGUI>();

        toggleX1 = GameObject.Find(StringDb.toggleX1).GetComponent<Toggle>();
        toggleX2 = GameObject.Find(StringDb.toggleX2).GetComponent<Toggle>();
        toggleX5 = GameObject.Find(StringDb.toggleX5).GetComponent<Toggle>();
        toggleX10 = GameObject.Find(StringDb.toggleX10).GetComponent<Toggle>();

        SetTogglePressed();
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
        switch (StringDb.speedMultiplier)
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
        switch (activeToggle.name)
        {
            case StringDb.toggleX1:
                StringDb.speedMultiplier = 1;
                break;
            case StringDb.toggleX2:
                StringDb.speedMultiplier = 2;
                break;
            case StringDb.toggleX5:
                StringDb.speedMultiplier = 5;
                break;
            case StringDb.toggleX10:
                StringDb.speedMultiplier = 10;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
