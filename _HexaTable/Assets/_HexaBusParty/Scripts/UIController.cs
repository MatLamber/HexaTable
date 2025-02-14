using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Image pointProgresFill;
    [SerializeField] private TextMeshProUGUI goalPointsText;
    [SerializeField] private TextMeshProUGUI currentMultiplierText;
    [SerializeField] private TextMeshProUGUI rawPointsText;
    [SerializeField] private TextMeshProUGUI modifierText;
    [SerializeField] private TextMeshProUGUI pointsText;
    [SerializeField] private GameObject scoreResultPanel;
    
    [Header("Settings")]
    [SerializeField] private List<Color> modifierColors;
    private static UIController _instance;
    public static UIController Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("GameController is not initialized!");
            }
            return _instance;
        }
    }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        SetGoalPoints();
        RefreshUI();
        EventsManager.Instance.onRefresUI += RefreshUI;
    }
    private void OnDisable()
    {
        EventsManager.Instance.onRefresUI -= RefreshUI;
    }

    public void RefreshUI()
    {
        LeanTween.value(gameObject, GameController.Instance.PrevPoints, GameController.Instance.Points, 0.5f)
            .setOnUpdate(UpdateText).setEase(LeanTweenType.easeOutSine);
    }

    public void SetGoalPoints()
    {
        goalPointsText.text = GameController.Instance.GoalPoints.ToString();
        RefreshUI();
    }

    public void SetCurrentMultiplier()
    {
        currentMultiplierText.text = $" current multiplier : {GameController.Instance.CurrentModifierValue}";
    }


    public void ShowResultPanel(int amount)
    {
        rawPointsText.text = $"{amount.ToString()}";
        switch (GameController.Instance.CurrentModifierType)
        {
            case ModifierType.Add:
                modifierText.color = modifierColors[0];
                modifierText.text = $"+{GameController.Instance.CurrentModifierValue}";
                break;
            case ModifierType.Substract:
                modifierText.color = modifierColors[1];
                modifierText.text = $"-{GameController.Instance.CurrentModifierValue}";
                break;
            case ModifierType.Divide:
                modifierText.color = modifierColors[2];
                modifierText.text = $"÷{GameController.Instance.CurrentModifierValue}";
                break;
            case ModifierType.Multiply:
                modifierText.color = modifierColors[3];
                modifierText.text = $"x{GameController.Instance.CurrentModifierValue}";
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        LeanTween.scale(scoreResultPanel, Vector3.one, 0.5f).setEase(LeanTweenType.easeOutBack);
        LeanTween.scale(scoreResultPanel, Vector3.zero, 0.5f).setEase(LeanTweenType.easeInBack).setDelay(1.5f);
    }

    public void UpdateText(float value)
    {
        pointsText.text = $"{(int) value}/{GameController.Instance.GoalPoints}";
        pointProgresFill.fillAmount = value / GameController.Instance.GoalPoints;
    }
    
}
