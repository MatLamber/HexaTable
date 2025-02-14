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
        EventsManager.Instance.onRefresUI += RefreshUI;
    }
    private void OnDisable()
    {
        EventsManager.Instance.onRefresUI -= RefreshUI;
    }

    public void RefreshUI()
    {
        pointProgresFill.fillAmount = (float)GameController.Instance.Points / GameController.Instance.GoalPoints;
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
}
